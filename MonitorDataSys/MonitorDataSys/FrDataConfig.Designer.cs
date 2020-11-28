namespace MonitorDataSys
{
    partial class FrDataConfig
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.airStationLoadBtn = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.dBTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cityComboBox = new System.Windows.Forms.ComboBox();
            this.stationQueryBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.proviceComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.UniqueCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.序号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AreaName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TableRelation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DBPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CsName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DBType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DBIPAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.load_lb = new System.Windows.Forms.Label();
            this.typeComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.typeComboBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.airStationLoadBtn);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.dBTypeComboBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cityComboBox);
            this.groupBox1.Controls.Add(this.stationQueryBtn);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.proviceComboBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(6, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(659, 51);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "操作区";
            // 
            // airStationLoadBtn
            // 
            this.airStationLoadBtn.Image = global::MonitorDataSys.Properties.Resources.tb;
            this.airStationLoadBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.airStationLoadBtn.Location = new System.Drawing.Point(575, 15);
            this.airStationLoadBtn.Name = "airStationLoadBtn";
            this.airStationLoadBtn.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.airStationLoadBtn.Size = new System.Drawing.Size(73, 29);
            this.airStationLoadBtn.TabIndex = 45;
            this.airStationLoadBtn.Text = "  同步";
            this.airStationLoadBtn.UseVisualStyleBackColor = true;
            this.airStationLoadBtn.Click += new System.EventHandler(this.airStationLoadBtn_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(475, -22);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 12);
            this.label10.TabIndex = 42;
            this.label10.Text = "*必填";
            // 
            // dBTypeComboBox
            // 
            this.dBTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dBTypeComboBox.FormattingEnabled = true;
            this.dBTypeComboBox.Location = new System.Drawing.Point(247, -25);
            this.dBTypeComboBox.Name = "dBTypeComboBox";
            this.dBTypeComboBox.Size = new System.Drawing.Size(222, 20);
            this.dBTypeComboBox.TabIndex = 39;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(154, -22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 30;
            this.label4.Text = "数据库类型：";
            // 
            // cityComboBox
            // 
            this.cityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cityComboBox.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cityComboBox.FormattingEnabled = true;
            this.cityComboBox.Location = new System.Drawing.Point(231, 14);
            this.cityComboBox.Name = "cityComboBox";
            this.cityComboBox.Size = new System.Drawing.Size(109, 28);
            this.cityComboBox.TabIndex = 6;
            // 
            // stationQueryBtn
            // 
            this.stationQueryBtn.Image = global::MonitorDataSys.Properties.Resources.search;
            this.stationQueryBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.stationQueryBtn.Location = new System.Drawing.Point(501, 14);
            this.stationQueryBtn.Name = "stationQueryBtn";
            this.stationQueryBtn.Size = new System.Drawing.Size(68, 30);
            this.stationQueryBtn.TabIndex = 4;
            this.stationQueryBtn.Text = "  查询";
            this.stationQueryBtn.UseVisualStyleBackColor = true;
            this.stationQueryBtn.Click += new System.EventHandler(this.stationQueryBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(175, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "选择城市：";
            // 
            // proviceComboBox
            // 
            this.proviceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.proviceComboBox.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.proviceComboBox.FormattingEnabled = true;
            this.proviceComboBox.Location = new System.Drawing.Point(62, 14);
            this.proviceComboBox.Name = "proviceComboBox";
            this.proviceComboBox.Size = new System.Drawing.Size(107, 28);
            this.proviceComboBox.TabIndex = 1;
            this.proviceComboBox.SelectedIndexChanged += new System.EventHandler(this.proviceComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "选择省份：";
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSize = true;
            this.groupBox2.Controls.Add(this.dataGridView1);
            this.groupBox2.Location = new System.Drawing.Point(3, 67);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(662, 286);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "站点数据列表";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.UniqueCode,
            this.序号,
            this.AreaName,
            this.TableRelation,
            this.DBPort,
            this.CsName,
            this.DBType,
            this.DBIPAddress,
            this.Column1});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 17);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle12;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(656, 266);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            // 
            // UniqueCode
            // 
            this.UniqueCode.DataPropertyName = "UniqueCode";
            this.UniqueCode.HeaderText = "UniqueCode";
            this.UniqueCode.Name = "UniqueCode";
            this.UniqueCode.ReadOnly = true;
            this.UniqueCode.Visible = false;
            // 
            // 序号
            // 
            this.序号.FillWeight = 72.30695F;
            this.序号.HeaderText = "序号";
            this.序号.Name = "序号";
            this.序号.ReadOnly = true;
            // 
            // AreaName
            // 
            this.AreaName.DataPropertyName = "Area";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.AreaName.DefaultCellStyle = dataGridViewCellStyle10;
            this.AreaName.FillWeight = 90.70252F;
            this.AreaName.HeaderText = "城市名称";
            this.AreaName.MinimumWidth = 3;
            this.AreaName.Name = "AreaName";
            this.AreaName.ReadOnly = true;
            // 
            // TableRelation
            // 
            this.TableRelation.DataPropertyName = "CityCode";
            this.TableRelation.FillWeight = 109.8428F;
            this.TableRelation.HeaderText = "城市编码";
            this.TableRelation.Name = "TableRelation";
            this.TableRelation.ReadOnly = true;
            // 
            // DBPort
            // 
            this.DBPort.DataPropertyName = "PositionName";
            this.DBPort.FillWeight = 109.8428F;
            this.DBPort.HeaderText = "站点名称";
            this.DBPort.Name = "DBPort";
            this.DBPort.ReadOnly = true;
            // 
            // CsName
            // 
            this.CsName.DataPropertyName = "StationCode";
            this.CsName.FillWeight = 109.8428F;
            this.CsName.HeaderText = "站点编码";
            this.CsName.Name = "CsName";
            this.CsName.ReadOnly = true;
            // 
            // DBType
            // 
            this.DBType.DataPropertyName = "Latitude";
            this.DBType.FillWeight = 87.55532F;
            this.DBType.HeaderText = "纬度";
            this.DBType.Name = "DBType";
            this.DBType.ReadOnly = true;
            // 
            // DBIPAddress
            // 
            this.DBIPAddress.DataPropertyName = "Longitude";
            this.DBIPAddress.FillWeight = 84.31587F;
            this.DBIPAddress.HeaderText = "经度";
            this.DBIPAddress.Name = "DBIPAddress";
            this.DBIPAddress.ReadOnly = true;
            // 
            // Column1
            // 
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.NullValue = "删除";
            this.Column1.DefaultCellStyle = dataGridViewCellStyle11;
            this.Column1.FillWeight = 70F;
            this.Column1.HeaderText = "操作";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column1.UseColumnTextForButtonValue = true;
            this.Column1.Visible = false;
            // 
            // load_lb
            // 
            this.load_lb.BackColor = System.Drawing.Color.Transparent;
            this.load_lb.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.load_lb.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.load_lb.Image = global::MonitorDataSys.Properties.Resources.timg;
            this.load_lb.Location = new System.Drawing.Point(0, -1);
            this.load_lb.Name = "load_lb";
            this.load_lb.Size = new System.Drawing.Size(668, 354);
            this.load_lb.TabIndex = 2;
            this.load_lb.Text = "正在拼命同步站点...";
            this.load_lb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.load_lb.Visible = false;
            // 
            // typeComboBox
            // 
            this.typeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.typeComboBox.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.typeComboBox.FormattingEnabled = true;
            this.typeComboBox.Items.AddRange(new object[] {
            "空气质量",
            "气象"});
            this.typeComboBox.Location = new System.Drawing.Point(381, 16);
            this.typeComboBox.Name = "typeComboBox";
            this.typeComboBox.Size = new System.Drawing.Size(88, 28);
            this.typeComboBox.TabIndex = 47;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(346, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 46;
            this.label3.Text = "类型：";
            // 
            // FrDataConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 355);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.load_lb);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrDataConfig";
            this.Text = "FrDataConfig";
            this.TransparencyKey = System.Drawing.Color.Transparent;
            this.Load += new System.EventHandler(this.FrDataConfig_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox proviceComboBox;
        private System.Windows.Forms.Button stationQueryBtn;
        private System.Windows.Forms.ComboBox dBTypeComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button airStationLoadBtn;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cityComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label load_lb;
        private System.Windows.Forms.DataGridViewTextBoxColumn UniqueCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn 序号;
        private System.Windows.Forms.DataGridViewTextBoxColumn AreaName;
        private System.Windows.Forms.DataGridViewTextBoxColumn TableRelation;
        private System.Windows.Forms.DataGridViewTextBoxColumn DBPort;
        private System.Windows.Forms.DataGridViewTextBoxColumn CsName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DBType;
        private System.Windows.Forms.DataGridViewTextBoxColumn DBIPAddress;
        private System.Windows.Forms.DataGridViewButtonColumn Column1;
        private System.Windows.Forms.ComboBox typeComboBox;
        private System.Windows.Forms.Label label3;
    }
}