using System.Data;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Chess.OpenSource
{
    public class SqliteHelper
    {
        //private static readonly string str = @"data source=D:\CSHARP\Chess\DB\KaiJuKu.db";
        private static readonly string str = @"data source=E:\source\repos\Chess\DB\KaiJuKu.db"; // 调试时使用
        //private static readonly string str = "data source=" + System.Environment.CurrentDirectory + @"\DB\KaiJuKu.db"; // 发行版使用此路径
        public static int ExecuteSql(string sql)
        {
            using SQLiteConnection con = new(str);
            using SQLiteCommand cmd = new(sql, con);
            con.Open();
            return cmd.ExecuteNonQuery();
        }
        public static int ExecuteSql(string sql, params SQLiteParameter[] param)
        {
            using SQLiteConnection con = new(str);
            using SQLiteCommand cmd = new(sql, con);
            con.Open();
            if (param != null)
            {
                cmd.Parameters.AddRange(param);
            }
            return cmd.ExecuteNonQuery();
        }

        public static string ExecuteScalar(string sql, params SQLiteParameter[] param)
        {
            using SQLiteConnection con = new(str);
            using SQLiteCommand cmd = new(sql, con);
            con.Open();
            if (param != null)
            {
                cmd.Parameters.AddRange(param);
            }
            object obj = cmd.ExecuteScalar();
            return obj == null ? "" : obj.ToString();
        }
        public static SQLiteDataReader ExecuteReader(string sql, params SQLiteParameter[] param)
        {
            using SQLiteConnection con = new(str);
            using SQLiteCommand cmd = new(sql, con);
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
        public static DataTable ExecuteTable(string sql)
        {
            DataTable dt = new();
            try
            {
                using SQLiteDataAdapter sda = new(sql, str);
                sda.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                _ = sda.Fill(dt);
            }
            catch (System.Exception ex)
            {
                _ = ex.Message;
            }
            return dt;
        }
        public static DataTable ExecuteTable(string sql, params SQLiteParameter[] param)
        {
            DataTable dt = new();
            try
            {
                using SQLiteDataAdapter sda = new(sql, str);
                if (param != null)
                {
                    sda.SelectCommand.Parameters.AddRange(param);
                }
                sda.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                _ = sda.Fill(dt);
            }
            catch (System.Exception ex)
            {
                _ = ex.Message;
            }
            return dt;
        }
        public static DataTable Select(string tbName, string fields = "*", string where = "1", string orderBy = "", string limit = "", params SQLiteParameter[] param)
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

            string sql = $"SELECT {fields} FROM `{tbName}` WHERE {where} {orderBy} {limit}";

            //return sql;
            return ExecuteTable(sql, param);
        }
        public static int Insert(string tbName, Dictionary<string, object> insertData)
        {
            string point = "";//分隔符号(,)
            string keyStr = "";//字段名拼接字符串
            string valueStr = "";//值的拼接字符串
            List<SQLiteParameter> param = new();
            foreach (string key in insertData.Keys)
            {
                keyStr += $"{point} `{key}`";
                valueStr += $"{point} @{key}";
                param.Add(new SQLiteParameter("@" + key, insertData[key]));
                point = ",";
            }
            string sql = $"INSERT INTO `{tbName}`({keyStr}) VALUES({valueStr})";
            return ExecuteSql(sql, param.ToArray());
        }
        public static int Update(string tbName, string where, Dictionary<string, object> insertData)
        {
            string point = "";//分隔符号(,)
            string kvStr = "";//键值对拼接字符串(Id=@Id)
            List<SQLiteParameter> param = new();
            foreach (string key in insertData.Keys)
            {
                kvStr += $"{point} {key}=@{key}";
                param.Add(new SQLiteParameter("@" + key, insertData[key]));
                point = ",";
            }
            string sql = $"UPDATE `{tbName}` SET {kvStr} WHERE {where}";
            return ExecuteSql(sql, param.ToArray());

        }
        public static int Delete(string tbName, string where)
        {
            if (string.IsNullOrEmpty(where)) return -1;
            string sql = $"DELETE FROM `{tbName}` WHERE {where}";
            return ExecuteSql(sql);

        }
    }
}