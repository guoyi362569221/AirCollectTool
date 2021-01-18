using Com.Hzexe.Air.OpenAirLibrary;
using Env.CnemcPublish.DAL;
using Env.CnemcPublish.RiaServices;
using MonitorDataSys.Models;
using MonitorDataSys.Repository.local;
using MonitorDataSys.UtilTool;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AreaRepository = MonitorDataSys.Repository.local.AreaRepository;

namespace MonitorDataSys
{
    public partial class FrDataConfig : Form
    {
        private readonly string XAP_URL = ConfigurationManager.AppSettings["dataUrl"];
        private readonly AreaRepository ar = new AreaRepository();
        private readonly AirStationRepository asr = new AirStationRepository();
        private readonly LogRepository lr = new LogRepository();

        public FrDataConfig()
        {
            InitializeComponent();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            dataGridView1.AutoGenerateColumns = false;
        }

        private void FrDataConfig_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable proviceTable = ar.AreaInfoQuery("*", "1");
                Area areaDefail = new Area();
                areaDefail.AreaCode = "*";
                areaDefail.AreaName = "请选择";
                proviceComboBox.Items.Add(areaDefail);
                if (proviceTable != null && proviceTable.Rows.Count > 0)
                {
                    List<Dictionary<string, string>> list = Utility.DataTableToList(proviceTable);
                    for (int i = 0; i < list.Count; i++)
                    {
                        Area area = Utility.ParseDictionaryToModel<Area>(list[i]);
                        proviceComboBox.Items.Add(area);
                        proviceComboBox.DisplayMember = "AreaName";
                    }
                }
                stationQueryBtn_Click(null, null);
            }
            catch (Exception ex)
            {
                //日志处理
                Loghelper.WriteErrorLog("初始化空气质量站点页面失败", ex);
                lr.AddLogInfo(ex.ToString(), "初始化空气质量站点页面失败", "", "Error");
                //throw e;
            }
        }

        private void proviceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cityComboBox.Items.Clear();
                Area areaDefail = new Area();
                areaDefail.AreaCode = "*";
                areaDefail.AreaName = "请选择";
                cityComboBox.Items.Add(areaDefail);
                if (proviceComboBox.SelectedItem != null)
                {
                    Area areaObj = (Area)(proviceComboBox.SelectedItem);
                    DataTable cityTable = ar.AreaInfoQuery(areaObj.AreaCode.Substring(0, 2), "2");
                    if (cityTable != null && cityTable.Rows.Count > 0)
                    {
                        List<Dictionary<string, string>> list = Utility.DataTableToList(cityTable);
                        for (int i = 0; i < list.Count; i++)
                        {
                            Area area = Utility.ParseDictionaryToModel<Area>(list[i]);
                            cityComboBox.Items.Add(area);
                            cityComboBox.DisplayMember = "AreaName";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //日志处理
                Loghelper.WriteErrorLog("查询级联城市失败", ex);
                lr.AddLogInfo(ex.ToString(), "查询级联城市失败", "", "Error");
                //throw e;
            }
        }

        private void airStationLoadBtn_Click(object sender, EventArgs e)
        {
            try
            {
                airStationLoadTask();
            }
            catch (Exception ex)
            {
                //日志处理
                Loghelper.WriteErrorLog("同步空气质量站点数据失败", ex);
                lr.AddLogInfo(ex.ToString(), "同步空气质量站点数据失败", "提示", "Error");
                //throw e;
            }
        }

        public async Task airStationLoadTask()
        {
            try
            {
                load_lb.Visible = true;
                string cityCode = "*";
                string cityName = "全国";
                if (cityComboBox.SelectedItem != null)
                {
                    Area areaCityObj = (Area)(cityComboBox.SelectedItem);
                    cityCode = areaCityObj.AreaCode;
                    cityName = areaCityObj.AreaName;
                }
                if (proviceComboBox.SelectedItem != null && (cityCode == "*" || String.IsNullOrEmpty(cityCode)))
                {
                    Area areaProviceObj = (Area)(proviceComboBox.SelectedItem);
                    cityCode = areaProviceObj.AreaCode.Substring(0, 2);
                    cityName = areaProviceObj.AreaName;
                }

                //创建domain客户端
                EnvCnemcPublishDomainContext publishCtx = new EnvCnemcPublishDomainContext(XAP_URL);

                //获取所有检测站，业务上通过citycode与城市对应
                IEnumerable<StationConfig> stations = await publishCtx.Load(publishCtx.GetStationConfigsQuery()).ResultAsync();

                List<JObject> list = new List<JObject>();
                if (stations != null)
                {
                    for (int i = 0; i < stations.Count(); i++)
                    {
                        StationConfig sc = stations.ElementAt(i);

                        if (cityCode == "*" || sc.CityCode.ToString().StartsWith(cityCode))
                        {
                            JObject jo = new JObject();
                            jo["UniqueCode"] = sc.UniqueCode;
                            jo["Area"] = sc.Area;
                            jo["CityCode"] = sc.CityCode;
                            jo["StationCode"] = sc.StationCode;
                            jo["Latitude"] = sc.Latitude;
                            jo["Longitude"] = sc.Longitude;
                            jo["PositionName"] = sc.PositionName;
                            list.Add(jo);
                        }

                    }
                }
                if (list.Count > 0)
                {
                    if (asr.DeleteAirStationInfo())
                    {
                        bool result = asr.AddAirStationInfo(list);
                        load_lb.Visible = false;
                        if (result)
                        {
                            stationQueryBtn_Click(null, null);
                            MessageBox.Show(cityName + list.Count + "个空气质量站点同步成功！", " 提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show(cityName + "空气质量站点同步失败！", " 提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    load_lb.Visible = false;
                    MessageBox.Show(cityName + "没有空气质量站点要同步", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception e)
            {
                load_lb.Visible = false;
                //日志处理
                Loghelper.WriteErrorLog("同步空气质量站点数据失败", e);
                lr.AddLogInfo(e.ToString(), "同步空气质量站点数据失败", "", "Error");
                //throw e;
            }
        }

        private void stationQueryBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string cityCodeFilter = "*";
                if (cityComboBox.SelectedItem != null)
                {
                    Area areaCityObj = (Area)(cityComboBox.SelectedItem);
                    cityCodeFilter = areaCityObj.AreaCode;
                }
                if (cityCodeFilter == "*" || String.IsNullOrEmpty(cityCodeFilter))
                {
                    if (proviceComboBox.SelectedItem != null)
                    {
                        Area areaProviceObj = (Area)(proviceComboBox.SelectedItem);
                        cityCodeFilter = areaProviceObj.AreaCode.Substring(0, 2);
                    }
                }
                DataTable dt = asr.AirStationInfoQuery(cityCodeFilter);
                if (dt != null)
                {
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                load_lb.Visible = false;
                //日志处理
                Loghelper.WriteErrorLog("查询空气质量站点数据失败", ex);
                lr.AddLogInfo(ex.ToString(), "查询空气质量站点数据失败", "", "Error");
                //throw e;
            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells[1].Value = row.Index + 1;
            }
        }
    }
}
