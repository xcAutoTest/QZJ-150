using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace childTest
{
    public partial class maintest : Form
    {
        public enum ENUM_WORK_MODE { WORK_MODE_NET, WORK_MODE_LOCAL };
        public ENUM_WORK_MODE workMode;
        public maintest()
        {
            InitializeComponent();
        }
        public string VIN="";
        public string VEHICLENAME="";
        public delegate void wtlsb(Label Msgowner, string Msgstr);                   //委托
        public delegate void wtlsbc(Label Msgowner, string Msgstr,Color color);                   //委托
        public delegate void wtlp(Label Msgowner, Panel Msgfather, int position);                                 //委托

        /// <summary>
        /// 信息显示
        /// </summary>
        /// <param name="Msgowner">信息显示的Label控件</param>
        /// <param name="Msgfather">Label控件的父级Panel</param>
        /// <param name="Msgstr">要显示的信息</param>
        /// <param name="position">0-居中 1-靠左 2-靠右</param>
        public void Msg(Label Msgowner, Panel Msgfather, string Msgstr, int position,Color color)
        {
            try
            {
                BeginInvoke(new wtlsbc(Msg_Show), Msgowner, Msgstr,color);
                BeginInvoke(new wtlp(Msg_Position), Msgowner, Msgfather, position);
            }
            catch
            { }
        }
        public void Msg(Label Msgowner, Panel Msgfather, string Msgstr, int position)
        {
            try
            {
                BeginInvoke(new wtlsb(Msg_Show), Msgowner, Msgstr);
                BeginInvoke(new wtlp(Msg_Position), Msgowner, Msgfather, position);
            }
            catch
            { }
        }

        public void Msg_Show(Label Msgowner, string Msgstr)
        {
            Msgowner.Text = Msgstr;
        }
        public void Msg_Show(Label Msgowner, string Msgstr,Color color)
        {
            Msgowner.Text = Msgstr;
            Msgowner.ForeColor = color;
        }

        public void Msg_Position(Label Msgowner, Panel Msgfather, int position)
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
        string extlinkdir = @"D:\QAII\PSDDAT\EXTLINK.ini";
        
        private bool writeTestResult(qzj_record model)
        {
            if (System.IO.File.Exists(extlinkdir))
            {
                ini.INIIO.WritePrivateProfileString("ExtLinkData", "Result_5147", model.LEFTTURNLEFT.ToString("0"), extlinkdir);
                ini.INIIO.WritePrivateProfileString("ExtLinkData", "Judge_5147", model.LEFTTURNLEFTPD=="合格"?"1":"2", extlinkdir);
                ini.INIIO.WritePrivateProfileString("ExtLinkData", "Result_5148", model.LEFTTURNRIGHT.ToString("0"), extlinkdir);
                ini.INIIO.WritePrivateProfileString("ExtLinkData", "Judge_5148", model.LEFTTURNRIGHTPD == "合格" ? "1" : "2", extlinkdir);
                ini.INIIO.WritePrivateProfileString("ExtLinkData", "Result_5149", model.RIGHTTURNRIGHT.ToString("0"), extlinkdir);
                ini.INIIO.WritePrivateProfileString("ExtLinkData", "Judge_5149", model.RIGHTTURNRIGHTPD == "合格" ? "1" : "2", extlinkdir);
                ini.INIIO.WritePrivateProfileString("ExtLinkData", "Result_5150", model.RIGHTTURNLEFT.ToString("0"), extlinkdir);
                ini.INIIO.WritePrivateProfileString("ExtLinkData", "Judge_5150", model.RIGHTTURNLEFTPD == "合格" ? "1" : "2", extlinkdir);
                ini.INIIO.WritePrivateProfileString("ExtLink", "ScanOK", "5", extlinkdir);
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool isTestStart = false;
        private Thread testThread = null;
        qzj_vehicle vehiclemodel = new qzj_vehicle();
        private void ZjyTest_Load(object sender, EventArgs e)
        {
            labelWorkMode.Text = (controller.workMode == controller.ENUM_WORDMODE.WORKMODE_DEBUG ? "调校模式" : "检验模式");
            string sendstring;
            if (VEHICLENAME == "")
            {
                logControl.saveLogInf("[车型名称为空，采用默认值]");
                if (SqlControl.getVehicleXz("DEFAULT", out vehiclemodel))
                {
                    logControl.saveLogInf("[获取默认车型数据成功]：" + vehiclemodel.VEHICLENAME + "|"
                        + vehiclemodel.LEFTTURNLEFT + "|" + vehiclemodel.LEFTTURNRIGHT + "|" + vehiclemodel.RIGHTTURNLEFT + "|" + vehiclemodel.RIGHTTURNRIGHT + "|"
                        + vehiclemodel.LEFTTURNLEFTWC + "|" + vehiclemodel.LEFTTURNRIGHTWC + "|" + vehiclemodel.RIGHTTURNLEFTWC + "|" + vehiclemodel.RIGHTTURNRIGHTWC + "|");
                }
                else
                {
                    Msg(labelOrder3, panelOrder3, "获取车辆数据失败", 0);
                    return;
                }
            }
            else if (SqlControl.getVehicleXz(VEHICLENAME, out vehiclemodel))
            {
                logControl.saveLogInf("[获取车型数据成功]：" + vehiclemodel.VEHICLENAME + "|" 
                    + vehiclemodel.LEFTTURNLEFT + "|" + vehiclemodel.LEFTTURNRIGHT + "|" + vehiclemodel.RIGHTTURNLEFT + "|" + vehiclemodel.RIGHTTURNRIGHT + "|"
                    + vehiclemodel.LEFTTURNLEFTWC + "|" + vehiclemodel.LEFTTURNRIGHTWC + "|" + vehiclemodel.RIGHTTURNLEFTWC + "|" + vehiclemodel.RIGHTTURNRIGHTWC + "|");
            }
            else
            {
                logControl.saveLogInf("[未获取到车型数据，采用默认值]");
                if (SqlControl.getVehicleXz("DEFAULT", out vehiclemodel))
                {
                    logControl.saveLogInf("[获取默认车型数据成功]：" + vehiclemodel.VEHICLENAME + "|"
                        + vehiclemodel.LEFTTURNLEFT + "|" + vehiclemodel.LEFTTURNRIGHT + "|" + vehiclemodel.RIGHTTURNLEFT + "|" + vehiclemodel.RIGHTTURNRIGHT + "|"
                        + vehiclemodel.LEFTTURNLEFTWC + "|" + vehiclemodel.LEFTTURNRIGHTWC + "|" + vehiclemodel.RIGHTTURNLEFTWC + "|" + vehiclemodel.RIGHTTURNRIGHTWC + "|");
                }
                else
                {
                    Msg(labelOrder3, panelOrder3, "获取车辆数据失败", 0);
                    return;
                }
            }
            Msg(labelllxz, panelll, "(" + vehiclemodel.LEFTTURNLEFT.ToString("0.0") + "±" + vehiclemodel.LEFTTURNLEFTWC.ToString("0.0") + ")",2);
            Msg(labellrxz, panellr, "(" + vehiclemodel.LEFTTURNRIGHT.ToString("0.0") + "±" + vehiclemodel.LEFTTURNRIGHTWC.ToString("0.0") + ")",2);
            Msg(labelrlxz, panelrl, "(" + vehiclemodel.RIGHTTURNLEFT.ToString("0.0") + "±" + vehiclemodel.RIGHTTURNLEFTWC.ToString("0.0") + ")",2);
            Msg(labelrrxz, panelrr, "(" + vehiclemodel.RIGHTTURNRIGHT.ToString("0.0") + "±" + vehiclemodel.RIGHTTURNRIGHTWC.ToString("0.0") + ")",2);
            buttonStart_Click(sender, e);
        }
        private void test_Process()
        {
            try
            {
                string sendstring;
                double leftangle, rightangle;
                double leftturnleft = 0, leftturnright = 0, rightturnleft = 0, rightturnright = 0;
                bool leftturnleftpd = false, leftturnrightpd = false, rightturnleftpd = false, rightturnrightpd = false,zhpd=false;
                Msg(labelOrder3, panelOrder3, "请上转盘", 0);
                int carAlready = 0;
                while (carAlready < 30)
                {
                    if (!controller.isCarAlready) carAlready = 0;
                    else carAlready++;
                    Thread.Sleep(50);
                }
                Msg(labelOrder3, panelOrder3, "车辆到位", 0);
                Thread.Sleep(1000);
                controller.setClearLeft(out sendstring);
                controller.setClearRight(out sendstring);
                Thread.Sleep(100);
                controller.straightCar(out sendstring);
                Thread.Sleep(100);
                controller.unLockTable(out sendstring);
                Thread.Sleep(1000);
                int stablecount = 0;
                Msg(labelOrder3, panelOrder3, "向左打方向盘", 0);
                //while (controller.angleLeft < 5)
                //{
                //   Msg(labelLeftData, panelLeftData, controller.angleLeft.ToString("0.0"), 0, Math.Abs(controller.angleLeft - vehiclemodel.LEFTTURNLEFT) <= vehiclemodel.LEFTTURNLEFTWC ? Color.Lime : Color.Red);
                //    Msg(labelRightData, panelRightData, controller.angleRight.ToString("0.0"), 0, Math.Abs(controller.angleRight - vehiclemodel.RIGHTTURNLEFT) <= vehiclemodel.LEFTTURNLEFTWC ? Color.Lime : Color.Red);
                //   Thread.Sleep(50);
                //}
                while (stablecount < 40)//大于5度2秒以上
                {
                    leftangle = controller.angleLeft;
                    rightangle = controller.angleRight;
                    Msg(labelLeftData, panelLeftData, leftangle.ToString("0.0"), 0, Math.Abs(leftangle - vehiclemodel.LEFTTURNLEFT) <= vehiclemodel.LEFTTURNLEFTWC ? Color.Lime : Color.Red);
                    Msg(labelRightData, panelRightData, rightangle.ToString("0.0"), 0, Math.Abs(-rightangle - vehiclemodel.RIGHTTURNLEFT) <= vehiclemodel.LEFTTURNLEFTWC ? Color.Lime : Color.Red);
                    if (controller.angleLeft < 5)
                    {
                        stablecount = 0;
                    }
                    else//显示过程中的最大值
                    {
                        if (controller.workMode == controller.ENUM_WORDMODE.WORKMODE_DEBUG)//在调试模式下一直等到连续两秒左右都在合格范围内了才继续进行
                        {
                            leftturnleft = leftangle;
                            rightturnleft = rightangle;
                            if ((Math.Abs(leftturnleft - vehiclemodel.LEFTTURNLEFT) <= vehiclemodel.LEFTTURNLEFTWC) && (Math.Abs(-rightturnleft - vehiclemodel.RIGHTTURNLEFT) <= vehiclemodel.LEFTTURNLEFTWC))//如果在合格范围内，开始计时
                            {
                                stablecount++;
                            }
                            else
                            {
                                stablecount = 0;
                            }
                        }
                        else//在检验模式下，要等到连续两秒左盘没有超过目前最大值时认为稳定
                        {
                            if (leftangle > leftturnleft)
                            {
                                leftturnleft = leftangle;
                                rightturnleft = rightangle;
                                stablecount = 0;
                            }
                            else
                            {
                                stablecount++;
                            }
                        }
                        Msg(labelLzlldata, panelLzlldata, leftturnleft.ToString("0.0"), 0, Math.Abs(leftturnleft - vehiclemodel.LEFTTURNLEFT) <= vehiclemodel.LEFTTURNLEFTWC ? Color.Lime : Color.Red);
                        Msg(labelRzlldata, panelRzlldata, rightturnleft.ToString("0.0"), 0, Math.Abs(-rightturnleft - vehiclemodel.RIGHTTURNLEFT) <= vehiclemodel.LEFTTURNLEFTWC ? Color.Lime : Color.Red);
                    }
                    Thread.Sleep(50);
                }
                stablecount = 0;
                Msg(labelOrder3, panelOrder3, "稳定1秒", 0);
                while (stablecount < 20)
                {
                    leftangle = controller.angleLeft;
                    rightangle = controller.angleRight;
                    Msg(labelLeftData, panelLeftData, leftangle.ToString("0.0"), 0, Math.Abs(leftangle - vehiclemodel.LEFTTURNLEFT) <= vehiclemodel.LEFTTURNLEFTWC ? Color.Lime : Color.Red);
                    Msg(labelRightData, panelRightData, rightangle.ToString("0.0"), 0, Math.Abs(-rightangle - vehiclemodel.RIGHTTURNLEFT) <= vehiclemodel.LEFTTURNLEFTWC ? Color.Lime : Color.Red);
                    if (controller.angleLeft < 5)
                    {
                        stablecount = 0;
                    }
                    else//显示过程中的最大值
                    {
                        if (controller.workMode == controller.ENUM_WORDMODE.WORKMODE_DEBUG)//在调试模式下一直等到连续两秒左右都在合格范围内了才继续进行
                        {
                            leftturnleft = leftangle;
                            rightturnleft = rightangle;
                            if ((Math.Abs(leftturnleft - vehiclemodel.LEFTTURNLEFT) <= vehiclemodel.LEFTTURNLEFTWC) && (Math.Abs(-rightturnleft - vehiclemodel.RIGHTTURNLEFT) <= vehiclemodel.LEFTTURNLEFTWC))//如果在合格范围内，开始计时
                            {
                                stablecount++;
                            }
                            else
                            {
                                stablecount = 0;
                            }
                        }
                        else//在检验模式下，要等到连续两秒左盘没有超过目前最大值时认为稳定
                        {
                            if (leftangle > leftturnleft)
                            {
                                leftturnleft = leftangle;
                                rightturnleft = rightangle;
                                stablecount = 0;
                            }
                            else
                            {
                                stablecount++;
                            }
                        }
                        Msg(labelLzlldata, panelLzlldata, leftturnleft.ToString("0.0"), 0, Math.Abs(leftturnleft - vehiclemodel.LEFTTURNLEFT) <= vehiclemodel.LEFTTURNLEFTWC ? Color.Lime : Color.Red);
                        Msg(labelRzlldata, panelRzlldata, rightturnleft.ToString("0.0"), 0, Math.Abs(-rightturnleft - vehiclemodel.RIGHTTURNLEFT) <= vehiclemodel.LEFTTURNLEFTWC ? Color.Lime : Color.Red);
                    }
                    Thread.Sleep(50);
                }
                leftturnleftpd = (Math.Abs(leftturnleft - vehiclemodel.LEFTTURNLEFT) <= vehiclemodel.LEFTTURNLEFTWC);
                Msg(labelllpd, panelllpd, leftturnleftpd ? "○" : "×", 0, leftturnleftpd ? Color.Lime : Color.Red);
                rightturnleftpd = (Math.Abs(-rightturnleft - vehiclemodel.RIGHTTURNLEFT) <= vehiclemodel.LEFTTURNLEFTWC);
                Msg(labelrlpd, panelrlpd, rightturnleftpd ? "○" : "×", 0, rightturnleftpd ? Color.Lime : Color.Red);
                Msg(labelOrder3, panelOrder3, "右打方向", 0);
                stablecount = 0;
                while (controller.angleRight < 5)
                {
                    leftangle = controller.angleLeft;
                    rightangle = controller.angleRight;
                    Msg(labelLeftData, panelLeftData, leftangle.ToString("0.0"), 0, Math.Abs(-leftangle - vehiclemodel.LEFTTURNRIGHT) <= vehiclemodel.LEFTTURNRIGHTWC ? Color.Lime : Color.Red);
                    Msg(labelRightData, panelRightData, rightangle.ToString("0.0"), 0, Math.Abs(rightangle - vehiclemodel.RIGHTTURNRIGHT) <= vehiclemodel.RIGHTTURNRIGHTWC ? Color.Lime : Color.Red);
                    Thread.Sleep(50);
                }
                Msg(labelOrder3, panelOrder3, "向右打方向盘", 0);
                stablecount = 0;
                while (stablecount < 40)//大于5度2秒以上
                {
                    leftangle = controller.angleLeft;
                    rightangle = controller.angleRight;
                    Msg(labelLeftData, panelLeftData, leftangle.ToString("0.0"), 0, Math.Abs(-leftangle - vehiclemodel.LEFTTURNRIGHT) <= vehiclemodel.LEFTTURNRIGHTWC ? Color.Lime : Color.Red);
                    Msg(labelRightData, panelRightData, rightangle.ToString("0.0"), 0, Math.Abs(rightangle - vehiclemodel.RIGHTTURNRIGHT) <= vehiclemodel.RIGHTTURNRIGHTWC ? Color.Lime : Color.Red);
                    if (controller.angleRight < 5)
                    {
                        stablecount = 0;
                    }
                    else//显示过程中的最大值
                    {
                        if (controller.workMode == controller.ENUM_WORDMODE.WORKMODE_DEBUG)//在调试模式下一直等到连续两秒左右都在合格范围内了才继续进行
                        {
                            leftturnright = leftangle;
                            rightturnright = rightangle;
                            if ((Math.Abs(-leftturnright - vehiclemodel.LEFTTURNRIGHT) <= vehiclemodel.LEFTTURNRIGHTWC) && (Math.Abs(rightturnright - vehiclemodel.RIGHTTURNRIGHT) <= vehiclemodel.RIGHTTURNRIGHTWC))//如果在合格范围内，开始计时
                            {
                                stablecount++;
                            }
                            else
                            {
                                stablecount = 0;
                            }
                        }
                        else//在检验模式下，要等到连续两秒左盘没有超过目前最大值时认为稳定
                        {
                            if (rightangle > rightturnright)
                            {
                                rightturnright = rightangle;
                                leftturnright = leftangle;
                                stablecount = 0;
                            }
                            else
                            {
                                stablecount++;
                            }
                        }
                        Msg(labelLzrldata, panelLzrldata, leftturnright.ToString("0.0"), 0, Math.Abs(-leftturnright - vehiclemodel.LEFTTURNRIGHT) <= vehiclemodel.LEFTTURNRIGHTWC ? Color.Lime : Color.Red);
                        Msg(labelRzrldata, panelRzrldata, rightturnright.ToString("0.0"), 0, Math.Abs(rightturnright - vehiclemodel.RIGHTTURNRIGHT) <= vehiclemodel.RIGHTTURNRIGHTWC ? Color.Lime : Color.Red);
                    }
                    Thread.Sleep(50);
                }
                stablecount = 0;
                Msg(labelOrder3, panelOrder3, "稳定1秒", 0);
                while (stablecount < 20)
                {
                    leftangle = controller.angleLeft;
                    rightangle = controller.angleRight;
                    Msg(labelLeftData, panelLeftData, leftangle.ToString("0.0"), 0, Math.Abs(-leftangle - vehiclemodel.LEFTTURNRIGHT) <= vehiclemodel.LEFTTURNRIGHTWC ? Color.Lime : Color.Red);
                    Msg(labelRightData, panelRightData, rightangle.ToString("0.0"), 0, Math.Abs(rightangle - vehiclemodel.RIGHTTURNRIGHT) <= vehiclemodel.RIGHTTURNRIGHTWC ? Color.Lime : Color.Red);
                    if (controller.angleRight < 5)
                    {
                        stablecount = 0;
                    }
                    else//显示过程中的最大值
                    {
                        if (controller.workMode == controller.ENUM_WORDMODE.WORKMODE_DEBUG)//在调试模式下一直等到连续两秒左右都在合格范围内了才继续进行
                        {
                            leftturnright = leftangle;
                            rightturnright = rightangle;
                            if ((Math.Abs(-leftturnright - vehiclemodel.LEFTTURNRIGHT) <= vehiclemodel.LEFTTURNRIGHTWC) && (Math.Abs(rightturnright - vehiclemodel.RIGHTTURNRIGHT) <= vehiclemodel.RIGHTTURNRIGHTWC))//如果在合格范围内，开始计时
                            {
                                stablecount++;
                            }
                            else
                            {
                                stablecount = 0;
                            }
                        }
                        else//在检验模式下，要等到连续两秒左盘没有超过目前最大值时认为稳定
                        {
                            if (rightangle > rightturnright)
                            {
                                rightturnright = rightangle;
                                leftturnright = leftangle;
                                stablecount = 0;
                            }
                            else
                            {
                                stablecount++;
                            }
                        }
                        Msg(labelLzrldata, panelLzrldata, leftturnright.ToString("0.0"), 0, Math.Abs(-leftturnright - vehiclemodel.LEFTTURNRIGHT) <= vehiclemodel.LEFTTURNRIGHTWC ? Color.Lime : Color.Red);
                        Msg(labelRzrldata, panelRzrldata, rightturnright.ToString("0.0"), 0, Math.Abs(rightturnright - vehiclemodel.RIGHTTURNRIGHT) <= vehiclemodel.RIGHTTURNRIGHTWC ? Color.Lime : Color.Red);
                    }
                    Thread.Sleep(50);
                }
                leftturnrightpd = (Math.Abs(-leftturnright - vehiclemodel.LEFTTURNRIGHT) <= vehiclemodel.LEFTTURNRIGHTWC);
                Msg(labellrpd, panellrpd, leftturnrightpd ? "○" : "×", 0, leftturnrightpd ? Color.Lime : Color.Red);
                rightturnrightpd = (Math.Abs(rightturnright - vehiclemodel.RIGHTTURNRIGHT) <= vehiclemodel.RIGHTTURNRIGHTWC);
                Msg(labelrrpd, panelrrpd, rightturnrightpd ? "○" : "×", 0, rightturnrightpd ? Color.Lime : Color.Red);
                Msg(labelOrder3, panelOrder3, "请回零", 0);
                stablecount = 0;
                while (stablecount<20)//当左盘和右盘均回到规定的回零限值后
                {
                    leftangle = controller.angleLeft;
                    rightangle = controller.angleRight;
                    Msg(labelLeftData, panelLeftData, leftangle.ToString("0.0"), 0, controller.isLeftZero ? Color.Lime : Color.Red);
                    Msg(labelRightData, panelRightData, rightangle.ToString("0.0"), 0, controller.isRightZero ? Color.Lime : Color.Red);
                    if (!controller.isLeftZero || !controller.isRightZero)
                        stablecount = 0;
                    else
                        stablecount++;
                    Thread.Sleep(50);
                }
                zhpd = (leftturnleftpd && leftturnrightpd && rightturnleftpd && rightturnrightpd);
                Msg(labelOrder3, panelOrder3, zhpd ? "转向角 ○" : "转向角 ×", 0);
                controller.lockTable(out sendstring);
                Thread.Sleep(100);
                controller.backStraight(out sendstring);
                Thread.Sleep(1000);
                qzj_record record = new qzj_record();
                record.CLID= System.Guid.NewGuid().ToString().Replace("-", "");
                record.STATIONID = "";
                record.LSH = "";
                record.CLHP = VIN;
                record.VEHICLENAME = VEHICLENAME;
                record.MAXSPEED = "";
                record.JCSJ = DateTime.Now;
                record.LEFTTURNLEFT = leftturnleft;
                record.RIGHTTURNLEFT = rightturnleft;
                record.LEFTTURNRIGHT = leftturnright;
                record.RIGHTTURNRIGHT = rightturnright;
                record.XZ = vehiclemodel.LEFTTURNLEFT.ToString() + "-" + vehiclemodel.LEFTTURNLEFTWC + "|"
                    + vehiclemodel.RIGHTTURNLEFT.ToString() + "-" + vehiclemodel.RIGHTTURNLEFTWC + "|"
                    + vehiclemodel.LEFTTURNRIGHT.ToString() + "-" + vehiclemodel.LEFTTURNRIGHTWC + "|"
                    + vehiclemodel.RIGHTTURNRIGHT.ToString() + "-" + vehiclemodel.RIGHTTURNRIGHTWC;
                record.LEFTTURNLEFTPD = leftturnleftpd ? "合格" : "不合格";
                record.RIGHTTURNLEFTPD = rightturnleftpd ? "合格" : "不合格";
                record.LEFTTURNRIGHTPD = leftturnrightpd ? "合格" : "不合格";
                record.RIGHTTURNRIGHTPD = rightturnrightpd ? "合格" : "不合格";
                record.ZHPD=zhpd ? "合格" : "不合格";
                record.HASUPLOAD = (workMode == ENUM_WORK_MODE.WORK_MODE_NET) ? "Y" : "N";
                if (!SqlControl.insertRecord(record))
                {
                    Msg(labelOrder3, panelOrder3, "数据保存失败", 0);
                    Thread.Sleep(2000);
                }
                else if(workMode==ENUM_WORK_MODE.WORK_MODE_NET)
                {
                    if (!writeTestResult(record))
                    {
                        Msg(labelOrder3, panelOrder3, "写结果数据失败", 0);
                        Thread.Sleep(2000);
                    }
                }
                stablecount = 0;
                while(stablecount<20)//车离开光电累计1秒后，检测流程退出 
                {
                    if (!controller.isCarAlready) stablecount++;
                    Thread.Sleep(50);
                }
                controller.scanState = 0;
                CloseForm();
            }
            catch(Exception er)
            {
                logControl.saveLogInf("[exception:]" + er.Message);
                CloseForm();
            }
        }
        public delegate void closeFormDelegate();
        //public bool isFirstRow = true;
        /// <summary> 
        /// 追加显示文本 
        /// </summary> 
        /// <param name="color">文本颜色</param> 
        /// <param name="text">显示文本</param> 
        public void delegateCloseForm()
        {
            this.Close();
        }
        public void CloseForm()
        {
            closeFormDelegate la = new closeFormDelegate(delegateCloseForm);
            this.Invoke(la);
        }
        private void buttonNetOrLoacl_Click(object sender, EventArgs e)
        {
            if (controller.workMode == controller.ENUM_WORDMODE.WORKMODE_DEBUG)
            {
                controller.workMode = controller.ENUM_WORDMODE.WORKMODE_TEST;
            }
            else
            {
                controller.workMode = controller.ENUM_WORDMODE.WORKMODE_DEBUG;
            }
            labelWorkMode.Text = (controller.workMode == controller.ENUM_WORDMODE.WORKMODE_DEBUG ? "调校模式" : "检验模式");
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            string sendstring;
            if (controller.equip_status == false)
            {
                Msg(labelOrder3, panelOrder3, "设备未开启", 0);
                return;
            }
            else
            {
                if (!controller.getContiniousRealData(out sendstring))
                {
                    Msg(labelOrder3, panelOrder3, "通讯失败", 0);
                    return;
                }
                Thread.Sleep(1000);
                if (!controller.isLeftZero)
                {
                    Msg(labelOrder3, panelOrder3, "左转盘未摆正", 0);
                    return;
                }
                if (!controller.isRightZero)
                {
                    Msg(labelOrder3, panelOrder3, "右转盘未摆正", 0);
                    return;
                }
                buttonStart.Enabled = false;
                testThread = new Thread(test_Process);
                testThread.Start();
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (testThread != null && testThread.IsAlive)
                {
                    testThread.Abort();
                }
            }
            catch
            { }
            Msg(labelOrder3, panelOrder3, "转向角检测",0);
            buttonStart.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (testThread != null && testThread.IsAlive)
                {
                    testThread.Abort();
                }
            }
            catch
            { }
            controller.scanState = 0;
            CloseForm();
            //this.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            action actionform = new action();
            actionform.Show();
        }
    }
}
