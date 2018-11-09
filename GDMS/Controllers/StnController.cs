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
using Newtonsoft.Json.Linq;

namespace GDMS.Controllers
{
    [RequestAuthorize]
    public class StnController : ApiController
    {

        //POST对象 (通过POST只能获取1个对象，因此POST多个数据需要使用类)
        public class StnAjax
        {
            public string userId { get; set; }
            public string systemId { get; set; }
            public string siteId { get; set; }
            public int page { get; set; }
            public int limit { get; set; }
            public string keyword { get; set; }
            public string filterStatus { get; set; }
        }
        //返回对象
        private class Response
        {
            public int code { get; set; }
            public string count { get; set; }
            public string msg { get; set; }
            public object data { get; set; }
        }

        //获取设备列表
        [ActionName("list")]
        public HttpResponseMessage StnList([FromBody] StnAjax stnajax)
        {
            Db db = new Db();
            string where = "";
            if (stnajax.systemId != null) { where = where + " AND B.SYSTEM_ID = '" + stnajax.systemId + "'"; }
            if (stnajax.siteId != null) { where = where + " AND A.SITE_ID = '" + stnajax.siteId + "'"; }
            if (stnajax.keyword != null && stnajax.keyword.Length != 0) { where = where + "AND ( A.NAME LIKE '" + stnajax.keyword + "' or A.DETAIL LIKE '" + stnajax.keyword + "' or A.REMARK LIKE '" + stnajax.keyword + "')"; }
            if (stnajax.filterStatus != null && stnajax.filterStatus.Length != 0){   //筛选状态
                where = where + " AND A.STATUS = '" + stnajax.filterStatus + "'";
            }

            string sqlnp = @"
                SELECT
                A.ID AS STN_ID,
                A.NAME AS STN_NAME,
                A.DETAIL,
                A.REMARK,
                A.STATUS,
                A.USER_ID,
                A.EDIT_DATE,
                B.ID AS SITE_ID,
                B.NAME AS SITE_NAME,
                C.ID AS SYSTEM_ID,
                C.NAME AS SYSTEM_NAME
                FROM
                GDMS_STN_MAIN A
                LEFT JOIN GDMS_SITE B ON A.SITE_ID = B.ID
                LEFT JOIN GDMS_SYSTEM C ON B.SYSTEM_ID = C.ID
                WHERE B.SYSTEM_ID IN (SELECT SYSTEM_ID FROM GDMS_USER_SYSTEM WHERE USER_ID = '" + stnajax.userId + "') " + where;

            int limit1 = (stnajax.page - 1) * stnajax.limit + 1;
            int limit2 = stnajax.page * stnajax.limit;
            string sql = "SELECT * FROM(SELECT p1.*,ROWNUM rn FROM(" + sqlnp + ")p1)WHERE rn BETWEEN " + limit1 + " AND " + limit2;
            var ds = db.QueryT(sql);
            Response res = new Response();
            ArrayList data = new ArrayList();
            foreach (DataRow col in ds.Rows)
            {
                var status = "";
                if (col["STATUS"].ToString() == "0") { status = "未使用"; }
                else if (col["STATUS"].ToString() == "1") { status = "使用中"; }
                Dictionary<string, string> dict = new Dictionary<string, string>
                {
                    { "STN_ID", col["STN_ID"].ToString() },
                    { "STN_NAME", col["STN_NAME"].ToString() },
                    { "DETAIL", col["DETAIL"].ToString() },
                    { "REMARK", col["REMARK"].ToString() },
                    { "STATUS", status },
                    { "USER_ID", col["USER_ID"].ToString() },
                    { "EDIT_DATE", col["EDIT_DATE"].ToString() },

                    { "SITE_ID", col["SITE_ID"].ToString() },
                    { "SITE_NAME", col["SITE_NAME"].ToString() },

                    { "SYSTEM_ID", col["SYSTEM_ID"].ToString() },
                    { "SYSTEM_NAME", col["SYSTEM_NAME"].ToString() },
                };

                data.Add(dict);
            }

            string sql2 = @"
                SELECT
                COUNT(*) AS COUNT
                FROM
                GDMS_STN_MAIN A
                LEFT JOIN GDMS_SITE B ON A.SITE_ID = B.ID
                LEFT JOIN GDMS_SYSTEM C ON B.SYSTEM_ID = C.ID
                WHERE B.SYSTEM_ID IN (SELECT SYSTEM_ID FROM GDMS_USER_SYSTEM WHERE USER_ID = '" + stnajax.userId + "') " + where;
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

        //获取select
        [ActionName("select")]
        public HttpResponseMessage StnSelect([FromBody] StnAjax stnajax)
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
                WHERE A.USER_ID = '" + stnajax.userId + "'";
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
                WHERE B.USER_ID = '" + stnajax.userId + "' ORDER BY A.SYSTEM_ID ASC ";
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
        public HttpResponseMessage StnDel([FromBody] String ajaxData)
        {
            Db db = new Db();
            JArray idArr = (JArray)JsonConvert.DeserializeObject(ajaxData);
            string sqlin = "";
            foreach (var stnId in idArr)
            {
                sqlin = sqlin + stnId + ",";
            }
            sqlin = sqlin.Substring(0, sqlin.Length - 1);
            string sql = "DELETE FROM GDMS_STN_MAIN WHERE ID IN (" + sqlin + ")";

            var rows = db.ExecuteSql(sql);
            Response res = new Response();

            res.code = 0;
            res.msg = "操作成功，删除了" + rows + "个地点";
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
        public HttpResponseMessage StnAdd([FromBody] String ajaxData)
        {
            JObject formData = (JObject)JsonConvert.DeserializeObject(ajaxData);
            Db db = new Db();
            string sql = @"SELECT STN_ADD(
                '" + (String)formData["site"] + @"', 
                '" + (String)formData["name"] + @"', 
                '" + (String)formData["detail"] + @"',
                '" + (String)formData["status"] + @"',
                '" + (String)formData["remark"] + @"',
                '" + (String)formData["userId"] + @"'
                ) AS STNID FROM DUAL";
            var ds = db.QueryT(sql);    //执行设备添加ORACLE函数，返回主键：devId
            var stnId = "";
            foreach (DataRow col in ds.Rows)
            {
                stnId = col["STNID"].ToString();
            }

            ArrayList sql2 = new ArrayList();
            foreach (JProperty item in formData.Properties())   //遍历更多信息，写入sql2数组列表
            {
                if (item.Name != "site" && item.Name != "name" && item.Name != "detail" && item.Name != "system" 
                    && item.Name != "status" && item.Name != "remark" && item.Name != "userId")
                {
                    sql2.Add("INSERT INTO GDMS_STN_MORE (STN_ID,ITEM,VALUE) VALUES ('" + stnId + "','" + item.Name + "','" + (String)item.Value + "')");
                }
            }
            db.ExecuteSqlTran(sql2);    //执行多条更多信息插入

            Response res = new Response();
            res.code = 0;
            res.msg = (string)stnId;
            res.data = null;

            var resJsonStr = JsonConvert.SerializeObject(res);
            HttpResponseMessage resJson = new HttpResponseMessage
            {
                Content = new StringContent(resJsonStr, Encoding.GetEncoding("UTF-8"), "application/json")
            };
            return resJson;
        }

        //修改位置
        [ActionName("edit")]
        public HttpResponseMessage StnEdit([FromBody] String ajaxData)
        {
            JObject formData = (JObject)JsonConvert.DeserializeObject(ajaxData);
            Db db = new Db();
            string sql = @"UPDATE GDMS_STN_MAIN SET 
                SITE_ID = '" + (String)formData["site"] + @"',
                NAME = '" + (String)formData["name"] + @"',
                DETAIL = '" + (String)formData["detail"] + @"',
                STATUS = '" + (String)formData["status"] + @"',
                REMARK = '" + (String)formData["remark"] + @"',
                USER_ID = '" + (String)formData["userId"] + @"',
                EDIT_DATE = SYSDATE
                WHERE ID = '" + (String)formData["stnId"] + "'";

            var rows = db.ExecuteSql(sql);

            ArrayList sql2 = new ArrayList();
            sql2.Add("DELETE FROM GDMS_STN_MORE WHERE STN_ID = '" + (String)formData["stnId"] + "'");
            foreach (JProperty item in formData.Properties())   //遍历更多信息，写入sql2数组列表
            {
                if (item.Name != "site" && item.Name != "name" && item.Name != "system" && item.Name != "detail"
                    && item.Name != "status" && item.Name != "remark" && item.Name != "userId" && item.Name != "stnId")
                {
                    sql2.Add("INSERT INTO GDMS_STN_MORE (STN_ID,ITEM,VALUE) VALUES ('" + (String)formData["stnId"] + "','" + item.Name + "','" + (String)item.Value + "')");
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
        public class StnMoreAjax
        {
            public string stnId { get; set; }
        }

        //获取更多信息
        [ActionName("more")]
        public HttpResponseMessage StnMore([FromBody] StnMoreAjax ajaxData)
        {
            Db db = new Db();
            string sql = "SELECT ITEM,VALUE FROM GDMS_STN_MORE WHERE STN_ID = '" + ajaxData.stnId + "'";

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
