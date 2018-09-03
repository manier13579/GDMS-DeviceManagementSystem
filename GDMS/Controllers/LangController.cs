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

namespace GDMS.Controllers
{
    [RequestAuthorize]
    public class LangController : ApiController
    {
        //POST对象
        public class LangAjax
        {
            public string lang { get; set; }
            public string pageName { get; set; }
        }
        //返回对象
        private class Response
        {
            public int code { get; set; }
            public string msg { get; set; }
            public object data { get; set; }
        }

        //通过POST只能获取1个对象，因此POST多个数据需要使用类
        public HttpResponseMessage Lang([FromBody] LangAjax langajax)
        {
            Db db = new Db();
            string sql = "SELECT XU_HAO, WEN_ZI FROM GDMS_LANG WHERE YE_MIAN_MING = '"+ langajax.pageName + "' AND YU_ZHONG = '"+ langajax.lang +"' order by XU_HAO";
            var ds = db.QueryT(sql);
            Response res = new Response();
            Dictionary<string, string> data = new Dictionary<string, string>();
            foreach (DataRow col in ds.Rows)
            {
                data.Add(col["XU_HAO"].ToString(), col["WEN_ZI"].ToString()); 
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
