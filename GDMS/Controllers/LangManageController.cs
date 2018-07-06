using GDMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using System.Text;
using System.Data;
using System.Collections;

namespace GDMS.Controllers
{
    [RequestAuthorize]
    public class LangManageController : ApiController
    {
        //POST对象
        public class LangAjax
        {
            public string keyword { get; set; }
            public string pageId { get; set; }
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

        //获取语言列表
        [ActionName("list")]
        public HttpResponseMessage Lang([FromBody] LangAjax langajax)
        {
            Db db = new Db();
            string where = "";
            if (langajax.keyword != null && langajax.keyword.Length != 0) { where = where + "AND ( XU_HAO LIKE '" + langajax.keyword + "' or WEN_ZI LIKE '" + langajax.keyword + "')"; }
            if (langajax.pageId != null) { where = where + "AND YE_MIAN_MING = '" + langajax.pageId + "'"; }
            string sqlnp = "select * from GDMS_LANG where 1=1 "+where+" order by YE_MIAN_MING,XU_HAO,YU_ZHONG ";
            int limit1 = (langajax.page - 1) * langajax.limit + 1;
            int limit2 = langajax.page * langajax.limit;
            string sql = "SELECT * FROM(SELECT p1.*,ROWNUM rn FROM(" + sqlnp + ")p1)WHERE rn BETWEEN "+ limit1 + " AND "+ limit2;
            var ds = db.QueryT(sql);
            Response res = new Response();
            ArrayList data = new ArrayList();
            foreach (DataRow col in ds.Rows)
            {
                Dictionary<string, string> dict = new Dictionary<string, string>
                {
                    { "YE_MIAN_MING", col["YE_MIAN_MING"].ToString() },
                    { "XU_HAO", col["XU_HAO"].ToString() },
                    { "YU_ZHONG", col["YU_ZHONG"].ToString() },
                    { "WEN_ZI", col["WEN_ZI"].ToString() }
                };

                data.Add(dict);
            }

            string sql2 = "select count(*) as count from GDMS_LANG where 1=1 " + where;
            var ds2 = db.QueryT(sql2);
            foreach (DataRow col in ds2.Rows)
            {
                res.count = col["count"].ToString();
            }

            res.code = 0;
            res.msg = sql;
            res.data = data;

            var resJsonStr = JsonConvert.SerializeObject(res);
            HttpResponseMessage resJson = new HttpResponseMessage
            {
                Content = new StringContent(resJsonStr, Encoding.GetEncoding("UTF-8"), "application/json")
            };
            return resJson;
        }
        //获取select
        [ActionName("select")]
        public HttpResponseMessage DeviceSelect()
        {
            Db db = new Db();
            Response res = new Response();
            ArrayList data = new ArrayList();

            //查询系统select
            string sql = "select DISTINCT YE_MIAN_MING as PAGE_NAME from GDMS_LANG";
            var ds = db.QueryT(sql);
            foreach (DataRow col in ds.Rows)
            {
                data.Add(col["PAGE_NAME"].ToString());
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
