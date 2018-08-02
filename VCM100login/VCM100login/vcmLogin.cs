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
        public bool testSqlLink(out string errmsg)
        {
            errmsg = "";
            string sql = "select * from thisstation";
            try
            {
                DataTable dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                return true; 
            }
            catch (Exception er)
            {
                errmsg = er.Message;
                return false;
            }
        }
        #region 获取该站的stationID
        public string getStationID()
            {
                string sql = "select * from thisstation";
                try
                {
                    DataTable dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                    if (dt.Rows.Count > 0)
                    {
                        ini.INIIO.saveLogInf("getStationID():" + dt.Rows[0]["STATIONID"].ToString());
                        return dt.Rows[0]["STATIONID"].ToString();
                    }
                    else
                        return "-2";       //当服务器上没有找到本线时，本线编号置为-2，以免为0
                }
                catch (Exception)
                {
                    throw;
                }
            }
            #endregion
            #region 检测同ID的车辆是否存在
            public bool checkIDIsAlive(string carID, string listName)
            {
                string sql = "select * from [" + listName + "] where CLID=" + "'" + carID + "'";
                try
                {
                    if (DBHelperSQL.GetDataTable(sql, CommandType.Text).Rows.Count > 0)
                    {
                        ini.INIIO.saveLogInf("checkIsAlive(" + carID + "," + listName + "):" + "true");
                        return true;
                    }
                    else
                    {
                        ini.INIIO.saveLogInf("checkIsAlive(" + carID + "," + listName + "):" + "false");
                        return false;
                    }
                }
                catch (Exception)
                {
                    ini.INIIO.saveLogInf("checkIsAlive(" + carID + "," + listName + "):" + "异常");
                    throw;
                }
            }
            #endregion
            #region 检测同号牌的车辆是否存在
            public bool checkHPIsAlive(string carID, string listName)
            {
                string sql = "select * from [" + listName + "] where CLHP=" + "'" + carID + "'";
                try
                {
                    if (DBHelperSQL.GetDataTable(sql, CommandType.Text).Rows.Count > 0)
                    {
                        ini.INIIO.saveLogInf("checkIsAlive(" + carID + "," + listName + "):" + "true");
                        return true;
                    }
                    else
                    {
                        ini.INIIO.saveLogInf("checkIsAlive(" + carID + "," + listName + "):" + "false");
                        return false;
                    }
                }
                catch (Exception)
                {
                    ini.INIIO.saveLogInf("checkIsAlive(" + carID + "," + listName + "):" + "异常");
                    throw;
                }
            }
            #endregion
            #region 检测该流水号的车辆是否存在
            public bool checkLSHIsAlive(string lsh, string listName)
            {
                string sql = "select * from [" + listName + "] where LSH=" + "'" + lsh + "'";
                try
                {
                    if (DBHelperSQL.GetDataTable(sql, CommandType.Text).Rows.Count > 0)
                    {
                        ini.INIIO.saveLogInf("checkLSHIsAlive(" + lsh + "," + listName + "):" + "true");
                        return true;
                    }
                    else
                    {
                        ini.INIIO.saveLogInf("checkLSHIsAlive(" + lsh + "," + listName + "):" + "false");
                        return false;
                    }
                }
                catch (Exception)
                {
                    ini.INIIO.saveLogInf("checkLSHIsAlive(" + lsh + "," + listName + "):" + "异常");
                    throw;
                }
            }
            #endregion
            #region 保存车辆被检信息
            public bool saveCarWait(carWaitModel model)
            {
                int i = 0;
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into [待检车辆] (");
                strSql.Append("CLID,");//=@PZLX,");1
                strSql.Append("STATIONID,");//=@PZLX,");1
                strSql.Append("CLHP,");//=@PZLX,");1
                strSql.Append("CLLX,");//=@PZLX,");1
                strSql.Append("DLSJ,");//=@PZLX,");1
                strSql.Append("MAXSPEED) ");//=@CLXHBH,");3
                strSql.Append("values (@CLID,@STATIONID,@CLHP,@CLLX,@DLSJ,@MAXSPEED)");
                SqlParameter[] parameters =
                {
                        new SqlParameter("@CLID", SqlDbType.VarChar,50),
                        new SqlParameter("@STATIONID", SqlDbType.VarChar,50),
                        new SqlParameter("@CLHP", SqlDbType.VarChar,50),
                        new SqlParameter("@CLLX", SqlDbType.VarChar,50),
                        new SqlParameter("@DLSJ", SqlDbType.DateTime),
                        new SqlParameter("@MAXSPEED",SqlDbType.VarChar,50)};
            
                parameters[i++].Value = model.CLID;
                parameters[i++].Value = model.STATIONID;
                parameters[i++].Value = model.CLHP;
                parameters[i++].Value = model.CLLX;
                parameters[i++].Value = model.DLSJ;
                parameters[i++].Value = model.MAXSPEED;
                try
                {
                    if (checkHPIsAlive(model.CLID, "待检车辆"))
                    {
                        if (UpdateCarWait(model))
                        {
                            ini.INIIO.saveLogInf("saveCarbj(" + model.CLHP + "):" + "true");
                            return true;
                        }
                        else
                        {
                            ini.INIIO.saveLogInf("saveCarbj(" + model.CLHP + "):" + "false");
                            return false;
                        }
                    }
                    else
                    {
                        if (DBHelperSQL.Execute(strSql.ToString(), parameters) > 0)
                        {
                            ini.INIIO.saveLogInf("saveCarbj(" + model.CLHP + "):" + "true");
                            return true;
                        }
                        else
                        {
                            ini.INIIO.saveLogInf("saveCarbj(" + model.CLHP + "):" + "false");
                            return false;
                        }
                    }
                }
                catch (Exception)
                {
                    ini.INIIO.saveLogInf("saveCarbj(" + model.CLID + "):" + "异常");
                    throw;
                }

            }
            #endregion
            #region 更新车辆待检信息
            public bool UpdateCarWait(carWaitModel model)
            {
                int i = 0;
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update [待检车辆] set ");
                strSql.Append("STATIONID=@STATIONID,");
                strSql.Append("CLID=@CLID,");
                strSql.Append("CLLX=@CLLX,");
                strSql.Append("DLSJ=@DLSJ,");
                strSql.Append("WIDTHBZ=@WIDTHBZ,");
                strSql.Append("LENGTHBZ=@LENGTHBZ,");
                strSql.Append("HEIGHTBZ=@HEIGHTBZ,");
                strSql.Append("LBBZ=@LBBZ,");
                strSql.Append("HXWIDTHBZ=@HXWIDTHBZ,");
                strSql.Append("HXLENGTHBZ=@HXLENGTHBZ,");
                strSql.Append("HXHEIGHTBZ=@HXHEIGHTBZ,");
                strSql.Append("CZ=@CZ,");//=@PZLX,");
                strSql.Append("YL1=@YL1,");//=@SYQK,");14
                strSql.Append("VIN=@VIN,");//=@JCBJ,");15
                strSql.Append("YL3=@YL3,");//=@JCZT,");16
                strSql.Append("YL4=@YL4,");//=@QRJCFF");17
                strSql.Append("YL5=@YL5");//=@RYLX,");18
                strSql.Append(" where CLHP='" + model.CLHP + "'");
                SqlParameter[] parameters =
                {
                        new SqlParameter("@STATIONID", SqlDbType.VarChar,50),
                        new SqlParameter("@CLID", SqlDbType.VarChar,50),
                        new SqlParameter("@CLLX", SqlDbType.VarChar,50),
                        new SqlParameter("@DLSJ", SqlDbType.DateTime),
                        new SqlParameter("@WIDTHBZ", SqlDbType.VarChar,50),
                        new SqlParameter("@LENGTHBZ", SqlDbType.VarChar,50),
                        new SqlParameter("@HEIGHTBZ", SqlDbType.VarChar,50),
                        new SqlParameter("@LBBZ", SqlDbType.VarChar,50),
                        new SqlParameter("@HXWIDTHBZ", SqlDbType.VarChar,50),
                        new SqlParameter("@HXLENGTHBZ", SqlDbType.VarChar,50),
                        new SqlParameter("@HXHEIGHTBZ", SqlDbType.VarChar,50),
                        new SqlParameter("@CZ", SqlDbType.VarChar,50),
                        
					    new SqlParameter("@YL1", SqlDbType.VarChar,50),
					    new SqlParameter("@VIN", SqlDbType.VarChar,50),
					    new SqlParameter("@YL3", SqlDbType.VarChar,50),
                        new SqlParameter("@YL4", SqlDbType.VarChar,50),
                        new SqlParameter("@YL5", SqlDbType.VarChar,50)
                };
                parameters[i++].Value = model.STATIONID;
                parameters[i++].Value = model.CLID;
                parameters[i++].Value = model.CLLX;
                parameters[i++].Value = model.DLSJ;
                parameters[i++].Value = model.MAXSPEED;
                try
                {
                    int rows = DBHelperSQL.Execute(strSql.ToString(), parameters);
                    if (rows > 0)
                    {
                        ini.INIIO.saveLogInf("UpdateCarBj(" + model.CLHP + "):" + "true");
                        return true;
                    }
                    else
                    {
                        ini.INIIO.saveLogInf("UpdateCarBj(" + model.CLHP + "):" + "false");
                        return false;
                    }
                }
                catch (Exception)
                {
                    ini.INIIO.saveLogInf("UpdateCarBj(" + model.CLHP + "):" + "异常");
                    throw;
                }
            }
            #endregion
            #region 获取所有待检车辆信息
            /// <summary>
            /// 获取所有检测线的信息
            /// </summary>
            public DataTable getAllCarWait(string id)
            {
                string sql = "select * from [待检车辆] where STATIONID='" + id + "' order by DLSJ desc";
                DataTable dt = null;
                try
                {
                    dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                    ini.INIIO.saveLogInf("getAllCarWait(" + id + "):" + "成功");
                    return dt;
                }
                catch
                {
                    ini.INIIO.saveLogInf("getAllCarWait(" + id + "):" + "异常");
                    throw;
                }
            }
            #endregion
            #region 获取所有待检车辆信息
            /// <summary>
            /// 获取所有检测线的信息
            /// </summary>
            public DataSet getAllCarWaitDataSet(string id)
            {
                string sql = "select * from [待检车辆] where STATIONID='" + id + "' order by DLSJ desc ";
                DataSet dt = null;
                try
                {
                    dt = DBHelperSQL.GetDataSet(sql, CommandType.Text);
                    ini.INIIO.saveLogInf("getAllCarWaitDataSet(" + id + "):" + "成功");
                    return dt;
                }
                catch
                {
                    throw;
                }
            }
            #endregion
        
            #region 获取某一辆车的待检测信息
            /// <summary>
            /// 获取所有检测线的信息
            /// </summary>
            public DataTable getCarWaitByPlate(string id,string plate)
            {
                string sql="";
                sql = "select * from [待检车辆]  where STATIONID='" + id + "' and  CLHP=" + "'" + plate + "'";
                DataTable dt = null;
                try
                {
                    dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                    ini.INIIO.saveLogInf("getCarWaitByPlate(" + id + "," + plate + "):" + "成功");
                    return dt;
                }
                catch
                {
                    throw;
                }
            }
            #endregion

            #region 获取待检列表中某一辆车的检测信息
            public carWaitModel getCarInfatWaitList(string clhp)
            {
                carWaitModel model = new carWaitModel();
                string sql = "select * from [待检车辆] where CLHP='" + clhp + "'";
                try
                {
                    DataTable dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                    if (dt.Rows.Count > 0)
                    {
                        model.CLID = dt.Rows[0]["CLID"].ToString();
                        model.STATIONID = dt.Rows[0]["STATIONID"].ToString();
                        model.DLSJ = DateTime.Parse(dt.Rows[0]["DLSJ"].ToString());
                        model.CLLX = dt.Rows[0]["CLLX"].ToString();
                        model.CLHP = dt.Rows[0]["CLHP"].ToString();
                        model.MAXSPEED = dt.Rows[0]["MAXSPEED"].ToString();
                        
                        ini.INIIO.saveLogInf("getCarInfatWaitList(" + clhp + "):" + "找到信息");
                    }
                    else
                    {
                        ini.INIIO.saveLogInf("getCarInfatWaitList(" + clhp + "):" + "未找到信息");
                        model.CLID = "-2";       //当服务器上没有找到本线时，本线编号置为-2，以免为0
                    }
                }
                catch (Exception)
                {
                    ini.INIIO.saveLogInf("getCarInfatWaitList(" + clhp + "):" + "异常");
                    throw;
                }
                return model;

            }
            #endregion
            #region 获取该号牌待检车的车辆ID
            public string getCarIDatWaitbyPlate(string plate)
            {
                string sql = "select CLID from [待检车辆] where CLHP='" + plate + "'";
                DataTable dt = null;
                try
                {
                    dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                    if (dt.Rows.Count > 0)
                    {
                        ini.INIIO.saveLogInf("getCarIDatWaitbyPlate(" + plate + "):" + "找到信息");
                        return dt.Rows[0]["CLID"].ToString();
                    }
                    else
                    {
                        ini.INIIO.saveLogInf("getCarIDatWaitbyPlate(" + plate + "):" + "未找到信息");
                        return "-2";
                    }
                }
                catch
                {
                    ini.INIIO.saveLogInf("getCarIDatWaitbyPlate(" + plate + "):" + "异常");
                    throw;
                }
            }
            #endregion
            #region 删除该号牌的待检车辆
            /// <summary>
            /// 获取所有检测线的信息
            /// </summary>
            public bool deleteCarAtWaitbyPlate(string plate)
            {
                string sql = "delete from [待检车辆] where CLHP='" + plate + "'";
                try
                {
                    if (DBHelperSQL.GetDataTable(sql, CommandType.Text).Rows.Count > 0)
                    {
                        ini.INIIO.saveLogInf("deleteCarAtWaitbyPlate(" + plate + "):" + "删除成功");
                        return true;
                    }
                    else
                    {
                        ini.INIIO.saveLogInf("deleteCarAtWaitbyPlate(" + plate + "):" + "删除失败");
                        return false;
                    }
                }
                catch
                {
                    ini.INIIO.saveLogInf("deleteCarAtWaitbyPlate(" + plate + "):" + "异常");
                    throw;
                }
            }
            #endregion

            #region 删除某一条待检信息
            public bool deleteDatabj(string plate)
            {
                string sql = "delete from [待检车辆] where CLHP='" + plate + "'";
                try
                {
                    if (DBHelperSQL.GetDataTable(sql, CommandType.Text).Rows.Count > 0)
                        return true;
                    else
                        return false;
                }
                catch
                {
                    throw;
                }
            }
            #endregion

            #region 获取comBoBox的Items值
            public DataTable getComBoBoxItemsInf(string comboboxName)
            {
                string sql = "select * from" + " [" + comboboxName + "]";
                try
                {
                    DataTable dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                    if (dt.Rows.Count > 0)
                    {
                        return dt;
                    }
                    else
                        return null;       //当服务器上没有找到本线时，本线编号置为-2，以免为0
                }
                catch (Exception)
                {
                    throw;
                }

            }
            #endregion
            #region 获取车辆类型的内容
            public cllxModel getCllxContentByName(string name)
            {
                string sql = "select * from [车辆类型] where MC='" + name + "'";
                try
                {
                    cllxModel model = new cllxModel();
                    DataTable dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                    if (dt.Rows.Count > 0)
                    {
                        model.ID = dt.Rows[0]["ID"].ToString();
                        model.MC = dt.Rows[0]["MC"].ToString();
                        
                        ini.INIIO.saveLogInf("getCllxContentByName(" + name + "):" + "找到信息");
                    }
                    else
                    {
                        ini.INIIO.saveLogInf("getCllxContentByName(" + name + "):" + "未找到信息");
                        model.ID = "-2";       //当服务器上没有找到本线时，本线编号置为-2，以免为0
                    }
                    return model;
                }
                catch (Exception)
                {
                    ini.INIIO.saveLogInf("getCllxContentByName(" + name + "):" + "异常");
                    throw;
                }

            }
            #endregion
            #region 检查员工是否存在
            public string checkUserIsAlive(string userName, string password)
            {
                string userID = "";
                string sql = "select ID from [用户] where NAME='" + userName + "' and PASSWORD='" + password + "'";
                try
                {
                    DataTable dt = DBHelperSQL.GetDataTable(sql, CommandType.Text);
                    if (dt.Rows.Count > 0)
                    {
                        return dt.Rows[0]["ID"].ToString(); ;
                    }
                    else
                        return "-2";       //当服务器上没有找到本线时，本线编号置为-2，以免为0
                }
                catch (Exception)
                {
                    return "-2";
                    throw;
                }

            }
            #endregion
    }
}
