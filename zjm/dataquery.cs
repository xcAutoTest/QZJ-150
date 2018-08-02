using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using System.IO;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using System.Threading;
using System.Diagnostics;
using VCM100login;
using SYS_MODEL;

namespace zjm
{
    public partial class dataquery : Form
    {
        public int Carwait_Scroll = 0;                                                  //待检车滚动条位置
        DataTable dt_wait = null;                                                         //等待车辆列表
        public string[] selectID = new string[1024];                                    //当前等待车辆选中的列表
        public bool ref_zt = true;
        private List<Stream> m_streams;
        //用来记录当前打印到第几页了 
        private int m_currentPageIndex;
        

        public dataquery()
        {
            InitializeComponent();
        }

        private void dataquery_Load(object sender, EventArgs e)
        {
            init_datagrid();
            ref_WaitCar();
            init_xh();
            comboBoxCLLX.Text = "所有型号";
            textBoxPlateAtWait.Text = "";
        }
        private void init_xh()
        {

            comboBoxCLLX.Items.Add("所有型号");
            for (int i = 0; i < carDetect.vehicleXhName.Count; i++)
            {
                comboBoxCLLX.Items.Add(carDetect.vehicleXhName[i].ToString());
            }
        }
        private void init_datagrid()
        {
            dt_wait = new DataTable();
            dt_wait.Columns.Add("检测编号");
            dt_wait.Columns.Add("VIN");
            dt_wait.Columns.Add("型号");
            dt_wait.Columns.Add("左轮左转");
            dt_wait.Columns.Add("左轮左转限值");
            dt_wait.Columns.Add("左轮右转");
            dt_wait.Columns.Add("左轮右转限值");
            dt_wait.Columns.Add("右轮左转");
            dt_wait.Columns.Add("右轮左转限值");
            dt_wait.Columns.Add("右轮右转");               
            dt_wait.Columns.Add("右轮右转限值");
            dt_wait.Columns.Add("判定结果");
            dt_wait.Columns.Add("检测时间");
            dataGrid_waitcar.DataSource = dt_wait;
        }

