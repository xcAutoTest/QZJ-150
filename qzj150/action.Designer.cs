namespace childTest
{
    partial class action
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
            this.buttonZpOff = new System.Windows.Forms.Button();
            this.buttonZpOn = new System.Windows.Forms.Button();
            this.buttonBzqOff = new System.Windows.Forms.Button();
            this.buttonBzqOn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonZpOff
            // 
            this.buttonZpOff.Font = new System.Drawing.Font("宋体", 14F);
            this.buttonZpOff.Location = new System.Drawing.Point(383, 2);
            this.buttonZpOff.Name = "buttonZpOff";
            this.buttonZpOff.Size = new System.Drawing.Size(123, 59);
            this.buttonZpOff.TabIndex = 84;
            this.buttonZpOff.Text = "转盘放松";
            this.buttonZpOff.UseVisualStyleBackColor = true;
            this.buttonZpOff.Click += new System.EventHandler(this.buttonZpOff_Click);
            // 
            // buttonZpOn
            // 
            this.buttonZpOn.Font = new System.Drawing.Font("宋体", 14F);
            this.buttonZpOn.Location = new System.Drawing.Point(257, 2);
            this.buttonZpOn.Name = "buttonZpOn";
            this.buttonZpOn.Size = new System.Drawing.Size(120, 59);
            this.buttonZpOn.TabIndex = 83;
            this.buttonZpOn.Text = "转盘锁紧";
            this.buttonZpOn.UseVisualStyleBackColor = true;
            this.buttonZpOn.Click += new System.EventHandler(this.buttonZpOn_Click);
            // 
            // buttonBzqOff
            // 
            this.buttonBzqOff.Font = new System.Drawing.Font("宋体", 14F);
            this.buttonBzqOff.Location = new System.Drawing.Point(128, 2);
            this.buttonBzqOff.Name = "buttonBzqOff";
            this.buttonBzqOff.Size = new System.Drawing.Size(123, 59);
            this.buttonBzqOff.TabIndex = 82;
            this.buttonBzqOff.Text = "摆正器收缩";
            this.buttonBzqOff.UseVisualStyleBackColor = true;
            this.buttonBzqOff.Click += new System.EventHandler(this.buttonBzqOff_Click);
            // 
            // buttonBzqOn
            // 
            this.buttonBzqOn.Font = new System.Drawing.Font("宋体", 14F);
            this.buttonBzqOn.Location = new System.Drawing.Point(2, 2);
            this.buttonBzqOn.Name = "buttonBzqOn";
            this.buttonBzqOn.Size = new System.Drawing.Size(120, 59);
            this.buttonBzqOn.TabIndex = 81;
            this.buttonBzqOn.Text = "摆正器张开";
            this.buttonBzqOn.UseVisualStyleBackColor = true;
            this.buttonBzqOn.Click += new System.EventHandler(this.buttonBzqOn_Click);
            // 
            // action
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 63);
            this.Controls.Add(this.buttonZpOff);
            this.Controls.Add(this.buttonZpOn);
            this.Controls.Add(this.buttonBzqOff);
            this.Controls.Add(this.buttonBzqOn);
            this.Name = "action";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "操作控制";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonZpOff;
        private System.Windows.Forms.Button buttonZpOn;
        private System.Windows.Forms.Button buttonBzqOff;
        private System.Windows.Forms.Button buttonBzqOn;
    }
}