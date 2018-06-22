using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Oracle.DataAccess.Client;

namespace GDMS.Models
{
    public class Db
    {
        public static string connString = "User ID=bing;Password=906124;Data Source=(DESCRIPTION = (ADDRESS_LIST= (ADDRESS = (PROTOCOL = TCP)(HOST = 127.0.0.1)(PORT = 1521))) (CONNECT_DATA = (SERVICE_NAME = XE)))";

        /// 执行数据库非查询操作,返回受影响的行数
        /// 参数：SQLString(SQL语句)
        /// 返回：影响的记录数
        public int ExecuteSql(string SQLString)
        {
            OracleConnection conn = new OracleConnection(connString);
            OracleCommand cmd = new OracleCommand(SQLString, conn);
            try
            {
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows;
            }
            catch (OracleException e)
            {
                conn.Close();
                throw new Exception(e.Message);
            }
        }

        /// 执行数据库事务非查询操作，多条语句分号隔开
        /// 参数：SQLStringList(多条SQL语句)   
        public void ExecuteSqlTran(ArrayList SQLStringList)
        {
            OracleConnection conn = new OracleConnection(connString);
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            OracleTransaction tx = conn.BeginTransaction();
            cmd.Transaction = tx;
            try
            {
                for (int n = 0; n < SQLStringList.Count; n++)
                {
                    string strsql = SQLStringList[n].ToString();
                    if (strsql.Trim().Length > 1)
                    {
                        cmd.CommandText = strsql;
                        cmd.ExecuteNonQuery();
                    }
                }
                tx.Commit();
            }
            catch (OracleException e)
            {
                tx.Rollback();
                throw new Exception(e.Message);
            }
        }

        /// 执行查询语句，返回DataSet  
        /// 参数：SQLString(查询语句)
        /// 返回：DataSet
        public DataSet Query(string SQLString)
        {
            using (OracleConnection conn = new OracleConnection(connString))
            {
                DataSet ds = new DataSet();
                try
                {
                    conn.Open();
                    OracleDataAdapter command = new OracleDataAdapter(SQLString, conn);
                    command.Fill(ds, "ds");
                }
                catch (OracleException e)
                {
                    throw new Exception(e.Message);
                }
                return ds;
            }
        }

        /// 执行数据库查询操作,返回DataTable类型的结果集  
        /// 参数：SQLString(查询语句)
        /// 返回：DataTable
        public DataTable QueryT(string SQLString)
        {
            using (OracleConnection conn = new OracleConnection(connString))
            {
                DataTable ds = new DataTable();
                try
                {
                    conn.Open();
                    OracleDataAdapter command = new OracleDataAdapter(SQLString, conn);
                    command.Fill(ds);
                }
                catch (OracleException e)
                {
                    throw new Exception(e.Message);
                }
                return ds;
            }
        }

        /// 执行带一个存储过程参数的的SQL语句
        /// 参数：SQLString(SQL语句)
        /// 参数：content(比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加)
        /// 返回：影响的记录数 
        public int ExecuteSql(string SQLString, string content)
        {
            OracleConnection conn = new OracleConnection(connString);
            OracleCommand cmd = new OracleCommand(SQLString, conn);
            OracleParameter myParameter = new OracleParameter("@content", OracleDbType.NVarchar2);
            myParameter.Value = content;
            cmd.Parameters.Add(myParameter);
            try
            {
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows;
            }
            catch (OracleException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }

        }

        /// 执行存储过程  
        /// 参数：storedProcName(存储过程名)
        /// 参数：CommandText(存储过程指令的SQL语句)
        /// 参数：tableName(DataSet结果中的表名)
        /// 返回：DataSet
        public DataSet RunProcedure(string storedProcName, string CommandText, string tableName)
        {
            using (OracleConnection conn = new OracleConnection(connString))
            {
                DataSet dataSet = new DataSet();
                conn.Open();
                OracleDataAdapter sqlDA = new OracleDataAdapter();

                sqlDA.SelectCommand.Connection = conn;
                sqlDA.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDA.SelectCommand.CommandText = CommandText;
                sqlDA.Fill(dataSet, tableName);
                conn.Close();
                return dataSet;
            }
        }

    }

}


