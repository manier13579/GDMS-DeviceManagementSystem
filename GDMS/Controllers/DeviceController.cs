using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using GDMS.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Oracle.DataAccess.Client;

namespace GDMS.Controllers
{
    [RequestAuthorize]
    public class DeviceController : ApiController
    {
        //返回对象
        private class Response
        {
            public int code { get; set; }
            public string count { get; set; }
            public string msg { get; set; }
            public object data { get; set; }
        }

        //获取设备列表 - POST对象
        public class DeviceAjax
        {
            public string userId { get; set; }
            public string devId { get; set; }
            public string systemId { get; set; }
            public string siteId { get; set; }
            public string typeId { get; set; }
            public string stnId { get; set; }
            public string styleId { get; set; }
            public string projectId { get; set; }
            public string keyword { get; set; }

            public int page { get; set; }
            public int limit { get; set; }
        }

        //获取设备列表
        [ActionName("list")]
        public HttpResponseMessage DeviceList([FromBody] DeviceAjax deviceAjax)
        {
            Db db = new Db();
            string where = "";
            if (deviceAjax.systemId != null) { where = where + " AND G.SYSTEM_ID = '" + deviceAjax.systemId + "'"; }
            if (deviceAjax.devId != null) { where = where + " AND A.ID = '" + deviceAjax.devId + "'"; }
            if (deviceAjax.siteId != null) { where = where + " AND B.SITE_ID = '" + deviceAjax.siteId + "'"; }
            if (deviceAjax.typeId != null) { where = where + " AND D.TYPE_ID = '" + deviceAjax.typeId + "'"; }
            if (deviceAjax.stnId != null) { where = where + " AND A.STN_ID = '" + deviceAjax.stnId + "'"; }
            if (deviceAjax.styleId != null) { where = where + " AND A.STYLE_ID = '" + deviceAjax.styleId + "'"; }
            if (deviceAjax.projectId != null) { where = where + " AND A.PROJECT_ID = '" + deviceAjax.projectId + "'"; }
            if (deviceAjax.keyword != null && deviceAjax.keyword.Length != 0) {
                //模糊搜索项：序列号、备注
                where = where + "AND ( "+
                    "A.SN LIKE '%" + deviceAjax.keyword + "%' or " +
                    "A.REMARK LIKE '%" + deviceAjax.keyword + "%')";
            }

            string sqlnp = @"
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
                LEFT JOIN GDMS_STN_MAIN B ON A.STN_ID = B.ID
                LEFT JOIN GDMS_SITE C ON B.SITE_ID = C.ID
                LEFT JOIN GDMS_STYLE D ON A.STYLE_ID = D.ID
                LEFT JOIN GDMS_PROJECT E ON A.PROJECT_ID = E.ID
                LEFT JOIN GDMS_SYSTEM F ON B.SITE_ID = F.ID
                LEFT JOIN GDMS_TYPE G ON D.TYPE_ID = G.ID
                LEFT JOIN GDMS_SYSTEM H ON G.SYSTEM_ID = H.ID
                WHERE G.SYSTEM_ID IN (SELECT SYSTEM_ID FROM GDMS_USER_SYSTEM WHERE USER_ID = '" + deviceAjax.userId + "') " + where +
                "ORDER BY G.ID ASC,D.ID ASC,A.DELIVERY_DATE DESC,A.SN ASC";

            int limit1 = (deviceAjax.page - 1) * deviceAjax.limit + 1;
            int limit2 = deviceAjax.page * deviceAjax.limit;
            string sql = "SELECT * FROM(SELECT p1.*,ROWNUM rn FROM(" + sqlnp + ")p1)WHERE rn BETWEEN " + limit1 + " AND " + limit2;

            var ds = db.QueryT(sql);
            Response res = new Response();
            ArrayList data = new ArrayList();
            foreach (DataRow col in ds.Rows)
            {
                var status = "";
                if (col["STATUS"].ToString() == "0") { status = "备件"; }
                else if (col["STATUS"].ToString() == "1") { status = "在用"; }
                else if (col["STATUS"].ToString() == "2") { status = "故障"; }
                else if (col["STATUS"].ToString() == "3") { status = "维修"; }
                Dictionary<string, string> dict = new Dictionary<string, string>
                {
                    { "TYPE_NAME", col["TYPE_NAME"].ToString() },
                    { "STN_NAME", col["STN_NAME"].ToString() },
                    { "SITE_NAME", col["SITE_NAME"].ToString() },
                    { "STYLE_NAME", col["STYLE_NAME"].ToString() },
                    { "PROJECT_NAME", col["PROJECT_NAME"].ToString() },

                    { "COUNT", col["COUNT"].ToString() },
                    { "SN", col["SN"].ToString() },
                    { "DELIVERY_DATE", col["DELIVERY_DATE"].ToString() },
                    { "STATUS", status },
                    { "STATUS_ID", col["STATUS"].ToString() },
                    { "REMARK", col["REMARK"].ToString() },

                    { "DEV_ID", col["DEV_ID"].ToString() },
                    { "STN_ID", col["STN_ID"].ToString() },
                    { "SITE_ID", col["SITE_ID"].ToString() },
                    { "STYLE_ID", col["STYLE_ID"].ToString() },
                    { "PROJECT_ID", col["PROJECT_ID"].ToString() },
                    { "TYPE_ID", col["TYPE_ID"].ToString() },
                    { "SYSTEM_ID", col["SYSTEM_ID"].ToString() }
                };

                data.Add(dict);
            }

            string sql2 = @"
                SELECT
                COUNT(*) AS COUNT
                FROM
                GDMS_DEV_MAIN A
                LEFT JOIN GDMS_STN_MAIN B ON A.STN_ID = B.ID
                LEFT JOIN GDMS_SITE C ON B.SITE_ID = C.ID
                LEFT JOIN GDMS_STYLE D ON A.STYLE_ID = D.ID
                LEFT JOIN GDMS_PROJECT E ON A.PROJECT_ID = E.ID
                LEFT JOIN GDMS_SYSTEM F ON B.SITE_ID = F.ID
                LEFT JOIN GDMS_TYPE G ON D.TYPE_ID = G.ID
                LEFT JOIN GDMS_SYSTEM H ON G.SYSTEM_ID = H.ID
                WHERE G.SYSTEM_ID IN (SELECT SYSTEM_ID FROM GDMS_USER_SYSTEM WHERE USER_ID = '" + deviceAjax.userId + "') " + where;
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

        //获取select下拉菜单选项
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
                    dict5.Add(col["STN_ID"].ToString(), col["STN_NAME"].ToString());
                    index = col["SITE_ID"].ToString();
                }
                else
                {
                    Dictionary<string, string> temp = new Dictionary<string, string>(dict5);
                    StnData.Add(index, temp);
                    dict5.Clear();
                    dict5.Add(col["STN_ID"].ToString(), col["STN_NAME"].ToString());
                    index = col["SITE_ID"].ToString();
                }
            }
            StnData.Add(index, dict5);
            data.Add("stn", StnData);

