using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace childTest
{
    public partial class recordForm : Form
    {
        public recordForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void recordForm_Load(object sender, EventArgs e)
        {
            init_datagrid();
        }
        private void init_datagrid()
        {
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
            columnHeaderStyle.BackColor = Color.Beige;
            columnHeaderStyle.Font = new Font("Verdana", 10, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle = columnHeaderStyle;
            DataGridViewButtonColumn btnsave = new DataGridViewButtonColumn();
            btnsave.Name = "btnsave";
            btnsave.DefaultCellStyle.NullValue = "上传";
            btnsave.HeaderText = "上传";
            btnsave.Width = 80;
            this.dataGridView1.Columns.Add("1", "ID");
            this.dataGridView1.Columns.Add("2", "VIN");
            this.dataGridView1.Columns.Add("3", "车型");
            this.dataGridView1.Columns.Add("4", "检测时间");
            this.dataGridView1.Columns.Add("5", "左轮左转");
            this.dataGridView1.Columns.Add("6", "判定");
            this.dataGridView1.Columns.Add("7", "左轮右转");
            this.dataGridView1.Columns.Add("8", "判定");
            this.dataGridView1.Columns.Add("9", "右轮左转");
            this.dataGridView1.Columns.Add("10", "判定");
            this.dataGridView1.Columns.Add("11", "右轮右转");
            this.dataGridView1.Columns.Add("12", "判定");
            this.dataGridView1.Columns.Add("13", "总判定");
            this.dataGridView1.Columns.Add("14", "状态");
            this.dataGridView1.Columns.Add(btnsave);
            dataGridView1.Columns["1"].Width = 150;
            dataGridView1.Columns["2"].Width = 100;
            dataGridView1.Columns["3"].Width = 80;
            dataGridView1.Columns["4"].Width = 120;
            dataGridView1.Columns["5"].Width = 100;
            dataGridView1.Columns["6"].Width = 80;
            dataGridView1.Columns["7"].Width = 100;
            dataGridView1.Columns["8"].Width = 80;
            dataGridView1.Columns["9"].Width = 100;
            dataGridView1.Columns["10"].Width = 80;
            dataGridView1.Columns["11"].Width = 100;
            dataGridView1.Columns["12"].Width = 80;
            dataGridView1.Columns["13"].Width = 80;
            dataGridView1.Columns["14"].Width = 80;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            DateTime starttime = dateTimePicker1.Value;
            DateTime endtime = dateTimePicker2.Value;
            DataTable dt = childTest.SqlControl.getRecordList(starttime, endtime,checkBox1.Checked);
            if(dt!=null)
            {
                foreach(DataRow dR in dt.Rows)
                {
                    DataGridViewRow dr1 = new DataGridViewRow();
                    foreach (DataGridViewColumn c in this.dataGridView1.Columns)
                    {
                        dr1.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);
                    }
                    dr1.Cells[0].Value = dR["CLID"].ToString();
                    dr1.Cells[1].Value = dR["CLHP"].ToString();
                    dr1.Cells[2].Value = dR["VEHICLENAME"].ToString();
                    dr1.Cells[3].Value = dR["JCSJ"].ToString();
                    dr1.Cells[4].Value = dR["LEFTTURNLEFT"].ToString();
                    dr1.Cells[5].Value = dR["LEFTTURNLEFTPD"].ToString();
                    dr1.Cells[6].Value = dR["LEFTTURNRIGHT"].ToString();
                    dr1.Cells[7].Value = dR["LEFTTURNRIGHTPD"].ToString();
                    dr1.Cells[8].Value = dR["RIGHTTURNLEFT"].ToString();
                    dr1.Cells[9].Value = dR["RIGHTTURNLEFTPD"].ToString();
                    dr1.Cells[10].Value = dR["RIGHTTURNRIGHT"].ToString();
                    dr1.Cells[11].Value = dR["RIGHTTURNRIGHTPD"].ToString();
                    dr1.Cells[12].Value = dR["ZHPD"].ToString();
                    dr1.Cells[13].Value = dR["HASUPLOAD"].ToString();
                    this.dataGridView1.Rows.Add(dr1);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["btnsave"].Index)
            {
                int index = e.RowIndex;
                string clid = dataGridView1.Rows[index].Cells[0].Value.ToString();
                if(childTest.SqlControl.updateUpload(true,clid))
                {

                }
            }
        }
    }
}
