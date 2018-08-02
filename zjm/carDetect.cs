using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Dynamometer;
using configIni;
using ini;
using System.Threading;
using VCM100login;
using SYS_MODEL;

namespace zjm
{
    public partial class carDetect : Form
    {
        ZJX_100 zjx100 = null;
        equipdata zjxConfigData = new equipdata();
        configDataIni configdataini = new configDataIni();
        public static lshModel lshmodel = new lshModel();
        public static vcmLogin vcmlogin = new vcmLogin();
        public delegate void wt_textboxtext(TextBox controlname, string text);                                //委托
        public delegate void wt_image(string dataleft, string dataright);
        public delegate void wt_frontimage(string data);

        public delegate void wtlsb(Label Msgowner, string Msgstr, Color Update_DB);                  //委托
        public delegate void wtlp(Label Msgowner, Panel Msgfather);                                 //委托

        public int Carwait_Scroll = 0;                                                  //待检车滚动条位置
        DataTable dt_wait = null;                                                         //等待车辆列表
        public string[] selectID = new string[1024];                                    //当前等待车辆选中的列表
        public bool ref_zt = true;

        private vehicleXh vehiclexh = new vehicleXh();
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
        public carDetect()
        {
            InitializeComponent();
        }

        private void init_vehicleXh()
        {
            try
            {
                vehiclexh = configdataini.getXHConfigIni();
                vehicleXhName.Clear();
                vehicleXhLLXz.Clear();
                vehicleXhLRXz.Clear();
                vehicleXhRLXz.Clear();
                vehicleXhRRXz.Clear();
                vehicleXhLLXzWc.Clear();
                vehicleXhLRXzWc.Clear();
                vehicleXhRLXzWc.Clear();
                vehicleXhRRXzWc.Clear();
                if (vehiclexh.XH != "")
                {
                    string[] vehicleXhArray = vehiclexh.XH.Split(';');
                    for (int i = 0; i < vehicleXhArray.Count() - 1; i++)
                    {
                        vehicleXhName.Add(vehicleXhArray[i].Split(':')[0]);
                        string[] xhxzarray = (vehicleXhArray[i].Split(':')[1]).Split('|');
                        vehicleXhLLXz.Add(double.Parse(xhxzarray[0].Split('-')[0]));
                        vehicleXhLRXz.Add(double.Parse(xhxzarray[1].Split('-')[0]));
                        vehicleXhRLXz.Add(double.Parse(xhxzarray[2].Split('-')[0]));
                        vehicleXhRRXz.Add(double.Parse(xhxzarray[3].Split('-')[0]));
                        vehicleXhLLXzWc.Add(double.Parse(xhxzarray[0].Split('-')[1]));
                        vehicleXhLRXzWc.Add(double.Parse(xhxzarray[1].Split('-')[1]));
                        vehicleXhRLXzWc.Add(double.Parse(xhxzarray[2].Split('-')[1]));
                        vehicleXhRRXzWc.Add(double.Parse(xhxzarray[3].Split('-')[1]));
                    }
                }
            }
            catch
            {
                MessageBox.Show("限值初始化失败");
                
            }
        }
        private void save_vehicleXh()
        {
            string savexh = "";
            for (int i = 0; i < vehicleXhName.Count; i++)
            {
                savexh += vehicleXhName[i]+":";
                savexh += vehicleXhLLXz[i].ToString() + "-" + vehicleXhLLXzWc[i].ToString()+"|"+vehicleXhLRXz[i].ToString()
                    + "-" + vehicleXhLRXzWc[i].ToString()+"|"+vehicleXhRLXz[i].ToString() + "-" + vehicleXhRLXzWc[i].ToString()+"|"+vehicleXhRRXz[i].ToString() + "-" + vehicleXhRRXzWc[i].ToString()+";";
            }
            configdataini.writeXHConfigIni(savexh);
        }
        private void init_datagrid()
        {
            dt_wait = new DataTable();
            dt_wait.Columns.Add("型号");
            dt_wait.Columns.Add("左轮左转");
            dt_wait.Columns.Add("左轮右转");
            dt_wait.Columns.Add("右轮左转");
            dt_wait.Columns.Add("右轮右转");
            dataGridViewXH.DataSource = dt_wait;
        }
        public void ref_vehicleXh()
        {
            try
            {
                dt_wait.Rows.Clear();
                comboBoxCLLX.Items.Clear();
                DataRow dr = null;
                if (true)
                {
                    for (int i = 0; i < vehicleXhName.Count;i++ )
                    {
                        dr = dt_wait.NewRow();
                        dr["型号"] = vehicleXhName[i].ToString();
                        dr["左轮左转"] = vehicleXhLLXz[i].ToString() + "±" + vehicleXhLLXzWc[i].ToString();
                        dr["左轮右转"] = vehicleXhLRXz[i].ToString() + "±" + vehicleXhLRXzWc[i].ToString();
                        dr["右轮左转"] = vehicleXhRLXz[i].ToString() + "±" + vehicleXhRLXzWc[i].ToString();
                        dr["右轮右转"] = vehicleXhRRXz[i].ToString() + "±" + vehicleXhRRXzWc[i].ToString();
                        dt_wait.Rows.Add(dr);
                        comboBoxCLLX.Items.Add(vehicleXhName[i].ToString());
                    }
                    
                }

                ref_zt = false;
                dataGridViewXH.DataSource = dt_wait;
                dataGridViewXH.FirstDisplayedScrollingRowIndex = Carwait_Scroll;
                dataGridViewXH.Sort(dataGridViewXH.Columns["型号"], ListSortDirection.Descending);
                ref_zt = true;
            }
            catch (Exception)
            {

            }
        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (true)
            {
                if (dataGridViewXH.SelectedRows.Count > 0)
                {
                    if (dataGridViewXH.SelectedRows.Count == 1)
                    {
                        selectID = new string[1024];
                        for (int i = 0; i < dataGridViewXH.SelectedRows.Count; i++)
                        {
                            selectID[i] = dataGridViewXH.SelectedRows[i].Cells["型号"].Value.ToString();
                        }
                    }
                    else if (dataGridViewXH.SelectedRows.Count > 1)
                    {
                        selectID = new string[1024];
                        for (int i = 0; i < dataGridViewXH.SelectedRows.Count; i++)
                        {
                            selectID[i] = dataGridViewXH.SelectedRows[i].Cells["型号"].Value.ToString();
                        }
                    }
                    else
                    {
                        selectID = new string[1024];
                        for (int i = 0; i < dataGridViewXH.SelectedRows.Count; i++)
                        {
                            selectID[i] = dataGridViewXH.SelectedRows[i].Cells["型号"].Value.ToString();
                        }
                    }

                }
                else
                {
                    selectID = new string[1024];
                    for (int i = 0; i < dataGridViewXH.SelectedRows.Count; i++)
                    {
                        selectID[i] = dataGridViewXH.SelectedRows[i].Cells["型号"].Value.ToString();
                    }
                }

            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            initSql();
            initCarInf();
            initConfigData();
            init_datagrid();
            init_vehicleXh();
            ref_vehicleXh();
            initCom();
            initEquip();
            //timer2.Start();
            //isTurnToLeft = true;
            this.WindowState = FormWindowState.Maximized; 
            
        }
        private void initSql()
        {
            StringBuilder temp = new StringBuilder();
            temp.Length = 2048;
            ini.INIIO.GetPrivateProfileString("sqldata", "IP", "", temp, 2048, @".\appConfig.ini");
            string severIP = temp.ToString().Trim();
            if (!ini.INIIO.ping(severIP))
            {
                MessageBox.Show("连接到数据库失败");
                labelSql.Text = "连接到数据库失败";
                return;
            }
            stationid = vcmlogin.getStationID();
            if (stationid != "-2")
            {
                lshmodel = vcmlogin.getLineLshInf(stationid);
                if (lshmodel.DATE.ToShortDateString() != DateTime.Now.ToShortDateString() || lshmodel.COUNT == "-2")
                {
                    vcmlogin.setLineLshDate(stationid, DateTime.Now.ToShortDateString());
                    vcmlogin.setLineLshCount(stationid, "1");
                }
                isSqlServerReady = true;
                buttonStart.Enabled = true;
                labelSql.Text = "连接到数据库成功";
            }
            else
            {
                buttonStart.Enabled = false;
                isSqlServerReady = false;
                labelSql.Text = "获取站点信息失败";
            }
        }
        private void initCom()
        {
            try
            {
                foreach (string com in System.IO.Ports.SerialPort.GetPortNames()) //自动获取串行口名称
                {
                    comboBoxCom.Items.Add(com);
                }
                // DisplaySqlData();
            }
            catch
            {
                MessageBox.Show("在该计算机上没有找到可使用的串口,该软件无法正常运行", "提示");     //没有获取到COM报错
            }
        }
        private void initCarInf()
        {
            testcar.clhp = "川AV7M82";
            testcar.isCarWait = false;
        }
        private void initThread()
        {
            testThread = new Thread(test_Process);
            testThread.Start();
        }
        private void test_Process()
        {
            while (true)
            {
                if (isTestStart)
                {
                    if (testcar.isCarWait)
                    {
                        if (zjxConfigData.ZPEFFCTIVE=="1")
                            zjx100.Set_GAB100RelayOn(zp);
                        else
                            zjx100.Set_GAB100RelayOff(zp);
                        if (zjxConfigData.BZEFFCTIVE == "1")
                            zjx100.Set_GAB100RelayOff(bzq);
                        else
                            zjx100.Set_GAB100RelayOn(bzq);
                        Msg(labelCLHP, panelCLHP,testcar.clhp, Color.White);
                        Msg(labelTS, panelTS, "请缓慢前行", Color.White);
                        Thread.Sleep(1000);
                        while (true)
                        {
                            if (isTestAlready)
                            {
                                Thread.Sleep(1000);
                                if (isTestAlready) break;
                            }
                            Thread.Sleep(300);
                        }
                        Msg(labelTS, panelTS, "车辆到位", Color.White);
                        Thread.Sleep(1000);
                        zjx100.Set_ClearLeft();
                        zjx100.Set_ClearRight();
                        if (zjxConfigData.BZEFFCTIVE == "1")
                            zjx100.Set_GAB100RelayOn(bzq);
                        else
                            zjx100.Set_GAB100RelayOff(bzq);
                        Thread.Sleep(3000);
                        if (zjxConfigData.ZPEFFCTIVE == "1")
                            zjx100.Set_GAB100RelayOff(zp);
                        else
                            zjx100.Set_GAB100RelayOn(zp);
                        
                        float leftangleleft = 0;
                        float rightangleleft = 0;
                        
                        int stablecount=0;
                        Msg(labelTS, panelTS, "左打方向盘", Color.White);
                        isTurnToLeft = true;
                        while (stablecount < 30)
                        {
                            Thread.Sleep(100);
                            if(zjx100.angleLeft<leftangleleft)
                                leftangleleft=zjx100.angleLeft;
                            if (zjx100.angleRight<rightangleleft)
                                rightangleleft = zjx100.angleRight;
                            if (leftangleleft <-6)
                            {
                                if (Math.Abs(zjx100.angleLeft - leftangleleft) < 4) stablecount++;
                                else stablecount = 0;
                            }
                        }
                        isTurnToLeft = false;
                        testcar.leftltleft = (float)Math.Round(-leftangleleft,1);
                        testcar.rightltleft =(float)Math.Round(-rightangleleft,1);
                        Msg(labelLzlldata, panelLzlldata, testcar.leftltleft.ToString("0.0"), Color.Yellow);
                        Msg(labelLzrldata, panelLzrldata, testcar.rightltleft.ToString("0.0"), Color.Yellow);
                        if (testcar.leftltleft <= testcar.llxz + testcar.llxzwc && testcar.leftltleft >= testcar.llxz - testcar.llxzwc)
                        {
                            testcar.leftltleftpd = "PASS";
                            Msg(labelLzLljg, panelLzLljg, testcar.leftltleftpd, Color.Lime);
                        }
                        else
                        {
                            testcar.leftltleftpd = "FAIL";
                            Msg(labelLzLljg, panelLzLljg, testcar.leftltleftpd, Color.Red);
                        }
                        if (testcar.rightltleft <= testcar.rlxz + testcar.rlxzwc && testcar.rightltleft >= testcar.rlxz - testcar.rlxzwc)
                        {
                            testcar.rightltleftpd = "PASS";
                            Msg(labelLzRljg, panelLzRljg, testcar.rightltleftpd, Color.Lime);
                        }
                        else
                        {
                            testcar.rightltleftpd = "FAIL";
                            Msg(labelLzRljg, panelLzRljg, testcar.rightltleftpd, Color.Red);
                        }
                        stablecount = 0;
                        Msg(labelTS, panelTS, "右打方向盘", Color.White);
                        isTurnToRight = true;
                        while (stablecount < 30)
                        {
                            Thread.Sleep(100);
                            if (zjx100.angleLeft > leftangleleft)
                                leftangleleft = zjx100.angleLeft;
                            if (zjx100.angleRight > rightangleleft)
                                rightangleleft = zjx100.angleRight;
                            if (leftangleleft > 6)
                            {
                                if (Math.Abs(zjx100.angleLeft - leftangleleft) < 4) stablecount++;
                                else stablecount = 0;
                            }
                        }
                        isTurnToRight = false;
                        testcar.leftltright = (float)Math.Round(leftangleleft,1);
                        testcar.rightltright = (float)Math.Round(rightangleleft,1);
                        Msg(labelRzlldata, panelRzlldata, testcar.leftltright.ToString("0.0"), Color.Yellow);
                        Msg(labelRzrldata, panelRzrldata, testcar.rightltright.ToString("0.0"), Color.Yellow);
                        if (testcar.leftltright <= testcar.lrxz + testcar.lrxzwc && testcar.leftltright >= testcar.lrxz - testcar.lrxzwc)
                        {
                            testcar.leftltrightpd = "PASS";
                            Msg(labelRzLljg, panelRzLljg, testcar.leftltrightpd, Color.Lime);
                        }
                        else
                        {
                            testcar.leftltrightpd = "FAIL";
                            Msg(labelRzLljg, panelRzLljg, testcar.leftltrightpd, Color.Red);
                        }
                        if (testcar.rightltright <= testcar.rrxz + testcar.rrxzwc && testcar.rightltright >= testcar.rrxz - testcar.rrxzwc)
                        {
                            testcar.rightltrightpd = "PASS";
                            Msg(labelRzRljg, panelRzRljg, testcar.rightltrightpd, Color.Lime);
                        }
                        else
                        {
                            testcar.rightltrightpd = "FAIL";
                            Msg(labelRzRljg, panelRzRljg, testcar.rightltrightpd, Color.Red);
                        }
                        if (testcar.rightltrightpd == "PASS" && testcar.leftltleftpd == "PASS" && testcar.rightltleftpd == "PASS" && testcar.leftltrightpd == "PASS")
                        {
                            testcar.totalpd = "PASS";
                            Msg(labelTotalPd, panelTotalPd, testcar.totalpd, Color.Lime);
                        }
                        else
                        {
                            testcar.totalpd = "FAIL";
                            Msg(labelTotalPd, panelTotalPd, testcar.totalpd, Color.Red);
                        }
                        Msg(labelTS, panelTS, "请回盘", Color.White);
                        stablecount = 0;
                        while (stablecount < 10)
                        {
                            if (isLeftZero&&isRightZero) stablecount++;
                            else stablecount = 0;
                            Thread.Sleep(100);
                        }
                        if (zjxConfigData.ZPEFFCTIVE == "1")
                            zjx100.Set_GAB100RelayOn(zp);
                        else
                            zjx100.Set_GAB100RelayOff(zp);
                        Thread.Sleep(1000);
                        if (zjxConfigData.BZEFFCTIVE == "1")
                            zjx100.Set_GAB100RelayOff(bzq);
                        else
                            zjx100.Set_GAB100RelayOn(bzq);

                        saveResult();                    
                        Msg(labelTS, panelTS, "请出车", Color.White);
                        while (true)
                        {
                            if (!isTestAlready)
                            {
                                Thread.Sleep(3000);
                                if (!isTestAlready) break;
                            }
                            Thread.Sleep(300);
                        }

                        Msg(labelTS, panelTS, "等待检测", Color.White);
                        testcar.isCarWait = false;
                        isTestStart = false;
                        buttonStart.Text = "启动";
                    }
                    Thread.Sleep(100);
                }
                Thread.Sleep(100);
            }
        }
        private bool saveResult()
        {
            recordModel model = new recordModel();
            model.CLHP = testcar.clhp;
            model.CLLX = testcar.cllx;
            model.STATIONID = stationid;
            model.JCSJ = DateTime.Now;
            model.CLID = testcar.clhp + DateTime.Now.ToString("yyMMddHHmmss");
            model.MAXSPEED = testcar.maxspeed.ToString();
            model.XZ = testcar.llxz.ToString() + "-" + testcar.llxzwc.ToString() + "|" + testcar.lrxz.ToString() + "-" + testcar.lrxzwc.ToString() + "|" + testcar.rlxz.ToString() + "-" + testcar.rlxzwc.ToString() + "|" + testcar.rrxz.ToString() + "-" + testcar.rrxzwc.ToString();
            model.LEFTTURNLEFT = testcar.leftltleft.ToString();
            model.LEFTTURNLEFTPD = testcar.leftltleftpd;
            model.LEFTTURNRIGHT = testcar.leftltright.ToString();
            model.LEFTTURNRIGHTPD = testcar.leftltrightpd;
            model.RIGHTTURNLEFT = testcar.rightltleft.ToString();
            model.RIGHTTURNLEFTPD = testcar.rightltleftpd;
            model.RIGHTTURNRIGHT = testcar.rightltright.ToString();
            model.RIGHTTURNRIGHTPD = testcar.rightltrightpd;
            model.ZHPD = testcar.totalpd;
            lshmodel = vcmlogin.getLineLshInf(stationid);
            int lshcount = int.Parse(lshmodel.COUNT);
            model.LSH = stationid + model.JCSJ.ToString("yyMMdd") + lshcount.ToString("0000");
            lshcount++;
            vcmlogin.setLineLshCount(stationid, lshcount.ToString());
            return(vcmlogin.saveCarDetect(model));
        }
        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void initConfigData()
        {
            zjxConfigData = configdataini.getEquipConfigIni();
        }
        private void initEquip()
        {
            if (zjx100==null)
            {
                try
                {
                    if (comboBoxCom.Items.Contains(zjxConfigData.LEDCOM))
                    {
                        try
                        {
                            comboBoxCom.Text = zjxConfigData.LEDCOM;
                            zjx100 = new ZJX_100("BNTD");
                            if (zjx100.Init_Comm(zjxConfigData.LEDCOM, zjxConfigData.LEDSTRING))
                            {
                                Msg(labelTS, panelTS, "等待检测", Color.White);
                                zjx100.Set_ClearLeft();
                                zjx100.Set_ClearRight();
                                zjx100.Set_ClearKey();
                                zjx100.Start_test();
                                timer1.Start();
                                timer2.Start();
                                initThread();
                            }
                            else
                            {
                                Msg(labelTS, panelTS, "设备未开启", Color.White);
                                zjx100 = null;
                            }
                        }
                        catch
                        {
                            Msg(labelTS, panelTS, "设备未开启", Color.White);
                            MessageBox.Show("串口" + zjxConfigData.LEDCOM + "打开失败,请检查是否有其他程序占用", "提示");     //没有获取到COM报错
                        }

                    }
                    else
                    {
                        MessageBox.Show("在该计算机上没有串口" + zjxConfigData.LEDCOM + "请重新选择串口", "提示");     //没有获取到COM报错
                        Msg(labelTS, panelTS, "设备未开启", Color.White);
                        return;
                    }
                    
                }
                catch
                {
                    Msg(labelTS, panelTS, "设备未开启", Color.White);
                    zjx100 = null;
                }
            }
        }
        #region 信息显示
        /// <summary>
        /// 信息显示
        /// </summary>
        /// <param name="Msgowner">信息显示的Label控件</param>
        /// <param name="Msgfather">Label控件的父级Panel</param>
        /// <param name="Msgstr">要显示的信息</param>
        /// <param name="Update_DB">是不是要更新到检测状态</param>
        public void Msg(Label Msgowner, Panel Msgfather, string Msgstr, Color Update_DB)
        {
            try
            {
                BeginInvoke(new wtlsb(Msg_Show), Msgowner, Msgstr, Update_DB);
                BeginInvoke(new wtlp(Msg_Position), Msgowner, Msgfather);
            }
            catch
            { }
        }

        public void Msg_Show(Label Msgowner, string Msgstr, Color Update_DB)
        {
            Msgowner.Text = Msgstr;
            Msgowner.ForeColor = Update_DB;

        }

        public void Msg_Position(Label Msgowner, Panel Msgfather)
        {
            if (Msgowner.Width < Msgfather.Width)
                Msgowner.Location = new Point((Msgfather.Width - Msgowner.Width) / 2, Msgowner.Location.Y);
            else
                Msgowner.Location = new Point(0, Msgowner.Location.Y);
        }

        
        #endregion

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (zjx100 != null)
                zjx100.closeEquipment();
            try
            {
                if (testThread != null)
                    testThread.Abort();
            }
            catch
            { }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (zjx100 != null)
            {
                Msg(labelLeftData, panelLeftData, zjx100.angleLeft.ToString("0.0"), Color.Yellow);
                if (zjx100.angleLeft > 180)
                    arcScaleComponentLeft.Value = 180f;
                else if (zjx100.angleLeft < -180)
                    arcScaleComponentLeft.Value = -180f;
                else
                    arcScaleComponentLeft.Value = zjx100.angleLeft;
                Msg(labelRightData, panelRightData, zjx100.angleRight.ToString("0.0"), Color.Yellow);
                if (zjx100.angleRight > 180)
                    arcScaleComponentLeft.Value = 180f;
                else if (zjx100.angleRight < -180)
                    arcScaleComponentRight.Value = -180f;
                else
                    arcScaleComponentRight.Value = zjx100.angleRight;
                if (highEffective)
                {
                    if ((zjx100.keyandgd & 0x01) == 0x01)
                    {
                        ovalShape1.FillColor = Color.Lime;
                        isLeftZero = true;
                    }
                    else
                    {
                        ovalShape1.FillColor = Color.DarkGray;
                        isLeftZero = false;
                    }
                    if ((zjx100.keyandgd & 0x04) == 0x04)
                    {
                        //ovalShape1.FillColor = Color.Lime;
                        isTestAlready = true;
                    }
                    else
                    {
                        //ovalShape1.FillColor = Color.DarkGray;
                        isTestAlready = false;
                    }
                }
                else
                {
                    if ((zjx100.keyandgd & 0x01) != 0x01)
                    {
                        ovalShape1.FillColor = Color.Lime;
                        isLeftZero = true;
                    }
                    else
                    {
                        ovalShape1.FillColor = Color.DarkGray;
                        isLeftZero = false;
                    }
                    if ((zjx100.keyandgd & 0x04) != 0x04)
                    {
                        //ovalShape1.FillColor = Color.Lime;
                        isTestAlready = true;
                    }
                    else
                    {
                        //ovalShape1.FillColor = Color.DarkGray;
                        isTestAlready = false;
                    }
                }
                if (highEffective)
                {
                    if ((zjx100.keyandgd & 0x02) == 0x02)
                    {
                        ovalShape2.FillColor = Color.Lime;
                        isRightZero = true;
                    }
                    else
                    {
                        ovalShape2.FillColor = Color.DarkGray;
                        isRightZero = false;
                    }
                }
                else
                {
                    if ((zjx100.keyandgd & 0x02) != 0x02)
                    {
                        ovalShape2.FillColor = Color.Lime;
                        isRightZero = true;
                    }
                    else
                    {
                        ovalShape2.FillColor = Color.DarkGray;
                        isRightZero = false;
                    }
                }
            }
            labelTime.Text = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");
        }

