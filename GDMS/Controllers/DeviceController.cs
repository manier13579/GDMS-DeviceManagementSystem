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
            Response res = new Response();
            Dictionary<string, object> data = new Dictionary<string, object>();

            //查询系统select
            string sql1 = @"
                SELECT
                A.SYSTEM_ID AS SYSTEM_ID,
                B.NAME AS SYSTEM_NAME
                FROM
                GDMS_USER_SYSTEM A
                LEFT JOIN GDMS_SYSTEM B ON A.SYSTEM_ID = B.ID
                WHERE A.USER_ID = '" + deviceAjax.userId + "'";
            var ds1 = db.QueryT(sql1);
            Dictionary<string, string> dict1 = new Dictionary<string, string>();
            foreach (DataRow col in ds1.Rows)
            {
                dict1.Add(col["SYSTEM_ID"].ToString(), col["SYSTEM_NAME"].ToString());
            }
            data.Add("system", dict1);

            //查询地点select
            string sql2 = @"
                SELECT
                A.SYSTEM_ID AS SYSTEM_ID,
                A.ID AS SITE_ID,
                A.NAME AS SITE_NAME
                FROM
                GDMS_SITE A
                LEFT JOIN GDMS_USER_SYSTEM B ON A.SYSTEM_ID = B.SYSTEM_ID
                WHERE B.USER_ID = '" + deviceAjax.userId + "' ORDER BY A.SYSTEM_ID ASC ";
            var ds2 = db.QueryT(sql2);
            Dictionary<string, object> siteData = new Dictionary<string, object>();
            Dictionary<string, string> dict2 = new Dictionary<string, string>();
            var index = "0";
            foreach (DataRow col in ds2.Rows)
            {
                if (index == "0" || index == col["SYSTEM_ID"].ToString())
                {
                    dict2.Add(col["SITE_ID"].ToString(), col["SITE_NAME"].ToString());
                    index = col["SYSTEM_ID"].ToString();
                }
                else
                {
                    Dictionary<string, string> temp = new Dictionary<string, string>(dict2);
                    siteData.Add(index, temp);
                    dict2.Clear();
                    dict2.Add(col["SITE_ID"].ToString(), col["SITE_NAME"].ToString());
                    index = col["SYSTEM_ID"].ToString();
                }
            }
            siteData.Add(index, dict2);
            data.Add("site", siteData);

            //查询类型select
            string sql3 = @"
                SELECT
                A.SYSTEM_ID AS SYSTEM_ID,
                A.ID AS TYPE_ID,
                A.NAME AS TYPE_NAME
                FROM
                GDMS_TYPE A
                LEFT JOIN GDMS_USER_SYSTEM B ON A.SYSTEM_ID = B.SYSTEM_ID
                WHERE B.USER_ID = '" + deviceAjax.userId + "' ORDER BY A.SYSTEM_ID ASC ";
            var ds3 = db.QueryT(sql3);
            Dictionary<string, object> TypeData = new Dictionary<string, object>();
            Dictionary<string, string> dict3 = new Dictionary<string, string>();
            index = "0";
            foreach (DataRow col in ds3.Rows)
            {
                if (index == "0" || index == col["SYSTEM_ID"].ToString())
                {
                    dict3.Add(col["TYPE_ID"].ToString(), col["TYPE_NAME"].ToString());
                    index = col["SYSTEM_ID"].ToString();
                }
                else
                {
                    Dictionary<string, string> temp = new Dictionary<string, string>(dict3);
                    TypeData.Add(index, temp);
                    dict3.Clear();
                    dict3.Add(col["TYPE_ID"].ToString(), col["TYPE_NAME"].ToString());
                    index = col["SYSTEM_ID"].ToString();
                }
            }
            TypeData.Add(index, dict3);
            data.Add("type", TypeData);

            //查询式样select
            string sql4 = @"
                SELECT
                A.TYPE_ID AS TYPE_ID,
                A.ID AS STYLE_ID,
                A.NAME AS STYLE_NAME
                FROM
                GDMS_STYLE A
                LEFT JOIN GDMS_TYPE B ON A.TYPE_ID = B.ID
                LEFT JOIN GDMS_USER_SYSTEM C ON B.SYSTEM_ID = C.SYSTEM_ID
                WHERE C.USER_ID = '" + deviceAjax.userId + "' ORDER BY A.TYPE_ID ASC";
            var ds4 = db.QueryT(sql4);
            Dictionary<string, object> StyleData = new Dictionary<string, object>();
            Dictionary<string, string> dict4 = new Dictionary<string, string>();
            index = "0";
            foreach (DataRow col in ds4.Rows)
            {
                if (index == "0" || index == col["TYPE_ID"].ToString())
                {
                    dict4.Add(col["STYLE_ID"].ToString(), col["STYLE_NAME"].ToString());
                    index = col["TYPE_ID"].ToString();
                }
                else
                {
                    Dictionary<string, string> temp = new Dictionary<string, string>(dict4);
                    StyleData.Add(index, temp);
                    dict4.Clear();
                    dict4.Add(col["STYLE_ID"].ToString(), col["STYLE_NAME"].ToString());
                    index = col["TYPE_ID"].ToString();
                }
            }
            StyleData.Add(index, dict4);
            data.Add("style", StyleData);

            //查询位置select
            string sql5 = @"
                SELECT
                A.SITE_ID AS SITE_ID,
                A.ID AS STN_ID,
                A.NAME AS STN_NAME
                FROM
                GDMS_STN_MAIN A
                LEFT JOIN GDMS_SITE B ON A.SITE_ID = B.ID
                LEFT JOIN GDMS_USER_SYSTEM C ON B.SYSTEM_ID = C.SYSTEM_ID
                WHERE C.USER_ID = '" + deviceAjax.userId + "' ORDER BY A.SITE_ID ASC";
            var ds5 = db.QueryT(sql5);
            Dictionary<string, object> StnData = new Dictionary<string, object>();
            Dictionary<string, string> dict5 = new Dictionary<string, string>();
            index = "0";
            foreach (DataRow col in ds5.Rows)
            {
                if (index == "0" || index == col["SITE_ID"].ToString())
                {
                    dict5.Add(col["SIN_ID"].ToString(), col["STN_NAME"].ToString());
                    index = col["SITE_ID"].ToString();
                }
                else
                {
                    Dictionary<string, string> temp = new Dictionary<string, string>(dict5);
                    StnData.Add(index, temp);
                    dict5.Clear();
                    dict5.Add(col["SIN_ID"].ToString(), col["STN_NAME"].ToString());
                    index = col["SITE_ID"].ToString();
                }
            }
            StnData.Add(index, dict5);
            data.Add("site", StnData);

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
