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
    public class DeviceController : ApiController
    {
        //POST对象
        public class DeviceAjax
        {
            public int systemId { get; set; }
            public int siteId { get; set; }
            public int typeId { get; set; }
            public int stnId { get; set; }
            public int styleId { get; set; }
            public int projectId { get; set; }
        }
        //返回对象
        private class Response
        {
            public int code { get; set; }
            public string msg { get; set; }
            public object data { get; set; }
        }

        //通过POST只能获取1个对象，因此POST多个数据需要使用类
        public HttpResponseMessage Device([FromBody] DeviceAjax deviceAjax)
        {
            Db db = new Db();
            string sql = @"
                SELECT
                A.NAME,
                A.COUNT,
                A.SN,
                A.DELIVERY_DATE,
                A.STATUS,
                A.REMARK,
                B.NAME AS STN_NAME,
                C.NAME AS SITE_NAME,
                D.NAME AS STYLE_NAME,
                E.NAME AS PROJECT_NAME
                FROM
                BING.GDMS_DEV_MAIN A
                INNER JOIN BING.GDMS_STN_MAIN B ON A.STN_ID = B.ID
                LEFT JOIN BING.GDMS_SITE C ON B.SITE_ID = C.ID
                LEFT JOIN BING.GDMS_STYLE D ON A.STYLE_ID = D.ID
                LEFT JOIN BING.GDMS_PROJECT E ON A.PROJECT_ID = E.ID
                LEFT JOIN BING.GDMS_SYSTEM f ON B.SITE_ID = f.ID
                where f.id = " + deviceAjax.systemId;
            var ds = db.QueryT(sql);
            int i = 0;
            Response res = new Response();
            ArrayList data = new ArrayList();
            foreach (DataRow col in ds.Rows)
            {
                List<string> list = new List<string>();
                data.Add(col["XU_HAO"].ToString());
                i++;
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
