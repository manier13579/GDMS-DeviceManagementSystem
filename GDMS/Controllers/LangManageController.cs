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
        }
        //返回对象
        private class Response
        {
            public int code { get; set; }
            public string count { get; set; }
            public string msg { get; set; }
            public object data { get; set; }
        }
        public HttpResponseMessage Lang([FromBody] LangAjax langajax)
        {
            Db db = new Db();
            string where = "";
            if (langajax.keyword != null) { where = where + "AND ( XU_HAO LIKE '" + langajax.keyword + "' or WEN_ZI LIKE '" + langajax.keyword + "')"; }
            if (langajax.pageId != null) { where = where + "AND YE_MIAN_MING = '" + langajax.keyword + "'"; }
            string sql = "select * from GDMS_LANG where 1=1 "+where+" order by YE_MIAN_MING,XU_HAO,YU_ZHONG";
            var ds = db.QueryT(sql);
            Response res = new Response();
            ArrayList data = new ArrayList();
            foreach (DataRow col in ds.Rows)
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("YE_MIAN_MING", col["YE_MIAN_MING"].ToString());
                dict.Add("XU_HAO", col["XU_HAO"].ToString());
                dict.Add("YU_ZHONG", col["YU_ZHONG"].ToString());
                dict.Add("WEN_ZI", col["WEN_ZI"].ToString());

                data.Add(dict);
            }

            string sql2 = "select count(*) as count from GDMS_LANG where 1=1 " + where;
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
