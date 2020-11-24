namespace MonitorDataSys
{
    partial class FrDataSourceConfig
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataSourceConfigAddBtn = new System.Windows.Forms.Button();
            this.dBTypeSearchComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.dBTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dataConfigQueryBtn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DBType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DBIPAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DBPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DBName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataSourceConfigAddBtn);
            this.groupBox1.Controls.Add(this.dBTypeSearchComboBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.dBTypeComboBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.dataConfigQueryBtn);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(661, 51);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "操作区";
            // 
            // dataSourceConfigAddBtn
            // 
            this.dataSourceConfigAddBtn.Image = global::MonitorDataSys.Properties.Resources.add;
            this.dataSourceConfigAddBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.dataSourceConfigAddBtn.Location = new System.Drawing.Point(577, 15);
            this.dataSourceConfigAddBtn.Name = "dataSourceConfigAddBtn";
            this.dataSourceConfigAddBtn.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dataSourceConfigAddBtn.Size = new System.Drawing.Size(74, 29);
            this.dataSourceConfigAddBtn.TabIndex = 45;
            this.dataSourceConfigAddBtn.Text = "  新增";
            this.dataSourceConfigAddBtn.UseVisualStyleBackColor = true;
            this.dataSourceConfigAddBtn.Click += new System.EventHandler(this.dataSourceConfigAddBtn_Click);
            // 
            // dBTypeSearchComboBox
            // 
            this.dBTypeSearchComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dBTypeSearchComboBox.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dBTypeSearchComboBox.FormattingEnabled = true;
            this.dBTypeSearchComboBox.Location = new System.Drawing.Point(85, 14);
            this.dBTypeSearchComboBox.Name = "dBTypeSearchComboBox";
            this.dBTypeSearchComboBox.Size = new System.Drawing.Size(101, 28);
            this.dBTypeSearchComboBox.TabIndex = 44;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 43;
            this.label3.Text = "数据库类型：";
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
            // dataConfigQueryBtn
            // 
            this.dataConfigQueryBtn.Image = global::MonitorDataSys.Properties.Resources.search;
            this.dataConfigQueryBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.dataConfigQueryBtn.Location = new System.Drawing.Point(501, 14);
            this.dataConfigQueryBtn.Name = "dataConfigQueryBtn";
            this.dataConfigQueryBtn.Size = new System.Drawing.Size(70, 30);
            this.dataConfigQueryBtn.TabIndex = 4;
            this.dataConfigQueryBtn.Text = "  查询";
            this.dataConfigQueryBtn.UseVisualStyleBackColor = true;
            this.dataConfigQueryBtn.Click += new System.EventHandler(this.dataConfigQueryBtn_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSize = true;
            this.groupBox2.Controls.Add(this.dataGridView1);
            this.groupBox2.Location = new System.Drawing.Point(3, 67);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(665, 285);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "数据源列表";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.DBType,
            this.DBIPAddress,
            this.DBPort,
            this.DBName,
            this.Column1});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 17);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(659, 265);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Visible = false;
            // 
            // DBType
            // 
            this.DBType.DataPropertyName = "DBType";
            this.DBType.FillWeight = 96.8818F;
            this.DBType.HeaderText = "数据库类型";
            this.DBType.Name = "DBType";
            this.DBType.ReadOnly = true;
            // 
            // DBIPAddress
            // 
            this.DBIPAddress.DataPropertyName = "DBIPAddress";
            this.DBIPAddress.FillWeight = 96.8818F;
            this.DBIPAddress.HeaderText = "数据库IP";
            this.DBIPAddress.Name = "DBIPAddress";
            this.DBIPAddress.ReadOnly = true;
            // 
            // DBPort
            // 
            this.DBPort.DataPropertyName = "DBPort";
            this.DBPort.FillWeight = 96.8818F;
            this.DBPort.HeaderText = "数据库端口";
            this.DBPort.Name = "DBPort";
            this.DBPort.ReadOnly = true;
            // 
            // DBName
            // 
            this.DBName.DataPropertyName = "DBName";
            this.DBName.FillWeight = 96.8818F;
            this.DBName.HeaderText = "数据库名称";
            this.DBName.Name = "DBName";
            this.DBName.ReadOnly = true;
            // 
            // Column1
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = "删除";
            this.Column1.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column1.FillWeight = 70F;
            this.Column1.HeaderText = "操作";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column1.UseColumnTextForButtonValue = true;
            // 
            // FrDataSourceConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 353);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrDataSourceConfig";
            this.Text = "FrDataConfig";
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
        private System.Windows.Forms.Button dataConfigQueryBtn;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox dBTypeComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox dBTypeSearchComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button dataSourceConfigAddBtn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn DBType;
        private System.Windows.Forms.DataGridViewTextBoxColumn DBIPAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn DBPort;
        private System.Windows.Forms.DataGridViewTextBoxColumn DBName;
        private System.Windows.Forms.DataGridViewButtonColumn Column1;
    }
}
