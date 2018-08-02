using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Data;

namespace childTest
{
    public class SqlControl
    {
        public static string strConn= initSqlControl();
        public static string initSqlControl()
        {
            try
            {
                string str="";
                OleDbConnection adoConn = new OleDbConnection();
                //string strConn = "";
                string udl = Application.StartupPath + "\\database.udl";
                StreamReader sr = new StreamReader(udl, Encoding.Default);
                for (int i = 0; i < 3; i++)
                {
                    str = sr.ReadLine();
                }
                return str;
            }
            catch(Exception er)
            {
                logControl.saveLogInf("初始化数据库接口发生异常:" + er.Message);
                MessageBox.Show("初始化数据库接口发生异常:" + er.Message);
                return "";
                //throw new Exception(er.Message);
            }
        }
        public static bool testSqlLink(out string errmsg)
        {
            errmsg = "";
            try
            {
                bool DB_status = false;
                OleDbConnection adoConn = new OleDbConnection();
                adoConn.ConnectionString = strConn;
                adoConn.Open();
                OleDbCommand cmd = adoConn.CreateCommand();
                OleDbTransaction Trans = adoConn.BeginTransaction();
                cmd.Transaction = Trans;
                Trans.Rollback();
                try
                {
                    logControl.saveLogInf("连接数据库成功");
                    string checksql = "select * from thisstation";
                    OleDbCommand datacheck = new OleDbCommand(checksql, adoConn);
                    OleDbDataReader reader = datacheck.ExecuteReader();
                    DB_status= true;
                }
                catch (Exception er)
                {
                    errmsg = er.Message;
                    logControl.saveLogInf("查询数据发生异常:" + er.Message);
                    Trans.Rollback();
                    DB_status = false;
                }
                adoConn.Close();
                return DB_status;
            }
            catch (Exception er)
            {
                errmsg = er.Message;
                logControl.saveLogInf("链接到驰达数据库失败:" + er.Message);
                return false;
            }

        }
        public static bool insertRecord(qzj_record model)
        {
            bool result;
            try
            {
                OleDbConnection adoConn = new OleDbConnection();                
                adoConn.ConnectionString = strConn;
                adoConn.Open();
                OleDbCommand cmd = adoConn.CreateCommand();
                OleDbTransaction Trans = adoConn.BeginTransaction();
                cmd.Transaction = Trans;
                Trans.Rollback();
                try
                {
                    logControl.saveLogInf("连接数据库成功");
                    string updatesql = SqlBuilderHelper.InsertSql<qzj_record>(model,"QZJ");
                    OleDbDataAdapter dataAdapter = new OleDbDataAdapter();
                    dataAdapter.InsertCommand = new OleDbCommand(updatesql, adoConn);
                    dataAdapter.InsertCommand.CommandText = updatesql;
                    if (dataAdapter.InsertCommand.ExecuteNonQuery() > 0)
                    {
                        result = true;
                        logControl.saveLogInf("插入数据成功");
                    }
                    else
                    {
                        result = false;
                        logControl.saveLogInf("插入数据失败");
                    }
                    dataAdapter.Dispose();
                }
                catch (Exception er)
                {
                    result = false;
                    logControl.saveLogInf("更新数据发生异常:" + er.Message);
                    Trans.Rollback();
                }
                adoConn.Close();
            }
            catch (Exception er)
            {
                result = false;
                logControl.saveLogInf("链接到数据库失败:" + er.Message);
            }
            return result;
        }
        public static bool insertVehicle(qzj_vehicle model)
        {
            bool result;
            try
            {
                OleDbConnection adoConn = new OleDbConnection();
                adoConn.ConnectionString = strConn;
                adoConn.Open();
                OleDbCommand cmd = adoConn.CreateCommand();
                OleDbTransaction Trans = adoConn.BeginTransaction();
                cmd.Transaction = Trans;
                Trans.Rollback();
                try
                {
                    logControl.saveLogInf("连接数据库成功");
                    string updatesql = SqlBuilderHelper.InsertSql<qzj_vehicle>(model, "QZJ_VEHICLE");
                    OleDbDataAdapter dataAdapter = new OleDbDataAdapter();
                    dataAdapter.InsertCommand = new OleDbCommand(updatesql, adoConn);
                    dataAdapter.InsertCommand.CommandText = updatesql;
                    if (dataAdapter.InsertCommand.ExecuteNonQuery() > 0)
                    {
                        result = true;
                        logControl.saveLogInf("插入数据成功");
                    }
                    else
                    {
                        result = false;
                        logControl.saveLogInf("插入数据失败");
                    }
                    dataAdapter.Dispose();
                }
                catch (Exception er)
                {
                    result = false;
                    logControl.saveLogInf("更新数据发生异常:" + er.Message);
                    Trans.Rollback();
                }
                adoConn.Close();
                
            }
            catch (Exception er)
            {
                result = false;
                logControl.saveLogInf("链接到数据库失败:" + er.Message);
            }
            return result;
        }
        public static bool checkVehicleIsAlive(string vehiclename)
        {
            bool result;
            try
            {
                OleDbConnection adoConn = new OleDbConnection();
                adoConn.ConnectionString = strConn;
                adoConn.Open();
                OleDbCommand cmd = adoConn.CreateCommand();
                OleDbTransaction Trans = adoConn.BeginTransaction();
                cmd.Transaction = Trans;
                Trans.Rollback();
                try
                {
                    logControl.saveLogInf("连接数据库成功");
                    string checksql = "select * from QZJ_VEHICLE where VEHICLENAME='"+vehiclename+"'";
                    OleDbCommand datacheck = new OleDbCommand(checksql, adoConn);
                    if (null != datacheck.ExecuteScalar())
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                catch (Exception er)
                {
                    result = false;
                    logControl.saveLogInf("查询数据发生异常:" + er.Message);
                    Trans.Rollback();

                }
                adoConn.Close();
            }
            catch (Exception er)
            {
                result = false;
                logControl.saveLogInf("链接到数据库失败:" + er.Message);
            }
            return result;

        }
        public static bool updateUpload(bool hasupload,string clid)
        {
            bool result;
            try
            {
                OleDbConnection adoConn = new OleDbConnection();
                adoConn.ConnectionString = strConn;
                adoConn.Open();
                OleDbCommand cmd = adoConn.CreateCommand();
                OleDbTransaction Trans = adoConn.BeginTransaction();
                cmd.Transaction = Trans;
                Trans.Rollback();
                try
                {
                    logControl.saveLogInf("连接数据库成功");
                    string updatesql = "UPDATE QZJ set HASUPLOAD='"+ (hasupload?"Y":"N")+"' where CLID='"+clid+"'";
                    OleDbDataAdapter dataAdapter = new OleDbDataAdapter();
                    dataAdapter.InsertCommand = new OleDbCommand(updatesql, adoConn);
                    dataAdapter.InsertCommand.CommandText = updatesql;
                    if (dataAdapter.InsertCommand.ExecuteNonQuery() > 0)
                    {
                        result = true;
                        logControl.saveLogInf("更新数据成功");
                    }
                    else
                    {
                        result = false;
                        logControl.saveLogInf("更新数据失败");
                    }
                    dataAdapter.Dispose();
                }
                catch (Exception er)
                {
                    result = false;
                    logControl.saveLogInf("更新数据发生异常:" + er.Message);
                    Trans.Rollback();
                }
                adoConn.Close();

            }
            catch (Exception er)
            {
                result = false;
                logControl.saveLogInf("链接到数据库失败:" + er.Message);
            }
            return result;
        }
        public static bool updateVehicle(qzj_vehicle model)
        {
            bool result;
            try
            {
                OleDbConnection adoConn = new OleDbConnection();
                adoConn.ConnectionString = strConn;
                adoConn.Open();
                OleDbCommand cmd = adoConn.CreateCommand();
                OleDbTransaction Trans = adoConn.BeginTransaction();
                cmd.Transaction = Trans;
                Trans.Rollback();
                try
                {
                    logControl.saveLogInf("连接数据库成功");
                    string updatesql = SqlBuilderHelper.UpdateSql<qzj_vehicle>(model, "QZJ_VEHICLE","VEHICLENAME='"+model.VEHICLENAME+"'");
                    OleDbDataAdapter dataAdapter = new OleDbDataAdapter();
                    dataAdapter.InsertCommand = new OleDbCommand(updatesql, adoConn);
                    dataAdapter.InsertCommand.CommandText = updatesql;
                    if (dataAdapter.InsertCommand.ExecuteNonQuery() > 0)
                    {
                        result = true;
                        logControl.saveLogInf("插入数据成功");
                    }
                    else
                    {
                        result = false;
                        logControl.saveLogInf("插入数据失败");
                    }
                    dataAdapter.Dispose();
                }
                catch (Exception er)
                {
                    result = false;
                    logControl.saveLogInf("更新数据发生异常:" + er.Message);
                    Trans.Rollback();
                }
                adoConn.Close();

            }
            catch (Exception er)
            {
                result = false;
                logControl.saveLogInf("链接到数据库失败:" + er.Message);
            }
            return result;
        }
        public static bool deleteVehicle(string vehiclename)
        {
            bool result;
            try
            {
                OleDbConnection adoConn = new OleDbConnection();
                adoConn.ConnectionString = strConn;
                adoConn.Open();
                OleDbCommand cmd = adoConn.CreateCommand();
                OleDbTransaction Trans = adoConn.BeginTransaction();
                cmd.Transaction = Trans;
                Trans.Rollback();
                try
                {
                    logControl.saveLogInf("连接数据库成功");
                    string checksql = "delete from QZJ_VEHICLE where VEHICLENAME='" + vehiclename + "'";
                    OleDbCommand datacheck = new OleDbCommand(checksql, adoConn);
                    if (null != datacheck.ExecuteScalar())
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                catch (Exception er)
                {
                    result = false;
                    logControl.saveLogInf("查询数据发生异常:" + er.Message);
                    Trans.Rollback();

                }
                adoConn.Close();
            }
            catch (Exception er)
            {
                result = false;
                logControl.saveLogInf("链接到数据库失败:" + er.Message);
            }
            return result;
        }
        public static DataTable getRecordList(DateTime starttime,DateTime endtime,bool hasnotupload)
        {
            DataTable dt = null;
            try
            {
                OleDbConnection adoConn = new OleDbConnection();
                adoConn.ConnectionString = strConn;
                adoConn.Open();
                OleDbCommand cmd = adoConn.CreateCommand();
                OleDbTransaction Trans = adoConn.BeginTransaction();
                cmd.Transaction = Trans;
                Trans.Rollback();
                try
                {
                    logControl.saveLogInf("连接数据库成功");
                    string checksql = "";
                    if (hasnotupload) checksql = "select * from QZJ where JCSJ >'" + starttime.ToString("yyyy-MM-dd")+" 00:00:00" + "' and JCSJ <'" + endtime.ToString("yyyy-MM-dd") + " 23:59:59" + "'"+ " and HASUPLOAD='N'"+" order by JCSJ";
                    else  checksql += "select * from QZJ where JCSJ >'" + starttime.ToString("yyyy-MM-dd") + " 00:00:00" + "' and JCSJ <'" + endtime.ToString("yyyy-MM-dd") + " 23:59:59" + "' order by JCSJ"; 
                    OleDbCommand datacheck = new OleDbCommand(checksql, adoConn);
                    OleDbDataReader reader=datacheck.ExecuteReader();
                    if (reader.HasRows)
                    {
                        dt = new DataTable();
                        dt.Columns.Add("CLID");
                        dt.Columns.Add("STATIONID");
                        dt.Columns.Add("LSH");
                        dt.Columns.Add("CLHP");
                        dt.Columns.Add("VEHICLENAME");
                        dt.Columns.Add("MAXSPEED");
                        dt.Columns.Add("JCSJ");
                        dt.Columns.Add("LEFTTURNLEFT");
                        dt.Columns.Add("LEFTTURNRIGHT");
                        dt.Columns.Add("RIGHTTURNLEFT");
                        dt.Columns.Add("RIGHTTURNRIGHT");
                        dt.Columns.Add("XZ");
                        dt.Columns.Add("LEFTTURNLEFTPD");
                        dt.Columns.Add("LEFTTURNRIGHTPD");
                        dt.Columns.Add("RIGHTTURNLEFTPD");
                        dt.Columns.Add("RIGHTTURNRIGHTPD");
                        dt.Columns.Add("ZHPD");
                        dt.Columns.Add("HASUPLOAD");
                        while (reader.Read())
                        {
                            dt.Rows.Add(new object[]{ reader[1].ToString(), reader[2].ToString(),
                                reader[3].ToString(), reader[4].ToString(), reader[5].ToString(), reader[6].ToString(),
                                reader[7].ToString(), reader[8].ToString(), reader[9].ToString(), reader[10].ToString(),
                            reader[11].ToString(), reader[12].ToString(), reader[13].ToString(), reader[14].ToString(),
                                reader[15].ToString(), reader[16].ToString(), reader[17].ToString(), reader[18].ToString()});
                        }
                        //return dt;
                    }
                }
                catch (Exception er)
                {
                    logControl.saveLogInf("查询数据发生异常:" + er.Message);
                    Trans.Rollback();
                    
                }
                adoConn.Close();
            }
            catch (Exception er)
            {
                logControl.saveLogInf("链接到驰达数据库失败:" + er.Message);
            }
            return dt;
        }

        public static DataTable getVehicleList()
        {
            DataTable dt = null;
            try
            {
                OleDbConnection adoConn = new OleDbConnection();
                adoConn.ConnectionString = strConn;
                adoConn.Open();
                OleDbCommand cmd = adoConn.CreateCommand();
                OleDbTransaction Trans = adoConn.BeginTransaction();
                cmd.Transaction = Trans;
                Trans.Rollback();
                try
                {
                    logControl.saveLogInf("连接数据库成功");
                    string checksql = "select * from QZJ_VEHICLE";
                    OleDbCommand datacheck = new OleDbCommand(checksql, adoConn);
                    OleDbDataReader reader = datacheck.ExecuteReader();
                    if (reader.HasRows)
                    {
                        dt = new DataTable();
                        dt.Columns.Add("VEHICLENAME");
                        dt.Columns.Add("LEFTTURNLEFT");
                        dt.Columns.Add("LEFTTURNRIGHT");
                        dt.Columns.Add("RIGHTTURNLEFT");
                        dt.Columns.Add("RIGHTTURNRIGHT");
                        dt.Columns.Add("LEFTTURNLEFTWC");
                        dt.Columns.Add("LEFTTURNRIGHTWC");
                        dt.Columns.Add("RIGHTTURNLEFTWC");
                        dt.Columns.Add("RIGHTTURNRIGHTWC");
                        while (reader.Read())
                        {
                            dt.Rows.Add(new object[]{reader[1].ToString(), reader[2].ToString(), reader[3].ToString(),
                                reader[4].ToString(), reader[5].ToString(), reader[6].ToString(), reader[7].ToString(),
                                reader[8].ToString(), reader[9].ToString()});
                        }
                        //return dt;
                    }
                }
                catch (Exception er)
                {
                    logControl.saveLogInf("查询数据发生异常:" + er.Message);
                    Trans.Rollback();

                }
                adoConn.Close();
            }
            catch (Exception er)
            {
                logControl.saveLogInf("链接到驰达数据库失败:" + er.Message);
            }
            return dt;
        }

        public static bool getVehicleXz(string vehiclename,out qzj_vehicle model)
        {
            model=new qzj_vehicle();
            bool result = false;
            try
            {
                OleDbConnection adoConn = new OleDbConnection();
                adoConn.ConnectionString = strConn;
                adoConn.Open();
                OleDbCommand cmd = adoConn.CreateCommand();
                OleDbTransaction Trans = adoConn.BeginTransaction();
                cmd.Transaction = Trans;
                Trans.Rollback();
                try
                {
                    logControl.saveLogInf("连接数据库成功");
                    string checksql = "select * from QZJ_VEHICLE where VEHICLENAME='"+vehiclename+"'";
                    OleDbCommand datacheck = new OleDbCommand(checksql, adoConn);
                    OleDbDataReader reader = datacheck.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        model.VEHICLENAME = reader["VEHICLENAME"].ToString();
                        model.LEFTTURNLEFT = double.Parse(reader["LEFTTURNLEFT"].ToString());
                        model.LEFTTURNRIGHT = double.Parse(reader["LEFTTURNRIGHT"].ToString());
                        model.RIGHTTURNLEFT = double.Parse(reader["RIGHTTURNLEFT"].ToString());
                        model.RIGHTTURNRIGHT = double.Parse(reader["RIGHTTURNRIGHT"].ToString());
                        model.LEFTTURNLEFTWC = double.Parse(reader["LEFTTURNLEFTWC"].ToString());
                        model.LEFTTURNRIGHTWC = double.Parse(reader["LEFTTURNRIGHTWC"].ToString());
                        model.RIGHTTURNLEFTWC = double.Parse(reader["RIGHTTURNLEFTWC"].ToString());
                        model.RIGHTTURNRIGHTWC = double.Parse(reader["RIGHTTURNRIGHTWC"].ToString());                        
                        result = true;
                        //return dt;
                    }
                }
                catch (Exception er)
                {
                    logControl.saveLogInf("查询数据发生异常:" + er.Message);
                    Trans.Rollback();

                }
                adoConn.Close();
            }
            catch (Exception er)
            {
                logControl.saveLogInf("链接到驰达数据库失败:" + er.Message);
            }
            return result;
        }
    }
    public class qzj_record
    {
        private string _CLID;
        private string _STATIONID;
        private string _LSH;
        private string _CLHP;
        private string _VEHICLENAME;
        private string _MAXSPEED;
        private DateTime _JCSJ;
        private double _LEFTTURNLEFT;
        private double _LEFTTURNRIGHT;
        private double _RIGHTTURNLEFT;
        private double _RIGHTTURNRIGHT;
        private string _XZ;
        private string _LEFTTURNLEFTPD;
        private string _LEFTTURNRIGHTPD;
        private string _RIGHTTURNLEFTPD;
        private string _RIGHTTURNRIGHTPD;
        private string _ZHPD;
        private string _HASUPLOAD;

