using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using GDMS.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GDMS.Controllers
{
    [RequestAuthorize]
    public class StyleController : ApiController
    {

        //POST对象 (通过POST只能获取1个对象，因此POST多个数据需要使用类)
        public class StyleAjax
        {
            public string userId { get; set; }
            public string systemId { get; set; }
            public string typeId { get; set; }
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

        //获取式样列表
        [ActionName("list")]
        public HttpResponseMessage StyleList([FromBody] StyleAjax styleajax)
        {
            Db db = new Db();
            string where = "";
            if (styleajax.systemId != null) { where = where + " AND B.SYSTEM_ID = '" + styleajax.systemId + "'"; }
            if (styleajax.typeId != null) { where = where + " AND A.TYPE_ID = '" + styleajax.typeId + "'"; }
            if (styleajax.keyword != null && styleajax.keyword.Length != 0) { where = where + "AND ( A.NAME LIKE '" + styleajax.keyword + "' or A.DETAIL LIKE '" + styleajax.keyword + "')"; }
            string sqlnp = @"
                SELECT
                A.ID AS STYLE_ID,
                A.NAME AS STYLE_NAME,
                A.DETAIL,
                A.IMG_URL,
                A.FILE_URL,
                A.USER_ID,
                A.EDIT_DATE,
                A.SERVICE_YEAR,
                B.ID AS TYPE_ID,
                B.NAME AS TYPE_NAME,
                C.ID AS SYSTEM_ID,
                C.NAME AS SYSTEM_NAME
                FROM
                GDMS_STYLE A
                LEFT JOIN GDMS_TYPE B ON A.TYPE_ID = B.ID
                LEFT JOIN GDMS_SYSTEM C ON B.SYSTEM_ID = C.ID
                WHERE B.SYSTEM_ID IN (SELECT SYSTEM_ID FROM GDMS_USER_SYSTEM WHERE USER_ID = '" + styleajax.userId + "') " + where;

            int limit1 = (styleajax.page - 1) * styleajax.limit + 1;
            int limit2 = styleajax.page * styleajax.limit;
            string sql = "SELECT * FROM(SELECT p1.*,ROWNUM rn FROM(" + sqlnp + ")p1)WHERE rn BETWEEN " + limit1 + " AND " + limit2;
            var ds = db.QueryT(sql);
            Response res = new Response();
            ArrayList data = new ArrayList();
            foreach (DataRow col in ds.Rows)
            {
                Dictionary<string, string> dict = new Dictionary<string, string>
                {
                    { "STYLE_ID", col["STYLE_ID"].ToString() },
                    { "STYLE_NAME", col["STYLE_NAME"].ToString() },
                    { "DETAIL", col["DETAIL"].ToString() },
                    { "IMG_URL", col["IMG_URL"].ToString() },
                    { "FILE_URL", col["FILE_URL"].ToString() },
                    { "USER_ID", col["USER_ID"].ToString() },
                    { "EDIT_DATE", col["EDIT_DATE"].ToString() },
                    { "TYPE_NAME", col["TYPE_NAME"].ToString() },
                    { "SYSTEM_NAME", col["SYSTEM_NAME"].ToString() },
                    { "SERVICE_YEAR", col["SERVICE_YEAR"].ToString()+"年" },
                };

                data.Add(dict);
            }

            string sql2 = @"
                SELECT
                COUNT(*) AS COUNT
                FROM
                GDMS_STYLE A
                LEFT JOIN GDMS_TYPE B ON A.TYPE_ID = B.ID
                LEFT JOIN GDMS_SYSTEM C ON B.SYSTEM_ID = C.ID
                WHERE B.SYSTEM_ID IN (SELECT SYSTEM_ID FROM GDMS_USER_SYSTEM WHERE USER_ID = '" + styleajax.userId + "') " + where;
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
        public HttpResponseMessage DeviceSelect([FromBody] StyleAjax styleajax)
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
                WHERE A.USER_ID = '" + styleajax.userId + "'";
            var ds1 = db.QueryT(sql1);
            Dictionary<string, string> dict1 = new Dictionary<string, string>();
            foreach (DataRow col in ds1.Rows)
            {
                dict1.Add(col["SYSTEM_ID"].ToString(), col["SYSTEM_NAME"].ToString());
            }
            data.Add("system", dict1);

            //查询类型select
            string sql3 = @"
                SELECT
                A.SYSTEM_ID AS SYSTEM_ID,
                A.ID AS TYPE_ID,
                A.NAME AS TYPE_NAME
                FROM
                GDMS_TYPE A
                LEFT JOIN GDMS_USER_SYSTEM B ON A.SYSTEM_ID = B.SYSTEM_ID
                WHERE B.USER_ID = '" + styleajax.userId + "' ORDER BY A.SYSTEM_ID ASC ";
            var ds3 = db.QueryT(sql3);
            Dictionary<string, object> TypeData = new Dictionary<string, object>();
            Dictionary<string, string> dict3 = new Dictionary<string, string>();
            var index = "0";
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

        //删除式样
        [ActionName("del")]
        public HttpResponseMessage StyleDel([FromBody] String ajaxData)
        {
            Db db = new Db();
            JArray idArr = (JArray)JsonConvert.DeserializeObject(ajaxData);
            string sqlin = "";
            foreach (var siteId in idArr)
            {
                sqlin = sqlin + siteId + ",";
            }
            sqlin = sqlin.Substring(0, sqlin.Length - 1);
            string sql = "DELETE FROM GDMS_STYLE WHERE ID IN (" + sqlin + ")";
            //！！！！这里先要把上传过的文件删除，再删除数据库记录
            var rows = db.ExecuteSql(sql);
            Response res = new Response();

            res.code = 0;
            res.msg = "操作成功，删除了" + rows + "个式样";
            res.data = null;

            var resJsonStr = JsonConvert.SerializeObject(res);
            HttpResponseMessage resJson = new HttpResponseMessage
            {
                Content = new StringContent(resJsonStr, Encoding.GetEncoding("UTF-8"), "application/json")
            };
            return resJson;
        }

        //添加式样
        [ActionName("add")]
        public HttpResponseMessage StyleAdd([FromBody] String ajaxData)
        {
            JObject formData = (JObject)JsonConvert.DeserializeObject(ajaxData);
            Db db = new Db();
            string sql = @"SELECT STYLE_ADD(
                '" + (String)formData["name"] + @"', 
                '" + (String)formData["detail"] + @"',
                '" + (String)formData["year"] + @"',
                '" + (String)formData["type"] + @"',
                '" + (String)formData["userid"] + @"'
                ) AS STYLEID FROM DUAL";
            var ds = db.QueryT(sql);    //执行设备添加ORACLE函数，返回主键：styleId
            var styleId = "";
            foreach (DataRow col in ds.Rows)
            {
                styleId = col["STYLEID"].ToString();
            }
            Response res = new Response();
            res.code = 0;
            res.msg = "添加成功";
            res.data = styleId;

            var resJsonStr = JsonConvert.SerializeObject(res);
            HttpResponseMessage resJson = new HttpResponseMessage
            {
                Content = new StringContent(resJsonStr, Encoding.GetEncoding("UTF-8"), "application/json")
            };
            return resJson;
        }

        //修改项目
        [ActionName("edit")]
        public HttpResponseMessage StyleEdit([FromBody] String ajaxData)
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

        //上传式样文件
        [ActionName("uploadFile")]
        public HttpResponseMessage StyleUploadFile(HttpContext Request)
        {
            HttpPostedFile filedata = Request.Request.Files[0];
            Response res = new Response();
            if (filedata == null || String.IsNullOrEmpty(filedata.FileName) || filedata.ContentLength == 0)
            {
                res.code = 1;
            }
            else {
                res.code = 0;
            }
            string filename = System.IO.Path.GetFileName(filedata.FileName);
            string virtualPath = String.Format("~/File/{0}", filename);
            string path = HttpContext.Current.Server.MapPath(virtualPath);
            filedata.SaveAs(path);
            /*
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
            */


            var resJsonStr = JsonConvert.SerializeObject(res);
            HttpResponseMessage resJson = new HttpResponseMessage
            {
                Content = new StringContent(resJsonStr, Encoding.GetEncoding("UTF-8"), "application/json")
            };
            return resJson;
        }

    }
}
