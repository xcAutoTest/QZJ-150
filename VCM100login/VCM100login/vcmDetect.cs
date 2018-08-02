using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SYS_MODEL;
using VCM100login;
//using SYS_DAL;
using System.Data.SqlClient;
using System.Data;

namespace VCM100login
{
    public partial class vcmLogin
    {          
           
            #region 保存车辆被检信息
            public bool saveCarDetect(recordModel model)
            {
                int i = 0;
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into QZJ (");
                strSql.Append("CLID,");//=@PZLX,");1
                strSql.Append("STATIONID,");//=@PZLX,");1
                strSql.Append("LSH,");//=@PZLX,");1
                strSql.Append("CLHP,");//=@PZLX,");1
                strSql.Append("CLLX,");//=@PZLX,");1
                strSql.Append("MAXSPEED,");//=@PZLX,");1
                strSql.Append("JCSJ,");//=@PZLX,");1
                strSql.Append("LEFTTURNLEFT,");//=@PZLX,");1
                strSql.Append("LEFTTURNRIGHT,");//=@PZLX,");1
                strSql.Append("RIGHTTURNLEFT,");//=@PZLX,");1
                strSql.Append("RIGHTTURNRIGHT,");//=@PZLX,");1
                strSql.Append("XZ,");//=@PZLX,");1
                strSql.Append("LEFTTURNLEFTPD,");//=@PZLX,");1
                strSql.Append("LEFTTURNRIGHTPD,");//=@PZLX,");1
                strSql.Append("RIGHTTURNLEFTPD,");//=@PZLX,");1
                strSql.Append("RIGHTTURNRIGHTPD,");//=@PZLX,");1
                strSql.Append("ZHPD) ");//=@CLXHBH,");3
                strSql.Append("values (@CLID,@STATIONID,@LSH,@CLHP,@CLLX,@MAXSPEED,@JCSJ,@LEFTTURNLEFT,@LEFTTURNRIGHT,@RIGHTTURNLEFT,@RIGHTTURNRIGHT,@XZ,@LEFTTURNLEFTPD,@LEFTTURNRIGHTPD,@RIGHTTURNLEFTPD,@RIGHTTURNRIGHTPD,@ZHPD)");
                SqlParameter[] parameters =
                {
                        new SqlParameter("@CLID", SqlDbType.VarChar,50),
                        new SqlParameter("@STATIONID", SqlDbType.VarChar,50),
                        new SqlParameter("@LSH", SqlDbType.VarChar,50),
                        new SqlParameter("@CLHP", SqlDbType.VarChar,50),
                        new SqlParameter("@CLLX", SqlDbType.VarChar,50),
                        new SqlParameter("@MAXSPEED", SqlDbType.VarChar,50),
                        new SqlParameter("@JCSJ", SqlDbType.DateTime),
                        new SqlParameter("@LEFTTURNLEFT", SqlDbType.VarChar,50),
                        new SqlParameter("@LEFTTURNRIGHT", SqlDbType.VarChar,50),
                        new SqlParameter("@RIGHTTURNLEFT", SqlDbType.VarChar,50),
                        new SqlParameter("@RIGHTTURNRIGHT", SqlDbType.VarChar,50),
                        new SqlParameter("@XZ", SqlDbType.VarChar,50),
                        new SqlParameter("@LEFTTURNLEFTPD", SqlDbType.VarChar,50),
                        new SqlParameter("@LEFTTURNRIGHTPD", SqlDbType.VarChar,50),
                        new SqlParameter("@RIGHTTURNLEFTPD", SqlDbType.VarChar,50),
                        new SqlParameter("@RIGHTTURNRIGHTPD", SqlDbType.VarChar,50),
                        new SqlParameter("@ZHPD", SqlDbType.VarChar,50)};
            
                parameters[i++].Value = model.CLID;
                parameters[i++].Value = model.STATIONID;
                parameters[i++].Value = model.LSH;
                parameters[i++].Value = model.CLHP;
                parameters[i++].Value = model.CLLX;
                parameters[i++].Value = model.MAXSPEED;
                parameters[i++].Value = model.JCSJ;
                parameters[i++].Value = model.LEFTTURNLEFT;
                parameters[i++].Value = model.LEFTTURNRIGHT;
                parameters[i++].Value = model.RIGHTTURNLEFT;
                parameters[i++].Value = model.RIGHTTURNRIGHT;
                parameters[i++].Value = model.XZ;
                parameters[i++].Value = model.LEFTTURNLEFTPD;
                parameters[i++].Value = model.LEFTTURNRIGHTPD;
                parameters[i++].Value = model.RIGHTTURNLEFTPD;
                parameters[i++].Value = model.RIGHTTURNRIGHTPD;
                parameters[i++].Value = model.ZHPD;
                try
                {
                    if (checkLSHIsAlive(model.LSH, "检测记录"))
                    {
                        if (UpdateCarDetect(model))
                        {
                            ini.INIIO.saveLogInf("saveCarDetect(" + model.CLHP + "):" + "true");
                            return true;
                        }
                        else
                        {
                            ini.INIIO.saveLogInf("saveCarDetect(" + model.CLHP + "):" + "false");
                            return false;
                        }
                    }
                    else
                    {
                        if (DBHelperSQL.Execute(strSql.ToString(), parameters) > 0)
                        {
                            ini.INIIO.saveLogInf("saveCarDetect(" + model.CLHP + "):" + "true");
                            return true;
                        }
                        else
                        {
                            ini.INIIO.saveLogInf("saveCarDetect(" + model.CLHP + "):" + "false");
                            return false;
                        }
                    }
                }
                catch (Exception)
                {
                    ini.INIIO.saveLogInf("saveCarDetect(" + model.CLID + "):" + "异常");
                    throw;
                }

            }
            #endregion
            #region 更新车辆待检信息
            public bool UpdateCarDetect(recordModel model)
            {
                int i = 0;
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update QZJ set ");
                strSql.Append("STATIONID=@STATIONID,");//=@PZLX,");1
                strSql.Append("LSH=@LSH,");//=@PZLX,");1
                strSql.Append("CLHP=@CLHP,");//=@PZLX,");1
                strSql.Append("CLLX=@CLLX,");//=@PZLX,");1
                strSql.Append("MAXSPEED=@MAXSPEED,");//=@PZLX,");1
                strSql.Append("JCSJ=@JCSJ,");//=@PZLX,");1
                strSql.Append("LEFTTURNLEFT=@LEFTTURNLEFT,");//=@PZLX,");1
                strSql.Append("LEFTTURNRIGHT=@LEFTTURNRIGHT,");//=@PZLX,");1
                strSql.Append("RIGHTTURNLEFT=@RIGHTTURNLEFT,");//=@PZLX,");1
                strSql.Append("RIGHTTURNRIGHT=@RIGHTTURNRIGHT,");//=@PZLX,");1
                strSql.Append("XZ=@XZ,");//=@PZLX,");1
                strSql.Append("LEFTTURNLEFTPD=@LEFTTURNLEFTPD,");//=@PZLX,");1
                strSql.Append("LEFTTURNRIGHTPD=@LEFTTURNRIGHTPD,");//=@PZLX,");1
                strSql.Append("RIGHTTURNLEFTPD=@RIGHTTURNLEFTPD,");//=@PZLX,");1
                strSql.Append("RIGHTTURNRIGHTPD=@RIGHTTURNRIGHTPD,");//=@PZLX,");1
                strSql.Append("ZHPD=@ZHPD");//=@PZLX,");1
                strSql.Append(" where CLID='" + model.CLID + "'");
                SqlParameter[] parameters =
                {
                        new SqlParameter("@STATIONID", SqlDbType.VarChar,50),
                        new SqlParameter("@LSH", SqlDbType.VarChar,50),
                        new SqlParameter("@CLHP", SqlDbType.VarChar,50),
                        new SqlParameter("@CLLX", SqlDbType.VarChar,50),
                        new SqlParameter("@MAXSPEED", SqlDbType.VarChar,50),
                        new SqlParameter("@JCSJ", SqlDbType.DateTime),
                        new SqlParameter("@LEFTTURNLEFT", SqlDbType.VarChar,50),
                        new SqlParameter("@LEFTTURNRIGHT", SqlDbType.VarChar,50),
                        new SqlParameter("@RIGHTTURNLEFT", SqlDbType.VarChar,50),
                        new SqlParameter("@RIGHTTURNRIGHT", SqlDbType.VarChar,50),
                        new SqlParameter("@XZ", SqlDbType.VarChar,50),
                        new SqlParameter("@LEFTTURNLEFTPD", SqlDbType.VarChar,50),
                        new SqlParameter("@LEFTTURNRIGHTPD", SqlDbType.VarChar,50),
                        new SqlParameter("@RIGHTTURNLEFTPD", SqlDbType.VarChar,50),
                        new SqlParameter("@RIGHTTURNRIGHTPD", SqlDbType.VarChar,50),
                        new SqlParameter("@ZHPD", SqlDbType.VarChar,50)};

                
                parameters[i++].Value = model.STATIONID;
                parameters[i++].Value = model.LSH;
                parameters[i++].Value = model.CLHP;
                parameters[i++].Value = model.CLLX;
                parameters[i++].Value = model.MAXSPEED;
                parameters[i++].Value = model.JCSJ;
                parameters[i++].Value = model.LEFTTURNLEFT;
                parameters[i++].Value = model.LEFTTURNRIGHT;
                parameters[i++].Value = model.RIGHTTURNLEFT;
                parameters[i++].Value = model.RIGHTTURNRIGHT;
                parameters[i++].Value = model.XZ;
                parameters[i++].Value = model.LEFTTURNLEFTPD;
                parameters[i++].Value = model.LEFTTURNRIGHTPD;
                parameters[i++].Value = model.RIGHTTURNLEFTPD;
                parameters[i++].Value = model.RIGHTTURNRIGHTPD;
                parameters[i++].Value = model.ZHPD;
                try
                {
                    int rows = DBHelperSQL.Execute(strSql.ToString(), parameters);
                    if (rows > 0)
                    {
                        ini.INIIO.saveLogInf("UpdateCarDetect(" + model.CLHP + "):" + "true");
                        return true;
                    }
                    else
                    {
                        ini.INIIO.saveLogInf("UpdateCarDetect(" + model.CLHP + "):" + "false");
                        return false;
                    }
                }
                catch (Exception)
                {
                    ini.INIIO.saveLogInf("UpdateCarDetect(" + model.CLHP + "):" + "异常");
                    throw;
                }
            }
            #endregion
            #region 获取某一辆车的所有检测信息
            /// <summary>
            /// 获取所有检测线的信息
            /// </summary>
            public DataTable getCarDetectedByPlate(string id, string lx, string plate)
            {
                string sql = "";
                switch (lx)
                {
                    case "当日":
                        sql = "select * from QZJ  where STATIONID='" + id + "' and  CLHP=" + "'" + plate + "'" + " and DATEPART(dd, JCSJ)=DATEPART(dd, GETDATE()) and DATEPART(mm, JCSJ)=DATEPART(mm, GETDATE()) and DATEPART(yy, JCSJ) = DATEPART(yy, GETDATE()) order by JCSJ desc";
                        break;
                    case "所有":
                        sql = "select * from QZJ  where STATIONID='" + id + "' and  CLHP=" + "'" + plate + "'" + " order by JCSJ desc";
                        break;
                }
                DataTable dt = null;
                try
                {
                    dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                    return dt;
                }
                catch
                {
                    throw;
                }
            }
            #endregion
            #region 获取所有已检车辆信息
            /// <summary>
            /// 获取所有检测线的信息
            /// </summary>
            public DataTable getAllCarDetected(string id, string lx)
            {
                string sql = "";
                switch (lx)
                {
                    case "当日":
                        sql = "select * from QZJ where STATIONID='" + id + "' and DATEPART(dd, JCSJ)=DATEPART(dd, GETDATE()) and DATEPART(mm, JCSJ)=DATEPART(mm, GETDATE()) and DATEPART(yy, JCSJ) = DATEPART(yy, GETDATE())";
                        //sql = "select * from [已检车辆信息] where JCSJ>="+DateTime.Now.ToShortDateString()+"order by JCSJ desc";
                        break;
                    case "当月":
                        sql = "select * from QZJ  where STATIONID='" + id + "' and  DATEPART(mm, JCSJ)=DATEPART(mm, GETDATE()) and DATEPART(yy, JCSJ) = DATEPART(yy, GETDATE())";
                        break;
                    case "当周":
                        sql = "select * from QZJ  where STATIONID='" + id + "' and  DATEPART(wk, JCSJ) = DATEPART(wk, GETDATE()) and DATEPART(yy, JCSJ) = DATEPART(yy, GETDATE())";
                        break;
                    default:
                        sql = "select * from QZJ  where STATIONID='" + id + "' and  DATEPART(dd, JCSJ)=DATEPART(dd, GETDATE()) and DATEPART(mm, JCSJ)=DATEPART(mm, GETDATE()) and DATEPART(yy, JCSJ) = DATEPART(yy, GETDATE())";
                        break;
                }
                DataTable dt = null;
                try
                {
                    dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                    return dt;
                }
                catch
                {
                    throw;
                }
            }
            #endregion
            #region 获取所有检测记录信息
            /// <summary>
            /// 获取所有检测线的信息
            /// </summary>
            public DataTable getAllCarDetect(string id)
            {
                string sql = "select * from QZJ where STATIONID='" + id + "' order by JCSJ desc";
                DataTable dt = null;
                try
                {
                    dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                    ini.INIIO.saveLogInf("getAllCarDetect(" + id + "):" + "成功");
                    return dt;
                }
                catch
                {
                    ini.INIIO.saveLogInf("getAllCarDetect(" + id + "):" + "异常");
                    throw;
                }
            }
            #endregion
            #region 获取所有检测记录信息
            /// <summary>
            /// 获取所有检测线的信息
            /// </summary>
            public DataSet getAllCarDetectDataSet(string id)
            {
                string sql = "select * from QZJ where STATIONID='" + id + "' order by JCSJ desc ";
                DataSet dt = null;
                try
                {
                    dt = DBHelperSQL.GetDataSet(sql, CommandType.Text);
                    ini.INIIO.saveLogInf("getAllCarDetectDataSet(" + id + "):" + "成功");
                    return dt;
                }
                catch
                {
                    ini.INIIO.saveLogInf("getAllCarDetectDataSet(" + id + "):" + "异常");
                    throw;
                }
            }
            #endregion
        
            #region 获取某一辆车的待检测信息
            /// <summary>
            /// 获取所有检测线的信息
            /// </summary>
            public DataTable getCarDetectByPlate(string id,string plate)
            {
                string sql="";
                sql = "select * from QZJ  where STATIONID='" + id + "' and  CLHP=" + "'" + plate + "'";
                DataTable dt = null;
                try
                {
                    dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                    ini.INIIO.saveLogInf("getCarDetectByPlate(" + id + "," + plate + "):" + "成功");
                    return dt;
                }
                catch
                {
                    ini.INIIO.saveLogInf("getCarDetectByPlate(" + id + "," + plate + "):" + "异常");
                    throw;
                }
            }
            #endregion

            #region 获取待检列表中某一辆车的检测信息
            public recordModel getCarInfatDetectList(string clhp)
            {
                recordModel model = new recordModel();
                string sql = "select * from QZJ where CLHP='" + clhp + "' order by JCSJ desc";
                try
                {
                    DataTable dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                    if (dt.Rows.Count > 0)
                    {
                        model.CLID = dt.Rows[0]["CLID"].ToString();
                        model.STATIONID = dt.Rows[0]["STATIONID"].ToString();
                        model.LSH = dt.Rows[0]["LSH"].ToString();
                        model.CLHP = dt.Rows[0]["CLHP"].ToString();
                        model.CLLX = dt.Rows[0]["CLLX"].ToString();
                        model.MAXSPEED = dt.Rows[0]["MAXSPEED"].ToString();
                        model.JCSJ = DateTime.Parse(dt.Rows[0]["JCSJ"].ToString());
                        model.LEFTTURNLEFT = dt.Rows[0]["LEFTTURNLEFT"].ToString();
                        model.LEFTTURNRIGHT = dt.Rows[0]["LEFTTURNRIGHT"].ToString();
                        model.RIGHTTURNLEFT = dt.Rows[0]["RIGHTTURNLEFT"].ToString();
                        model.RIGHTTURNRIGHT = dt.Rows[0]["RIGHTTURNRIGHT"].ToString();
                        model.XZ = dt.Rows[0]["XZ"].ToString();
                        model.LEFTTURNLEFTPD = dt.Rows[0]["LEFTTURNLEFTPD"].ToString();
                        model.LEFTTURNRIGHTPD = dt.Rows[0]["LEFTTURNRIGHTPD"].ToString();
                        model.RIGHTTURNLEFTPD = dt.Rows[0]["RIGHTTURNLEFTPD"].ToString();
                        model.RIGHTTURNRIGHTPD = dt.Rows[0]["RIGHTTURNRIGHTPD"].ToString();
                        model.ZHPD = dt.Rows[0]["ZHPD"].ToString();
                        ini.INIIO.saveLogInf("getCarInfatDetectList(" + clhp + "):" + "找到信息");
                    }
                    else
                    {
                        ini.INIIO.saveLogInf("getCarInfatDetectList(" + clhp + "):" + "未找到信息");
                        model.CLID = "-2";       //当服务器上没有找到本线时，本线编号置为-2，以免为0
                    }
                }
                catch (Exception)
                {
                    ini.INIIO.saveLogInf("getCarInfatDetectList(" + clhp + "):" + "异常");
                    throw;
                }
                return model;

            }
            #endregion
            #region 根据流水号获取待检列表中某一辆车的检测信息
            public recordModel getCarInfatDetectListByLSH(string lsh)
            {
                recordModel model = new recordModel();
                string sql = "select * from QZJ where LSH='" + lsh + "'";
                try
                {
                    DataTable dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                    if (dt.Rows.Count > 0)
                    {
                        model.CLID = dt.Rows[0]["CLID"].ToString();
                        model.STATIONID = dt.Rows[0]["STATIONID"].ToString();
                        model.LSH = dt.Rows[0]["LSH"].ToString();
                        model.CLHP = dt.Rows[0]["CLHP"].ToString();
                        model.CLLX = dt.Rows[0]["CLLX"].ToString();
                        model.MAXSPEED = dt.Rows[0]["MAXSPEED"].ToString();
                        model.JCSJ = DateTime.Parse(dt.Rows[0]["JCSJ"].ToString());
                        model.LEFTTURNLEFT = dt.Rows[0]["LEFTTURNLEFT"].ToString();
                        model.LEFTTURNRIGHT = dt.Rows[0]["LEFTTURNRIGHT"].ToString();
                        model.RIGHTTURNLEFT = dt.Rows[0]["RIGHTTURNLEFT"].ToString();
                        model.RIGHTTURNRIGHT = dt.Rows[0]["RIGHTTURNRIGHT"].ToString();
                        model.XZ = dt.Rows[0]["XZ"].ToString();
                        model.LEFTTURNLEFTPD = dt.Rows[0]["LEFTTURNLEFTPD"].ToString();
                        model.LEFTTURNRIGHTPD = dt.Rows[0]["LEFTTURNRIGHTPD"].ToString();
                        model.RIGHTTURNLEFTPD = dt.Rows[0]["RIGHTTURNLEFTPD"].ToString();
                        model.RIGHTTURNRIGHTPD = dt.Rows[0]["RIGHTTURNRIGHTPD"].ToString();
                        model.ZHPD = dt.Rows[0]["ZHPD"].ToString();
                        ini.INIIO.saveLogInf("getCarInfatDetectListByLSH(" + lsh + "):" + "找到信息");
                    }
                    else
                    {
                        ini.INIIO.saveLogInf("getCarInfatDetectListByLSH(" + lsh + "):" + "未找到信息");
                        model.CLID = "-2";       //当服务器上没有找到本线时，本线编号置为-2，以免为0
                    }
                }
                catch (Exception)
                {
                    ini.INIIO.saveLogInf("getCarInfatDetectListByLSH(" + lsh + "):" + "异常");
                    throw;
                }
                return model;

            }
            #endregion
            #region 获取该号牌待检车的车辆ID
            public string getCarIDatDetectbyPlate(string plate)
            {
                string sql = "select CLID from QZJ where CLHP='" + plate + "'";
                DataTable dt = null;
                try
                {
                    dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                    if (dt.Rows.Count > 0)
                    {
                        ini.INIIO.saveLogInf("getCarIDatDetectbyPlate(" + plate + "):" + "找到信息");
                        return dt.Rows[0]["CLID"].ToString();
                    }
                    else
                    {
                        ini.INIIO.saveLogInf("getCarIDatDetectbyPlate(" + plate + "):" + "未找到信息");
                        return "-2";
                    }
                }
                catch
                {
                    ini.INIIO.saveLogInf("getCarIDatDetectbyPlate(" + plate + "):" + "异常");
                    throw;
                }
            }
            #endregion
            #region 删除该号牌的检测记录
            /// <summary>
            /// 获取所有检测线的信息
            /// </summary>
            public bool deleteCarDetectbyPlate(string plate)
            {
                string sql = "delete from QZJ where CLHP='" + plate + "'";
                try
                {
                    if (DBHelperSQL.GetDataTable(sql, CommandType.Text).Rows.Count > 0)
                    {
                        ini.INIIO.saveLogInf("deleteCarDetectbyPlate(" + plate + "):" + "删除成功");
                        return true;
                    }
                    else
                    {
                        ini.INIIO.saveLogInf("deleteCarDetectbyPlate(" + plate + "):" + "删除失败");
                        return false;
                    }
                }
                catch
                {
                    ini.INIIO.saveLogInf("deleteCarDetectbyPlate(" + plate + "):" + "异常");
                    throw;
                }
            }
            #endregion
            #region 获取该检测站的已检车辆
            /// <summary>
            /// 获取所有检测线的信息
            /// stationid:站号，lineid:线号，0表示所有线，lx:0——全部，1——当年，2——当月，3——当天,result:检测结果：-1—不合格，1-合格，0-不限
            /// </summary>
            public DataSet getStationLineDetectedDataset(string id, string lx, string result,string clhp)
            {
                string sql = "";
                sql = "select * from QZJ where STATIONID='" + id + "' and CLHP LIKE '%" + clhp + "%'";//+ "' and CLHP LIKE '" + plate + "'";
                switch (lx)
                {
                    case "0":
                        break;
                    case "1":
                        sql += " and DATEPART(yy, JCSJ)=DATEPART(yy, GETDATE())";
                        break;
                    case "2":
                        sql += " and DATEPART(mm, JCSJ)=DATEPART(mm, GETDATE()) and DATEPART(yy, JCSJ) = DATEPART(yy, GETDATE())";
                        break;
                    case "3":
                        sql += " and DATEPART(dd, JCSJ)=DATEPART(dd, GETDATE()) and DATEPART(mm, JCSJ)=DATEPART(mm, GETDATE()) and DATEPART(yy, JCSJ) = DATEPART(yy, GETDATE())";
                        break;
                    default: break;
                }
                switch (result)
                {
                    case "0": break;
                    case "-1":
                        if (lx == "0")
                            sql += " and ZHPD='FAIL'";
                        else
                            sql += " and ZHPD='FAIL'";
                        break;
                    case "1":
                        if (lx == "0")
                            sql += " and ZHPD='PASS'";
                        else
                            sql += " and ZHPD='PASS'";
                        break;
                    default: break;
                }

                DataSet dt = null;
                try
                {
                    dt = DBHelperSQL.GetDataSet(sql, CommandType.Text);
                    return dt;
                }
                catch
                {
                    return dt;
                    throw;
                }
            }
            #endregion
            #region 获取所有已检车辆信息
            /// <summary>
            /// 获取所有检测线的信息
            /// </summary>
            public DataTable getAllCarDetected(string id, string lx, string result,string clhp,string xh)
            {
                string sql = "";
                sql = "select * from QZJ where STATIONID='" + id + "' and CLHP LIKE '%"+clhp+"%'";//+ "' and CLHP LIKE '" + plate + "'";
                switch (lx)
                {
                    case "0":
                        break;
                    case "1":
                        sql += " and DATEPART(yy, JCSJ)=DATEPART(yy, GETDATE())";
                        break;
                    case "2":
                        sql += " and DATEPART(mm, JCSJ)=DATEPART(mm, GETDATE()) and DATEPART(yy, JCSJ) = DATEPART(yy, GETDATE())";
                        break;
                    case "3":
                        sql += " and DATEPART(dd, JCSJ)=DATEPART(dd, GETDATE()) and DATEPART(mm, JCSJ)=DATEPART(mm, GETDATE()) and DATEPART(yy, JCSJ) = DATEPART(yy, GETDATE())";
                        break;
                    default: break;
                }
                switch (result)
                {
                    case "0": break;
                    case "-1":
                        if (lx == "0")
                            sql += " and ZHPD='FAIL'";
                        else
                            sql += " and ZHPD='FAIL'";
                        break;
                    case "1":
                        if (lx == "0")
                            sql += " and ZHPD='PASS'";
                        else
                            sql += " and ZHPD='PASS'";
                        break;
                    default: break;
                }
                if (xh != "")
                {
                    if (lx == "0" && result=="0")
                        sql += " and CLLX='" + xh + "'";
                    else
                        sql += " and CLLX='" + xh + "'";
                }
                DataTable dt = null;
                try
                {
                    dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                    return dt;
                }
                catch
                {
                    throw;
                }
            }
            #endregion
            #region 获取该检测站的已检车辆
            /// <summary>
            /// 获取所有检测线的信息
            /// stationid:站号，lineid:线号，0表示所有线，startTime:起始时间,endtime:终止时间,plate:车牌,jcff:0-不限,result:检测结果：-1—不合格，1-合格，0-不限,cllx:1-轻型车，2-中型车，3-重型车，0，不限
            /// </summary>
            public DataSet getStationLineDetectedDataset(string id, DateTime starttime, DateTime endtime, string result, string clhp)
            {
                string sql = "";
                sql = "select * from QZJ where STATIONID='" + id + "' and CLHP LIKE '%" + clhp + "%'";//+ "' and CLHP LIKE '" + plate + "'";
                sql += " and JCSJ>='" + starttime.ToShortDateString() + " 00:00:00" + "' and JCSJ<='" + endtime.ToShortDateString() + " 23:59:59" + "'";
                switch (result)
                {
                    case "0": break;
                    case "-1": sql += " and ZHPD='FAIL'"; break;
                    case "1": sql += " and ZHPD='PASS'"; break;
                    default: break;
                }

                DataSet dt = null;
                try
                {
                    dt = DBHelperSQL.GetDataSet(sql, CommandType.Text);
                    return dt;
                }
                catch
                {
                    return dt;
                    throw;
                }
            }
            #endregion
            #region 获取所有已检车辆信息
            /// <summary>
            /// 获取所有检测线的信息
            /// </summary>
            public DataTable getAllCarDetected(string id, DateTime starttime, DateTime endtime, string result,string clhp,string xh)
            {
                string sql = "";
                sql = "select * from QZJ where STATIONID='" + id + "' and CLHP LIKE '%" + clhp + "%'";//+ "' and CLHP LIKE '" + plate + "'";
                sql += " and JCSJ>='" + starttime.ToShortDateString() + " 00:00:00" + "' and JCSJ<='" + endtime.ToShortDateString() + " 23:59:59" + "'";
                switch (result)
                {
                    case "0": break;
                    case "-1": sql += " and ZHPD='FAIL'"; break;
                    case "1": sql += " and ZHPD='PASS'"; break;
                    default: break;
                }
                if (xh != "")
                {
                    sql += " and CLLX='"+xh+"'";
                }
                DataTable dt = null;
                try
                {
                    dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                    return dt;
                }
                catch
                {
                    throw;
                }
            }
            #endregion
            #region 获取该检测站的已检车辆
            /// <summary>
            /// 获取所有检测线的信息
            /// stationid:站号，lineid:线号，0表示所有线，startTime:起始时间,endtime:终止时间,plate:车牌,jcff:0-不限,result:检测结果：-1—不合格，1-合格，0-不限,cllx:1-轻型车，2-中型车，3-重型车，0，不限
            /// </summary>
            public DataSet getStationLineAniDataset(string id, DateTime starttime, DateTime endtime)
            {
                string sql = "";
                sql = "select * from QZJ where STATIONID='" + id + "' and JCSJ>='" + starttime.ToShortDateString() + " 00:00:00" + "' and JCSJ<='" + endtime.ToShortDateString() + " 23:59:59" + "' and JCCS='1'";//+ "' and CLHP LIKE '" + plate + "'";
                DataSet dt = null;
                try
                {
                    dt = DBHelperSQL.GetDataSet(sql, CommandType.Text);
                    return dt;
                }
                catch
                {
                    return dt;
                    throw;
                }
            }
            #endregion

    }
}
