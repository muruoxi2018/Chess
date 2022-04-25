using System.Data;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Chess.OpenSource
{
    public class SqliteHelper
    {
        //private static readonly string str = @"data source=D:\CSHARP\Chess\DB\KaiJuKu.db";
        //private static readonly string str = @"data source=E:\source\repos\Chess\DB\KaiJuKu.db";
        private static readonly string str = "data source="+System.Environment.CurrentDirectory + @"\DB\KaiJuKu.db";
        public static int ExecuteSql(string sql)
        {
            using (SQLiteConnection con = new SQLiteConnection(str))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                {
                    con.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        public static int ExecuteSql(string sql, params SQLiteParameter[] param)
        {
            using (SQLiteConnection con = new SQLiteConnection(str))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                {
                    con.Open();
                    if (param != null)
                    {
                        cmd.Parameters.AddRange(param);
                    }
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public static string ExecuteScalar(string sql, params SQLiteParameter[] param)
        {
            using (SQLiteConnection con = new SQLiteConnection(str))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                {
                    con.Open();
                    if (param != null)
                    {
                        cmd.Parameters.AddRange(param);
                    }
                    object obj = cmd.ExecuteScalar();
                    if (obj == null)
                        return "";
                    else
                        return obj.ToString();
                }
            }
        }
        public static SQLiteDataReader ExecuteReader(string sql, params SQLiteParameter[] param)
        {
            using (SQLiteConnection con = new SQLiteConnection(str))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                {
                    if (param != null)
                    {
                        cmd.Parameters.AddRange(param);
                    }
                    try
                    {
                        con.Open();
                        return cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    }
                    catch (System.Exception)
                    {
                        con.Close();
                        con.Dispose();
                        //throw ex;
                        return null;
                    }
                }
            }
        }
        public static DataTable ExecuteTable(string sql)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SQLiteDataAdapter sda = new SQLiteDataAdapter(sql, str))
                {
                    sda.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                    sda.Fill(dt);
                }
            }
            catch (System.Exception ex)
            {
                string s = ex.Message;
            }
            return dt;
        }
        public static DataTable ExecuteTable(string sql, params SQLiteParameter[] param)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SQLiteDataAdapter sda = new SQLiteDataAdapter(sql, str))
                {
                    if (param != null)
                    {
                        sda.SelectCommand.Parameters.AddRange(param);
                    }
                    sda.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                    sda.Fill(dt);
                }
            }
            catch (System.Exception ex)
            {
                string s = ex.Message;
            }
            return dt;
        }
        public static DataTable QueryTable(string tbName, string fields = "*", string where = "1", string orderBy = "", string limit = "", params SQLiteParameter[] param)
        {
            //排序
            if (orderBy != "")
            {
                orderBy = "ORDER BY " + orderBy;//Deom: ORDER BY id desc
            }

            //分页
            if (limit != "")
            {
                limit = "LIMIT " + limit;//Deom: LIMIT 0,10
            }

            string sql = string.Format("SELECT {0} FROM `{1}` WHERE {2} {3} {4}", fields, tbName, where, orderBy, limit);

            //return sql;
            return ExecuteTable(sql, param);
        }
        public static int ExecuteInsert(string tbName, Dictionary<string, string> insertData)
        {
            string point = "";//分隔符号(,)
            string keyStr = "";//字段名拼接字符串
            string valueStr = "";//值的拼接字符串
            List<SQLiteParameter> param = new List<SQLiteParameter>();
            foreach (string key in insertData.Keys)
            {
                keyStr += string.Format("{0} `{1}`", point, key);
                valueStr += string.Format("{0} @{1}", point, key);
                param.Add(new SQLiteParameter("@" + key, insertData[key]));
                point = ",";
            }
            string sql = string.Format("INSERT INTO `{0}`({1}) VALUES({2})", tbName, keyStr, valueStr);
            return ExecuteSql(sql, param.ToArray());
        }
        public static int ExecuteUpdate(string tbName, string where, Dictionary<string, string> insertData)
        {
            string point = "";//分隔符号(,)
            string kvStr = "";//键值对拼接字符串(Id=@Id)
            List<SQLiteParameter> param = new List<SQLiteParameter>();
            foreach (string key in insertData.Keys)
            {
                kvStr += string.Format("{0} {1}=@{2}", point, key, key);
                param.Add(new SQLiteParameter("@" + key, insertData[key]));
                point = ",";
            }
            string sql = string.Format("UPDATE `{0}` SET {1} WHERE {2}", tbName, kvStr, where);
            return ExecuteSql(sql, param.ToArray());

        }
    }
}