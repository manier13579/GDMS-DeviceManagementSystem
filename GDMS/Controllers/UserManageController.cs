using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using GDMS.Models;
using Newtonsoft.Json;

namespace GDMS.Controllers
{
    [RequestAuthorize]
    public class UserManageController : ApiController
    {
        //POST对象
        public class UserAjax
        {
            public int page { get; set; }
            public int limit { get; set; }
        }
        //返回对象
        private class Response
        {
            public int code { get; set; }
            public string count { get; set; }
            public string msg { get; set; }
            public object data { get; set; }
        }

        //获取用户列表
        [ActionName("list")]
        public HttpResponseMessage Lang([FromBody] UserAjax userajax)
        {
            Db db = new Db();
            string sqlnp = "SELECT * FROM GDMS_USER order by USER_ID";
            int limit1 = (userajax.page - 1) * userajax.limit + 1;
            int limit2 = userajax.page * userajax.limit;
            string sql = "SELECT * FROM(SELECT p1.*,ROWNUM rn FROM(" + sqlnp + ")p1)WHERE rn BETWEEN " + limit1 + " AND " + limit2;
            var ds = db.QueryT(sql);
            Response res = new Response();
            ArrayList data = new ArrayList();
            foreach (DataRow col in ds.Rows)
            {
                Dictionary<string, string> dict = new Dictionary<string, string>
                {
                    { "USER_ID", col["USER_ID"].ToString() },
                    { "USER_NAME", col["USER_NAME"].ToString() },
                    { "USER_TYPE", col["USER_TYPE"].ToString() },
                    { "FAILED_LOGINS", col["FAILED_LOGINS"].ToString() },
                    { "LAST_IP", col["LAST_IP"].ToString() },
                    { "EMAIL", col["EMAIL"].ToString() },
                    { "LAST_LOGIN", col["LAST_LOGIN"].ToString() },
                    { "JOIN_DATE", col["JOIN_DATE"].ToString() },
                };

                data.Add(dict);
            }

            //获取用户数量
            string sql2 = "select count(*) as count from GDMS_USER";
            var ds2 = db.QueryT(sql2);
            foreach (DataRow col in ds2.Rows)
            {
                res.count = col["count"].ToString();
            }

            res.code = 0;
            res.msg = "";
            res.data = data;

            var resJsonStr = JsonConvert.SerializeObject(res);
            HttpResponseMessage resJson = new HttpResponseMessage
            {
                Content = new StringContent(resJsonStr, Encoding.GetEncoding("UTF-8"), "application/json")
            };
            return resJson;
        }
    }
}
