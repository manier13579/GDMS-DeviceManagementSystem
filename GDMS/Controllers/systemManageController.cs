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
    public class SystemManageController : ApiController
    {
        //systenList POST对象
        public class SystemAjax
        {
            public int page { get; set; }
            public int limit { get; set; }
        }
        //userList POST对象
        public class UserAjax
        {
            public int page { get; set; }
            public int limit { get; set; }
            public int systemId { get; set; }
        }
        //返回对象
        private class Response
        {
            public int code { get; set; }
            public string count { get; set; }
            public string msg { get; set; }
            public object data { get; set; }
        }

        //获取系统列表
        [ActionName("list")]
        public HttpResponseMessage List([FromBody] SystemAjax systemajax)
        {
            Db db = new Db();
            string sqlnp = "SELECT * FROM GDMS_SYSTEM order by NAME";
            int limit1 = (systemajax.page - 1) * systemajax.limit + 1;
            int limit2 = systemajax.page * systemajax.limit;
            string sql = "SELECT * FROM(SELECT p1.*,ROWNUM rn FROM(" + sqlnp + ")p1)WHERE rn BETWEEN " + limit1 + " AND " + limit2;
            var ds = db.QueryT(sql);
            Response res = new Response();
            ArrayList data = new ArrayList();
            foreach (DataRow col in ds.Rows)
            {
                Dictionary<string, string> dict = new Dictionary<string, string>
                {
                    { "NAME", col["NAME"].ToString() },
                    { "ID", col["ID"].ToString() },
                    { "REMARK", col["REMARK"].ToString() },
                    { "USER_ID", col["USER_ID"].ToString() },
                    { "EDIT_DATE", col["EDIT_DATE"].ToString() },
                };

                data.Add(dict);
            }

            //获取用户数量
            string sql2 = "select count(*) as count from GDMS_SYSTEM";
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

        //获取系统用户列表
        [ActionName("userList")]
        public HttpResponseMessage userList([FromBody] UserAjax userajax)
        {
            Db db = new Db();
            string sqlnp = "SELECT a.*,b.USER_NAME FROM GDMS_USER_SYSTEM a LEFT JOIN GDMS_USER b ON a.USER_ID = b.USER_ID WHERE a.SYSTEM_ID = "+userajax.systemId+" order by a.USER_ID";
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
                };

                data.Add(dict);
            }

            //获取用户数量
            string sql2 = "select count(*) as count from GDMS_USER_SYSTEM WHERE SYSTEM_ID = " + userajax.systemId;
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
