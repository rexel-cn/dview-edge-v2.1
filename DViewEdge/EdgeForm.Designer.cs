
namespace DViewEdge
{
    partial class EdgeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EdgeForm));
            this.lbl1 = new System.Windows.Forms.Label();
            this.lbl6 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtRepeate = new System.Windows.Forms.TextBox();
            this.lbl10 = new System.Windows.Forms.Label();
            this.txtSelectTag = new System.Windows.Forms.TextBox();
            this.lbl9 = new System.Windows.Forms.Label();
            this.lbl11 = new System.Windows.Forms.Label();
            this.txtOffset = new System.Windows.Forms.TextBox();
            this.lbl7 = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.lbl8 = new System.Windows.Forms.Label();
            this.predict_speed = new System.Windows.Forms.Label();
            this.lbl12 = new System.Windows.Forms.Label();
            this.lbl13 = new System.Windows.Forms.Label();
            this.txtUserClientId = new System.Windows.Forms.TextBox();
            this.grbMonitor = new System.Windows.Forms.GroupBox();
            this.lblSize = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            this.lblStartTime = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lbl4 = new System.Windows.Forms.Label();
            this.lbl3 = new System.Windows.Forms.Label();
            this.lbl2 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRestart = new System.Windows.Forms.Button();
            this.btnUpMeta = new System.Windows.Forms.Button();
            this.btnLog = new System.Windows.Forms.Button();
            this.grbPlatform = new System.Windows.Forms.GroupBox();
            this.req8 = new System.Windows.Forms.Label();
            this.req7 = new System.Windows.Forms.Label();
            this.req6 = new System.Windows.Forms.Label();
            this.req5 = new System.Windows.Forms.Label();
            this.lbl5 = new System.Windows.Forms.Label();
            this.grbCollection = new System.Windows.Forms.GroupBox();
            this.req11 = new System.Windows.Forms.Label();
            this.req10 = new System.Windows.Forms.Label();
            this.req9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.grbDevice = new System.Windows.Forms.GroupBox();
            this.lbl14 = new System.Windows.Forms.Label();
            this.lblMachineCode = new System.Windows.Forms.Label();
            this.txtDeviceDescribe = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.grbMonitor.SuspendLayout();
            this.grbPlatform.SuspendLayout();
            this.grbCollection.SuspendLayout();
            this.grbDevice.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.Location = new System.Drawing.Point(13, 41);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(59, 17);
            this.lbl1.TabIndex = 0;
            this.lbl1.Text = "连接状态:";
            this.lbl1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl6
            // 
            this.lbl6.AutoSize = true;
            this.lbl6.Location = new System.Drawing.Point(12, 85);
            this.lbl6.Name = "lbl6";
            this.lbl6.Size = new System.Drawing.Size(35, 17);
            this.lbl6.TabIndex = 3;
            this.lbl6.Text = "密码:";
            // 
            // txtUsername
            // 
            this.txtUsername.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txtUsername.Location = new System.Drawing.Point(77, 42);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(220, 23);
            this.txtUsername.TabIndex = 2;
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txtPassword.Location = new System.Drawing.Point(77, 82);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(220, 23);
            this.txtPassword.TabIndex = 5;
            // 
            // txtRepeate
            // 
            this.txtRepeate.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txtRepeate.Location = new System.Drawing.Point(92, 82);
            this.txtRepeate.Name = "txtRepeate";
            this.txtRepeate.Size = new System.Drawing.Size(161, 23);
            this.txtRepeate.TabIndex = 6;
            this.txtRepeate.Leave += new System.EventHandler(this.RepeateLeave);
            // 
            // lbl10
            // 
            this.lbl10.AutoSize = true;
            this.lbl10.Location = new System.Drawing.Point(12, 85);
            this.lbl10.Name = "lbl10";
            this.lbl10.Size = new System.Drawing.Size(59, 17);
            this.lbl10.TabIndex = 4;
            this.lbl10.Text = "采集频率:";
            // 
            // txtSelectTag
            // 
            this.txtSelectTag.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txtSelectTag.Location = new System.Drawing.Point(92, 42);
            this.txtSelectTag.Name = "txtSelectTag";
            this.txtSelectTag.Size = new System.Drawing.Size(161, 23);
            this.txtSelectTag.TabIndex = 2;
            this.txtSelectTag.Leave += new System.EventHandler(this.SelectTagLeave);
            // 
            // lbl9
            // 
            this.lbl9.AutoSize = true;
            this.lbl9.Location = new System.Drawing.Point(12, 45);
            this.lbl9.Name = "lbl9";
            this.lbl9.Size = new System.Drawing.Size(59, 17);
            this.lbl9.TabIndex = 0;
            this.lbl9.Text = "变量类型:";
            // 
            // lbl11
            // 
            this.lbl11.AutoSize = true;
            this.lbl11.Location = new System.Drawing.Point(12, 125);
            this.lbl11.Name = "lbl11";
            this.lbl11.Size = new System.Drawing.Size(59, 17);
            this.lbl11.TabIndex = 8;
            this.lbl11.Text = "时间偏移:";
            // 
            // txtOffset
            // 
            this.txtOffset.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txtOffset.Location = new System.Drawing.Point(92, 122);
            this.txtOffset.Name = "txtOffset";
            this.txtOffset.Size = new System.Drawing.Size(161, 23);
            this.txtOffset.TabIndex = 10;
            this.txtOffset.Leave += new System.EventHandler(this.OffsetLeave);
            // 
            // lbl7
            // 
            this.lbl7.AutoSize = true;
            this.lbl7.Location = new System.Drawing.Point(12, 125);
            this.lbl7.Name = "lbl7";
            this.lbl7.Size = new System.Drawing.Size(35, 17);
            this.lbl7.TabIndex = 6;
            this.lbl7.Text = "地址:";
            // 
            // txtAddress
            // 
            this.txtAddress.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txtAddress.Location = new System.Drawing.Point(77, 122);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(220, 23);
            this.txtAddress.TabIndex = 8;
            this.txtAddress.Leave += new System.EventHandler(this.AddressLeave);
            // 
            // txtPort
            // 
            this.txtPort.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txtPort.Location = new System.Drawing.Point(77, 162);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(220, 23);
            this.txtPort.TabIndex = 11;
            this.txtPort.Leave += new System.EventHandler(this.PortLeave);
            // 
            // lbl8
            // 
            this.lbl8.AutoSize = true;
            this.lbl8.Location = new System.Drawing.Point(12, 165);
            this.lbl8.Name = "lbl8";
            this.lbl8.Size = new System.Drawing.Size(35, 17);
            this.lbl8.TabIndex = 9;
            this.lbl8.Text = "端口:";
            // 
            // predict_speed
            // 
            this.predict_speed.AutoSize = true;
            this.predict_speed.Location = new System.Drawing.Point(856, 278);
            this.predict_speed.Name = "predict_speed";
            this.predict_speed.Size = new System.Drawing.Size(0, 17);
            this.predict_speed.TabIndex = 32;
            // 
            // lbl12
            // 
            this.lbl12.AutoSize = true;
            this.lbl12.Location = new System.Drawing.Point(12, 45);
            this.lbl12.Name = "lbl12";
            this.lbl12.Size = new System.Drawing.Size(83, 17);
            this.lbl12.TabIndex = 0;
            this.lbl12.Text = "机器唯一标识:";
            // 
            // lbl13
            // 
            this.lbl13.AutoSize = true;
            this.lbl13.Location = new System.Drawing.Point(12, 85);
            this.lbl13.Name = "lbl13";
            this.lbl13.Size = new System.Drawing.Size(83, 17);
            this.lbl13.TabIndex = 2;
            this.lbl13.Text = "自定义客户端:";
            // 
            // txtUserClientId
            // 
            this.txtUserClientId.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txtUserClientId.Location = new System.Drawing.Point(111, 82);
            this.txtUserClientId.Name = "txtUserClientId";
            this.txtUserClientId.Size = new System.Drawing.Size(595, 23);
            this.txtUserClientId.TabIndex = 3;
            this.txtUserClientId.Leave += new System.EventHandler(this.UserClientIdLeave);
            // 
            // grbMonitor
            // 
            this.grbMonitor.Controls.Add(this.lblSize);
            this.grbMonitor.Controls.Add(this.lblCount);
            this.grbMonitor.Controls.Add(this.lblStartTime);
            this.grbMonitor.Controls.Add(this.lblStatus);
            this.grbMonitor.Controls.Add(this.lbl4);
            this.grbMonitor.Controls.Add(this.lbl3);
            this.grbMonitor.Controls.Add(this.lbl2);
            this.grbMonitor.Controls.Add(this.lbl1);
            this.grbMonitor.Location = new System.Drawing.Point(15, 65);
            this.grbMonitor.Name = "grbMonitor";
            this.grbMonitor.Size = new System.Drawing.Size(728, 87);
            this.grbMonitor.TabIndex = 6;
            this.grbMonitor.TabStop = false;
            this.grbMonitor.Text = "运行状态";
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblSize.Location = new System.Drawing.Point(615, 42);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(15, 14);
            this.lblSize.TabIndex = 7;
            this.lblSize.Text = "-";
            this.lblSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblCount.Location = new System.Drawing.Point(465, 42);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(15, 14);
            this.lblCount.TabIndex = 5;
            this.lblCount.Text = "-";
            this.lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblStartTime
            // 
            this.lblStartTime.AutoSize = true;
            this.lblStartTime.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblStartTime.Location = new System.Drawing.Point(225, 42);
            this.lblStartTime.Name = "lblStartTime";
            this.lblStartTime.Size = new System.Drawing.Size(15, 14);
            this.lblStartTime.TabIndex = 3;
            this.lblStartTime.Text = "-";
            this.lblStartTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblStatus.Location = new System.Drawing.Point(78, 42);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(37, 14);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "离线";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl4
            // 
            this.lbl4.AutoSize = true;
            this.lbl4.Location = new System.Drawing.Point(550, 41);
            this.lbl4.Name = "lbl4";
            this.lbl4.Size = new System.Drawing.Size(59, 17);
            this.lbl4.TabIndex = 6;
            this.lbl4.Text = "发送流量:";
            this.lbl4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl3
            // 
            this.lbl3.AutoSize = true;
            this.lbl3.Location = new System.Drawing.Point(400, 41);
            this.lbl3.Name = "lbl3";
            this.lbl3.Size = new System.Drawing.Size(59, 17);
            this.lbl3.TabIndex = 4;
            this.lbl3.Text = "发送次数:";
            this.lbl3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl2
            // 
            this.lbl2.AutoSize = true;
            this.lbl2.Location = new System.Drawing.Point(160, 41);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(59, 17);
            this.lbl2.TabIndex = 2;
            this.lbl2.Text = "启动时间:";
            this.lbl2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSave
            // 
            this.btnSave.AutoSize = true;
            this.btnSave.BackColor = System.Drawing.SystemColors.Menu;
            this.btnSave.BackgroundImage = global::DViewEdge.Properties.Resources.save;
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(15, 15);
            this.btnSave.Name = "btnSave";
            this.btnSave.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.btnSave.Size = new System.Drawing.Size(115, 34);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "保存配置";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.SaveButtonClick);
            // 
            // btnRestart
            // 
            this.btnRestart.AutoSize = true;
            this.btnRestart.BackColor = System.Drawing.SystemColors.Menu;
            this.btnRestart.BackgroundImage = global::DViewEdge.Properties.Resources.rebort;
            this.btnRestart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnRestart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRestart.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnRestart.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRestart.Location = new System.Drawing.Point(165, 15);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.btnRestart.Size = new System.Drawing.Size(115, 34);
            this.btnRestart.TabIndex = 2;
            this.btnRestart.Text = "重新启动";
            this.btnRestart.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRestart.UseVisualStyleBackColor = false;
            this.btnRestart.Click += new System.EventHandler(this.RestartButtonClick);
            // 
            // btnUpMeta
            // 
            this.btnUpMeta.AutoSize = true;
            this.btnUpMeta.BackColor = System.Drawing.SystemColors.Menu;
            this.btnUpMeta.BackgroundImage = global::DViewEdge.Properties.Resources.upload;
            this.btnUpMeta.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnUpMeta.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUpMeta.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnUpMeta.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUpMeta.Location = new System.Drawing.Point(315, 15);
            this.btnUpMeta.Name = "btnUpMeta";
            this.btnUpMeta.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.btnUpMeta.Size = new System.Drawing.Size(115, 34);
            this.btnUpMeta.TabIndex = 3;
            this.btnUpMeta.Text = "上报测点";
            this.btnUpMeta.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUpMeta.UseVisualStyleBackColor = false;
            // 
            // btnLog
            // 
            this.btnLog.AutoSize = true;
            this.btnLog.BackColor = System.Drawing.SystemColors.Menu;
            this.btnLog.BackgroundImage = global::DViewEdge.Properties.Resources.log;
            this.btnLog.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnLog.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLog.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnLog.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLog.Location = new System.Drawing.Point(465, 15);
            this.btnLog.Name = "btnLog";
            this.btnLog.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.btnLog.Size = new System.Drawing.Size(115, 34);
            this.btnLog.TabIndex = 4;
            this.btnLog.Text = "日志监控";
            this.btnLog.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLog.UseVisualStyleBackColor = false;
            this.btnLog.Click += new System.EventHandler(this.LogBtnClieck);
            // 
            // grbPlatform
            // 
            this.grbPlatform.Controls.Add(this.txtUsername);
            this.grbPlatform.Controls.Add(this.req8);
            this.grbPlatform.Controls.Add(this.req7);
            this.grbPlatform.Controls.Add(this.req6);
            this.grbPlatform.Controls.Add(this.req5);
            this.grbPlatform.Controls.Add(this.lbl5);
            this.grbPlatform.Controls.Add(this.lbl6);
            this.grbPlatform.Controls.Add(this.txtPassword);
            this.grbPlatform.Controls.Add(this.lbl7);
            this.grbPlatform.Controls.Add(this.txtAddress);
            this.grbPlatform.Controls.Add(this.lbl8);
            this.grbPlatform.Controls.Add(this.txtPort);
            this.grbPlatform.Location = new System.Drawing.Point(15, 168);
            this.grbPlatform.Name = "grbPlatform";
            this.grbPlatform.Size = new System.Drawing.Size(322, 211);
            this.grbPlatform.TabIndex = 7;
            this.grbPlatform.TabStop = false;
            this.grbPlatform.Text = "物联平台";
            // 
            // req8
            // 
            this.req8.AutoSize = true;
            this.req8.ForeColor = System.Drawing.Color.Red;
            this.req8.Location = new System.Drawing.Point(62, 165);
            this.req8.Name = "req8";
            this.req8.Size = new System.Drawing.Size(13, 17);
            this.req8.TabIndex = 10;
            this.req8.Text = "*";
            this.req8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // req7
            // 
            this.req7.AutoSize = true;
            this.req7.ForeColor = System.Drawing.Color.Red;
            this.req7.Location = new System.Drawing.Point(62, 125);
            this.req7.Name = "req7";
            this.req7.Size = new System.Drawing.Size(13, 17);
            this.req7.TabIndex = 7;
            this.req7.Text = "*";
            this.req7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // req6
            // 
            this.req6.AutoSize = true;
            this.req6.ForeColor = System.Drawing.Color.Red;
            this.req6.Location = new System.Drawing.Point(62, 85);
            this.req6.Name = "req6";
            this.req6.Size = new System.Drawing.Size(13, 17);
            this.req6.TabIndex = 4;
            this.req6.Text = "*";
            this.req6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // req5
            // 
            this.req5.AutoSize = true;
            this.req5.ForeColor = System.Drawing.Color.Red;
            this.req5.Location = new System.Drawing.Point(62, 45);
            this.req5.Name = "req5";
            this.req5.Size = new System.Drawing.Size(13, 17);
            this.req5.TabIndex = 1;
            this.req5.Text = "*";
            this.req5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl5
            // 
            this.lbl5.AutoSize = true;
            this.lbl5.Location = new System.Drawing.Point(12, 45);
            this.lbl5.Name = "lbl5";
            this.lbl5.Size = new System.Drawing.Size(35, 17);
            this.lbl5.TabIndex = 0;
            this.lbl5.Text = "账号:";
            this.lbl5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grbCollection
            // 
            this.grbCollection.Controls.Add(this.lbl9);
            this.grbCollection.Controls.Add(this.txtSelectTag);
            this.grbCollection.Controls.Add(this.lbl10);
            this.grbCollection.Controls.Add(this.txtRepeate);
            this.grbCollection.Controls.Add(this.req11);
            this.grbCollection.Controls.Add(this.req10);
            this.grbCollection.Controls.Add(this.req9);
            this.grbCollection.Controls.Add(this.label3);
            this.grbCollection.Controls.Add(this.label2);
            this.grbCollection.Controls.Add(this.label1);
            this.grbCollection.Controls.Add(this.lbl11);
            this.grbCollection.Controls.Add(this.txtOffset);
            this.grbCollection.Location = new System.Drawing.Point(360, 168);
            this.grbCollection.Name = "grbCollection";
            this.grbCollection.Size = new System.Drawing.Size(383, 211);
            this.grbCollection.TabIndex = 8;
            this.grbCollection.TabStop = false;
            this.grbCollection.Text = "数据采集";
            // 
            // req11
            // 
            this.req11.AutoSize = true;
            this.req11.ForeColor = System.Drawing.Color.Red;
            this.req11.Location = new System.Drawing.Point(77, 125);
            this.req11.Name = "req11";
            this.req11.Size = new System.Drawing.Size(13, 17);
            this.req11.TabIndex = 9;
            this.req11.Text = "*";
            this.req11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // req10
            // 
            this.req10.AutoSize = true;
            this.req10.ForeColor = System.Drawing.Color.Red;
            this.req10.Location = new System.Drawing.Point(77, 85);
            this.req10.Name = "req10";
            this.req10.Size = new System.Drawing.Size(13, 17);
            this.req10.TabIndex = 5;
            this.req10.Text = "*";
            this.req10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // req9
            // 
            this.req9.AutoSize = true;
            this.req9.ForeColor = System.Drawing.Color.Red;
            this.req9.Location = new System.Drawing.Point(77, 45);
            this.req9.Name = "req9";
            this.req9.Size = new System.Drawing.Size(13, 17);
            this.req9.TabIndex = 1;
            this.req9.Text = "*";
            this.req9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(255, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "例：AR,DI,VA";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(255, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "秒";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(255, 125);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 17);
            this.label1.TabIndex = 11;
            this.label1.Text = "秒";
            // 
            // grbDevice
            // 
            this.grbDevice.Controls.Add(this.lbl12);
            this.grbDevice.Controls.Add(this.lbl14);
            this.grbDevice.Controls.Add(this.lblMachineCode);
            this.grbDevice.Controls.Add(this.lbl13);
            this.grbDevice.Controls.Add(this.txtDeviceDescribe);
            this.grbDevice.Controls.Add(this.txtUserClientId);
            this.grbDevice.Location = new System.Drawing.Point(15, 395);
            this.grbDevice.Name = "grbDevice";
            this.grbDevice.Size = new System.Drawing.Size(727, 165);
            this.grbDevice.TabIndex = 9;
            this.grbDevice.TabStop = false;
            this.grbDevice.Text = "设备标识";
            // 
            // lbl14
            // 
            this.lbl14.AutoSize = true;
            this.lbl14.Location = new System.Drawing.Point(12, 125);
            this.lbl14.Name = "lbl14";
            this.lbl14.Size = new System.Drawing.Size(83, 17);
            this.lbl14.TabIndex = 4;
            this.lbl14.Text = "设备名称描述:";
            // 
            // lblMachineCode
            // 
            this.lblMachineCode.AutoSize = true;
            this.lblMachineCode.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblMachineCode.Location = new System.Drawing.Point(111, 48);
            this.lblMachineCode.Name = "lblMachineCode";
            this.lblMachineCode.Size = new System.Drawing.Size(15, 14);
            this.lblMachineCode.TabIndex = 1;
            this.lblMachineCode.Text = "-";
            this.lblMachineCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDeviceDescribe
            // 
            this.txtDeviceDescribe.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txtDeviceDescribe.Location = new System.Drawing.Point(111, 122);
            this.txtDeviceDescribe.Name = "txtDeviceDescribe";
            this.txtDeviceDescribe.Size = new System.Drawing.Size(595, 23);
            this.txtDeviceDescribe.TabIndex = 5;
            this.txtDeviceDescribe.Leave += new System.EventHandler(this.DeviceDescribeLeave);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 595);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 17);
            this.label5.TabIndex = 20;
            // 
            // EdgeForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(761, 579);
            this.Controls.Add(this.grbDevice);
            this.Controls.Add(this.grbCollection);
            this.Controls.Add(this.grbPlatform);
            this.Controls.Add(this.btnLog);
            this.Controls.Add(this.btnUpMeta);
            this.Controls.Add(this.btnRestart);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.grbMonitor);
            this.Controls.Add(this.predict_speed);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EdgeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DView数据采集程序";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EdgeFormClosing);
            this.grbMonitor.ResumeLayout(false);
            this.grbMonitor.PerformLayout();
            this.grbPlatform.ResumeLayout(false);
            this.grbPlatform.PerformLayout();
            this.grbCollection.ResumeLayout(false);
            this.grbCollection.PerformLayout();
            this.grbDevice.ResumeLayout(false);
            this.grbDevice.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.Label lbl6;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtRepeate;
        private System.Windows.Forms.Label lbl10;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lbl9;
        private System.Windows.Forms.TextBox txtSelectTag;
        private System.Windows.Forms.Label lbl11;
        private System.Windows.Forms.TextBox txtOffset;
        private System.Windows.Forms.Label lbl7;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label lbl8;
        private System.Windows.Forms.Label predict_speed;
        private System.Windows.Forms.Label lbl12;
        private System.Windows.Forms.Label lbl13;
        private System.Windows.Forms.TextBox txtUserClientId;
        private System.Windows.Forms.GroupBox grbMonitor;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Label lblStartTime;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lbl4;
        private System.Windows.Forms.Label lbl3;
        private System.Windows.Forms.Label lbl2;
        private System.Windows.Forms.Button btnRestart;
        private System.Windows.Forms.Button btnUpMeta;
        private System.Windows.Forms.Button btnLog;
        private System.Windows.Forms.GroupBox grbPlatform;
        private System.Windows.Forms.Label lbl5;
        private System.Windows.Forms.Label req8;
        private System.Windows.Forms.Label req7;
        private System.Windows.Forms.Label req6;
        private System.Windows.Forms.Label req5;
        private System.Windows.Forms.GroupBox grbCollection;
        private System.Windows.Forms.Label req9;
        private System.Windows.Forms.Label req11;
        private System.Windows.Forms.Label req10;
        private System.Windows.Forms.GroupBox grbDevice;
        private System.Windows.Forms.Label lbl14;
        private System.Windows.Forms.TextBox txtDeviceDescribe;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblMachineCode;
    }
}