            //查询项目select
            string sql6 = @"
                SELECT DISTINCT
                A.ID AS PROJECT_ID,
                A.NAME AS PROJECT_NAME
                FROM
                GDMS_PROJECT A
                LEFT JOIN GDMS_DEV_MAIN B ON A.ID = B.PROJECT_ID
                LEFT JOIN GDMS_STYLE C ON B.STYLE_ID = C.ID
                LEFT JOIN GDMS_TYPE D ON C.TYPE_ID = D.ID
                LEFT JOIN GDMS_USER_SYSTEM E ON D.SYSTEM_ID = E.SYSTEM_ID
                WHERE E.USER_ID = '" + deviceAjax.userId + "' ORDER BY A.NAME ASC";
            var ds6 = db.QueryT(sql6);
            Dictionary<string, string> dict6 = new Dictionary<string, string>();
            index = "0";
            foreach (DataRow col in ds6.Rows)
            {
                dict6.Add(col["PROJECT_ID"].ToString(), col["PROJECT_NAME"].ToString());
            }
            data.Add("project", dict6);

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
        public HttpResponseMessage DeviceDel([FromBody] String ajaxData)
        {
            Db db = new Db();
            JArray idArr = (JArray)JsonConvert.DeserializeObject(ajaxData);
            string sqlin = "";
            foreach (var devId in idArr)
            {
                sqlin = sqlin + devId + ",";
            }
            sqlin = sqlin.Substring(0, sqlin.Length - 1);
            string sql = "DELETE FROM GDMS_DEV_MAIN WHERE ID IN ("+ sqlin + ")";

            var rows = db.ExecuteSql(sql);
            Response res = new Response();

            res.code = 0;
            res.msg = "操作成功，删除了" + rows + "个设备";
            res.data = null;
            
            var resJsonStr = JsonConvert.SerializeObject(res);
            HttpResponseMessage resJson = new HttpResponseMessage
            {
                Content = new StringContent(resJsonStr, Encoding.GetEncoding("UTF-8"), "application/json")
            };
            return resJson;
        }


        //添加设备
        [ActionName("add")]
        public HttpResponseMessage DeviceAdd([FromBody] String ajaxData)
        {
            JObject formData = (JObject)JsonConvert.DeserializeObject(ajaxData);
            Db db = new Db();
            string sql = @"SELECT DEVICE_ADD(
                '" + (String)formData["stn"] + @"', 
                '" + (String)formData["style"] + @"', 
                '" + (String)formData["project"] + @"',
                '" + (String)formData["sn"] + @"', 
                to_date('" + (String)formData["delivery"] + @"', 'yyyy-mm-dd'),
                '" + (String)formData["status"] + @"',
                '" + (String)formData["remark"] + @"',
                '" + (String)formData["userId"] + @"'
                ) AS DEVID FROM DUAL";
            var ds = db.QueryT(sql);    //执行设备添加ORACLE函数，返回主键：devId
            var devId = "";
            foreach (DataRow col in ds.Rows)
            {
                devId = col["DEVID"].ToString();
            }

