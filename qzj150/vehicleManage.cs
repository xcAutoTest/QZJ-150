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
    public partial class vehicleManage : Form
    {
        public vehicleManage()
        {
            InitializeComponent();
        }
        private void init_datagrid()
        {
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
            columnHeaderStyle.BackColor = Color.Beige;
            columnHeaderStyle.Font = new Font("Verdana", 10, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle = columnHeaderStyle;
            DataGridViewButtonColumn btnsave = new DataGridViewButtonColumn();
            btnsave.Name = "btnsave";
            btnsave.DefaultCellStyle.NullValue = "保存";
            btnsave.HeaderText = "保存";
            btnsave.Width = 50;           
            DataGridViewButtonColumn btndelete = new DataGridViewButtonColumn();
            btndelete.Name = "btndelete";
            btndelete.DefaultCellStyle.NullValue = "删除";
            btndelete.HeaderText = "删除";
            btndelete.Width = 50;
            this.dataGridView1.Columns.Add("1", "车型");
            this.dataGridView1.Columns.Add("2", "左轮左转标准值");
            this.dataGridView1.Columns.Add("3", "左轮右转标准值");
            this.dataGridView1.Columns.Add("4", "右轮左转标准值");
            this.dataGridView1.Columns.Add("5", "右轮右转标准值");
            this.dataGridView1.Columns.Add("6", "左轮左转误差");
            this.dataGridView1.Columns.Add("7", "左轮右转误差");
            this.dataGridView1.Columns.Add("8", "右轮左转误差");
            this.dataGridView1.Columns.Add("9", "右轮右转误差");
            this.dataGridView1.Columns.Add(btnsave);
            this.dataGridView1.Columns.Add(btndelete);
            dataGridView1.Columns["1"].ReadOnly = true;
            dataGridView1.Columns["2"].Width = 180;
            dataGridView1.Columns["3"].Width = 180;
            dataGridView1.Columns["4"].Width = 180;
            dataGridView1.Columns["5"].Width = 180;
            dataGridView1.Columns["6"].Width = 150;
            dataGridView1.Columns["7"].Width = 150;
            dataGridView1.Columns["8"].Width = 150;
            dataGridView1.Columns["9"].Width = 150;
        }
        public void ref_Staff()
        {
            try
            {
                DataTable dt = SqlControl.getVehicleList();
                DataRow dr = null;
                if (dt != null)
                {
                    this.dataGridView1.Rows.Clear();
                    foreach (DataRow dR in dt.Rows)
                    {
                        DataGridViewRow dr1 = new DataGridViewRow();
                        foreach (DataGridViewColumn c in this.dataGridView1.Columns)
                        {
                            dr1.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);
                        }
                        dr1.Cells[0].Value = dR["VEHICLENAME"].ToString();
                        dr1.Cells[1].Value = dR["LEFTTURNLEFT"].ToString();
                        dr1.Cells[2].Value = dR["LEFTTURNRIGHT"].ToString();
                        dr1.Cells[3].Value = dR["RIGHTTURNLEFT"].ToString();
                        dr1.Cells[4].Value = dR["RIGHTTURNRIGHT"].ToString();
                        dr1.Cells[5].Value = dR["LEFTTURNLEFTWC"].ToString();
                        dr1.Cells[6].Value = dR["LEFTTURNRIGHTWC"].ToString();
                        dr1.Cells[7].Value = dR["RIGHTTURNLEFTWC"].ToString();
                        dr1.Cells[8].Value = dR["RIGHTTURNRIGHTWC"].ToString();
                        this.dataGridView1.Rows.Add(dr1);
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        private void vehicleManage_Load(object sender, EventArgs e)
        {
            init_datagrid();
            ref_Staff();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == dataGridView1.Columns["btnsave"].Index)
                {
                    int index = e.RowIndex;
                    if (dataGridView1.Rows[index].Cells[1].Value.ToString() == "admin")
                    {
                        MessageBox.Show("禁止修改管理员用户", "系统提示");
                        return;
                    }
                    qzj_vehicle model = new qzj_vehicle();
                    model.VEHICLENAME = dataGridView1.Rows[index].Cells[0].Value.ToString();
                    model.LEFTTURNLEFT = double.Parse(dataGridView1.Rows[index].Cells[1].Value.ToString());
                    model.LEFTTURNRIGHT = double.Parse(dataGridView1.Rows[index].Cells[2].Value.ToString());
                    model.RIGHTTURNLEFT = double.Parse(dataGridView1.Rows[index].Cells[3].Value.ToString());
                    model.RIGHTTURNRIGHT = double.Parse(dataGridView1.Rows[index].Cells[4].Value.ToString());
                    model.LEFTTURNLEFTWC = double.Parse(dataGridView1.Rows[index].Cells[5].Value.ToString());
                    model.LEFTTURNRIGHTWC = double.Parse(dataGridView1.Rows[index].Cells[6].Value.ToString());
                    model.RIGHTTURNLEFTWC = double.Parse(dataGridView1.Rows[index].Cells[7].Value.ToString());
                    model.RIGHTTURNRIGHTWC = double.Parse(dataGridView1.Rows[index].Cells[8].Value.ToString());
                    if (SqlControl.updateVehicle(model))
                    {                        
                        MessageBox.Show("更改成功", "系统提示");
                    }
                    else
                        MessageBox.Show("未知原因导致信息更改失败", "系统提示");
                }
                else if (e.ColumnIndex == dataGridView1.Columns["btndelete"].Index)
                {
                    int index = e.RowIndex;
                    if (dataGridView1.Rows[index].Cells[0].Value.ToString() == "DEFAULT")
                    {
                        MessageBox.Show("禁止删除DEFAULT车型信息", "系统提示");
                    }
                    else
                    {
                        SqlControl.deleteVehicle(dataGridView1.Rows[index].Cells[0].Value.ToString());
                        MessageBox.Show("删除成功", "系统提示");
                        ref_Staff();
                    }
                }
            }
            catch(Exception er)
            {
                MessageBox.Show("操作失败:"+er.Message, "系统提示");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                qzj_vehicle model = new qzj_vehicle();
                model.VEHICLENAME = textBoxVehicleName.Text;
                model.LEFTTURNLEFT = double.Parse(textBoxll.Text);
                model.LEFTTURNRIGHT = double.Parse(textBoxlr.Text);
                model.RIGHTTURNLEFT = double.Parse(textBoxrl.Text);
                model.RIGHTTURNRIGHT = double.Parse(textBoxrr.Text);
                model.LEFTTURNLEFTWC = double.Parse(textBoxllwc.Text);
                model.LEFTTURNRIGHTWC = double.Parse(textBoxlrwc.Text);
                model.RIGHTTURNLEFTWC = double.Parse(textBoxrlwc.Text);
                model.RIGHTTURNRIGHTWC = double.Parse(textBoxrrwc.Text);
                if (model.VEHICLENAME.Trim()=="")
                {
                    MessageBox.Show("请先填写车型名称", "系统提示");
                    return;
                }
                if (SqlControl.checkVehicleIsAlive(model.VEHICLENAME))
                {
                    MessageBox.Show("此车型信息已存在，请勿重复添加", "系统提示");
                }
                else if (SqlControl.insertVehicle(model))
                {
                    MessageBox.Show("添加成功", "系统提示");
                }
                else
                    MessageBox.Show("添加失败", "系统提示");
                ref_Staff();
            }
            catch
            {
                MessageBox.Show("数据格式有误", "系统提示");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
