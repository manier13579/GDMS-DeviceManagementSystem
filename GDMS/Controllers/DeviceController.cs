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

        //POST对象 (通过POST只能获取1个对象，因此POST多个数据需要使用类)
        public class DeviceAjax
        {
            public string userId { get; set; }
            public int devId { get; set; }
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

        //获取设备列表
        [ActionName("list")]
        public HttpResponseMessage DeviceList([FromBody] DeviceAjax deviceAjax)
        {
            Db db = new Db();
            string sql = @"
                SELECT
                A.COUNT,
                A.SN,
                TO_CHAR(A.DELIVERY_DATE,'YYYY-MM-DD') AS DELIVERY_DATE,
                A.STATUS,
                A.REMARK,
                B.NAME AS STN_NAME,
                C.NAME AS SITE_NAME,
                D.NAME AS STYLE_NAME,
                E.NAME AS PROJECT_NAME,
                G.NAME AS TYPE_NAME,
                A.ID AS DEV_ID,
                B.ID AS STN_ID,
                C.ID AS SITE_ID,
                D.ID AS STYLE_ID, 
                E.ID AS PROJECT_ID,
                G.ID AS TYPE_ID,
                H.ID AS SYSTEM_ID
                FROM
                GDMS_DEV_MAIN A
                INNER JOIN GDMS_STN_MAIN B ON A.STN_ID = B.ID
                LEFT JOIN GDMS_SITE C ON B.SITE_ID = C.ID
                LEFT JOIN GDMS_STYLE D ON A.STYLE_ID = D.ID
                LEFT JOIN GDMS_PROJECT E ON A.PROJECT_ID = E.ID
                LEFT JOIN GDMS_SYSTEM F ON B.SITE_ID = F.ID
                LEFT JOIN GDMS_TYPE G ON D.TYPE_ID = G.ID
                LEFT JOIN GDMS_SYSTEM H ON G.SYSTEM_ID = H.ID
                WHERE G.SYSTEM_ID IN (SELECT SYSTEM_ID FROM GDMS_USER_SYSTEM WHERE USER_ID = '" + deviceAjax.userId + "')";

            
            var ds = db.QueryT(sql);
            Response res = new Response();
            ArrayList data = new ArrayList();
            foreach (DataRow col in ds.Rows)
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("TYPE_NAME", col["TYPE_NAME"].ToString());
                dict.Add("STN_NAME", col["STN_NAME"].ToString());
                dict.Add("SITE_NAME", col["SITE_NAME"].ToString());
                dict.Add("STYLE_NAME", col["STYLE_NAME"].ToString());
                dict.Add("PROJECT_NAME", col["PROJECT_NAME"].ToString());

                dict.Add("COUNT", col["COUNT"].ToString());
                dict.Add("SN", col["SN"].ToString());
                dict.Add("DELIVERY_DATE", col["DELIVERY_DATE"].ToString());
                dict.Add("STATUS", col["STATUS"].ToString());
                dict.Add("REMARK", col["REMARK"].ToString());
                
                dict.Add("DEV_ID", col["DEV_ID"].ToString());
                dict.Add("STN_ID", col["STN_ID"].ToString());
                dict.Add("SITE_ID", col["SITE_ID"].ToString());
                dict.Add("STYLE_ID", col["STYLE_ID"].ToString());
                dict.Add("PROJECT_ID", col["PROJECT_ID"].ToString());
                dict.Add("TYPE_ID", col["TYPE_ID"].ToString());

                data.Add(dict);
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

        //获取select
        [ActionName("select")]
        public HttpResponseMessage DeviceSelect([FromBody] DeviceAjax deviceAjax)
        {
            Db db = new Db();
            string sql1 = @"
                SELECT
                A.SYSTEM_ID AS SYSTEM_ID,
                B.NAME AS SYSTEM_NAME
                FROM
                GDMS_USER_SYSTEM A
                LEFT JOIN GDMS_SYSTEM B ON A.SYSTEM_ID = B.ID
                WHERE A.USER_ID = '" + deviceAjax.userId + "'";


            var ds = db.QueryT(sql1);
            Response res = new Response();
            ArrayList systemData = new ArrayList();
            Dictionary<string, object> systemDataDict = new Dictionary<string, object>();
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (DataRow col in ds.Rows)
            {
                //Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add(col["SYSTEM_ID"].ToString(), col["SYSTEM_NAME"].ToString());

                //systemData.Add(dict);
            }
            systemDataDict.Add("SYSTEM", dict);

            res.code = 0;
            res.msg = "";
            res.data = systemDataDict;

            var resJsonStr = JsonConvert.SerializeObject(res);
            HttpResponseMessage resJson = new HttpResponseMessage
            {
                Content = new StringContent(resJsonStr, Encoding.GetEncoding("UTF-8"), "application/json")
            };
            return resJson;
        }

        //删除设备
        [ActionName("del")]
        public HttpResponseMessage DeviceDel([FromBody] DeviceAjax deviceAjax)
        {
            Db db = new Db();
            string sql = @"";

            var ds = db.QueryT(sql);
            Response res = new Response();

            res.code = 0;
            res.msg = "";
            res.data = null;

            var resJsonStr = JsonConvert.SerializeObject(res);
            HttpResponseMessage resJson = new HttpResponseMessage
            {
                Content = new StringContent(resJsonStr, Encoding.GetEncoding("UTF-8"), "application/json")
            };
            return resJson;
        }

    }
}