            ArrayList sql2 = new ArrayList();
            foreach (JProperty item in formData.Properties())   //遍历更多信息，写入sql2数组列表
            {
                if (item.Name != "stn" && item.Name != "style" && item.Name != "project" && item.Name != "sn" && item.Name != "system" && item.Name != "site"
                    && item.Name != "delivery" && item.Name != "status" && item.Name != "remark" && item.Name != "userId" && item.Name != "type")
                {
                    sql2.Add("INSERT INTO GDMS_DEV_MORE (DEV_ID,ITEM,VALUE) VALUES ('"+ devId + "','"+ item.Name + "','"+ (String)item.Value + "')");
                }
            }
            db.ExecuteSqlTran(sql2);    //执行多条更多信息插入

            Response res = new Response();
            res.code = 0;
            res.msg = (string)devId;
            res.data = null;

            var resJsonStr = JsonConvert.SerializeObject(res);
            HttpResponseMessage resJson = new HttpResponseMessage
            {
                Content = new StringContent(resJsonStr, Encoding.GetEncoding("UTF-8"), "application/json")
            };
            return resJson;
        }

        //修改设备
        [ActionName("edit")]
        public HttpResponseMessage DeviceEdit([FromBody] String ajaxData)
        {
            JObject formData = (JObject)JsonConvert.DeserializeObject(ajaxData);
            Db db = new Db();
            string sql = @"UPDATE GDMS_DEV_MAIN SET 
                STN_ID = '" + (String)formData["stn"] + @"',
                STYLE_ID = '" + (String)formData["style"] + @"',
                PROJECT_ID = '" + (String)formData["project"] + @"',
                SN = '" + (String)formData["sn"] + @"',
                DELIVERY_DATE = to_date('" + (String)formData["delivery"] + @"', 'yyyy-mm-dd'),
                STATUS = '" + (String)formData["status"] + @"',
                REMARK = '" + (String)formData["remark"] + @"',
                USER_ID = '" + (String)formData["userId"] + @"',
                EDIT_DATE = SYSDATE
                WHERE ID = '" + (String)formData["devId"] + "'";

            var rows = db.ExecuteSql(sql);

            ArrayList sql2 = new ArrayList();
            sql2.Add("DELETE FROM GDMS_DEV_MORE WHERE DEV_ID = '"+ (String)formData["devId"] + "'");
            foreach (JProperty item in formData.Properties())   //遍历更多信息，写入sql2数组列表
            {
                if (item.Name != "stn" && item.Name != "style" && item.Name != "project" && item.Name != "sn" && item.Name != "system" && item.Name != "site"
                    && item.Name != "delivery" && item.Name != "status" && item.Name != "remark" && item.Name != "userId" && item.Name != "type" && item.Name != "devId")
                {
                    sql2.Add("INSERT INTO GDMS_DEV_MORE (DEV_ID,ITEM,VALUE) VALUES ('" + (String)formData["devId"] + "','" + item.Name + "','" + (String)item.Value + "')");
                }
            }
            db.ExecuteSqlTran(sql2);    //执行多条更多信息插入

            Response res = new Response();
            res.code = 0;
            res.msg = "更新成功";
            res.data = null;

            var resJsonStr = JsonConvert.SerializeObject(res);
            HttpResponseMessage resJson = new HttpResponseMessage
            {
                Content = new StringContent(resJsonStr, Encoding.GetEncoding("UTF-8"), "application/json")
            };
            return resJson;
        }

        //获取更多信息 - POST对象
        public class DeviceMoreAjax
        {
            public string devId { get; set; }
        }

        //获取更多信息
        [ActionName("more")]
        public HttpResponseMessage DeviceMore([FromBody] DeviceMoreAjax ajaxData)
        {
            Db db = new Db();
            string sql = "SELECT ITEM,VALUE FROM GDMS_DEV_MORE WHERE DEV_ID = '"+ ajaxData.devId + "'";

            var ds = db.QueryT(sql);
            Response res = new Response();
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (DataRow col in ds.Rows)
            {
                dict.Add(col["ITEM"].ToString(), col["VALUE"].ToString());
            }

            res.code = 0;
            res.msg = "";
            res.data = dict;

            var resJsonStr = JsonConvert.SerializeObject(res);
            HttpResponseMessage resJson = new HttpResponseMessage
            {
                Content = new StringContent(resJsonStr, Encoding.GetEncoding("UTF-8"), "application/json")
            };
            return resJson;
        }
    }
}
