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
    public class BackupChartController : ApiController
    {

        //POST对象 (通过POST只能获取1个对象，因此POST多个数据需要使用类)
        public class BackupChartAjax
        {
            public string systemId { get; set; }
        }
        //返回对象
        private class Response
        {
            public int code { get; set; }
            public string msg { get; set; }
            public object data { get; set; }
        }

        //按设备类型返回图表数据
        [ActionName("type")]
        public HttpResponseMessage BackupChart([FromBody] BackupChartAjax ajaxData)
        {
            Db db = new Db();
            string sql = @"
                SELECT
                A.COUNT,
                A.STATUS,
                B.TYPE_ID,
                C.NAME AS TYPE_NAME
                FROM GDMS_DEV_MAIN A
                LEFT JOIN GDMS_STYLE B ON A.STYLE_ID = B.ID
                LEFT JOIN GDMS_TYPE C ON B.TYPE_ID = C.ID
                LEFT JOIN GDMS_SYSTEM D ON C.SYSTEM_ID = D.ID
                WHERE D.ID = '" + ajaxData.systemId + @"'
                ORDER BY C.ID ASC";

            var ds = db.QueryT(sql);
            Response res = new Response();
            ArrayList data = new ArrayList();
            //初始化计数
            string typeId = "";
            string typeName = "";
            int useCountSum = 0;
            int bakCountSum = 0;
            int errCountSum = 0;
            int fixCountSum = 0;
            foreach (DataRow col in ds.Rows)
            {
                //第一次循环 或 本次类型与上次相同，数值加和
                if (typeId == "" || typeId == col["TYPE_ID"].ToString())
                {
                    typeId = col["TYPE_ID"].ToString();     //当前类型id
                    typeName = col["TYPE_NAME"].ToString();     //当前类型名称
                    var countStr = col["COUNT"].ToString();     //获取数量(字符串格式)
                    var status = col["STATUS"].ToString();        //获取设备状态
                    if (status == "0")      //如果是备件
                    {
                        bakCountSum = bakCountSum + int.Parse(countStr);
                    }
                    else if(status == "1")      //如果是在用
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
                //循环到新类型
                else {
                    //将上一类型的数据写入返回数据中data
                    Dictionary<string, string> dict = new Dictionary<string, string>
                    {
                        { "TYPE_NAME", typeName },
                        { "BAK_COUNT", bakCountSum.ToString() },
                        { "USE_COUNT", useCountSum.ToString() },
                        { "ERR_COUNT", errCountSum.ToString() },
                        { "FIX_COUNT", fixCountSum.ToString() },
                    };
                    data.Add(dict);
                    //初始化计数
                    bakCountSum = 0;
                    useCountSum = 0;
                    errCountSum = 0;
                    fixCountSum = 0;

                    typeId = col["TYPE_ID"].ToString();     //当前类型id
                    typeName = col["TYPE_NAME"].ToString();     //当前类型名称
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
            }
            //循环之后加上最后一个类型的信息
            Dictionary<string, string> dictLast = new Dictionary<string, string>
            {
                { "TYPE_NAME", typeName },
                { "BAK_COUNT", bakCountSum.ToString() },
                { "USE_COUNT", useCountSum.ToString() },
                { "ERR_COUNT", errCountSum.ToString() },
                { "FIX_COUNT", fixCountSum.ToString() },
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
