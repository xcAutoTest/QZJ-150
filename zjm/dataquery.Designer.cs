namespace zjm
{
    partial class dataquery
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGrid_waitcar = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxPlateAtWait = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.标记为已缴费ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.标记未领ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.radioButtonAnyTime = new System.Windows.Forms.RadioButton();
            this.dateTimeEndtTime = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimeStartTime = new System.Windows.Forms.DateTimePicker();
            this.radioButtonAllTime = new System.Windows.Forms.RadioButton();
            this.radioButtonThisYear = new System.Windows.Forms.RadioButton();
            this.radioButtonThisMonth = new System.Windows.Forms.RadioButton();
            this.radioButtonToday = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.radioButtonFail = new System.Windows.Forms.RadioButton();
            this.radioButtonPass = new System.Windows.Forms.RadioButton();
            this.radioButtonPassAndFail = new System.Windows.Forms.RadioButton();
            this.panelData = new System.Windows.Forms.Panel();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.comboBoxCLLX = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid_waitcar)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.panelData.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel1.Controls.Add(this.panelData);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(798, 587);
            this.panel1.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.groupBox6);
            this.groupBox3.Controls.Add(this.groupBox5);
            this.groupBox3.Controls.Add(this.groupBox1);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Controls.Add(this.button2);
            this.groupBox3.Controls.Add(this.button3);
            this.groupBox3.ForeColor = System.Drawing.Color.Blue;
            this.groupBox3.Location = new System.Drawing.Point(625, 7);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(164, 572);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "操作";
            // 
            // button2
            // 
            this.button2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Font = new System.Drawing.Font("宋体", 10.5F);
            this.button2.ForeColor = System.Drawing.Color.Yellow;
            this.button2.Location = new System.Drawing.Point(25, 524);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(110, 30);
            this.button2.TabIndex = 103;
            this.button2.Text = "关    闭";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button3.Font = new System.Drawing.Font("宋体", 10.5F);
            this.button3.ForeColor = System.Drawing.Color.Yellow;
            this.button3.Location = new System.Drawing.Point(25, 453);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(110, 30);
            this.button3.TabIndex = 95;
            this.button3.Text = "刷新数据";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Font = new System.Drawing.Font("宋体", 10.5F);
            this.button1.ForeColor = System.Drawing.Color.Yellow;
            this.button1.Location = new System.Drawing.Point(25, 488);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(110, 30);
            this.button1.TabIndex = 91;
            this.button1.Text = "查看报告";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.panel2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.ForeColor = System.Drawing.Color.Blue;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(616, 572);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "机动车列表";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGrid_waitcar);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Font = new System.Drawing.Font("宋体", 10.5F);
            this.panel2.Location = new System.Drawing.Point(3, 17);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(610, 552);
            this.panel2.TabIndex = 2;
            // 
            // dataGrid_waitcar
            // 
            this.dataGrid_waitcar.AllowUserToAddRows = false;
            this.dataGrid_waitcar.AllowUserToDeleteRows = false;
            this.dataGrid_waitcar.AllowUserToResizeColumns = false;
            this.dataGrid_waitcar.AllowUserToResizeRows = false;
            this.dataGrid_waitcar.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGrid_waitcar.BackgroundColor = System.Drawing.Color.Silver;
            this.dataGrid_waitcar.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid_waitcar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid_waitcar.Location = new System.Drawing.Point(0, 0);
            this.dataGrid_waitcar.MultiSelect = false;
            this.dataGrid_waitcar.Name = "dataGrid_waitcar";
            this.dataGrid_waitcar.ReadOnly = true;
            this.dataGrid_waitcar.RowHeadersVisible = false;
            this.dataGrid_waitcar.RowTemplate.Height = 23;
            this.dataGrid_waitcar.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGrid_waitcar.Size = new System.Drawing.Size(610, 552);
            this.dataGrid_waitcar.TabIndex = 1;
            this.dataGrid_waitcar.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.textBoxPlateAtWait);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 12F);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(7, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(146, 51);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "VIN";
            // 
            // textBoxPlateAtWait
            // 
            this.textBoxPlateAtWait.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxPlateAtWait.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxPlateAtWait.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.textBoxPlateAtWait.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.textBoxPlateAtWait.Location = new System.Drawing.Point(6, 17);
            this.textBoxPlateAtWait.Name = "textBoxPlateAtWait";
            this.textBoxPlateAtWait.Size = new System.Drawing.Size(130, 25);
            this.textBoxPlateAtWait.TabIndex = 88;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.标记为已缴费ToolStripMenuItem,
            this.标记未领ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(149, 48);
            // 
            // 标记为已缴费ToolStripMenuItem
            // 
            this.标记为已缴费ToolStripMenuItem.Name = "标记为已缴费ToolStripMenuItem";
            this.标记为已缴费ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.标记为已缴费ToolStripMenuItem.Text = "标记报告已领";
            // 
            // 标记未领ToolStripMenuItem
            // 
            this.标记未领ToolStripMenuItem.Name = "标记未领ToolStripMenuItem";
            this.标记未领ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.标记未领ToolStripMenuItem.Text = "标记报告未领";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.radioButtonThisYear);
            this.groupBox4.Controls.Add(this.radioButtonAnyTime);
            this.groupBox4.Controls.Add(this.radioButtonThisMonth);
            this.groupBox4.Controls.Add(this.dateTimeEndtTime);
            this.groupBox4.Controls.Add(this.radioButtonToday);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.dateTimeStartTime);
            this.groupBox4.Controls.Add(this.radioButtonAllTime);
            this.groupBox4.Font = new System.Drawing.Font("宋体", 10.5F);
            this.groupBox4.ForeColor = System.Drawing.Color.White;
            this.groupBox4.Location = new System.Drawing.Point(7, 225);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(150, 217);
            this.groupBox4.TabIndex = 94;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "检测时间";
            // 
            // radioButtonAnyTime
            // 
            this.radioButtonAnyTime.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.radioButtonAnyTime.AutoSize = true;
            this.radioButtonAnyTime.Font = new System.Drawing.Font("宋体", 10.5F);
            this.radioButtonAnyTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.radioButtonAnyTime.Location = new System.Drawing.Point(24, 103);
            this.radioButtonAnyTime.Name = "radioButtonAnyTime";
            this.radioButtonAnyTime.Size = new System.Drawing.Size(67, 18);
            this.radioButtonAnyTime.TabIndex = 102;
            this.radioButtonAnyTime.Text = "自定义";
            this.radioButtonAnyTime.UseVisualStyleBackColor = true;
            // 
            // dateTimeEndtTime
            // 
            this.dateTimeEndtTime.Location = new System.Drawing.Point(18, 178);
            this.dateTimeEndtTime.Name = "dateTimeEndtTime";
            this.dateTimeEndtTime.Size = new System.Drawing.Size(121, 23);
            this.dateTimeEndtTime.TabIndex = 101;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(14, 155);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 20);
            this.label2.TabIndex = 100;
            this.label2.Text = "至";
            // 
            // dateTimeStartTime
            // 
            this.dateTimeStartTime.Location = new System.Drawing.Point(18, 127);
            this.dateTimeStartTime.Name = "dateTimeStartTime";
            this.dateTimeStartTime.Size = new System.Drawing.Size(121, 23);
            this.dateTimeStartTime.TabIndex = 99;
            // 
            // radioButtonAllTime
            // 
            this.radioButtonAllTime.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.radioButtonAllTime.AutoSize = true;
            this.radioButtonAllTime.Font = new System.Drawing.Font("宋体", 10.5F);
            this.radioButtonAllTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.radioButtonAllTime.Location = new System.Drawing.Point(24, 83);
            this.radioButtonAllTime.Name = "radioButtonAllTime";
            this.radioButtonAllTime.Size = new System.Drawing.Size(53, 18);
            this.radioButtonAllTime.TabIndex = 98;
            this.radioButtonAllTime.Text = "不限";
            this.radioButtonAllTime.UseVisualStyleBackColor = true;
            // 
            // radioButtonThisYear
            // 
            this.radioButtonThisYear.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.radioButtonThisYear.AutoSize = true;
            this.radioButtonThisYear.Font = new System.Drawing.Font("宋体", 10.5F);
            this.radioButtonThisYear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.radioButtonThisYear.Location = new System.Drawing.Point(24, 62);
            this.radioButtonThisYear.Name = "radioButtonThisYear";
            this.radioButtonThisYear.Size = new System.Drawing.Size(53, 18);
            this.radioButtonThisYear.TabIndex = 100;
            this.radioButtonThisYear.Text = "本年";
            this.radioButtonThisYear.UseVisualStyleBackColor = true;
            // 
            // radioButtonThisMonth
            // 
            this.radioButtonThisMonth.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.radioButtonThisMonth.AutoSize = true;
            this.radioButtonThisMonth.Font = new System.Drawing.Font("宋体", 10.5F);
            this.radioButtonThisMonth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.radioButtonThisMonth.Location = new System.Drawing.Point(24, 41);
            this.radioButtonThisMonth.Name = "radioButtonThisMonth";
            this.radioButtonThisMonth.Size = new System.Drawing.Size(53, 18);
            this.radioButtonThisMonth.TabIndex = 99;
            this.radioButtonThisMonth.Text = "本月";
            this.radioButtonThisMonth.UseVisualStyleBackColor = true;
            // 
            // radioButtonToday
            // 
            this.radioButtonToday.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.radioButtonToday.AutoSize = true;
            this.radioButtonToday.Checked = true;
            this.radioButtonToday.Font = new System.Drawing.Font("宋体", 10.5F);
            this.radioButtonToday.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.radioButtonToday.Location = new System.Drawing.Point(24, 20);
            this.radioButtonToday.Name = "radioButtonToday";
            this.radioButtonToday.Size = new System.Drawing.Size(53, 18);
            this.radioButtonToday.TabIndex = 98;
            this.radioButtonToday.TabStop = true;
            this.radioButtonToday.Text = "当天";
            this.radioButtonToday.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.radioButtonFail);
            this.groupBox5.Controls.Add(this.radioButtonPass);
            this.groupBox5.Controls.Add(this.radioButtonPassAndFail);
            this.groupBox5.Font = new System.Drawing.Font("宋体", 10.5F);
            this.groupBox5.ForeColor = System.Drawing.Color.White;
            this.groupBox5.Location = new System.Drawing.Point(6, 134);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(150, 90);
            this.groupBox5.TabIndex = 104;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "检测结果";
            // 
            // radioButtonFail
            // 
            this.radioButtonFail.AutoSize = true;
            this.radioButtonFail.Font = new System.Drawing.Font("宋体", 10.5F);
            this.radioButtonFail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.radioButtonFail.Location = new System.Drawing.Point(24, 41);
            this.radioButtonFail.Name = "radioButtonFail";
            this.radioButtonFail.Size = new System.Drawing.Size(67, 18);
            this.radioButtonFail.TabIndex = 99;
            this.radioButtonFail.Text = "不合格";
            this.radioButtonFail.UseVisualStyleBackColor = true;
            // 
            // radioButtonPass
            // 
            this.radioButtonPass.AutoSize = true;
            this.radioButtonPass.Font = new System.Drawing.Font("宋体", 10.5F);
            this.radioButtonPass.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.radioButtonPass.Location = new System.Drawing.Point(24, 19);
            this.radioButtonPass.Name = "radioButtonPass";
            this.radioButtonPass.Size = new System.Drawing.Size(53, 18);
            this.radioButtonPass.TabIndex = 98;
            this.radioButtonPass.Text = "合格";
            this.radioButtonPass.UseVisualStyleBackColor = true;
            // 
            // radioButtonPassAndFail
            // 
            this.radioButtonPassAndFail.AutoSize = true;
            this.radioButtonPassAndFail.Checked = true;
            this.radioButtonPassAndFail.Font = new System.Drawing.Font("宋体", 10.5F);
            this.radioButtonPassAndFail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.radioButtonPassAndFail.Location = new System.Drawing.Point(24, 62);
            this.radioButtonPassAndFail.Name = "radioButtonPassAndFail";
            this.radioButtonPassAndFail.Size = new System.Drawing.Size(53, 18);
            this.radioButtonPassAndFail.TabIndex = 98;
            this.radioButtonPassAndFail.TabStop = true;
            this.radioButtonPassAndFail.Text = "不限";
            this.radioButtonPassAndFail.UseVisualStyleBackColor = true;
            // 
            // panelData
            // 
            this.panelData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelData.Controls.Add(this.groupBox2);
            this.panelData.Location = new System.Drawing.Point(3, 7);
            this.panelData.Name = "panelData";
            this.panelData.Size = new System.Drawing.Size(616, 572);
            this.panelData.TabIndex = 6;
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.BackColor = System.Drawing.Color.Transparent;
            this.groupBox6.Controls.Add(this.comboBoxCLLX);
            this.groupBox6.Font = new System.Drawing.Font("宋体", 12F);
            this.groupBox6.ForeColor = System.Drawing.Color.White;
            this.groupBox6.Location = new System.Drawing.Point(6, 77);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(146, 51);
            this.groupBox6.TabIndex = 105;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "型号";
            // 
            // comboBoxCLLX
            // 
            this.comboBoxCLLX.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCLLX.Font = new System.Drawing.Font("宋体", 15F);
            this.comboBoxCLLX.FormattingEnabled = true;
            this.comboBoxCLLX.Location = new System.Drawing.Point(7, 17);
            this.comboBoxCLLX.Name = "comboBoxCLLX";
            this.comboBoxCLLX.Size = new System.Drawing.Size(131, 28);
            this.comboBoxCLLX.TabIndex = 84;
            // 
            // dataquery
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(798, 587);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "dataquery";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "报表查询";
            this.Load += new System.EventHandler(this.dataquery_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid_waitcar)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.panelData.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dataGrid_waitcar;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxPlateAtWait;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 标记为已缴费ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 标记未领ToolStripMenuItem;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton radioButtonAnyTime;
        private System.Windows.Forms.DateTimePicker dateTimeEndtTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimeStartTime;
        private System.Windows.Forms.RadioButton radioButtonAllTime;
        private System.Windows.Forms.RadioButton radioButtonThisYear;
        private System.Windows.Forms.RadioButton radioButtonThisMonth;
        private System.Windows.Forms.RadioButton radioButtonToday;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton radioButtonFail;
        private System.Windows.Forms.RadioButton radioButtonPass;
        private System.Windows.Forms.RadioButton radioButtonPassAndFail;
        private System.Windows.Forms.Panel panelData;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ComboBox comboBoxCLLX;
    }
}