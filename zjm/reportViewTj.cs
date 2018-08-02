using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace zjm
{
    public partial class reportViewTj : Form
    {
        public reportViewTj()
        {
            InitializeComponent();
        }

        private void reportViewTj_Load(object sender, EventArgs e)
        {

        }
        private void reportView_panelFormClosing(object sender, FormClosingEventArgs e)
        {
            reportViewer1.LocalReport.ReleaseSandboxAppDomain();
        }
        public void display_Asm(string stationid, DateTime starttime, DateTime endtime, string plate,  string result,string xh)
        {
            try
            {
                DataTable model = carDetect.vcmlogin.getAllCarDetected(stationid, starttime, endtime, result, plate,xh);
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("VIN", typeof(string)));
                dt.Columns.Add(new DataColumn("XH", typeof(string)));
                dt.Columns.Add(new DataColumn("LSH", typeof(string)));
                dt.Columns.Add(new DataColumn("JCSJ", typeof(string)));
                dt.Columns.Add(new DataColumn("LL", typeof(string)));
                dt.Columns.Add(new DataColumn("LR", typeof(string)));
                dt.Columns.Add(new DataColumn("RL", typeof(string)));
                dt.Columns.Add(new DataColumn("RR", typeof(string)));
                dt.Columns.Add(new DataColumn("RESULT", typeof(string)));
                DataRow dr = null;
                if (model != null)
                {
                    foreach (DataRow dR in model.Rows)
                    {
                        dr = dt.NewRow();
                        dr["LSH"] = dR["LSH"].ToString();
                        dr["VIN"] = dR["CLHP"].ToString();
                        dr["XH"] = dR["CLLX"].ToString();
                        //dr["最大设计速度"] = dR["MAXSPEED"].ToString();
                        dr["LL"] = dR["LEFTTURNLEFT"].ToString() + ((dR["LEFTTURNLEFTPD"].ToString() == "PASS") ? "√" : "×") + System.Environment.NewLine + "(" + dR["XZ"].ToString().Split('|')[0].Split('-')[0] + "±" + dR["XZ"].ToString().Split('|')[0].Split('-')[1] + ")";
                        dr["LR"] = dR["LEFTTURNRIGHT"].ToString() + ((dR["LEFTTURNRIGHTPD"].ToString() == "PASS") ? "√" : "×") + System.Environment.NewLine + "(" + dR["XZ"].ToString().Split('|')[1].Split('-')[0] + "±" + dR["XZ"].ToString().Split('|')[0].Split('-')[1] + ")";
                        dr["RL"] = dR["RIGHTTURNLEFT"].ToString() + ((dR["RIGHTTURNLEFTPD"].ToString() == "PASS") ? "√" : "×") + System.Environment.NewLine + "(" + dR["XZ"].ToString().Split('|')[2].Split('-')[0] + "±" + dR["XZ"].ToString().Split('|')[0].Split('-')[1] + ")";
                        dr["RR"] = dR["RIGHTTURNRIGHT"].ToString() + ((dR["RIGHTTURNRIGHTPD"].ToString() == "PASS") ? "√" : "×") + System.Environment.NewLine + "(" + dR["XZ"].ToString().Split('|')[3].Split('-')[0] + "±" + dR["XZ"].ToString().Split('|')[0].Split('-')[1] + ")";
                        //dr["左轮左转限值"] = dR["XZ"].ToString().Split('|')[0].Split('-')[0] + "±" + dR["XZ"].ToString().Split('|')[0].Split('-')[1];
                        //dr["左轮右转限值"] = dR["XZ"].ToString().Split('|')[1].Split('-')[0] + "±" + dR["XZ"].ToString().Split('|')[0].Split('-')[1];
                        //dr["右轮左转限值"] = dR["XZ"].ToString().Split('|')[2].Split('-')[0] + "±" + dR["XZ"].ToString().Split('|')[0].Split('-')[1];
                        //dr["右轮右转限值"] = dR["XZ"].ToString().Split('|')[3].Split('-')[0] + "±" + dR["XZ"].ToString().Split('|')[0].Split('-')[1];
                        dr["RESULT"] = (dR["ZHPD"].ToString() == "PASS") ? "通过" : "未通过";
                        dr["JCSJ"] = DateTime.Parse(dR["JCSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        dt.Rows.Add(dr);
                    }
                }
                reportViewer1.LocalReport.ReleaseSandboxAppDomain();
                reportViewer1.LocalReport.Dispose();
                reportViewer1.Visible = true;
                string lxname = "";
                string resultname = "";
                if (result == "0") resultname = "不限";
                else if (result == "-1") resultname = "不合格";
                else resultname = "合格";
                ReportParameter[] rptpara =
                {
                    new ReportParameter("starttime",starttime.ToString("yyyy-MM-dd")),
                    new ReportParameter("endtime", endtime.ToString("yyyy-MM-dd")),
                    new ReportParameter("result", resultname),
                    new ReportParameter("datetime",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                    new ReportParameter("vin",plate)
                };
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.SetParameters(rptpara);
                reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", dt));
            }
            catch
            {
                throw;
            }
            reportViewer1.RefreshReport();
        }
        public void display_Asm(string stationid, string lx, string plate, string result,string xh)
        {
            try
            {
                DataTable model = carDetect.vcmlogin.getAllCarDetected(stationid, lx, result, plate,xh);
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("VIN", typeof(string)));
                dt.Columns.Add(new DataColumn("XH", typeof(string)));
                dt.Columns.Add(new DataColumn("LSH", typeof(string)));
                dt.Columns.Add(new DataColumn("JCSJ", typeof(string)));
                dt.Columns.Add(new DataColumn("LL", typeof(string)));
                dt.Columns.Add(new DataColumn("LR", typeof(string)));
                dt.Columns.Add(new DataColumn("RL", typeof(string)));
                dt.Columns.Add(new DataColumn("RR", typeof(string)));
                dt.Columns.Add(new DataColumn("RESULT", typeof(string)));
                DataRow dr = null;
                if (model != null)
                {
                    foreach (DataRow dR in model.Rows)
                    {
                        dr = dt.NewRow();
                        dr["LSH"] = dR["LSH"].ToString();
                        dr["VIN"] = dR["CLHP"].ToString();
                        dr["XH"] = dR["CLLX"].ToString();
                        //dr["最大设计速度"] = dR["MAXSPEED"].ToString();
                        dr["LL"] = dR["LEFTTURNLEFT"].ToString() + ((dR["LEFTTURNLEFTPD"].ToString() == "PASS") ? "√" : "×") + System.Environment.NewLine+"(" + dR["XZ"].ToString().Split('|')[0].Split('-')[0] + "±" + dR["XZ"].ToString().Split('|')[0].Split('-')[1] + ")";
                        dr["LR"] = dR["LEFTTURNRIGHT"].ToString() + ((dR["LEFTTURNRIGHTPD"].ToString() == "PASS") ? "√" : "×") +System.Environment.NewLine+ "(" + dR["XZ"].ToString().Split('|')[1].Split('-')[0] + "±" + dR["XZ"].ToString().Split('|')[0].Split('-')[1] + ")";
                        dr["RL"] = dR["RIGHTTURNLEFT"].ToString() + ((dR["RIGHTTURNLEFTPD"].ToString() == "PASS") ? "√" : "×") + System.Environment.NewLine + "(" + dR["XZ"].ToString().Split('|')[2].Split('-')[0] + "±" + dR["XZ"].ToString().Split('|')[0].Split('-')[1] + ")";
                        dr["RR"] = dR["RIGHTTURNRIGHT"].ToString() + ((dR["RIGHTTURNRIGHTPD"].ToString() == "PASS") ? "√" : "×") + System.Environment.NewLine + "(" + dR["XZ"].ToString().Split('|')[3].Split('-')[0] + "±" + dR["XZ"].ToString().Split('|')[0].Split('-')[1] + ")";
                        //dr["左轮左转限值"] = dR["XZ"].ToString().Split('|')[0].Split('-')[0] + "±" + dR["XZ"].ToString().Split('|')[0].Split('-')[1];
                        //dr["左轮右转限值"] = dR["XZ"].ToString().Split('|')[1].Split('-')[0] + "±" + dR["XZ"].ToString().Split('|')[0].Split('-')[1];
                        //dr["右轮左转限值"] = dR["XZ"].ToString().Split('|')[2].Split('-')[0] + "±" + dR["XZ"].ToString().Split('|')[0].Split('-')[1];
                        //dr["右轮右转限值"] = dR["XZ"].ToString().Split('|')[3].Split('-')[0] + "±" + dR["XZ"].ToString().Split('|')[0].Split('-')[1];
                        dr["RESULT"] = (dR["ZHPD"].ToString() == "PASS") ? "通过" : "未通过";
                        dr["JCSJ"] = DateTime.Parse(dR["JCSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        dt.Rows.Add(dr);
                    }
                }
                reportViewer1.LocalReport.ReleaseSandboxAppDomain();
                reportViewer1.LocalReport.Dispose();
                reportViewer1.Visible = true;
                string lxname = "";
                string resultname = "";
                if (result == "0") resultname = "不限";
                else if (result == "-1") resultname = "不合格";
                else resultname = "合格";
                if (lx == "0") lxname = "不限";
                else if (lx == "1") lxname = "本年";
                else if (lx == "2") lxname = "本月";
                else if(lx=="3")lxname = "当日";
                ReportParameter[] rptpara =
                {
                    new ReportParameter("starttime",lxname),
                    new ReportParameter("endtime", lxname),
                    new ReportParameter("result", resultname),
                    new ReportParameter("datetime",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                    new ReportParameter("vin",plate)
                };
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.SetParameters(rptpara);
                reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", dt));
            }
            catch
            {
                throw;
            }
            reportViewer1.RefreshReport();
        }
    }
}