        public string CLID
        {
            get
            {
                return _CLID;
            }

            set
            {
                _CLID = value;
            }
        }

        public string STATIONID
        {
            get
            {
                return _STATIONID;
            }

            set
            {
                _STATIONID = value;
            }
        }

        public string LSH
        {
            get
            {
                return _LSH;
            }

            set
            {
                _LSH = value;
            }
        }

        public string CLHP
        {
            get
            {
                return _CLHP;
            }

            set
            {
                _CLHP = value;
            }
        }

        public string VEHICLENAME
        {
            get
            {
                return _VEHICLENAME;
            }

            set
            {
                _VEHICLENAME = value;
            }
        }

        public string MAXSPEED
        {
            get
            {
                return _MAXSPEED;
            }

            set
            {
                _MAXSPEED = value;
            }
        }

        public DateTime JCSJ
        {
            get
            {
                return _JCSJ;
            }

            set
            {
                _JCSJ = value;
            }
        }

        public double LEFTTURNLEFT
        {
            get
            {
                return _LEFTTURNLEFT;
            }

            set
            {
                _LEFTTURNLEFT = value;
            }
        }

        public double LEFTTURNRIGHT
        {
            get
            {
                return _LEFTTURNRIGHT;
            }

            set
            {
                _LEFTTURNRIGHT = value;
            }
        }

