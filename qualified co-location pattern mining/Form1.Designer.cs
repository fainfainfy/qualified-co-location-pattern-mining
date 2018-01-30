namespace qualified_co_location_pattern_mining
{
    partial class Form
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_occupancy = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label_inputpath = new System.Windows.Forms.Label();
            this.button_scan = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_r = new System.Windows.Forms.TextBox();
            this.textBox_prev = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button_datasource = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label_outputpath = new System.Windows.Forms.Label();
            this.buttonTest = new System.Windows.Forms.Button();
            this.textBox_weight = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button_beginWT = new System.Windows.Forms.Button();
            this.textBox_quality = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AllowDrop = true;
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Consolas", 32.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label6.Location = new System.Drawing.Point(132, 82);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(886, 51);
            this.label6.TabIndex = 63;
            this.label6.Text = "Qualified Co-location Pattern Mining";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // textBox_occupancy
            // 
            this.textBox_occupancy.Cursor = System.Windows.Forms.Cursors.Default;
            this.textBox_occupancy.Location = new System.Drawing.Point(426, 426);
            this.textBox_occupancy.Name = "textBox_occupancy";
            this.textBox_occupancy.Size = new System.Drawing.Size(283, 21);
            this.textBox_occupancy.TabIndex = 62;
            this.textBox_occupancy.Text = "0.2";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label4.Location = new System.Drawing.Point(226, 423);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(190, 24);
            this.label4.TabIndex = 61;
            this.label4.Text = "min_Occupation:";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label_inputpath
            // 
            this.label_inputpath.AutoSize = true;
            this.label_inputpath.BackColor = System.Drawing.Color.Transparent;
            this.label_inputpath.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label_inputpath.Location = new System.Drawing.Point(229, 235);
            this.label_inputpath.Name = "label_inputpath";
            this.label_inputpath.Size = new System.Drawing.Size(191, 12);
            this.label_inputpath.TabIndex = 60;
            this.label_inputpath.Text = "INPUT DATA IS NOT LORDING......";
            // 
            // button_scan
            // 
            this.button_scan.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button_scan.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_scan.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_scan.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_scan.Font = new System.Drawing.Font("Consolas", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_scan.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.button_scan.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button_scan.Location = new System.Drawing.Point(767, 263);
            this.button_scan.Name = "button_scan";
            this.button_scan.Size = new System.Drawing.Size(143, 23);
            this.button_scan.TabIndex = 58;
            this.button_scan.Text = "Scan Output  path";
            this.button_scan.UseVisualStyleBackColor = false;
            this.button_scan.Click += new System.EventHandler(this.button_scan_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label2.Location = new System.Drawing.Point(226, 262);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(514, 24);
            this.label2.TabIndex = 56;
            this.label2.Text = "Please choose  a path to store the result:";
            // 
            // textBox_r
            // 
            this.textBox_r.Location = new System.Drawing.Point(475, 345);
            this.textBox_r.Name = "textBox_r";
            this.textBox_r.Size = new System.Drawing.Size(234, 21);
            this.textBox_r.TabIndex = 55;
            this.textBox_r.Text = "10";
            // 
            // textBox_prev
            // 
            this.textBox_prev.Location = new System.Drawing.Point(426, 385);
            this.textBox_prev.Name = "textBox_prev";
            this.textBox_prev.Size = new System.Drawing.Size(283, 21);
            this.textBox_prev.TabIndex = 54;
            this.textBox_prev.Text = "0.3";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label7.Location = new System.Drawing.Point(226, 342);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(238, 24);
            this.label7.TabIndex = 53;
            this.label7.Text = "distance threshold:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label5.Location = new System.Drawing.Point(226, 379);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(190, 24);
            this.label5.TabIndex = 52;
            this.label5.Text = "min_prevalence:";
            // 
            // button_datasource
            // 
            this.button_datasource.BackColor = System.Drawing.SystemColors.Window;
            this.button_datasource.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_datasource.Cursor = System.Windows.Forms.Cursors.Default;
            this.button_datasource.FlatAppearance.BorderSize = 0;
            this.button_datasource.Font = new System.Drawing.Font("Consolas", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_datasource.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.button_datasource.Location = new System.Drawing.Point(588, 197);
            this.button_datasource.Name = "button_datasource";
            this.button_datasource.Size = new System.Drawing.Size(323, 23);
            this.button_datasource.TabIndex = 51;
            this.button_datasource.Text = "Scan Input path";
            this.button_datasource.UseVisualStyleBackColor = false;
            this.button_datasource.Click += new System.EventHandler(this.button_datasource_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label1.Location = new System.Drawing.Point(227, 196);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(359, 24);
            this.label1.TabIndex = 50;
            this.label1.Text = "Please choose a input data ：";
            // 
            // label_outputpath
            // 
            this.label_outputpath.AutoSize = true;
            this.label_outputpath.BackColor = System.Drawing.Color.Transparent;
            this.label_outputpath.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label_outputpath.Location = new System.Drawing.Point(232, 306);
            this.label_outputpath.Name = "label_outputpath";
            this.label_outputpath.Size = new System.Drawing.Size(215, 12);
            this.label_outputpath.TabIndex = 64;
            this.label_outputpath.Text = "OUTPUT RESULTS IS NOT LORDING......";
            // 
            // buttonTest
            // 
            this.buttonTest.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonTest.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.buttonTest.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonTest.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.buttonTest.Location = new System.Drawing.Point(1052, 12);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(102, 40);
            this.buttonTest.TabIndex = 65;
            this.buttonTest.Text = "测试";
            this.buttonTest.UseVisualStyleBackColor = false;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // textBox_weight
            // 
            this.textBox_weight.Cursor = System.Windows.Forms.Cursors.Default;
            this.textBox_weight.Location = new System.Drawing.Point(426, 467);
            this.textBox_weight.Name = "textBox_weight";
            this.textBox_weight.Size = new System.Drawing.Size(283, 21);
            this.textBox_weight.TabIndex = 67;
            this.textBox_weight.Text = "0.2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label3.Location = new System.Drawing.Point(226, 464);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(178, 24);
            this.label3.TabIndex = 66;
            this.label3.Text = "weight([0,1]):";
            // 
            // button_beginWT
            // 
            this.button_beginWT.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button_beginWT.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_beginWT.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_beginWT.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.button_beginWT.Location = new System.Drawing.Point(412, 574);
            this.button_beginWT.Name = "button_beginWT";
            this.button_beginWT.Size = new System.Drawing.Size(346, 40);
            this.button_beginWT.TabIndex = 68;
            this.button_beginWT.Text = "Weight+Threshold";
            this.button_beginWT.UseVisualStyleBackColor = false;
            this.button_beginWT.Click += new System.EventHandler(this.button_beginWT_Click);
            // 
            // textBox_quality
            // 
            this.textBox_quality.Cursor = System.Windows.Forms.Cursors.Default;
            this.textBox_quality.Location = new System.Drawing.Point(426, 505);
            this.textBox_quality.Name = "textBox_quality";
            this.textBox_quality.Size = new System.Drawing.Size(283, 21);
            this.textBox_quality.TabIndex = 71;
            this.textBox_quality.Text = "0.2";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label8.Location = new System.Drawing.Point(226, 502);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(154, 24);
            this.label8.TabIndex = 70;
            this.label8.Text = "min_Quality:";
            // 
            // Form
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1166, 659);
            this.Controls.Add(this.textBox_quality);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.button_beginWT);
            this.Controls.Add(this.textBox_weight);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonTest);
            this.Controls.Add(this.label_outputpath);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox_occupancy);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label_inputpath);
            this.Controls.Add(this.button_scan);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_r);
            this.Controls.Add(this.textBox_prev);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button_datasource);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.DarkOliveGreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Form";
            this.Text = "Form_Qualified Co-location Mining";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_occupancy;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label_inputpath;
        private System.Windows.Forms.Button button_scan;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_r;
        private System.Windows.Forms.TextBox textBox_prev;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button_datasource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_outputpath;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.TextBox textBox_weight;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_beginWT;
        private System.Windows.Forms.TextBox textBox_quality;
        private System.Windows.Forms.Label label8;
    }
}

