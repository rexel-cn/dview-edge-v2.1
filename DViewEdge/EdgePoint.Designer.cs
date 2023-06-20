namespace DViewEdge
{
    partial class EdgePoint
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
            this.dataGridAR = new System.Windows.Forms.DataGridView();
            this.pointId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.repeate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelAR = new System.Windows.Forms.Label();
            this.labelAI = new System.Windows.Forms.Label();
            this.dataGridAI = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelAO = new System.Windows.Forms.Label();
            this.dataGridAO = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelVA = new System.Windows.Forms.Label();
            this.dataGridVA = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelVD = new System.Windows.Forms.Label();
            this.dataGridVD = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelDO = new System.Windows.Forms.Label();
            this.dataGridDO = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelDI = new System.Windows.Forms.Label();
            this.dataGridDI = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelDR = new System.Windows.Forms.Label();
            this.dataGridDR = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridAR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridAI)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridAO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridVA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridVD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridDO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridDI)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridDR)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridAR
            // 
            this.dataGridAR.AllowUserToAddRows = false;
            this.dataGridAR.AllowUserToDeleteRows = false;
            this.dataGridAR.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridAR.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.pointId,
            this.repeate});
            this.dataGridAR.Location = new System.Drawing.Point(3, 66);
            this.dataGridAR.Name = "dataGridAR";
            this.dataGridAR.RowHeadersVisible = false;
            this.dataGridAR.RowTemplate.Height = 25;
            this.dataGridAR.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridAR.Size = new System.Drawing.Size(300, 360);
            this.dataGridAR.TabIndex = 6;
            // 
            // pointId
            // 
            this.pointId.HeaderText = "测点ID";
            this.pointId.Name = "pointId";
            this.pointId.ReadOnly = true;
            this.pointId.Width = 200;
            // 
            // repeate
            // 
            this.repeate.HeaderText = "频率(毫秒)";
            this.repeate.Name = "repeate";
            // 
            // labelAR
            // 
            this.labelAR.AutoSize = true;
            this.labelAR.Location = new System.Drawing.Point(3, 46);
            this.labelAR.Name = "labelAR";
            this.labelAR.Size = new System.Drawing.Size(101, 17);
            this.labelAR.TabIndex = 7;
            this.labelAR.Text = "AR-模拟读写变量";
            // 
            // labelAI
            // 
            this.labelAI.AutoSize = true;
            this.labelAI.Location = new System.Drawing.Point(309, 46);
            this.labelAI.Name = "labelAI";
            this.labelAI.Size = new System.Drawing.Size(97, 17);
            this.labelAI.TabIndex = 9;
            this.labelAI.Text = "AI-模拟只读变量";
            // 
            // dataGridAI
            // 
            this.dataGridAI.AllowUserToAddRows = false;
            this.dataGridAI.AllowUserToDeleteRows = false;
            this.dataGridAI.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridAI.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.dataGridAI.Location = new System.Drawing.Point(309, 66);
            this.dataGridAI.Name = "dataGridAI";
            this.dataGridAI.RowHeadersVisible = false;
            this.dataGridAI.RowTemplate.Height = 25;
            this.dataGridAI.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridAI.Size = new System.Drawing.Size(300, 360);
            this.dataGridAI.TabIndex = 8;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "测点ID";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 200;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "频率(毫秒)";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // labelAO
            // 
            this.labelAO.AutoSize = true;
            this.labelAO.Location = new System.Drawing.Point(615, 46);
            this.labelAO.Name = "labelAO";
            this.labelAO.Size = new System.Drawing.Size(103, 17);
            this.labelAO.TabIndex = 11;
            this.labelAO.Text = "AO-模拟只写变量";
            // 
            // dataGridAO
            // 
            this.dataGridAO.AllowUserToAddRows = false;
            this.dataGridAO.AllowUserToDeleteRows = false;
            this.dataGridAO.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridAO.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            this.dataGridAO.Location = new System.Drawing.Point(615, 66);
            this.dataGridAO.Name = "dataGridAO";
            this.dataGridAO.RowHeadersVisible = false;
            this.dataGridAO.RowTemplate.Height = 25;
            this.dataGridAO.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridAO.Size = new System.Drawing.Size(300, 360);
            this.dataGridAO.TabIndex = 10;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "测点ID";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 200;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "频率(毫秒)";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // labelVA
            // 
            this.labelVA.AutoSize = true;
            this.labelVA.Location = new System.Drawing.Point(921, 46);
            this.labelVA.Name = "labelVA";
            this.labelVA.Size = new System.Drawing.Size(101, 17);
            this.labelVA.TabIndex = 13;
            this.labelVA.Text = "VA-内部模拟变量";
            // 
            // dataGridVA
            // 
            this.dataGridVA.AllowUserToAddRows = false;
            this.dataGridVA.AllowUserToDeleteRows = false;
            this.dataGridVA.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridVA.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6});
            this.dataGridVA.Location = new System.Drawing.Point(921, 66);
            this.dataGridVA.Name = "dataGridVA";
            this.dataGridVA.RowHeadersVisible = false;
            this.dataGridVA.RowTemplate.Height = 25;
            this.dataGridVA.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridVA.Size = new System.Drawing.Size(300, 360);
            this.dataGridVA.TabIndex = 12;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "测点ID";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 200;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "频率(毫秒)";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // labelVD
            // 
            this.labelVD.AutoSize = true;
            this.labelVD.Location = new System.Drawing.Point(921, 437);
            this.labelVD.Name = "labelVD";
            this.labelVD.Size = new System.Drawing.Size(102, 17);
            this.labelVD.TabIndex = 21;
            this.labelVD.Text = "VD-内部开关变量";
            // 
            // dataGridVD
            // 
            this.dataGridVD.AllowUserToAddRows = false;
            this.dataGridVD.AllowUserToDeleteRows = false;
            this.dataGridVD.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridVD.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8});
            this.dataGridVD.Location = new System.Drawing.Point(921, 457);
            this.dataGridVD.Name = "dataGridVD";
            this.dataGridVD.RowHeadersVisible = false;
            this.dataGridVD.RowTemplate.Height = 25;
            this.dataGridVD.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridVD.Size = new System.Drawing.Size(300, 360);
            this.dataGridVD.TabIndex = 20;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "测点ID";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Width = 200;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "频率(毫秒)";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            // 
            // labelDO
            // 
            this.labelDO.AutoSize = true;
            this.labelDO.Location = new System.Drawing.Point(615, 437);
            this.labelDO.Name = "labelDO";
            this.labelDO.Size = new System.Drawing.Size(104, 17);
            this.labelDO.TabIndex = 19;
            this.labelDO.Text = "DO-开关只写变量";
            // 
            // dataGridDO
            // 
            this.dataGridDO.AllowUserToAddRows = false;
            this.dataGridDO.AllowUserToDeleteRows = false;
            this.dataGridDO.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridDO.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn9,
            this.dataGridViewTextBoxColumn10});
            this.dataGridDO.Location = new System.Drawing.Point(615, 457);
            this.dataGridDO.Name = "dataGridDO";
            this.dataGridDO.RowHeadersVisible = false;
            this.dataGridDO.RowTemplate.Height = 25;
            this.dataGridDO.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridDO.Size = new System.Drawing.Size(300, 360);
            this.dataGridDO.TabIndex = 18;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.HeaderText = "测点ID";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            this.dataGridViewTextBoxColumn9.Width = 200;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.HeaderText = "频率(毫秒)";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            // 
            // labelDI
            // 
            this.labelDI.AutoSize = true;
            this.labelDI.Location = new System.Drawing.Point(309, 437);
            this.labelDI.Name = "labelDI";
            this.labelDI.Size = new System.Drawing.Size(98, 17);
            this.labelDI.TabIndex = 17;
            this.labelDI.Text = "DI-开关只读变量";
            // 
            // dataGridDI
            // 
            this.dataGridDI.AllowUserToAddRows = false;
            this.dataGridDI.AllowUserToDeleteRows = false;
            this.dataGridDI.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridDI.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn11,
            this.dataGridViewTextBoxColumn12});
            this.dataGridDI.Location = new System.Drawing.Point(309, 457);
            this.dataGridDI.Name = "dataGridDI";
            this.dataGridDI.RowHeadersVisible = false;
            this.dataGridDI.RowTemplate.Height = 25;
            this.dataGridDI.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridDI.Size = new System.Drawing.Size(300, 360);
            this.dataGridDI.TabIndex = 16;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.HeaderText = "测点ID";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.ReadOnly = true;
            this.dataGridViewTextBoxColumn11.Width = 200;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.HeaderText = "频率(毫秒)";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            // 
            // labelDR
            // 
            this.labelDR.AutoSize = true;
            this.labelDR.Location = new System.Drawing.Point(3, 437);
            this.labelDR.Name = "labelDR";
            this.labelDR.Size = new System.Drawing.Size(102, 17);
            this.labelDR.TabIndex = 15;
            this.labelDR.Text = "DR-开关读写变量";
            // 
            // dataGridDR
            // 
            this.dataGridDR.AllowUserToAddRows = false;
            this.dataGridDR.AllowUserToDeleteRows = false;
            this.dataGridDR.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridDR.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn13,
            this.dataGridViewTextBoxColumn14});
            this.dataGridDR.Location = new System.Drawing.Point(3, 457);
            this.dataGridDR.Name = "dataGridDR";
            this.dataGridDR.RowHeadersVisible = false;
            this.dataGridDR.RowTemplate.Height = 25;
            this.dataGridDR.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridDR.Size = new System.Drawing.Size(300, 360);
            this.dataGridDR.TabIndex = 14;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.HeaderText = "测点ID";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.ReadOnly = true;
            this.dataGridViewTextBoxColumn13.Width = 200;
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.HeaderText = "频率(毫秒)";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
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
            this.btnSave.Location = new System.Drawing.Point(3, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.btnSave.Size = new System.Drawing.Size(115, 34);
            this.btnSave.TabIndex = 22;
            this.btnSave.Text = "保存配置";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.SaveButtonClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(553, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(668, 17);
            this.label1.TabIndex = 23;
            this.label1.Text = "默认使用统一采集频率。如需特殊采集频率，请输入频率后保存配置并重新启动采集软件。特殊频率的测点以红色字体标识。";
            // 
            // EdgePoint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1229, 833);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.labelVD);
            this.Controls.Add(this.dataGridVD);
            this.Controls.Add(this.labelDO);
            this.Controls.Add(this.dataGridDO);
            this.Controls.Add(this.labelDI);
            this.Controls.Add(this.dataGridDI);
            this.Controls.Add(this.labelDR);
            this.Controls.Add(this.dataGridDR);
            this.Controls.Add(this.labelVA);
            this.Controls.Add(this.dataGridVA);
            this.Controls.Add(this.labelAO);
            this.Controls.Add(this.dataGridAO);
            this.Controls.Add(this.labelAI);
            this.Controls.Add(this.dataGridAI);
            this.Controls.Add(this.labelAR);
            this.Controls.Add(this.dataGridAR);
            this.Name = "EdgePoint";
            this.Text = "EdgePoint";
            this.Activated += new System.EventHandler(this.EdgePoint_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EdgePoint_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridAR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridAI)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridAO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridVA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridVD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridDO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridDI)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridDR)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridAR;
        private System.Windows.Forms.Label labelAR;
        private System.Windows.Forms.Label labelAI;
        private System.Windows.Forms.DataGridView dataGridAI;
        private System.Windows.Forms.Label labelAO;
        private System.Windows.Forms.DataGridView dataGridAO;
        private System.Windows.Forms.Label labelVA;
        private System.Windows.Forms.DataGridView dataGridVA;
        private System.Windows.Forms.Label labelVD;
        private System.Windows.Forms.DataGridView dataGridView4;
        private System.Windows.Forms.Label labelDO;
        private System.Windows.Forms.DataGridView dataGridView5;
        private System.Windows.Forms.Label labelDI;
        private System.Windows.Forms.DataGridView dataGridView6;
        private System.Windows.Forms.Label labelDR;
        private System.Windows.Forms.DataGridView dataGridDR;
        private System.Windows.Forms.DataGridView dataGridVD;
        private System.Windows.Forms.DataGridView dataGridDO;
        private System.Windows.Forms.DataGridView dataGridDI;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridViewTextBoxColumn pointId;
        private System.Windows.Forms.DataGridViewTextBoxColumn repeate;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private System.Windows.Forms.Label label1;
    }
}