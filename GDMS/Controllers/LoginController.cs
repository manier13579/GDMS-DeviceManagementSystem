using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using GDMS.Models;
using System.Data;
using System.Web;
using System.Security.Cryptography;

namespace GDMS.Controllers
{


    public class LoginController : ApiController
    {
        //POST对象
        public class LoginUser {
            public string username { get; set; }
            public string password { get; set; }
        }

        //返回对象
        private class Response
        {
            public int code { get; set; }
            public string msg { get; set; }
            public object data { get; set; }
        }

        /// 获取IP地址 
        public static string IPAddress
        {
            get
            {
                string userIP;
                // HttpRequest Request = HttpContext.Current.Request; 
                HttpRequest Request = HttpContext.Current.Request; // ForumContext.Current.Context.Request; 
                                                                   // 如果使用代理，获取真实IP 
                if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != "")
                    userIP = Request.ServerVariables["REMOTE_ADDR"];
                else
                    userIP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (userIP == null || userIP == "")
                    userIP = Request.UserHostAddress;
                return userIP;
            }
        }

        //MD5加密函数
        public static string MD5Encrypt32(string password)
        {
            string cl = password;
            string pwd = "";
            MD5 md5 = MD5.Create(); //实例化一个md5对像
                                    // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                pwd = pwd + s[i].ToString("X");
            }
            return pwd;
        }

        //通过POST只能获取1个对象，因此POST多个数据需要使用类
        public HttpResponseMessage Login([FromBody] LoginUser user) {
            Db db = new Db();
            string sql = "SELECT count(*) AS count FROM GDMS_USER WHERE USER_ID = '" + user.username + "' AND PASS= '" + MD5Encrypt32(user.password) + "'";
            var ds = db.QueryT(sql);
            //foreach (DataRow col in ds.Tables[0].Rows)           //set.Tables[0].Rows 找到指定表的所有行 0这里可以填表名
            //{
            //    Console.WriteLine(col[0].ToString());                 //col[0]这一行的索引是0单元格，也就是列，你只要在0这里填上你要输出的第几列就行了
            //}
            //var count = ds.Tables[0].Rows[0][0].ToString();
            var count = int.Parse(ds.Rows[0]["count"].ToString());

            Response res = new Response();
            //判断用户名与密码是否正确
            if (count != 0)
            {
                string sql2 = "SELECT * FROM GDMS_USER WHERE USER_ID = '" + user.username + "' AND PASS= '" + MD5Encrypt32(user.password) + "'";
                var ds2 = db.QueryT(sql2);
                var FAILED_LOGINS = int.Parse(ds2.Rows[0]["FAILED_LOGINS"].ToString());
                //判断账号锁定状态
                if (FAILED_LOGINS <= 5) {
                    res.code = 0;
                    res.msg = "登录成功";
                    var token = GetToken(user.username);
                    var USER_NAME = ds2.Rows[0]["USER_NAME"].ToString();
                    var USER_TYPE = ds2.Rows[0]["USER_TYPE"].ToString();
                    res.data = new Dictionary<string, object>
                    {
                        {"access_token", token },
                        {"userid", user.username },
                        {"username", USER_NAME },
                        {"role", USER_TYPE },
                    };

                    string sql3 = "UPDATE GDMS_USER SET FAILED_LOGINS = 0, LAST_LOGIN = SYSDATE, LAST_IP = '" + IPAddress + "' WHERE USER_ID = '" + user.username + "'";
                    db.ExecuteSql(sql3);
                }
                else
                {
                    res.code = 1;
                    res.msg = "登录错误次数太多，账号已锁定";
                }
            }
            else
            {
                string sql4 = "UPDATE GDMS_USER SET FAILED_LOGINS = FAILED_LOGINS + 1 where USER_ID = '" + user.username + "'";
                db.ExecuteSql(sql4);
                res.code = 1;
                res.msg = "用户名或密码错误";
            }

            var resJsonStr =  JsonConvert.SerializeObject(res);
            HttpResponseMessage resJson = new HttpResponseMessage {
                Content = new StringContent(resJsonStr, Encoding.GetEncoding("UTF-8"), "application/json")
            };
            return resJson;
        }

        //生成access_token
        private string GetToken(string username)
        {
            IDateTimeProvider provider = new UtcDateTimeProvider();
            var now = provider.GetNow();
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); // or use JwtValidator.UnixEpoch
            var secondsSinceEpoch = Math.Round((now - unixEpoch).TotalSeconds);

            var payload = new Dictionary<string, object>
            {
                {"name", username },
                //{"exp",secondsSinceEpoch+(20) }
                {"exp",secondsSinceEpoch+(3600 * 24 * 30) }     //超时时间，单位：秒
            };
            var secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
            
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            var token = encoder.Encode(payload, secret);

            return token;
        }

    }
}
