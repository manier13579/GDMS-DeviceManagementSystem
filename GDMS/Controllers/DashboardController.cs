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
    public class DashboardController : ApiController
    {

        //POST对象 (通过POST只能获取1个对象，因此POST多个数据需要使用类)
        public class DashboardAjax
        {
            public string systemId { get; set; }
            public string oldBenchmark { get; set; }
        }
        //返回对象
        private class Response
        {
            public int code { get; set; }
            public string msg { get; set; }
            public object data { get; set; }
        }

        //获取老旧图表数据
        [ActionName("chartOld")]
        public HttpResponseMessage ChartOld([FromBody] DashboardAjax ajaxData)
        {
            Db db = new Db();
            string sql = @"
                SELECT 
                A.COUNT,
                A.DELIVERY_DATE,
                ADD_MONTHS(A.DELIVERY_DATE,12*B.SERVICE_YEAR) AS SERVICE_DATE,
                ADD_MONTHS(A.DELIVERY_DATE,12*" + ajaxData.oldBenchmark + @") AS OLD_DATE
                FROM GDMS_DEV_MAIN A
                LEFT JOIN GDMS_STYLE B ON A.STYLE_ID = B.ID
                LEFT JOIN GDMS_TYPE C ON B.TYPE_ID = C.ID
                LEFT JOIN GDMS_SYSTEM D ON C.SYSTEM_ID = D.ID
                WHERE D.ID = '" + ajaxData.systemId + @"'
                ORDER BY C.NAME ASC";

            var ds = db.QueryT(sql);
            Response res = new Response();
            //初始化计数
            int okCountSum = 0;
            int serviceCountSum = 0;
            int oldCountSum = 0;
            foreach (DataRow col in ds.Rows)
            {
                var countStr = col["COUNT"].ToString();     //获取数量(字符串格式)
                var serviceDate = Convert.ToDateTime(col["SERVICE_DATE"].ToString());       //过保日期
                var oldDate = Convert.ToDateTime(col["OLD_DATE"].ToString());       //老旧日期
                var nowDate = System.DateTime.Now;      //当前日期
                if(nowDate <= serviceDate && nowDate <= oldDate)      //如果设备未过保，未老旧
                {
                    okCountSum = okCountSum + int.Parse(countStr);      //当前类型正常数量
                }else if(nowDate > serviceDate && nowDate <= oldDate)      //如果设备过保，未老旧
                {
                    serviceCountSum = serviceCountSum + int.Parse(countStr);        //当前类型过保数量
                }else if (nowDate > oldDate)      //如果设备老旧
                {
                    oldCountSum = oldCountSum + int.Parse(countStr);        //当前类型老旧数量
                }
            }
            //循环之后加上最后一个类型的信息
            Dictionary<string, string> data = new Dictionary<string, string>
            {
                { "OK_COUNT", okCountSum.ToString() },
                { "SERVICE_COUNT", serviceCountSum.ToString() },
                { "OLD_COUNT", oldCountSum.ToString() },
            };

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

        //获取老旧图表数据
        [ActionName("chartBackup")]
        public HttpResponseMessage ChartBackup([FromBody] DashboardAjax ajaxData)
        {
            Db db = new Db();
            string sql = @"
                SELECT
                A.COUNT,
                A.STATUS
                FROM GDMS_DEV_MAIN A
                LEFT JOIN GDMS_STYLE B ON A.STYLE_ID = B.ID
                LEFT JOIN GDMS_TYPE C ON B.TYPE_ID = C.ID
                LEFT JOIN GDMS_SYSTEM D ON C.SYSTEM_ID = D.ID
                WHERE D.ID = '" + ajaxData.systemId + @"'
                ORDER BY C.ID ASC";

            var ds = db.QueryT(sql);
            Response res = new Response();
            //初始化计数
            int useCountSum = 0;
            int bakCountSum = 0;
            int errCountSum = 0;
            int fixCountSum = 0;
            foreach (DataRow col in ds.Rows)
            {
                var countStr = col["COUNT"].ToString();     //获取数量(字符串格式)
                var status = col["STATUS"].ToString();        //获取设备状态
                if (status == "0")      //如果是备件
                {
                    bakCountSum = bakCountSum + int.Parse(countStr);
                }
                else if (status == "1")      //如果是在用
                {
                    useCountSum = useCountSum + int.Parse(countStr);
                }
                else if (status == "2")      //如果是故障
                {
                    errCountSum = errCountSum + int.Parse(countStr);
                }
                else if (status == "3")      //如果是维修
                {
                    fixCountSum = fixCountSum + int.Parse(countStr);
                }
            }
            //循环之后加上最后一个类型的信息
            Dictionary<string, string> data = new Dictionary<string, string>
            {
                { "BAK_COUNT", bakCountSum.ToString() },
                { "USE_COUNT", useCountSum.ToString() },
                { "ERR_COUNT", errCountSum.ToString() },
                { "FIX_COUNT", fixCountSum.ToString() },
            };

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