        public void ref_WaitCar()
        {
            try
            {
                dt_wait.Rows.Clear();
                if (true)
                {
                    string lx = "";
                    string result = "";
                    string xh = "";
                    if (radioButtonAllTime.Checked)
                        lx = "0";
                    else if (radioButtonThisYear.Checked)
                        lx = "1";
                    else if (radioButtonThisMonth.Checked)
                        lx = "2";
                    else if (radioButtonToday.Checked)
                        lx = "3";
                    else if (radioButtonAnyTime.Checked)
                        lx = "4";
                    else
                        lx = "0";
                    if (radioButtonPass.Checked)
                        result = "1";
                    else if (radioButtonFail.Checked)
                        result = "-1";
                    else
                        result = "0";
                    if (comboBoxCLLX.Text == "所有型号")
                        xh = "";
                    else
                        xh = comboBoxCLLX.Text;
                    DataTable dt = null;
                    if (lx == "4")
                    {
                        DateTime starttime=dateTimeStartTime.Value;
                        DateTime endtime=dateTimeEndtTime.Value;
                        dt = carDetect.vcmlogin.getAllCarDetected(carDetect.stationid, starttime, endtime, result, textBoxPlateAtWait.Text,xh);
                    }
                    else
                    {
                        dt = carDetect.vcmlogin.getAllCarDetected(carDetect.stationid, lx,result,textBoxPlateAtWait.Text,xh);
                    }
                    DataRow dr = null;
                    if (dt != null)
                    {
                        foreach (DataRow dR in dt.Rows)
                        {
                            dr = dt_wait.NewRow();
                            dr["检测编号"] = dR["LSH"].ToString();
                            dr["VIN"] = dR["CLHP"].ToString();
                            dr["型号"] = dR["CLLX"].ToString();
                            //dr["最大设计速度"] = dR["MAXSPEED"].ToString();
                            dr["左轮左转"] = dR["LEFTTURNLEFT"].ToString() + ((dR["LEFTTURNLEFTPD"].ToString() == "PASS") ? "√" : "×");
                            dr["左轮右转"] = dR["LEFTTURNRIGHT"].ToString() + ((dR["LEFTTURNRIGHTPD"].ToString() == "PASS") ? "√" : "×");
                            dr["右轮左转"] = dR["RIGHTTURNLEFT"].ToString() + ((dR["RIGHTTURNLEFTPD"].ToString() == "PASS") ? "√" : "×");
                            dr["右轮右转"] = dR["RIGHTTURNRIGHT"].ToString() + ((dR["RIGHTTURNRIGHTPD"].ToString() == "PASS") ? "√" : "×");
                            dr["左轮左转限值"] = dR["XZ"].ToString().Split('|')[0].Split('-')[0] + "±" + dR["XZ"].ToString().Split('|')[0].Split('-')[1];
                            dr["左轮右转限值"] = dR["XZ"].ToString().Split('|')[1].Split('-')[0] + "±" + dR["XZ"].ToString().Split('|')[0].Split('-')[1];
                            dr["右轮左转限值"] = dR["XZ"].ToString().Split('|')[2].Split('-')[0] + "±" + dR["XZ"].ToString().Split('|')[0].Split('-')[1];
                            dr["右轮右转限值"] = dR["XZ"].ToString().Split('|')[3].Split('-')[0] + "±" + dR["XZ"].ToString().Split('|')[0].Split('-')[1]; 
                            dr["判定结果"] = (dR["ZHPD"].ToString() == "PASS") ? "通过" : "未通过";
                            dr["检测时间"] =DateTime.Parse( dR["JCSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                            dt_wait.Rows.Add(dr);
                        }
                    }
                }
                
                ref_zt = false;
                dataGrid_waitcar.DataSource = dt_wait;
                dataGrid_waitcar.FirstDisplayedScrollingRowIndex = Carwait_Scroll;
                dataGrid_waitcar.Sort(dataGrid_waitcar.Columns["检测时间"], ListSortDirection.Descending);
                ref_zt = true;
            }
            catch (Exception)
            {

            }
        }
        public void ref_WaitCar(string lx, string plate)
        {
            try
            {
                dt_wait.Rows.Clear();
                DataTable dt = carDetect.vcmlogin.getCarDetectedByPlate(carDetect.stationid, lx, plate);
                DataRow dr = null;
                if (dt != null)
                {
                    foreach (DataRow dR in dt.Rows)
                    {
                        dr = dt_wait.NewRow();
                        dr["检测编号"] = dR["LSH"].ToString();
                        dr["VIN"] = dR["CLHP"].ToString();
                        dr["型号"] = dR["CLLX"].ToString();
                        //dr["最大设计速度"] = dR["MAXSPEED"].ToString();
                        dr["左轮左转"] = dR["LEFTTURNLEFT"].ToString() + ((dR["LEFTTURNLEFTPD"].ToString() == "PASS") ? "√" : "×");
                        dr["左轮右转"] = dR["LEFTTURNRIGHT"].ToString() + ((dR["LEFTTURNRIGHTPD"].ToString() == "PASS") ? "√" : "×");
                        dr["右轮左转"] = dR["RIGHTTURNLEFT"].ToString() + ((dR["RIGHTTURNLEFTPD"].ToString() == "PASS") ? "√" : "×");
                        dr["右轮右转"] = dR["RIGHTTURNRIGHT"].ToString() + ((dR["RIGHTTURNRIGHTPD"].ToString() == "PASS") ? "√" : "×");
                        dr["左轮左转限值"] = dR["XZ"].ToString().Split('|')[0].Split('-')[0] + "±" + dR["XZ"].ToString().Split('|')[0].Split('-')[1];
                        dr["左轮右转限值"] = dR["XZ"].ToString().Split('|')[1].Split('-')[0] + "±" + dR["XZ"].ToString().Split('|')[0].Split('-')[1];
                        dr["右轮左转限值"] = dR["XZ"].ToString().Split('|')[2].Split('-')[0] + "±" + dR["XZ"].ToString().Split('|')[0].Split('-')[1];
                        dr["右轮右转限值"] = dR["XZ"].ToString().Split('|')[3].Split('-')[0] + "±" + dR["XZ"].ToString().Split('|')[0].Split('-')[1];
                        dr["判定结果"] = (dR["ZHPD"].ToString() == "PASS") ? "通过" : "未通过";
                        dr["检测时间"] = DateTime.Parse(dR["JCSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        dt_wait.Rows.Add(dr);
                    }
                }
                ref_zt = false;
                dataGrid_waitcar.DataSource = dt_wait;
                dataGrid_waitcar.FirstDisplayedScrollingRowIndex = Carwait_Scroll;
                dataGrid_waitcar.Sort(dataGrid_waitcar.Columns["检测时间"], ListSortDirection.Descending);
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
                if (dataGrid_waitcar.SelectedRows.Count > 0)
                {
                    if (dataGrid_waitcar.SelectedRows.Count == 1)
                    {
                        selectID = new string[1024];
                        for (int i = 0; i < dataGrid_waitcar.SelectedRows.Count; i++)
                        {
                            selectID[i] = dataGrid_waitcar.SelectedRows[i].Cells["检测编号"].Value.ToString();
                        }
                    }
                    else if (dataGrid_waitcar.SelectedRows.Count > 1)
                    {
                        selectID = new string[1024];
                        for (int i = 0; i < dataGrid_waitcar.SelectedRows.Count; i++)
                        {
                            selectID[i] = dataGrid_waitcar.SelectedRows[i].Cells["检测编号"].Value.ToString();
                        }
                    }
                    else
                    {
                        selectID = new string[1024];
                        for (int i = 0; i < dataGrid_waitcar.SelectedRows.Count; i++)
                        {
                            selectID[i] = dataGrid_waitcar.SelectedRows[i].Cells["检测编号"].Value.ToString();
                        }
                    }
                    
                }
                else
                {
                    selectID = new string[1024];
                    for (int i = 0; i < dataGrid_waitcar.SelectedRows.Count; i++)
                    {
                        selectID[i] = dataGrid_waitcar.SelectedRows[i].Cells["检测编号"].Value.ToString();
                    }
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string lx = "";
            if (radioButtonAllTime.Checked)
                lx = "0";
            else if (radioButtonThisYear.Checked)
                lx = "1";
            else if (radioButtonThisMonth.Checked)
                lx = "2";
            else if (radioButtonToday.Checked)
                lx = "3";
            else if (radioButtonAnyTime.Checked)
                lx = "4";
            else
                lx = "0";
            ref_WaitCar();
        }
        private void buttonCheckToday_Click(object sender, EventArgs e)
        {
            ref_WaitCar("当日", textBoxPlateAtWait.Text);
        }

        private void buttonCheckInHistory_Click(object sender, EventArgs e)
        {
            ref_WaitCar("所有", textBoxPlateAtWait.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string plate = "";
            string stationid = carDetect.stationid;
            string jcsj = "0";
            string xh = "";
            DateTime starttime = dateTimeStartTime.Value;
            DateTime endtime = dateTimeEndtTime.Value;
            string result="";
            if (radioButtonAllTime.Checked)
                jcsj = "0";
            else if (radioButtonThisYear.Checked)
                jcsj = "1";
            else if (radioButtonThisMonth.Checked)
                jcsj = "2";
            else if (radioButtonToday.Checked)
                jcsj = "3";
            else if (radioButtonAnyTime.Checked)
                jcsj = "4";
            else
                jcsj = "0";
            if (radioButtonPass.Checked)
                result = "1";
            else if (radioButtonFail.Checked)
                result = "-1";
            else
                result = "0";
            if (comboBoxCLLX.Text == "所有型号")
                xh = "";
            else
                xh = comboBoxCLLX.Text;
            reportViewTj childreportStatistics = new reportViewTj();
            childreportStatistics.TopLevel = false;
            ((Panel)this.Parent).Controls.Add(childreportStatistics);
            childreportStatistics.Dock = DockStyle.Fill;
            if (jcsj == "4")
            {
                childreportStatistics.display_Asm(stationid, starttime, endtime, plate, result,xh);
            }
            else
            {
                childreportStatistics.display_Asm(stationid, jcsj, plate, result,xh);
            }
            childreportStatistics.BringToFront();
            childreportStatistics.Show();
        }

   

    
    }
}
