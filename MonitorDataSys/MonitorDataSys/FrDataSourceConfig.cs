using MonitorDataSys.Models;
using MonitorDataSys.Repository.local;
using MonitorDataSys.UtilTool;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace MonitorDataSys
{
    public partial class FrDataSourceConfig : Form
    {

        private readonly DataConfigRepository dcr = new DataConfigRepository();
        private readonly LogRepository lr = new LogRepository();

        public FrDataSourceConfig()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
        }

        private void FrDataConfig_Load(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, string> dir = MonitorDataSys.UtilTool.DBType.GetDBTypes();
                foreach (var item in dir)
                {
                    DataBaseType dataBaseType = new DataBaseType();
                    dataBaseType.dataBaseName = item.Key;
                    dataBaseType.dataBsseValue = item.Value;
                    dBTypeSearchComboBox.Items.Add(dataBaseType);
                    dBTypeSearchComboBox.DisplayMember = "dataBaseName";
                }
                dataConfigQueryBtn_Click(null, null);
            }
            catch (Exception ex)
            {
                //日志处理
                lr.AddLogInfo(ex.StackTrace.ToString(), "捕获异常信息", "捕获异常信息", "Error");
            }
        }

        private void dataConfigQueryBtn_Click(object sender, EventArgs e)
        {
            try
            {
                DataBaseType dataBaseTypeObj = (DataBaseType)(dBTypeSearchComboBox.SelectedItem);
                string dBTypeFilter = "";
                if (dBTypeSearchComboBox.SelectedItem != null)
                {
                    dBTypeFilter = dataBaseTypeObj.dataBsseValue;
                }
                DataTable dt = dcr.DataConfigInfoQuery("*", "*", dBTypeFilter);
                if (dt != null)
                {
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                //日志处理
                lr.AddLogInfo(ex.StackTrace.ToString(), "捕获异常信息", "捕获异常信息", "Error");
            }
        }


        /// <summary>
        /// 添加联系人的回调函数
        /// </summary>
        /// <param name="isSucess"></param>
        void acaReturnAddResult(bool isSucess)
        {
            try
            {
                if (isSucess)
                {
                    dataConfigQueryBtn_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                //日志处理
                lr.AddLogInfo(ex.StackTrace.ToString(), "捕获异常信息", "捕获异常信息", "Error");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewColumn column = dataGridView1.Columns[e.ColumnIndex];
                    if (column is DataGridViewButtonColumn)
                    {
                        if (MessageBox.Show("确定删除数据源配置吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            string id = this.dataGridView1.Rows[e.RowIndex].Cells["Id"].Value.ToString();
                            bool result = dcr.DeleteDataConfigInfo(id);
                            if (result)
                            {
                                this.dataGridView1.Rows.RemoveAt(e.RowIndex);//删除当前行
                                dataConfigQueryBtn_Click(null, null);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //日志处理
                lr.AddLogInfo(ex.StackTrace.ToString(), "捕获异常信息", "捕获异常信息", "Error");
            }
        }

        private void dataSourceConfigAddBtn_Click(object sender, EventArgs e)
        {
            try
            {
                FrDataConfigAdd dca = new FrDataConfigAdd();
                dca.returnAddResult += acaReturnAddResult;
                dca.ShowDialog();
            }
            catch (Exception ex)
            {
                //日志处理
                lr.AddLogInfo(ex.StackTrace.ToString(), "捕获异常信息", "捕获异常信息", "Error");
            }
        }
    }
}
