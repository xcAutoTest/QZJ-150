using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace childTest
{
    public partial class settings : Form
    {
        
        public settings()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public bool isCOM(string input)
        {
            Regex regex = new Regex(@"^COM([1-9][0-9]*)$");
            if (!regex.IsMatch(input))
            {
                MessageBox.Show("输入非有效的COM口，正确输入如：COM10");
                return false;
            }
            return true;
        }
        public bool isCOMString(string input)
        {
            Regex regex = new Regex(@"^([1-9][0-9]*),[N,O,E],8,[1-2]$");
            if (!regex.IsMatch(input))
            {
                MessageBox.Show("输入非有效的串口配置字，正确输入如：38400,N,8,1");
                return false;
            }
            return true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            double tempd;
            if (!isCOM(comboBox1.Text))
            {
                //MessageBox.Show("串口号格式错误,应如:\r\nCOM1", "错误");
                return;
            }
            if (!isCOMString(comboBox2.Text))
            {
                //MessageBox.Show("串口配置字格式错误，应如:\r\n38400,N,8,1", "错误");
                return;
            }
            if (!double.TryParse(textBoxZEROXZ.Text, out tempd))
            {
                MessageBox.Show("回零限值格式不正确，应如:\r\n3.0", "错误");
                return;
            }
            controller.zero_xz = tempd;
            if (!double.TryParse(textBoxTurnTime.Text, out tempd))
            {
                MessageBox.Show("转盘转动时间格式不正确，应如:\r\n3.0", "错误");
                return;
            }
            controller.turn_time = tempd;
            if (!double.TryParse(textBoxStableTime.Text, out tempd))
            {
                MessageBox.Show("转盘稳定时间格式不正确，应如:\r\n3.0", "错误");
                return;
            }
            controller.stable_time = tempd;
            if (!double.TryParse(textBoxZeroTime.Text, out tempd))
            {
                MessageBox.Show("回零稳定时间格式不正确，应如:\r\n3.0", "错误");
                return;
            }
            controller.zero_time = tempd;
            string newcom = comboBox1.Text;
            string newcomstring = comboBox2.Text;
            if (radioButtonWorkMode_Test.Checked)
                controller.workMode = controller.ENUM_WORDMODE.WORKMODE_TEST;
            else
                controller.workMode = controller.ENUM_WORDMODE.WORKMODE_DEBUG;
            ini.INIIO.WritePrivateProfileString("QZJ", "COM", comboBox1.Text, @".\appConfig.ini");
            ini.INIIO.WritePrivateProfileString("QZJ", "COMSTRING", comboBox2.Text, @".\appConfig.ini");
            ini.INIIO.WritePrivateProfileString("QZJ", "WORKMODE", ((int)(controller.workMode)).ToString(), @".\appConfig.ini");
            ini.INIIO.WritePrivateProfileString("QZJ", "ZERO", textBoxZEROXZ.Text, @".\appConfig.ini");
            ini.INIIO.WritePrivateProfileString("QZJ", "TURN_TIME", textBoxTurnTime.Text, @".\appConfig.ini");
            ini.INIIO.WritePrivateProfileString("QZJ", "STABLE_TIME", textBoxStableTime.Text, @".\appConfig.ini");
            ini.INIIO.WritePrivateProfileString("QZJ", "ZERO_TIME", textBoxZeroTime.Text, @".\appConfig.ini");
            if (controller.equip_COM!=newcom||controller.equip_COMString!=newcomstring)
            {
                controller.equipStatus=controller.reInit_Equip();
            }
        }

        private void settings_Load(object sender, EventArgs e)
        {
            comboBox1.Text = controller.equip_COM;
            comboBox2.Text = controller.equip_COMString;
            textBoxZEROXZ.Text = controller.zero_xz.ToString();
            textBoxTurnTime.Text = controller.turn_time.ToString();
            textBoxStableTime.Text = controller.stable_time.ToString();
            textBoxZeroTime.Text = controller.zero_time.ToString();
            if (controller.workMode == controller.ENUM_WORDMODE.WORKMODE_TEST)
                radioButtonWorkMode_Test.Checked = true;
            else
                radioButtonWorkMode_Debug.Checked = true;
        }
    }
}
