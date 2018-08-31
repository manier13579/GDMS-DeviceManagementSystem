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
    public class SiteController : ApiController
    {

        //POST对象 (通过POST只能获取1个对象，因此POST多个数据需要使用类)
        public class SiteAjax
        {
            public string userId { get; set; }
            public string systemId { get; set; }
            public int page { get; set; }
            public int limit { get; set; }
            public string keyword { get; set; }
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
        public HttpResponseMessage SiteList([FromBody] SiteAjax siteajax)
        {
            Db db = new Db();
            string where = "";
            if (siteajax.systemId != null) { where = where + " AND A.SYSTEM_ID = '" + siteajax.systemId + "'"; }
            if (siteajax.keyword != null && siteajax.keyword.Length != 0) { where = where + "AND ( A.REMARK LIKE '" + siteajax.keyword + "' or A.NAME LIKE '" + siteajax.keyword + "')"; }
            string sqlnp = @"
                SELECT
                A.ID AS SITE_ID,
                A.PARENT_ID,
                A.NAME,
                A.REMARK,
                A.USER_ID,
                A.EDIT_DATE,
                B.ID AS SYSTEM_ID,
                B.NAME AS SYSTEM_NAME,
                C.NAME AS PARENT_NAME
                FROM
                GDMS_SITE A
                LEFT JOIN GDMS_SYSTEM B ON A.SYSTEM_ID = B.ID
                LEFT JOIN GDMS_SITE C ON A.PARENT_ID = C.ID
                WHERE A.SYSTEM_ID IN (SELECT SYSTEM_ID FROM GDMS_USER_SYSTEM WHERE USER_ID = '" + siteajax.userId + "') " + where + "ORDER BY PARENT_ID,NAME ASC";

            int limit1 = (siteajax.page - 1) * siteajax.limit + 1;
            int limit2 = siteajax.page * siteajax.limit;
            string sql = "SELECT * FROM(SELECT p1.*,ROWNUM rn FROM(" + sqlnp + ")p1)WHERE rn BETWEEN " + limit1 + " AND " + limit2;
            var ds = db.QueryT(sql);
            Response res = new Response();
            ArrayList data = new ArrayList();
            foreach (DataRow col in ds.Rows)
            {
                Dictionary<string, string> dict = new Dictionary<string, string>
                {
                    { "SITE_ID", col["SITE_ID"].ToString() },
                    { "PARENT_ID", col["PARENT_ID"].ToString() },
                    { "PARENT_NAME", col["PARENT_NAME"].ToString() },
                    { "NAME", col["NAME"].ToString() },
                    { "REMARK", col["REMARK"].ToString() },
                    { "USER_ID", col["USER_ID"].ToString() },
                    { "EDIT_DATE", col["EDIT_DATE"].ToString() },
                    { "SYSTEM_ID", col["SYSTEM_ID"].ToString() },
                    { "SYSTEM_NAME", col["SYSTEM_NAME"].ToString() },
                };

                data.Add(dict);
            }

            string sql2 = @"
                SELECT
                COUNT(*) AS COUNT
                FROM
                GDMS_SITE A
                LEFT JOIN GDMS_SYSTEM B ON A.SYSTEM_ID = B.ID
                WHERE A.SYSTEM_ID IN (SELECT SYSTEM_ID FROM GDMS_USER_SYSTEM WHERE USER_ID = '" + siteajax.userId + "') " + where;
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
        public HttpResponseMessage DeviceSelect([FromBody] SiteAjax siteajax)
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
                WHERE A.USER_ID = '" + siteajax.userId + "'";
            var ds1 = db.QueryT(sql1);
            Dictionary<string, string> dict1 = new Dictionary<string, string>();
            foreach (DataRow col in ds1.Rows)
            {
                dict1.Add(col["SYSTEM_ID"].ToString(), col["SYSTEM_NAME"].ToString());
            }
            data.Add("system", dict1);

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

        //删除项目
        [ActionName("del")]
        public HttpResponseMessage SiteDel([FromBody] String ajaxData)
        {
            Db db = new Db();
            JArray idArr = (JArray)JsonConvert.DeserializeObject(ajaxData);
            string sqlin = "";
            foreach (var siteId in idArr)
            {
                sqlin = sqlin + siteId + ",";
            }
            sqlin = sqlin.Substring(0, sqlin.Length - 1);
            string sql = "DELETE FROM GDMS_SITE WHERE ID IN (" + sqlin + ")";

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

        //添加项目
        [ActionName("add")]
        public HttpResponseMessage SiteAdd([FromBody] String ajaxData)
        {
            JObject formData = (JObject)JsonConvert.DeserializeObject(ajaxData);
            Db db = new Db();
            string sql = @"INSERT INTO GDMS_SITE(ID,PARENT_ID,SYSTEM_ID,NAME,REMARK,USER_ID,EDIT_DATE) VALUES (
                GDMS_SITE_SEQ.nextVal, 
                '" + (String)formData["parent"] + @"', 
                '" + (String)formData["system"] + @"',
                '" + (String)formData["name"] + @"', 
                '" + (String)formData["remark"] + @"',
                '" + (String)formData["userId"] + @"',
                SYSDATE
                )";
            var rows = db.ExecuteSql(sql);


            Response res = new Response();
            res.code = 0;
            res.msg = "添加成功" + rows;
            res.data = null;

            var resJsonStr = JsonConvert.SerializeObject(res);
            HttpResponseMessage resJson = new HttpResponseMessage
            {
                Content = new StringContent(resJsonStr, Encoding.GetEncoding("UTF-8"), "application/json")
            };
            return resJson;
        }

        //修改项目
        [ActionName("edit")]
        public HttpResponseMessage SiteEdit([FromBody] String ajaxData)
        {
            JObject formData = (JObject)JsonConvert.DeserializeObject(ajaxData);
            Db db = new Db();
            string sql = @"UPDATE GDMS_SITE SET 
                SYSTEM_ID = '" + (String)formData["system"] + @"',
                NAME = '" + (String)formData["name"] + @"',
                REMARK = '" + (String)formData["remark"] + @"',
                PARENT_ID = '" + (String)formData["parent"] + @"',
                USER_ID = '" + (String)formData["userId"] + @"',
                EDIT_DATE = SYSDATE
                WHERE ID = '" + (String)formData["siteId"] + "'";

            var rows = db.ExecuteSql(sql);

            Response res = new Response();
            res.code = 0;
            res.msg = "更新成功" + rows;
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
