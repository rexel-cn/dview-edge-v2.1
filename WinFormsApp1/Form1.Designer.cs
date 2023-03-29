
namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.user_t = new System.Windows.Forms.TextBox();
            this.pass_t = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.pinlv = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.selectTag_t = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.ectime = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.broker_t = new System.Windows.Forms.TextBox();
            this.port_t = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.network_bytes = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
            this.package_once = new System.Windows.Forms.Label();
            this.predict_speed = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.sim_car_network_package = new System.Windows.Forms.TextBox();
            this.c_gb = new System.Windows.Forms.CheckBox();
            this.c_mb = new System.Windows.Forms.CheckBox();
            this.button4 = new System.Windows.Forms.Button();
            this.point_filter = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.machine_code = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.point_count = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.user_client_id = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "用户名";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "密码";
            // 
            // user_t
            // 
            this.user_t.Location = new System.Drawing.Point(131, 12);
            this.user_t.Name = "user_t";
            this.user_t.Size = new System.Drawing.Size(110, 23);
            this.user_t.TabIndex = 1;
            this.user_t.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pass_t
            // 
            this.pass_t.Location = new System.Drawing.Point(131, 52);
            this.pass_t.Name = "pass_t";
            this.pass_t.Size = new System.Drawing.Size(110, 23);
            this.pass_t.TabIndex = 2;
            this.pass_t.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.pass_t.UseSystemPasswordChar = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(16, 553);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 35);
            this.button3.TabIndex = 16;
            this.button3.Text = "保存配置";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // pinlv
            // 
            this.pinlv.Location = new System.Drawing.Point(131, 92);
            this.pinlv.Name = "pinlv";
            this.pinlv.Size = new System.Drawing.Size(46, 23);
            this.pinlv.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 17);
            this.label4.TabIndex = 11;
            this.label4.Text = "上报频率";
            // 
            // selectTag_t
            // 
            this.selectTag_t.Location = new System.Drawing.Point(131, 172);
            this.selectTag_t.Name = "selectTag_t";
            this.selectTag_t.Size = new System.Drawing.Size(110, 23);
            this.selectTag_t.TabIndex = 6;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 175);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 17);
            this.label8.TabIndex = 18;
            this.label8.Text = "选择类型";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 135);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 17);
            this.label9.TabIndex = 20;
            this.label9.Text = "纠错时间";
            // 
            // ectime
            // 
            this.ectime.Location = new System.Drawing.Point(131, 132);
            this.ectime.Name = "ectime";
            this.ectime.Size = new System.Drawing.Size(110, 23);
            this.ectime.TabIndex = 5;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 215);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(56, 17);
            this.label10.TabIndex = 22;
            this.label10.Text = "服务地址";
            // 
            // broker_t
            // 
            this.broker_t.Location = new System.Drawing.Point(131, 212);
            this.broker_t.Name = "broker_t";
            this.broker_t.Size = new System.Drawing.Size(110, 23);
            this.broker_t.TabIndex = 7;
            // 
            // port_t
            // 
            this.port_t.Location = new System.Drawing.Point(131, 252);
            this.port_t.Name = "port_t";
            this.port_t.Size = new System.Drawing.Size(110, 23);
            this.port_t.TabIndex = 8;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(16, 255);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(56, 17);
            this.label11.TabIndex = 24;
            this.label11.Text = "服务端口";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(16, 375);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(80, 17);
            this.label12.TabIndex = 27;
            this.label12.Text = "本次流量使用";
            // 
            // network_bytes
            // 
            this.network_bytes.BackColor = System.Drawing.SystemColors.Control;
            this.network_bytes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.network_bytes.Location = new System.Drawing.Point(131, 412);
            this.network_bytes.Name = "network_bytes";
            this.network_bytes.Size = new System.Drawing.Size(110, 21);
            this.network_bytes.TabIndex = 14;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(16, 455);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(75, 21);
            this.checkBox1.TabIndex = 15;
            this.checkBox1.Text = "开机启动";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.CheckBox1_CheckedChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(16, 415);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(80, 17);
            this.label14.TabIndex = 30;
            this.label14.Text = "单条数据大小";
            // 
            // package_once
            // 
            this.package_once.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.package_once.Location = new System.Drawing.Point(131, 372);
            this.package_once.Name = "package_once";
            this.package_once.Size = new System.Drawing.Size(110, 20);
            this.package_once.TabIndex = 13;
            // 
            // predict_speed
            // 
            this.predict_speed.AutoSize = true;
            this.predict_speed.Location = new System.Drawing.Point(210, 122);
            this.predict_speed.Name = "predict_speed";
            this.predict_speed.Size = new System.Drawing.Size(0, 17);
            this.predict_speed.TabIndex = 32;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(16, 335);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(87, 17);
            this.label16.TabIndex = 33;
            this.label16.Text = "SIM卡流量(月)";
            // 
            // sim_car_network_package
            // 
            this.sim_car_network_package.Location = new System.Drawing.Point(131, 332);
            this.sim_car_network_package.Name = "sim_car_network_package";
            this.sim_car_network_package.ReadOnly = true;
            this.sim_car_network_package.Size = new System.Drawing.Size(38, 23);
            this.sim_car_network_package.TabIndex = 10;
            this.sim_car_network_package.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Sim_car_network_package_KeyPress);
            // 
            // c_gb
            // 
            this.c_gb.AutoSize = true;
            this.c_gb.Location = new System.Drawing.Point(175, 334);
            this.c_gb.Name = "c_gb";
            this.c_gb.Size = new System.Drawing.Size(44, 21);
            this.c_gb.TabIndex = 11;
            this.c_gb.Text = "GB";
            this.c_gb.UseVisualStyleBackColor = true;
            this.c_gb.CheckedChanged += new System.EventHandler(this.C_gb_CheckedChanged);
            // 
            // c_mb
            // 
            this.c_mb.AutoSize = true;
            this.c_mb.Location = new System.Drawing.Point(218, 334);
            this.c_mb.Name = "c_mb";
            this.c_mb.Size = new System.Drawing.Size(47, 21);
            this.c_mb.TabIndex = 12;
            this.c_mb.Text = "MB";
            this.c_mb.UseVisualStyleBackColor = true;
            this.c_mb.CheckedChanged += new System.EventHandler(this.C_mb_CheckedChanged);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(158, 553);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(100, 35);
            this.button4.TabIndex = 17;
            this.button4.Text = "重新启动";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Button4_Click_1);
            // 
            // point_filter
            // 
            this.point_filter.Location = new System.Drawing.Point(195, 92);
            this.point_filter.Name = "point_filter";
            this.point_filter.Size = new System.Drawing.Size(46, 23);
            this.point_filter.TabIndex = 4;
            this.point_filter.Text = "ALL";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(285, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(545, 504);
            this.richTextBox1.TabIndex = 18;
            this.richTextBox1.Text = "";
            // 
            // machine_code
            // 
            this.machine_code.Location = new System.Drawing.Point(386, 529);
            this.machine_code.Name = "machine_code";
            this.machine_code.ReadOnly = true;
            this.machine_code.Size = new System.Drawing.Size(444, 23);
            this.machine_code.TabIndex = 19;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(285, 532);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(80, 17);
            this.label13.TabIndex = 41;
            this.label13.Text = "机器唯一标识";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 295);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 17);
            this.label1.TabIndex = 42;
            this.label1.Text = "发送总点数";
            // 
            // point_count
            // 
            this.point_count.Location = new System.Drawing.Point(131, 292);
            this.point_count.Name = "point_count";
            this.point_count.ReadOnly = true;
            this.point_count.Size = new System.Drawing.Size(110, 23);
            this.point_count.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(285, 571);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 17);
            this.label5.TabIndex = 44;
            this.label5.Text = "自定义客户端";
            // 
            // user_client_id
            // 
            this.user_client_id.Location = new System.Drawing.Point(386, 565);
            this.user_client_id.Name = "user_client_id";
            this.user_client_id.Size = new System.Drawing.Size(444, 23);
            this.user_client_id.TabIndex = 20;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(848, 626);
            this.Controls.Add(this.user_client_id);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.point_count);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.machine_code);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.point_filter);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.c_mb);
            this.Controls.Add(this.c_gb);
            this.Controls.Add(this.sim_car_network_package);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.predict_speed);
            this.Controls.Add(this.package_once);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.network_bytes);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.port_t);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.broker_t);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.ectime);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.selectTag_t);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pinlv);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.pass_t);
            this.Controls.Add(this.user_t);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "DVIEW MQTT CLIENT - by rexel ids";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox port_t;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox pinlv;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox pass_t;
        private System.Windows.Forms.TextBox user_t;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox selectTag_t;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox ectime;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox broker_t;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label network_bytes;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label package_once;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label predict_speed;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox sim_car_network_package;
        private System.Windows.Forms.CheckBox c_gb;
        private System.Windows.Forms.CheckBox c_mb;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox point_filter;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TextBox machine_code;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox point_count;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox user_client_id;
    }
}