        private void buttonClearLeft_Click(object sender, EventArgs e)
        {
            if (zjx100 != null)
            {
                zjx100.Set_ClearLeft();
            }
        }

        private void buttonClearRight_Click(object sender, EventArgs e)
        {
            if (zjx100 != null)
            {
                zjx100.Set_ClearRight();
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (zjx100 != null)
            {
                if (testcar.isCarWait)
                {
                    MessageBox.Show("有车辆在等待检测,不能完成登录", "提示");
                    return;
                }
                else
                {
                    if(textBoxCLHP.Text==""||comboBoxCLLX.Text==""||textBoxMAXSPEED.Text=="")
                    {
                        MessageBox.Show("信息未填写完整", "提示");
                        return;
                    }
                    try
                    {
                        testcar.clhp = textBoxCLHP.Text;
                        testcar.isCarWait = true;
                        testcar.cllx = comboBoxCLLX.Text;
                        testcar.maxspeed = int.Parse(textBoxMAXSPEED.Text);
                        int selecxhindex = vehicleXhName.IndexOf(testcar.cllx);
                        testcar.llxz =(float)vehicleXhLLXz[selecxhindex];
                        testcar.lrxz = (float)vehicleXhLRXz[selecxhindex];
                        testcar.rlxz = (float)vehicleXhRLXz[selecxhindex];
                        testcar.rrxz = (float)vehicleXhRRXz[selecxhindex];
                        testcar.llxzwc = (float)vehicleXhLLXzWc[selecxhindex];
                        testcar.lrxzwc = (float)vehicleXhLRXzWc[selecxhindex];
                        testcar.rlxzwc = (float)vehicleXhRLXzWc[selecxhindex];
                        testcar.rrxzwc = (float)vehicleXhRRXzWc[selecxhindex];
                        panelLogin.Visible = false;
                        panelAddXh.Visible = false;
                        panelXh.Visible = false;
                        panelModify.Visible = false;
                    }
                    catch
                    {
                        MessageBox.Show("最大速度格式不规范", "提示");
                        return;
                    }
                    
                }
            }
            else
            {
                MessageBox.Show("设备未正常开启,不能完成登录", "提示");
                return;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            panelLogin.Visible = false;
            panelAddXh.Visible = false;
            panelModify.Visible = false;
            panelXh.Visible = false;
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            if (zjx100 != null)
            {
                testcar.isCarWait = false;
                try
                {
                    buttonLock_Click(sender, e);
                    button1_Click(sender, e);
                    isTurnToLeft = false;
                    isTurnToRight = false;
                    init_msgShow();
                    isTestStart = false;
                    buttonStart.Text = "启动";
                    testThread.Abort();
                    testThread = new Thread(test_Process);
                    testThread.Start();
                }
                catch
                { }
            }
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            panelLogin.Visible = !panelLogin.Visible;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (zjx100 != null&&testcar.isCarWait)
            {
                
                if (buttonStart.Text == "启动")
                {
                    if (!isLeftZero && isRightZero)
                        MessageBox.Show("左转盘未摆正");
                    else if (isLeftZero && !isRightZero)
                        MessageBox.Show("右转盘未摆正");
                    else if (!isLeftZero && !isRightZero)
                        MessageBox.Show("左转盘及右转盘未摆正");
                    else
                    {
                        isTestStart = true;
                        buttonStart.Text = "停止";
                    }
                }
                else
                {
                    isTestStart = false;
                    buttonStart.Text = "启动";
                }
            }
            else
                MessageBox.Show("设备未正常开启或未登录车辆,不能进行检测", "提示");
        }

        private void buttonLock_Click(object sender, EventArgs e)
        {
            if (zjxConfigData.ZPEFFCTIVE == "1")
                zjx100.Set_GAB100RelayOn(zp);
            else
                zjx100.Set_GAB100RelayOff(zp);
        }

        private void buttonRelock_Click(object sender, EventArgs e)
        {
            if (zjxConfigData.ZPEFFCTIVE == "1")
                zjx100.Set_GAB100RelayOff(zp);
            else
                zjx100.Set_GAB100RelayOn(zp);
        }
        private void init_msgShow()
        {
            if (zjx100 != null)
            {
                Msg(labelTS, panelTS, "等待检测", Color.White);
                Msg(labelCLHP, panelCLHP, "车辆牌号", Color.White);
                Msg(labelLzlldata, panelLzlldata, testcar.leftltleft.ToString("+0.0"), Color.Yellow);
                Msg(labelLzrldata, panelLzrldata, testcar.rightltleft.ToString("+0.0"), Color.Yellow);
                Msg(labelLzLljg, panelLzLljg, "TEST", Color.Yellow);
                Msg(labelLzRljg, panelLzRljg, "TEST", Color.Yellow);
                Msg(labelRzlldata, panelRzlldata, testcar.leftltleft.ToString("+0.0"), Color.Yellow);
                Msg(labelRzrldata, panelRzrldata, testcar.rightltleft.ToString("+0.0"), Color.Yellow);
                Msg(labelRzLljg, panelRzLljg, "TEST", Color.Yellow);
                Msg(labelRzRljg, panelRzRljg, "TEST", Color.Yellow);
                Msg(labelTotalPd, panelTotalPd, "TEST", Color.Yellow);

            }
        }

        private void buttonTools_Click(object sender, EventArgs e)
        {
            //if (!testcar.isCarWait)
            //{
            //    testcar.clhp = "川AV7M82";
            //    testcar.isCarWait = true;
            //    testcar.xz = 30f;
            //}
            //else
            //    MessageBox.Show("有车辆正在待检测,不能进行测试", "提示");
            dataquery dataqueryform = new dataquery();
            dataqueryform.TopLevel = false;
            dataqueryform.Dock = DockStyle.Fill;
            dataqueryform.FormBorderStyle = FormBorderStyle.None;
            panel3.Controls.Add(dataqueryform);
            dataqueryform.BringToFront();
            //dataqueryform.Location = new Point(sc_width / 2 - childcarlogin.Width / 2, 4);
            dataqueryform.Show();
        }

        private void buttonCOM_Click(object sender, EventArgs e)
        {
            ini.INIIO.WritePrivateProfileString("配置信息", "串口", comboBoxCom.Text.ToString(), @".\appConfig.ini");
            zjxConfigData.LEDCOM=comboBoxCom.Text;
            try
            {
                if (zjx100 != null)
                {
                    try
                    {
                        if (testThread != null)
                        {
                            if (testThread.IsAlive) testThread.Abort();
                        }
                    }
                    catch
                    { }
                    try
                    {
                        if (zjx100.ComPort_2.IsOpen) zjx100.ComPort_2.Close();
                    }
                    catch
                    { }
                    try
                    {
                        //zjx100 = new ZJX_100("BNTD");
                        if (zjx100.Init_Comm(zjxConfigData.LEDCOM, zjxConfigData.LEDSTRING))
                        {
                            Msg(labelTS, panelTS, "等待检测", Color.White);
                            zjx100.Set_ClearLeft();
                            zjx100.Set_ClearRight();
                            zjx100.Set_ClearKey();
                            zjx100.Start_test();
                            timer1.Start();
                            timer2.Start();
                            initThread();
                            panelComConfig.Visible = false;
                        }
                        else
                        {
                            Msg(labelTS, panelTS, "设备未开启", Color.White);
                            zjx100 = null;
                        }
                    }
                    catch
                    {
                        Msg(labelTS, panelTS, "设备未开启", Color.White);
                        MessageBox.Show("串口" + zjxConfigData.LEDCOM + "打开失败,请检查是否有其他程序占用", "提示");     //没有获取到COM报错
                    }
                }
                else
                {
                    try
                    {
                        zjx100 = new ZJX_100("BNTD");
                        if (zjx100.Init_Comm(zjxConfigData.LEDCOM, zjxConfigData.LEDSTRING))
                        {
                            Msg(labelTS, panelTS, "等待检测", Color.White);
                            zjx100.Set_ClearLeft();
                            zjx100.Set_ClearRight();
                            zjx100.Set_ClearKey();
                            zjx100.Start_test();
                            timer1.Start();
                            timer2.Start();
                            initThread();
                            panelComConfig.Visible = false;
                        }
                        else
                        {
                            Msg(labelTS, panelTS, "设备未开启", Color.White);
                            zjx100 = null;
                        }
                    }
                    catch
                    {
                        Msg(labelTS, panelTS, "设备未开启", Color.White);
                        MessageBox.Show("串口" + zjxConfigData.LEDCOM + "打开失败,请检查是否有其他程序占用", "提示");     //没有获取到COM报错
                    }
                }
            }
            catch
            {
                //comInitSuccess = false;
                MessageBox.Show("串口" + zjxConfigData.LEDCOM + "打开失败,请检查是否有其他程序占用", "提示");     //没有获取到COM报错
            }
        }

        private void buttonSureK_Click(object sender, EventArgs e)
        {
            try
            {
                int leftPulse = 0;
                int rightPulse = 0;
                leftPulse = int.Parse(textBoxKl.Text);
                rightPulse = int.Parse(textBoxKr.Text);
                float kl = 360f / (float)leftPulse;
                float kr = 360f / (float)rightPulse;
                if (zjx100 != null)
                {
                    zjx100.Set_Kl(kl);
                    zjx100.Set_Kr(kr);
                    Thread.Sleep(100);
                    zjx100.writeKtoRom();
                    MessageBox.Show("修改成功", "提示");
                    panelKconfig.Visible = false;
                }
                else
                {
                    MessageBox.Show("设备未开启，不能完成配置", "提示"); 
                }
            }
            catch
            {
                MessageBox.Show("输入格式有误", "提示"); 
            }
        }

        private void buttonDemarcate_Click(object sender, EventArgs e)
        {
            if (panelKconfig.Visible == false)
            {
                if (zjx100 != null)
                {
                    zjx100.readKfromRom();
                    Thread.Sleep(500);
                    int leftpulse = 0;
                    int rightpulse = 0;
                    leftpulse = (int)(360.0 / zjx100.kl);
                    rightpulse = (int)(360.0 / zjx100.kr);
                    textBoxKl.Text = leftpulse.ToString();
                    textBoxKr.Text = rightpulse.ToString();
                }
            }
            panelKconfig.Visible = !panelKconfig.Visible;
        }

        private void buttonCancelK_Click(object sender, EventArgs e)
        {
            panelKconfig.Visible = false;
        }

        private void buttonCancelCOM_Click(object sender, EventArgs e)
        {
            panelComConfig.Visible = false;
        }

        private void buttonConfig_Click(object sender, EventArgs e)
        {
            panelComConfig.Visible = !panelComConfig.Visible;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (isTurnToLeft)
            {
                if (pictureBox1.BackColor == Color.Transparent)
                    pictureBox1.BackColor = Color.GreenYellow;
                else
                    pictureBox1.BackColor = Color.Transparent;
                pictureBox2.BackColor = Color.Transparent;
            }
            else if (isTurnToRight)
            {
                if (pictureBox2.BackColor == Color.Transparent)
                    pictureBox2.BackColor = Color.GreenYellow;
                else
                    pictureBox2.BackColor = Color.Transparent;
                pictureBox1.BackColor = Color.Transparent;
            }
            else
            {
                pictureBox1.BackColor = Color.Transparent;
                pictureBox2.BackColor = Color.Transparent;
            }
        }

        private void panelLogin_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (zjxConfigData.BZEFFCTIVE == "1")
                zjx100.Set_GAB100RelayOn(bzq);
            else
                zjx100.Set_GAB100RelayOff(bzq);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (zjxConfigData.BZEFFCTIVE == "1")
                zjx100.Set_GAB100RelayOff(bzq);
            else
                zjx100.Set_GAB100RelayOn(bzq); 
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            panelXh.Visible = true;
        }

        private void buttonAddXh_Click(object sender, EventArgs e)
        {
            panelAddXh.Visible = true;
        }

        private void buttonCancelAdd_Click(object sender, EventArgs e)
        {
            panelAddXh.Visible = false;
        }

        private void buttonQuitXh_Click(object sender, EventArgs e)
        {
            panelXh.Visible = false;
            panelAddXh.Visible = false;
            panelModify.Visible = false;
        }

        private void buttonDeletXh_Click(object sender, EventArgs e)
        {
            if (selectID[0] != null && selectID[0] != "")
            {
                int deleteIndex = vehicleXhName.IndexOf(selectID[0]);
                vehicleXhName.RemoveAt(deleteIndex);
                vehicleXhLLXz.RemoveAt(deleteIndex);
                vehicleXhLRXz.RemoveAt(deleteIndex);
                vehicleXhRLXz.RemoveAt(deleteIndex);
                vehicleXhRRXz.RemoveAt(deleteIndex);
                vehicleXhLLXzWc.RemoveAt(deleteIndex);
                vehicleXhLRXzWc.RemoveAt(deleteIndex);
                vehicleXhRLXzWc.RemoveAt(deleteIndex);
                vehicleXhRRXzWc.RemoveAt(deleteIndex);
                save_vehicleXh();
                ref_vehicleXh();
            }
        }

        private void buttonSureAdd_Click(object sender, EventArgs e)
        {
            if (textBoxAddedXh.Text != "" && textBoxAddedXhLLXz.Text != "")
            {
                try
                {
                    string addedName = textBoxAddedXh.Text;
                    double addedLLXz = double.Parse(textBoxAddedXhLLXz.Text);
                    double addedLRXz = double.Parse(textBoxAddedXhLRXz.Text);
                    double addedRLXz = double.Parse(textBoxAddedXhRLXz.Text);
                    double addedRRXz = double.Parse(textBoxAddedXhRRXz.Text);
                    double addedLLXzWc = double.Parse(textBoxAddedXhLLXzWc.Text);
                    double addedLRXzWc = double.Parse(textBoxAddedXhLRXzWc.Text);
                    double addedRLXzWc = double.Parse(textBoxAddedXhRLXzWc.Text);
                    double addedRRXzWc = double.Parse(textBoxAddedXhRRXzWc.Text);
                    if (vehicleXhName.Contains(addedName))
                    {
                        MessageBox.Show("该车型已存在,不能重复添加");
                    }
                    else
                    {
                        vehicleXhName.Add(addedName);
                        vehicleXhLLXz.Add(addedLLXz);
                        vehicleXhLRXz.Add(addedLRXz);
                        vehicleXhRLXz.Add(addedRLXz);
                        vehicleXhRRXz.Add(addedRRXz);
                        vehicleXhLLXzWc.Add(addedLLXzWc);
                        vehicleXhLRXzWc.Add(addedLRXzWc);
                        vehicleXhRLXzWc.Add(addedRLXzWc);
                        vehicleXhRRXzWc.Add(addedRRXzWc);
                        save_vehicleXh();
                        ref_vehicleXh();
                    }
                }
                catch
                {
                    MessageBox.Show("限值输入格式有误");
                }
            }
            else
            {
                MessageBox.Show("请填写完整");
            }
        }

        private void buttonModify_Click(object sender, EventArgs e)
        {
            if (selectID[0] != null && selectID[0] != "")
            {
                textBoxModifyName.Text = selectID[0];
                int deleteIndex = vehicleXhName.IndexOf(selectID[0]);
                textBoxModifyLL.Text = vehicleXhLLXz[deleteIndex].ToString();
                textBoxModifyLR.Text = vehicleXhLRXz[deleteIndex].ToString();
                textBoxModifyRL.Text = vehicleXhRLXz[deleteIndex].ToString();
                textBoxModifyRR.Text = vehicleXhRRXz[deleteIndex].ToString();
                textBoxModifyLLWc.Text = vehicleXhLLXzWc[deleteIndex].ToString();
                textBoxModifyLRWc.Text = vehicleXhLRXzWc[deleteIndex].ToString();
                textBoxModifyRLWc.Text = vehicleXhRLXzWc[deleteIndex].ToString();
                textBoxModifyRRWc.Text = vehicleXhRRXzWc[deleteIndex].ToString();
                panelModify.Location = new Point(826, 102);
                panelModify.Visible = true;
            }
        }

        private void buttonCancelModify_Click(object sender, EventArgs e)
        {
            panelModify.Visible = false;
        }

        private void buttonOKmodify_Click(object sender, EventArgs e)
        {
            if (selectID[0] != null && selectID[0] != "")
            {
                try
                {
                    int deleteIndex = vehicleXhName.IndexOf(selectID[0]);
                    vehicleXhLLXz[deleteIndex]=double.Parse(textBoxModifyLL.Text);
                    vehicleXhLRXz[deleteIndex] = double.Parse(textBoxModifyLR.Text);
                    vehicleXhRLXz[deleteIndex] = double.Parse(textBoxModifyRL.Text);
                    vehicleXhRRXz[deleteIndex] = double.Parse(textBoxModifyRR.Text);
                    vehicleXhLLXzWc[deleteIndex] = double.Parse(textBoxModifyLLWc.Text);
                    vehicleXhLRXzWc[deleteIndex] = double.Parse(textBoxModifyLRWc.Text);
                    vehicleXhRLXzWc[deleteIndex] = double.Parse(textBoxModifyRLWc.Text);
                    vehicleXhRRXzWc[deleteIndex] = double.Parse(textBoxModifyRRWc.Text);
                    save_vehicleXh();
                    ref_vehicleXh();
                    panelModify.Visible = false;
                }
                catch
                {
                    MessageBox.Show("限值输入格式有误");
                }
            }
        }
    }
}
