using MonitorDataSys.Models;
using MonitorDataSys.Repository.local;
using MonitorDataSys.UtilTool;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace MonitorDataSys
{
    public partial class FrDataConfigAdd : Form
    {
        private readonly DataConfigRepository dcr = new DataConfigRepository();
        private readonly LogRepository lr = new LogRepository();

        //添加配置数据后的委托和事件
        public delegate void AddResultStatus(bool isSucess);
        public event AddResultStatus returnAddResult;

        public FrDataConfigAdd()
        {
            InitializeComponent();
        }

        private void saveDataConfigBtn_Click(object sender, EventArgs e)
        {
            try
            {
                DataBaseType dataBaseTypeObj = (DataBaseType)(dBTypeComboBox.SelectedItem);
                if (dBTypeComboBox.SelectedItem != null && txt_DBIPAddress.Text != "" && txt_DBName.Text != "" && txt_UserName.Text != "" && txt_DBIPAddress.Text != "" && txt_Password.Text != "" && txt_Password2.Text != "")
                {
                    JObject item = new JObject();
                    item["DBIPAddress"] = txt_DBIPAddress.Text;
                    item["DBPort"] = txt_DBPort.Text;
                    item["DBUserName"] = txt_UserName.Text;
                    item["DBPassword"] = MD5Helper.Md5Encrypt(txt_Password.Text);
                    item["DBName"] = txt_DBName.Text;
                    item["DBType"] = dataBaseTypeObj.dataBsseValue;

                    if (txt_Password.Text != txt_Password2.Text)
                    {
                        MessageBox.Show("两次密码不一致。", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    DataTable hasDt = dcr.DataConfigInfoQuery(txt_DBIPAddress.Text, txt_DBPort.Text);
                    if (hasDt == null || hasDt.Rows.Count == 0)
                    {
                        bool result = dcr.AddDataConfigInfo(item);
                        if (result)
                        {
                            returnAddResult(true);
                            this.Dispose();
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("该IP和端口已经添加，无需重复添加。", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("请填写所有必填内容。", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            catch (Exception ex)
            {
                //日志处理
                lr.AddLogInfo(ex.StackTrace.ToString(), "捕获异常信息", "捕获异常信息", "Error");
            }
        }

        private void cancleDataConfigBtn_Click(object sender, EventArgs e)
        {
            try
            {
                this.Dispose();
                this.Close();
            }
            catch (Exception ex)
            {
                //日志处理
                Loghelper.WriteErrorLog("捕获异常信息", ex);
                lr.AddLogInfo(ex.ToString(), "捕获异常信息", "捕获异常信息", "Error");
            }
        }

        private void FrDataConfigAdd_Load(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, string> dir = DBType.GetDBTypes();
                foreach (var item in dir)
                {
                    DataBaseType dataBaseType = new DataBaseType();
                    dataBaseType.dataBaseName = item.Key;
                    dataBaseType.dataBsseValue = item.Value;
                    dBTypeComboBox.Items.Add(dataBaseType);
                    dBTypeComboBox.DisplayMember = "dataBaseName";
                }
            }
            catch (Exception ex)
            {
                //日志处理
                Loghelper.WriteErrorLog("捕获异常信息", ex);
                lr.AddLogInfo(ex.ToString(), "捕获异常信息", "捕获异常信息", "Error");
            }
        }

    }
}
