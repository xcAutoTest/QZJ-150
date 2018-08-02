using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace childTest
{
    public partial class calibration : Form
    {
        public calibration()
        {
            InitializeComponent();
        }
        public delegate void wtlsb(Label Msgowner, string Msgstr);                   //委托
        public delegate void wtlp(Label Msgowner, Panel Msgfather, int position);                                 //委托

        /// <summary>
        /// 信息显示
        /// </summary>
        /// <param name="Msgowner">信息显示的Label控件</param>
        /// <param name="Msgfather">Label控件的父级Panel</param>
        /// <param name="Msgstr">要显示的信息</param>
        /// <param name="position">0-居中 1-靠左 2-靠右</param>

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
        private void buttonStart_Click(object sender, EventArgs e)
        {
            string sendstring = "";
            if (controller.setMemoryPara(out sendstring))
            {
                textBoxSENDSTATUS.Text = "发送成功";
            }
            else
            {
                textBoxSENDSTATUS.Text = "发送失败";
            }
            textBoxSend.Text = sendstring;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sendstring = "";
            if (controller.setReadPara(out sendstring))
            {
                textBoxSENDSTATUS.Text = "发送成功";
                textBoxkl.Text = controller.kl.ToString();
                textBoxkr.Text = controller.kr.ToString();
            }
            else
            {
                textBoxSENDSTATUS.Text = "发送失败";
            }
            textBoxSend.Text = sendstring;
        }

        private void calibration_Load(object sender, EventArgs e)
        {
            comboBoxGd.Text = controller.gd_chanel_car.ToString();
            comboBoxLeftZero.Text = controller.gd_chanel_leftzero.ToString();
            comboBoxRightZero.Text = controller.gd_chanel_rightzero.ToString();
            comboBoxBzqch_straight.Text = controller.bzq_chanel_straight.ToString();
            comboBoxBzqch_back.Text = controller.bzq_chanel_back.ToString();
            comboBoxZp_lock.Text = controller.zp_chanel_lock.ToString();
            comboBoxZp_unlock.Text = controller.zp_chanel_unlock.ToString();
            textBoxkl.Text = controller.left_pulse.ToString("0");
            textBoxkr.Text = controller.right_pulse.ToString("0");
            timer1.Start();
        }

        private void buttonRdSingle_Click(object sender, EventArgs e)
        {
            string sendstring = "";
            if (controller.getSingleRealData(out sendstring))
            {
                textBoxSENDSTATUS.Text = "发送成功";
            }
            else
            {
                textBoxSENDSTATUS.Text = "发送失败";
            }
            textBoxSend.Text = sendstring;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Msg(labelldata, panelldata, controller.angleLeft.ToString("0.0"), 0);
            Msg(labelrdata, panelrdata, controller.angleRight.ToString("0.0"), 0);
            panelGD_LED.BackColor = (controller.isCarAlready ? Color.Red : Color.Gray);
            panelGD_LEFTZERO.BackColor = (controller.isLeftZero ? Color.Red : Color.Gray);
            panelGD_RIGHTZERO.BackColor = (controller.isRightZero ? Color.Red : Color.Gray);
            textBoxRecv.Text = controller.recbufstring;
        }

        private void buttonSaveRelayChanel_Click(object sender, EventArgs e)
        {
            controller.bzq_chanel_straight = byte.Parse(comboBoxBzqch_straight.Text);
            controller.bzq_chanel_back = byte.Parse(comboBoxBzqch_back.Text);
            controller.zp_chanel_lock = byte.Parse(comboBoxZp_lock.Text);
            controller.zp_chanel_unlock = byte.Parse(comboBoxZp_unlock.Text);
            //controller.gd_chanel = byte.Parse(comboBoxGd.Text);
            ini.INIIO.WritePrivateProfileString("QZJ", "BZQ_STRAIGHT", controller.bzq_chanel_straight.ToString("0"), @".\appConfig.ini");
            ini.INIIO.WritePrivateProfileString("QZJ", "BZQ_BACK", controller.bzq_chanel_back.ToString("0"), @".\appConfig.ini");
            ini.INIIO.WritePrivateProfileString("QZJ", "ZP_LOCK", controller.zp_chanel_lock.ToString("0"), @".\appConfig.ini");
            ini.INIIO.WritePrivateProfileString("QZJ", "ZP_UNLOCK", controller.zp_chanel_unlock.ToString("0"), @".\appConfig.ini");
            //ini.INIIO.WritePrivateProfileString("QZJ", "GD", controller.gd_chanel.ToString("0"), @".\appConfig.ini");
            MessageBox.Show("保存成功");
        }

        private void buttonBzqOn_Click(object sender, EventArgs e)
        {
            string sendstring = "";
            if (controller.straightCar(out sendstring))
            {
                textBoxSENDSTATUS.Text = "发送成功";
            }
            else
            {
                textBoxSENDSTATUS.Text = "发送失败";
            }
            textBoxSend.Text = sendstring;
        }

        private void buttonBzqOff_Click(object sender, EventArgs e)
        {
            string sendstring = "";
            if (controller.backStraight( out sendstring))
            {
                textBoxSENDSTATUS.Text = "发送成功";
            }
            else
            {
                textBoxSENDSTATUS.Text = "发送失败";
            }
            textBoxSend.Text = sendstring;
        }

        private void buttonZpOn_Click(object sender, EventArgs e)
        {

            string sendstring = "";
            if (controller.lockTable(out sendstring))
            {
                textBoxSENDSTATUS.Text = "发送成功";
            }
            else
            {
                textBoxSENDSTATUS.Text = "发送失败";
            }
            textBoxSend.Text = sendstring;
        }

        private void buttonZpOff_Click(object sender, EventArgs e)
        {

            string sendstring = "";
            if (controller.unLockTable( out sendstring))
            {
                textBoxSENDSTATUS.Text = "发送成功";
            }
            else
            {
                textBoxSENDSTATUS.Text = "发送失败";
            }
            textBoxSend.Text = sendstring;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            this.Close();
        }

        private void buttonRdConti_Click(object sender, EventArgs e)
        {
            string sendstring = "";
            if (controller.getContiniousRealData(out sendstring))
            {
                textBoxSENDSTATUS.Text = "发送成功";
            }
            else
            {
                textBoxSENDSTATUS.Text = "发送失败";
            }
            textBoxSend.Text = sendstring;

        }

        private void buttonClrL_Click(object sender, EventArgs e)
        {
            string sendstring = "";
            if (controller.setClearLeft(out sendstring))
            {
                textBoxSENDSTATUS.Text = "发送成功";
            }
            else
            {
                textBoxSENDSTATUS.Text = "发送失败";
            }
            textBoxSend.Text = sendstring;
        }

        private void buttonClrR_Click(object sender, EventArgs e)
        {
            string sendstring = "";
            if (controller.setClearRight(out sendstring))
            {
                textBoxSENDSTATUS.Text = "发送成功";
            }
            else
            {
                textBoxSENDSTATUS.Text = "发送失败";
            }
            textBoxSend.Text = sendstring;
        }

        private void buttonSetLeft_Click(object sender, EventArgs e)
        {
            try
            {
                ushort value = ushort.Parse(textBoxkl.Text);
                controller.left_pulse = value;
                ini.INIIO.WritePrivateProfileString("QZJ", "LEFT_PULSE", controller.left_pulse.ToString("0"), @".\appConfig.ini");
                MessageBox.Show("保存成功");
            }
            catch
            {
                MessageBox.Show("输入格式错误，必须为整数");
            }
        }

        private void buttonSetRight_Click(object sender, EventArgs e)
        {
            try
            {
                ushort value = ushort.Parse(textBoxkr.Text);
                controller.right_pulse = value;
                ini.INIIO.WritePrivateProfileString("QZJ", "RIGHT_PULSE", controller.left_pulse.ToString("0"), @".\appConfig.ini");
                MessageBox.Show("保存成功");
            }
            catch
            {
                MessageBox.Show("输入格式错误，必须为整数");
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //controller.bzq_chanel = byte.Parse(comboBoxBzqch.Text);
            //controller.zp_chanel = byte.Parse(comboBoxZpch.Text);
            controller.gd_chanel_car = byte.Parse(comboBoxGd.Text);
            controller.gd_chanel_leftzero = byte.Parse(comboBoxLeftZero.Text);
            controller.gd_chanel_rightzero = byte.Parse(comboBoxRightZero.Text);
            //ini.INIIO.WritePrivateProfileString("QZJ", "BZQ", controller.bzq_chanel.ToString("0"), @".\appConfig.ini");
            //ini.INIIO.WritePrivateProfileString("QZJ", "ZP", controller.zp_chanel.ToString("0"), @".\appConfig.ini");
            ini.INIIO.WritePrivateProfileString("QZJ", "GD_CAR", controller.gd_chanel_car.ToString("0"), @".\appConfig.ini");
            ini.INIIO.WritePrivateProfileString("QZJ", "GD_LEFTZERO", controller.gd_chanel_leftzero.ToString("0"), @".\appConfig.ini");
            ini.INIIO.WritePrivateProfileString("QZJ", "GD_RIGHTZERO", controller.gd_chanel_rightzero.ToString("0"), @".\appConfig.ini");
            MessageBox.Show("保存成功");
        }
    }
}
