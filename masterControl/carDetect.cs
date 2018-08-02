using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ini;
using System.Threading;

namespace masterControl
{
    public partial class carDetect : Form
    {
        public enum ENUM_EQUIP_TYPE { EQUIP_SPEED,EQUIP_WHEELALIGN,EQUIP_LIGHT,
            EQUIP_BRAKE,EQUIP_SLIP,EQUIP_ABS,
            EQUIP_RAIN,EQUIP_DYNAMETER,EQUIP_EXHAUST,
            EQUIP_TURNANGLE };
        public string[] equipNameAry =
        {
            "速度仪","四轮定位仪","灯光仪",
            "制动仪","侧滑仪","ABS",
            "淋雨房","测功机","尾气检测",
            "转角仪"            
        };
        public enum ENUM_WORK_MODE { WORK_MODE_NET, WORK_MODE_LOCAL };
        public struct sysConfig
        {
            public string lineID;
            public ENUM_EQUIP_TYPE equipType;
            public string dllDir;
            public string versionNumber;
            public ENUM_WORK_MODE workMode;
            public int BarCodeLength;
        }
        public sysConfig thisSysConfig;
        string extlinkdir = @"D:\QAII\PSDDAT\EXTLINK.ini";
        public struct vehicleInfo//上线车辆信息，生成目录：D:\QAII\PSDDAT\EXTLINK.ini
        {
            public string scanCode;//VIN码
            public string scanOK;//1表示 VIN准备好，转角台获取VIN后，会置为0
            public ENUM_EQUIP_TYPE testDev;//9代表转角台设备
            public string carType;//车型名称
        }
        //equipdata zjxConfigData = new equipdata();
        //configDataIni configdataini = new configDataIni();
        public delegate void wt_textboxtext(TextBox controlname, string text);                                //委托
        public delegate void wt_image(string dataleft, string dataright);
        public delegate void wt_frontimage(string data);
        

        public int Carwait_Scroll = 0;                                                  //待检车滚动条位置
        DataTable dt_wait = null;                                                         //等待车辆列表
        public string[] selectID = new string[1024];                                    //当前等待车辆选中的列表
        public bool ref_zt = true;

        //private vehicleXh vehiclexh = new vehicleXh();
        public static List<string> vehicleXhName=new List<string>();
        List<double> vehicleXhLLXz=new List<double>();
        List<double> vehicleXhLRXz = new List<double>();
        List<double> vehicleXhRLXz = new List<double>();
        List<double> vehicleXhRRXz = new List<double>();
        List<double> vehicleXhLLXzWc = new List<double>();
        List<double> vehicleXhLRXzWc = new List<double>();
        List<double> vehicleXhRLXzWc = new List<double>();
        List<double> vehicleXhRRXzWc = new List<double>();
        private bool isLeftZero=false;
        private bool isRightZero=false;

        private bool isTestAlready = false;

        private bool isTestStart = false;
        private Thread testThread = null;

        private bool highEffective = false;

        private byte zp = 1;
        private byte bzq = 3;

        

        public struct testCar
        {
            public string clhp;
            public bool isCarWait;
            public string cllx;
            public int maxspeed;
            public float leftltleft;
            public float leftltright;
            public float rightltleft;
            public float rightltright;
            public string leftltleftpd;
            public string leftltrightpd;
            public string rightltleftpd;
            public string rightltrightpd;
            public float llxz;
            public float lrxz;
            public float rlxz;
            public float rrxz;
            public float llxzwc;
            public float lrxzwc;
            public float rlxzwc;
            public float rrxzwc;
            public string totalpd;
        }
        public testCar testcar;

        private bool isTurnToLeft = false;
        private bool isTurnToRight = false;

        public static string stationid = "";
        private bool isSqlServerReady = false;

