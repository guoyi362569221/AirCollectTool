using MonitorDataSys.Repository.local;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonitorDataSys
{
    public partial class FrLookLog : Form
    {
        public FrLookLog()
        {
            InitializeComponent();
            dataGridView_Log.AutoGenerateColumns = false;
        }

        private void logQueryBtn_Click(object sender, EventArgs e)
        {
            logInfoQuery();
        }

        private void logInfoQuery() 
        {
            try
            {
                DateTime startTime = this.dt_StartTime.Value;
                DateTime endTime = this.dt_EndTime.Value;

                LogRepository lr = new LogRepository();
                DataTable dt = lr.LogInfoQuery(startTime.ToString("yyyy-MM-dd 00:00:00.000"), endTime.ToString("yyyy-MM-dd 23:59:59.999"));
                if (dt != null)
                {
                    dataGridView_Log.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void FrLookLog_Load(object sender, EventArgs e)
        {
            logInfoQuery();
        }
    }
}
