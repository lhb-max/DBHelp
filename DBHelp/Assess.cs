using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace DBHelp
{
    /// <summary>
    /// 连接Sqlserver工具类
    /// </summary>
    public class Assess
    {
        /// <summary>
        /// 连接数据库字符串
        /// </summary>
        private static string conString =
           System.Configuration.ConfigurationManager.ConnectionStrings["conString"].ConnectionString;

        /// <summary>
        /// 执行增删改的方法
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="pms">传入的参数</param>
        /// <returns>返回受影响的行数</returns>
        public static int ExecuteNonQuery(string sql,params SqlParameter[] parameters)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                using (SqlCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandText = sql;
                    com.Parameters.AddRange(parameters);
                    return com.ExecuteNonQuery();
                }
            }
        }

        public static int ExecuteNonQuery(string sql)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                using (SqlCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandText = sql;
                    return com.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 执行查询，返回单个值
        /// </summary>
        /// <param name="sql"> 传入的sql语句字符串</param>
        /// <param name="pms">sql语句中的参数</param>
        /// <returns>返回一个Object</returns>
        public static object ExecuteScalar(string sql,CommandType type, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(conString))
            {
                using (SqlCommand comm = conn.CreateCommand())
                {
                    conn.Open();
                    comm.CommandType = type;
                    comm.CommandText = sql;
                    comm.Parameters.AddRange(parameters);
                    return comm.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// 无存储过程的ExecuteScalar()方法
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string sql)
        {
            using (SqlConnection conn = new SqlConnection(conString))
            {
                using (SqlCommand com = conn.CreateCommand())
                {
                    conn.Open();
                    com.CommandText = sql;
                    return com.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// 执行查询，返回多个值
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>返回一个Reader,用于获取返回的每个结果</returns>
        public static SqlDataReader ExecuteReader(string sql, params SqlParameter[] parameters)
        {
            //SqlDataReader要求，它读取数据的时候有，它独占它的SqlConnection对象，而且SqlConnection必须是Open状态
            SqlConnection conn = new SqlConnection(conString);//不要释放连接，因为后面还需要连接打开状态
            SqlCommand cmd = conn.CreateCommand();
            conn.Open();
            cmd.CommandText = sql;
            cmd.Parameters.AddRange(parameters);
            //CommandBehavior.CloseConnection当SqlDataReader释放的时候，顺便把SqlConnection对象也释放掉
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// 查询操作
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>返回DataTable</returns>
        public static DataTable ExcuteDatable(string sql, CommandType type,params SqlParameter[] parameters)
        {
        
            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand com = con.CreateCommand();
                con.Open();
                com.CommandType = type;
                com.CommandText = sql;
                com.Parameters.AddRange(parameters);
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable db = new DataTable();
                da.Fill(db);
                return db;
            }
        }

        /// <summary>
        /// 无存储过程返回受影响行数的方法
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable ExecuteQuery(string sql)
        {
            using (SqlConnection conn = new SqlConnection(conString))
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(sql, conn);
                DataTable dt = new DataTable();
                SqlDataAdapter ada = new SqlDataAdapter(comm);
                ada.Fill(dt);
                return dt;
            }
        }




        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="outPara"></param>
        /// <param name="p"></param>
        public static void RunProc(string procName,SqlParameter outPara,params SqlParameter[] p)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                SqlCommand com = new SqlCommand(procName,con);
                //存储过程
                com.CommandType = CommandType.StoredProcedure;
                //指定参数的方向
                outPara.Direction = ParameterDirection.Output;
                com.Parameters.Add(outPara);  //添加一个输出参数
               
                com.Parameters.AddRange(p);
                com.ExecuteNonQuery();
            }
        }
    }
}
