namespace MonitorDataSys
{
    partial class FrLookLog
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridView_Log = new System.Windows.Forms.DataGridView();
            this.CreateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AreaCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TableName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Content = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dt_EndTime = new System.Windows.Forms.DateTimePicker();
            this.dt_StartTime = new System.Windows.Forms.DateTimePicker();
            this.logQueryBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Log)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSize = true;
            this.groupBox2.Controls.Add(this.dataGridView_Log);
            this.groupBox2.Location = new System.Drawing.Point(4, 63);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(665, 287);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "日志列表";
            // 
            // dataGridView_Log
            // 
            this.dataGridView_Log.AllowUserToAddRows = false;
            this.dataGridView_Log.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_Log.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView_Log.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Log.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CreateTime,
            this.Type,
            this.AreaCode,
            this.TableName,
            this.Content});
            this.dataGridView_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_Log.Location = new System.Drawing.Point(3, 17);
            this.dataGridView_Log.Name = "dataGridView_Log";
            this.dataGridView_Log.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView_Log.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView_Log.RowTemplate.Height = 23;
            this.dataGridView_Log.Size = new System.Drawing.Size(659, 267);
            this.dataGridView_Log.TabIndex = 0;
            // 
            // CreateTime
            // 
            this.CreateTime.DataPropertyName = "CreateTime";
            this.CreateTime.HeaderText = "日志时间";
            this.CreateTime.Name = "CreateTime";
            this.CreateTime.ReadOnly = true;
            // 
            // Type
            // 
            this.Type.DataPropertyName = "Type";
            this.Type.HeaderText = "日志类型";
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            // 
            // AreaCode
            // 
            this.AreaCode.DataPropertyName = "AreaCode";
            this.AreaCode.HeaderText = "区域编码";
            this.AreaCode.Name = "AreaCode";
            this.AreaCode.ReadOnly = true;
            // 
            // TableName
            // 
            this.TableName.DataPropertyName = "TableName";
            this.TableName.HeaderText = "表名称";
            this.TableName.Name = "TableName";
            this.TableName.ReadOnly = true;
            // 
            // Content
            // 
            this.Content.DataPropertyName = "Content";
            this.Content.HeaderText = "日志内容";
            this.Content.Name = "Content";
            this.Content.ReadOnly = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dt_EndTime);
            this.groupBox1.Controls.Add(this.dt_StartTime);
            this.groupBox1.Controls.Add(this.logQueryBtn);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(5, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(664, 64);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "操作区";
            // 
            // dt_EndTime
            // 
            this.dt_EndTime.CustomFormat = "yyyy-MM-dd";
            this.dt_EndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dt_EndTime.Location = new System.Drawing.Point(273, 20);
            this.dt_EndTime.Name = "dt_EndTime";
            this.dt_EndTime.Size = new System.Drawing.Size(97, 21);
            this.dt_EndTime.TabIndex = 6;
            // 
            // dt_StartTime
            // 
            this.dt_StartTime.CustomFormat = "yyyy-MM-dd";
            this.dt_StartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dt_StartTime.Location = new System.Drawing.Point(83, 20);
            this.dt_StartTime.Name = "dt_StartTime";
            this.dt_StartTime.Size = new System.Drawing.Size(97, 21);
            this.dt_StartTime.TabIndex = 5;
            // 
            // logQueryBtn
            // 
            this.logQueryBtn.Image = global::MonitorDataSys.Properties.Resources.search;
            this.logQueryBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.logQueryBtn.Location = new System.Drawing.Point(572, 15);
            this.logQueryBtn.Name = "logQueryBtn";
            this.logQueryBtn.Size = new System.Drawing.Size(86, 35);
            this.logQueryBtn.TabIndex = 4;
            this.logQueryBtn.Text = "查询";
            this.logQueryBtn.UseVisualStyleBackColor = true;
            this.logQueryBtn.Click += new System.EventHandler(this.logQueryBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(211, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "终止时间：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "起始时间：";
            // 
            // FrLookLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 350);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrLookLog";
            this.Text = "FrLookLog";
            this.Load += new System.EventHandler(this.FrLookLog_Load);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Log)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dataGridView_Log;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dt_EndTime;
        private System.Windows.Forms.DateTimePicker dt_StartTime;
        private System.Windows.Forms.Button logQueryBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn AreaCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn TableName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Content;
    }
}