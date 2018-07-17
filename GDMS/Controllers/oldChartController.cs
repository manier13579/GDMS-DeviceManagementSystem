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
    public class OldChartController : ApiController
    {

        //POST对象 (通过POST只能获取1个对象，因此POST多个数据需要使用类)
        public class OldChartAjax
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

        //获取设备列表
        [ActionName("type")]
        public HttpResponseMessage SiteList([FromBody] OldChartAjax oldchartajax)
        {
            Db db = new Db();
            string sql = @"
                SELECT 
                A.COUNT,
                A.DELIVERY_DATE,
                ADD_MONTHS(A.DELIVERY_DATE,12*B.SERVICE_YEAR) AS SERVICE_DATE,
                ADD_MONTHS(A.DELIVERY_DATE,12*" + oldchartajax.oldBenchmark + @") AS OLD_DATE,
                C.ID AS TYPE_ID,
                C.NAME AS TYPE_NAME
                FROM GDMS_DEV_MAIN A
                LEFT JOIN GDMS_STYLE B ON A.STYLE_ID = B.ID
                LEFT JOIN GDMS_TYPE C ON B.TYPE_ID = C.ID
                LEFT JOIN GDMS_SYSTEM D ON C.SYSTEM_ID = D.ID
                WHERE D.ID = '" + oldchartajax.systemId + @"'
                ORDER BY C.ID ASC";

            var ds = db.QueryT(sql);
            Response res = new Response();
            ArrayList data = new ArrayList();
            //初始化计数
            string typeId = "";
            string typeName = "";
            int okCountSum = 0;
            int serviceCountSum = 0;
            int oldCountSum = 0;
            foreach (DataRow col in ds.Rows)
            {
                //第一次循环 或 本次类型与上次相同，数值加和
                if (typeId == "" || typeId == col["TYPE_ID"].ToString())
                {
                    typeId = col["TYPE_ID"].ToString();     //当前类型id
                    typeName = col["TYPE_NAME"].ToString();     //当前类型名称
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
                //循环到新类型
                else {
                    //将上一类型的数据写入返回数据中data
                    Dictionary<string, string> dict = new Dictionary<string, string>
                    {
                        { "TYPE_NAME", typeName },
                        { "OK_COUNT", okCountSum.ToString() },
                        { "SERVICE_COUNT", serviceCountSum.ToString() },
                        { "OLD_COUNT", oldCountSum.ToString() },
                    };
                    data.Add(dict);

                    //初始化计数
                    okCountSum = 0;
                    serviceCountSum = 0;
                    oldCountSum = 0;

                    typeId = col["TYPE_ID"].ToString();     //当前类型id
                    typeName = col["TYPE_NAME"].ToString();     //当前类型名称
                    var countStr = col["COUNT"].ToString();     //获取数量(字符串格式)
                    var serviceDate = Convert.ToDateTime(col["SERVICE_DATE"].ToString());       //过保日期
                    var oldDate = Convert.ToDateTime(col["OLD_DATE"].ToString());       //老旧日期
                    var nowDate = System.DateTime.Now;      //当前日期
                    if (nowDate <= serviceDate && nowDate <= oldDate)      //如果设备未过保，未老旧
                    {
                        okCountSum = okCountSum + int.Parse(countStr);      //当前类型正常数量
                    }
                    else if (nowDate > serviceDate && nowDate <= oldDate)      //如果设备过保，未老旧
                    {
                        serviceCountSum = serviceCountSum + int.Parse(countStr);        //当前类型过保数量
                    }
                    else if (nowDate > oldDate)      //如果设备老旧
                    {
                        oldCountSum = oldCountSum + int.Parse(countStr);        //当前类型老旧数量
                    }
                }
            }
            //循环之后加上最后一个类型的信息
            Dictionary<string, string> dictLast = new Dictionary<string, string>
            {
                { "TYPE_NAME", typeName },
                { "OK_COUNT", okCountSum.ToString() },
                { "SERVICE_COUNT", serviceCountSum.ToString() },
                { "OLD_COUNT", oldCountSum.ToString() },
            };
            data.Add(dictLast);

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
