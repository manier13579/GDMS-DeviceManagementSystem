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

        //获取设备列表
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
                A.FILE_TYPE,
                A.FILE_URL,
                A.USER_ID,
                A.EDIT_DATE,
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
                    { "FILE_TYPE", col["FILE_TYPE"].ToString() },
                    { "FILE_URL", col["FILE_URL"].ToString() },
                    { "USER_ID", col["USER_ID"].ToString() },
                    { "EDIT_DATE", col["EDIT_DATE"].ToString() },
                    { "TYPE_NAME", col["TYPE_NAME"].ToString() },
                    { "SYSTEM_NAME", col["SYSTEM_NAME"].ToString() },
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

        //删除设备
        [ActionName("del")]
        public HttpResponseMessage DeviceDel([FromBody] StyleAjax styleajax)
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