        private Point panelTestLocation = new Point(3, 88);
        BardCodeHooK BarCode = new BardCodeHooK();
        public carDetect()
        {
            InitializeComponent();
            BarCode.BarCodeEvent += new BardCodeHooK.BardCodeDeletegate(BarCode_BarCodeEvent);
        }
        private delegate void ShowInfoDelegate(BardCodeHooK.BarCodes barCode);
        private void ShowInfo(BardCodeHooK.BarCodes barCode)
        {
            richTextBox1.Focus();
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new ShowInfoDelegate(ShowInfo), new object[] { barCode });
            }
            else
            {
                //richTextBox1.Focus();
                //textBox1.Text = barCode.KeyName;
                //textBox2.Text = barCode.VirtKey.ToString();
                //textBox3.Text = barCode.ScanCode.ToString();
                //textBox4.Text = barCode.Ascll.ToString();
                //textBox5.Text = barCode.Chr.ToString();
                //textBox6.Text = barCode.IsValid ? barCode.BarCode : "";//是否为扫描枪输入，如果为true则是 否则为键盘输入
                //textBox7.Text += barCode.KeyName; 
                if (barCode.IsValid && thisSysConfig.workMode == ENUM_WORK_MODE.WORK_MODE_LOCAL)
                {
                    LogMessage("条码内容：" + barCode.BarCode);textBoxCLHP.Text = barCode.BarCode;
                    if (textBoxCLHP.Text.Trim().Length == thisSysConfig.BarCodeLength)
                    {
                        startTest();
                    }
                    else
                    {
                        LogWarning("VIN码长度错误，规定长度为" + thisSysConfig.BarCodeLength.ToString() + ",该车VIN长度为" + textBoxCLHP.Text.Length.ToString());
                    }
                }
            }
        }
        void BarCode_BarCodeEvent(BardCodeHooK.BarCodes barCode)
        {
            ShowInfo(barCode);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            init_LineModel();
            initSql();
            initEquip();
            initVehicleTypeList();
            this.WindowState = FormWindowState.Maximized;
            timer1.Start();
            BarCode.Start();
            Msg(labelTS, panelTS, "等待测试命令", 0);
        }
        private sysConfig getSysConfig()
        {
            sysConfig syscfg = new sysConfig();
            try
            {
                StringBuilder temp = new StringBuilder();
                temp.Length = 2048;
                ini.INIIO.GetPrivateProfileString("SYS", "lineID", "1", temp, 2048, @".\appConfig.ini");
                syscfg.lineID = temp.ToString().Trim();
                ini.INIIO.GetPrivateProfileString("SYS", "BarCodeLength", "13", temp, 2048, @".\appConfig.ini");
                syscfg.BarCodeLength =int.Parse( temp.ToString().Trim());
                ini.INIIO.GetPrivateProfileString("SYS", "equipType", "9", temp, 2048, @".\appConfig.ini");
                syscfg.equipType = (ENUM_EQUIP_TYPE)(int.Parse(temp.ToString().Trim()));
                ini.INIIO.GetPrivateProfileString("SYS", "dllDir", "", temp, 2048, @".\appConfig.ini");
                syscfg.dllDir = temp.ToString().Trim();
                ini.INIIO.GetPrivateProfileString("SYS", "versionNumber", "", temp, 2048, @".\appConfig.ini");
                syscfg.versionNumber = temp.ToString().Trim();
                ini.INIIO.GetPrivateProfileString("SYS", "workMode", "0", temp, 2048, @".\appConfig.ini");
                syscfg.workMode = (ENUM_WORK_MODE)(int.Parse(temp.ToString().Trim()));
            }
            catch
            { }
            return syscfg;
        }
        /// <summary>
        /// 读取车辆信息
        /// </summary>
        /// <param name="vinf"></param>
        /// <returns></returns>
        private bool getVehicleInfo(out vehicleInfo vinf)
        {
            vinf = new vehicleInfo();
            try
            {
                StringBuilder temp = new StringBuilder();
                temp.Length = 2048;
                ini.INIIO.GetPrivateProfileString("ExtLink", "ScanCode", "", temp, 2048, extlinkdir);
                vinf.scanCode = temp.ToString().Trim();
                ini.INIIO.GetPrivateProfileString("ExtLink", "ScanOK", "", temp, 2048, extlinkdir);
                vinf.scanOK = temp.ToString().Trim();
                ini.INIIO.GetPrivateProfileString("ExtLink", "TestDev", "9", temp, 2048, extlinkdir);
                vinf.testDev = (ENUM_EQUIP_TYPE)(int.Parse(temp.ToString().Trim()));
                ini.INIIO.GetPrivateProfileString("ExtLink", "CarType", "", temp, 2048, extlinkdir);
                vinf.carType = temp.ToString().Trim();
                if (vinf.scanOK == "1")
                {
                    LogMessage("获取到车辆信息：" + vinf.scanCode + "|" + vinf.scanOK + "|" + vinf.testDev + "|" + vinf.carType);
                    return true;
                }
                else
                    return false;
            }
            catch(Exception er)
            {
                return false;
            }
        }
        /// <summary>
        /// 标记ScanOk为0，表示VIN已获取
        /// </summary>
        /// <returns></returns>
        private bool setScanOK()
        {
            if(System.IO.File.Exists(extlinkdir))
            {
                ini.INIIO.WritePrivateProfileString("ExtLink", "ScanOK", "0", extlinkdir);
            }
            return true;
        }
        /// <summary>
        /// 标记ScanOK为5，表示检测已经完成
        /// </summary>
        /// <returns></returns>
        private bool setTestFinish()
        {
            if (System.IO.File.Exists(extlinkdir))
            {
                ini.INIIO.WritePrivateProfileString("ExtLink", "ScanOK", "5", extlinkdir);
            }
            return true;
        }
        private void init_LineModel()
        {
            thisSysConfig = getSysConfig();
            if (thisSysConfig.workMode == ENUM_WORK_MODE.WORK_MODE_LOCAL)
            {
                Msg(labelWorkMode, panelWorkMode, "单机", 0);
                buttonStart.Visible = true;
                textBoxCLHP.Visible = true;
                comboBoxVehicleName.Visible = true;
            }
            else
            {
                Msg(labelWorkMode, panelWorkMode, "联网", 0);
                buttonStart.Visible = false;
                textBoxCLHP.Visible = false;
                comboBoxVehicleName.Visible = false;
            }
            Msg(labelEquipName, panelEquipName, equipNameAry[(int)(thisSysConfig.equipType)], 0);
            Msg(labelLine, panelLine, thisSysConfig.lineID+"号工位", 0);
            //Msg(labelDllSrc, panelDllSrc, thisSysConfig.dllDir, 1);
            Msg(labelversion, panelversion, thisSysConfig.versionNumber, 1);
        }
        private void initSql()
        {
            StringBuilder temp = new StringBuilder();
            temp.Length = 2048;
            string sqlstatus = "";
            if (childTest.SqlControl.testSqlLink(out sqlstatus))
            {
                LogMessage("数据据连接成功");
                labelSql.Text = "数据据连接成功";
                isSqlServerReady = true;
            }
            else
            {
                LogError("数据据连接失败:"+sqlstatus);
                labelSql.Text = sqlstatus;
            }   
        }
        private void initVehicleTypeList()
        {
            DataTable dt = childTest.SqlControl.getVehicleList();
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    comboBoxVehicleName.Items.Add(dr["VEHICLENAME"].ToString());
                }
                if (comboBoxVehicleName.Items.Count > 0)
                {
                    comboBoxVehicleName.SelectedIndex = 0;
                    LogMessage("获取车辆类型表成功");
                }
            }
            else
            {
                LogWarning("获取车辆类型表为空");
            }
        }
        private void initEquip()
        {
            string equipstatus;
            if (childTest.controller.test(out equipstatus))
            {
                LogMessage("设备打开正常");
                labelEquipStatus.Text = "设备打开正常";
            }
            else
            {
                LogError("设备打开失败:" + equipstatus);
                labelEquipStatus.Text = equipstatus;
            }
        }
        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (testThread != null)
                    testThread.Abort();
            }
            catch
            { }
            BarCode.Stop();
            System.Environment.Exit(0);
        }
        private int timerCount = 0;
        private int fileExistTime = 0;
        private vehicleInfo thisVehicleInfo;
        private void timer1_Tick(object sender, EventArgs e)
        {
            Msg(labelTime, panelTime, DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss"), 2);
            if (childTest.controller.scanState == 0)
            {
                timerCount++;
                if (timerCount == 8)
                {
                    timerCount = 0;
                    Msg(labelOrder1, panelOrder1, "等待检测指令", 0);
                }
                else if (timerCount == 6)
                {
                    Msg(labelOrder1, panelOrder1, "", 0);
                }
            }
            else
            { Msg(labelOrder1, panelOrder1, "系统正忙", 0); }
            if (childTest.controller.scanState == 0)
            {
                if (thisSysConfig.workMode == ENUM_WORK_MODE.WORK_MODE_NET)//联网模式下
                {
                    if (System.IO.File.Exists(extlinkdir))
                    {
                        fileExistTime++;
                        if (fileExistTime == 2)
                        {
                            fileExistTime = 0;
                            vehicleInfo thisVehicleInfo_temp;
                            if (getVehicleInfo(out thisVehicleInfo_temp))
                            {
                                setScanOK();
                                thisVehicleInfo = thisVehicleInfo_temp;
                                Msg(labelCLHP, panelCLHP, thisVehicleInfo.scanCode, 1);
                                Msg(labelVehicleName, panelVehicleName, thisVehicleInfo.carType, 1);
                                startTest();
                            }
                        }
                    }
                }
                else
                { }
            }
        }
        

        private void buttonDemarcate_Click(object sender, EventArgs e)
        {
            childTest.controller.scanState = 1;
            childTest.calibration carliform = new childTest.calibration();
            carliform.ShowDialog();
            childTest.controller.scanState = 0;
        }

        private void buttonExit_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {

        }
        
        public delegate void wtlsb(Label Msgowner, string Msgstr);                   //委托
        public delegate void wtlp(Label Msgowner, Panel Msgfather,int position);                                 //委托

        /// <summary>
        /// 信息显示
        /// </summary>
        /// <param name="Msgowner">信息显示的Label控件</param>
        /// <param name="Msgfather">Label控件的父级Panel</param>
        /// <param name="Msgstr">要显示的信息</param>
        /// <param name="position">0-居中 1-靠左 2-靠右</param>

        public void Msg(Label Msgowner, Panel Msgfather, string Msgstr,int position)
        {
            BeginInvoke(new wtlsb(Msg_Show), Msgowner, Msgstr);
            BeginInvoke(new wtlp(Msg_Position), Msgowner, Msgfather,position);
        }

        public void Msg_Show(Label Msgowner, string Msgstr)
        {
            Msgowner.Text = Msgstr;
        }

        public void Msg_Position(Label Msgowner, Panel Msgfather,int position)
        {
            // Msgowner.Font = new System.Drawing.Font("微软雅黑", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            if (position == 0)
            {
                if (Msgowner.Width < Msgfather.Width)
                {
                    Msgowner.Location = new Point((Msgfather.Width - Msgowner.Width) / 2, Msgowner.Location.Y);
                }
                else
                {
                    Msgowner.Location = new Point(0, Msgowner.Location.Y);
                    //Msgowner.Font = new System.Drawing.Font("微软雅黑", 30F * Msgfather.Width/Msgowner.Width , System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
                }
            }
            else if (position == 1)
            {
                Msgowner.Location = new Point(0, Msgowner.Location.Y);
            }
            else
            {
                Msgowner.Location = new Point(Msgfather.Width - Msgowner.Width, Msgowner.Location.Y);
            }
        }

        private void buttonVehicle_Click(object sender, EventArgs e)
        {
            childTest.controller.scanState = 1;
            childTest.vehicleManage carliform = new childTest.vehicleManage();
            carliform.ShowDialog();
            childTest.controller.scanState = 0;
        }

        private void buttonConfig_Click(object sender, EventArgs e)
        {
            childTest.controller.scanState = 1;
            childTest.settings settingform = new childTest.settings();
            settingform.ShowDialog();
            labelEquipStatus.Text = childTest.controller.equipStatus;
            childTest.controller.scanState = 0;
        }
        childTest.maintest testform ;
        private void carDetect_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                startTest();
            }
            else if (e.KeyCode == Keys.F6)
            {
                buttonDemarcate_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F7)
            { buttonVehicle_Click(sender, e); }
            else if (e.KeyCode == Keys.F8)
            { buttonTools_Click(sender, e); }
        }
        private void startTest()
        {
            if (!panelParent.Controls.Contains(testform))
            {
                string vin = "", vehiclename = "";
                if (thisSysConfig.workMode == ENUM_WORK_MODE.WORK_MODE_LOCAL)
                {
                    if (textBoxCLHP.Text == "")
                    {
                        Msg(labelTS, panelTS, "VIN为空", 0);
                        LogWarning("VIN为空,不能进行检测");
                        return;
                    }
                    vin = textBoxCLHP.Text.Trim();
                    vehiclename = comboBoxVehicleName.Text.Trim();
                }
                else
                {
                    if (thisVehicleInfo.scanCode == ""||thisVehicleInfo.scanCode==null)
                    {
                        Msg(labelTS, panelTS, "VIN为空", 0);
                        LogWarning("VIN为空,不能进行检测");
                        return;
                    }
                    vin = thisVehicleInfo.scanCode.Trim();
                    vehiclename = thisVehicleInfo.carType.Trim();
                }
                childTest.controller.scanState = 1;
                testform = new childTest.maintest();
                testform.VIN = vin;
                testform.VEHICLENAME = vehiclename;
                testform.workMode = (childTest.maintest.ENUM_WORK_MODE)(thisSysConfig.workMode);
                testform.FormBorderStyle = FormBorderStyle.None;
                testform.TopLevel = false;
                panelParent.Controls.Add(testform);
                testform.BringToFront();
                testform.Location = panelTestLocation;
                testform.Show();
            }
        }
        #region 日志记录、支持其他线程访问 
        public delegate void LogAppendDelegate(Color color, string text);
        //public bool isFirstRow = true;
        /// <summary> 
        /// 追加显示文本 
        /// </summary> 
        /// <param name="color">文本颜色</param> 
        /// <param name="text">显示文本</param> 
        public void LogAppend(Color color, string text)
        {
            richTextBox1.SelectionColor = color;
            richTextBox1.AppendText(text);
            richTextBox1.AppendText("\n");
            richTextBox1.ScrollToCaret();
        }
        /// <summary> 
        /// 显示错误日志 
        /// </summary> 
        /// <param name="text"></param> 
        public void LogError(string text)
        {
            LogAppendDelegate la = new LogAppendDelegate(LogAppend);
            richTextBox1.Invoke(la, Color.Red, DateTime.Now.ToString("HH:mm:ss ") + text);
        }
        /// <summary> 
        /// 显示警告信息 
        /// </summary> 
        /// <param name="text"></param> 
        public void LogWarning(string text)
        {
            LogAppendDelegate la = new LogAppendDelegate(LogAppend);
            richTextBox1.Invoke(la, Color.Violet, DateTime.Now.ToString("HH:mm:ss ") + text);
        }
        /// <summary> 
        /// 显示信息 
        /// </summary> 
        /// <param name="text"></param> 
        public void LogMessage(string text)
        {
            LogAppendDelegate la = new LogAppendDelegate(LogAppend);
            richTextBox1.Invoke(la, Color.Black, DateTime.Now.ToString("HH:mm:ss ") + text);
        }
        #endregion

        private void buttonTools_Click(object sender, EventArgs e)
        {
            childTest.controller.scanState = 1;
            childTest.recordForm recordform = new childTest.recordForm();
            recordform.ShowDialog();
            childTest.controller.scanState = 0;
        }

        private void buttonNetOrLoacl_Click(object sender, EventArgs e)
        {
            if (thisSysConfig.workMode == ENUM_WORK_MODE.WORK_MODE_NET)
            {
                thisSysConfig.workMode = ENUM_WORK_MODE.WORK_MODE_LOCAL;
                Msg(labelWorkMode, panelWorkMode, "单机", 0);
                buttonStart.Visible = true;
                textBoxCLHP.Visible = true;
                comboBoxVehicleName.Visible = true;
            }
            else
            {
                thisSysConfig.workMode = ENUM_WORK_MODE.WORK_MODE_NET;
                Msg(labelWorkMode, panelWorkMode, "联网", 0);
                buttonStart.Visible = false;
                textBoxCLHP.Visible = false;
                comboBoxVehicleName.Visible = false;
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            startTest();
        }
    }
}