        public double RIGHTTURNLEFT
        {
            get
            {
                return _RIGHTTURNLEFT;
            }

            set
            {
                _RIGHTTURNLEFT = value;
            }
        }

        public double RIGHTTURNRIGHT
        {
            get
            {
                return _RIGHTTURNRIGHT;
            }

            set
            {
                _RIGHTTURNRIGHT = value;
            }
        }

        public string XZ
        {
            get
            {
                return _XZ;
            }

            set
            {
                _XZ = value;
            }
        }

        public string LEFTTURNLEFTPD
        {
            get
            {
                return _LEFTTURNLEFTPD;
            }

            set
            {
                _LEFTTURNLEFTPD = value;
            }
        }

        public string LEFTTURNRIGHTPD
        {
            get
            {
                return _LEFTTURNRIGHTPD;
            }

            set
            {
                _LEFTTURNRIGHTPD = value;
            }
        }

        public string RIGHTTURNLEFTPD
        {
            get
            {
                return _RIGHTTURNLEFTPD;
            }

            set
            {
                _RIGHTTURNLEFTPD = value;
            }
        }

        public string RIGHTTURNRIGHTPD
        {
            get
            {
                return _RIGHTTURNRIGHTPD;
            }

            set
            {
                _RIGHTTURNRIGHTPD = value;
            }
        }

        public string ZHPD
        {
            get
            {
                return _ZHPD;
            }

            set
            {
                _ZHPD = value;
            }
        }

        public string HASUPLOAD
        {
            get
            {
                return _HASUPLOAD;
            }

            set
            {
                _HASUPLOAD = value;
            }
        }
    }
    public class qzj_vehicle
    {
        private string _VEHICLENAME;
        public string VEHICLENAME
        {
            get { return _VEHICLENAME; }
            set { _VEHICLENAME = value; }
        }
        private double _LEFTTURNLEFT;
        public double LEFTTURNLEFT
        {
            get { return _LEFTTURNLEFT; }
            set { _LEFTTURNLEFT = value; }
        }
        private double _LEFTTURNRIGHT;
        public double LEFTTURNRIGHT
        {
            get { return _LEFTTURNRIGHT; }
            set { _LEFTTURNRIGHT = value; }
        }
        private double _RIGHTTURNLEFT;
        public double RIGHTTURNLEFT
        {
            get { return _RIGHTTURNLEFT; }
            set { _RIGHTTURNLEFT = value; }
        }
        private double _RIGHTTURNRIGHT;
        public double RIGHTTURNRIGHT
        {
            get { return _RIGHTTURNRIGHT; }
            set { _RIGHTTURNRIGHT = value; }
        }
        private double _LEFTTURNLEFTWC;
        public double LEFTTURNLEFTWC
        {
            get { return _LEFTTURNLEFTWC; }
            set { _LEFTTURNLEFTWC = value; }
        }
        private double _LEFTTURNRIGHTWC;
        public double LEFTTURNRIGHTWC
        {
            get { return _LEFTTURNRIGHTWC; }
            set { _LEFTTURNRIGHTWC = value; }
        }
        private double _RIGHTTURNLEFTWC;
        public double RIGHTTURNLEFTWC
        {
            get { return _RIGHTTURNLEFTWC; }
            set { _RIGHTTURNLEFTWC = value; }
        }
        private double _RIGHTTURNRIGHTWC;
        public double RIGHTTURNRIGHTWC
        {
            get { return _RIGHTTURNRIGHTWC; }
            set { _RIGHTTURNRIGHTWC = value; }
        }
    }
    /// <summary>
    /// 对象拼接sql语句
    /// </summary>
    public class SqlBuilderHelper
    {
        /// <summary>
        /// Insert SQL语句
        /// </summary>
        /// <param name="obj">要转换的对象，不可空</param>
        /// <param name="tableName">要添加的表明，不可空</param>
        /// <returns>
        /// 空
        /// sql语句
        /// </returns>
        public static string InsertSql<T>(T t, string tableName) where T : class
        {
            if (t == null || string.IsNullOrEmpty(tableName))
            {
                return string.Empty;
            }
            string columns = GetColmons(t);
            if (string.IsNullOrEmpty(columns))
            {
                return string.Empty;
            }
            string values = GetValues(t);
            if (string.IsNullOrEmpty(values))
            {
                return string.Empty;
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("Insert into " + tableName);
            sql.Append("(" + columns + ")");
            sql.Append(" values(" + values + ")");
            return sql.ToString();
        }
        public static string UpdateSql<T>(T t, string tableName,string where) where T : class
        {
            if (t == null || string.IsNullOrEmpty(tableName))
            {
                return string.Empty;
            }
            string columns = GetColmons(t);
            if (string.IsNullOrEmpty(columns))
            {
                return string.Empty;
            }
            string values = GetValues(t);
            if (string.IsNullOrEmpty(values))
            {
                return string.Empty;
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("update " + tableName);
            sql.Append(" set ");
            string[] valuepara = values.Split(',');
            string[] colpara = columns.Split(',');
            for(int i=0;i<colpara.Count();i++)
            {
                sql.Append(colpara[i]+"=" + valuepara[i]);
                if (i < colpara.Count()-1)
                    sql.Append(",");
            }
            sql.Append(" where "+where);
            return sql.ToString();
        }
        /// <summary>
        /// BulkInsert SQL语句（批量添加）
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="objs">要转换的对象集合，不可空</param>
        /// <param name="tableName">>要添加的表明，不可空</param>
        /// <returns>
        /// 空
        /// sql语句
        /// </returns>
        public static string BulkInsertSql<T>(List<T> objs, string tableName) where T : class
        {
            if (objs == null || objs.Count == 0 || string.IsNullOrEmpty(tableName))
            {
                return string.Empty;
            }
            string columns = GetColmons(objs[0]);
            if (string.IsNullOrEmpty(columns))
            {
                return string.Empty;
            }
            string values = string.Join(",", objs.Select(p => string.Format("({0})", GetValues(p))).ToArray());
            StringBuilder sql = new StringBuilder();
            sql.Append("Insert into " + tableName);
            sql.Append("(" + columns + ")");
            sql.Append(" values " + values + "");
            return sql.ToString();
        }

        /// <summary>
        /// 获得类型的列名
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static string GetColmons<T>(T obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            return string.Join(",", obj.GetType().GetProperties().Select(p => p.Name).ToList());
        }
        private static string GetColmons2<T>(T obj)
        {
            string temp = "";
            Type t =obj.GetType();
            foreach(System.Reflection.PropertyInfo pi in t.GetProperties())
            {
                string name = pi.Name;
                temp += name + ",";
            }
            /*System.Reflection.MemberInfo[] minfos = type.GetMembers();
            foreach(System.Reflection.MemberInfo m in minfos)
            {
                temp+=m.Name+",";
            }*/
            temp = temp.Remove(temp.Length - 1, 1);
            /*
            if (obj == null)
            {
                return string.Empty;
            }*/
            return temp;
           // return string.Join(",", temp).ToList();
        }
        private static string GetValues2<T>(T obj)
        {
            string temp = "";
            Type t = obj.GetType();
            foreach (System.Reflection.PropertyInfo pi in t.GetProperties())
            {
                object value = pi.GetValue(obj,null);
                temp += value + ",";
            }
            /*System.Reflection.MemberInfo[] minfos = type.GetMembers();
            foreach(System.Reflection.MemberInfo m in minfos)
            {
                temp+=m.Name+",";
            }*/
            temp = temp.Remove(temp.Length - 1, 1);
            /*
            if (obj == null)
            {
                return string.Empty;
            }*/
            return temp;
        }
        /// <summary>
        /// 获得值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static string GetValues<T>(T obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            return string.Join(",", obj.GetType().GetProperties().Select(p => string.Format("'{0}'", p.GetValue(obj))).ToArray());
        }
    }
}
