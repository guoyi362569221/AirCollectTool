using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MonitorDataSys.UtilTool
{
    public class AirParseUtil
    {
        public static String URLParser(HttpClient client, String url)
        {
            //用来接收解析的数据
            String entity = "";
            //获取网站响应的html，这里调用了HTTPUtils类
            HttpResponse response = HTTPUtils.getRawHtml(client, url);
            //获取响应状态码
            int StatusCode = response.getStatusLine().getStatusCode();
            //如果状态响应码为200，则获取html实体内容或者json文件
            if (StatusCode == 200)
            {
                entity = EntityUtils.toString(response.getEntity(), "utf-8");
            }
            else
            {
                //否则，消耗掉实体
                EntityUtils.consume(response.getEntity());
            }
            return entity;
        }

        public static List<String> getAirQualityForecastSubData(String html)
        {
            //获取的数据，存放在集合中
            List<String> data = new List<string>();
            try
            {
                //采用Jsoup解析
                Document doc = Jsoup.parse(html);
                //获取html标签中的内容
                Elements e = doc.getElementsByTag("script").eq(12);
                if (null != e)
                {
                    String cityData = e.get(0).data().split("var")[3].replace("\n", "").replace("\t", "").replace("\r", "");
                    cityData = cityData.substring(cityData.indexOf("[") + 1, cityData.indexOf("]") + 1);
                    data = JSONObject.parseArray(cityData, 实体.class);
                }
}
            catch (Exception e) 
            {
                e.printStackTrace();
            }
            //返回数据
            return data;
        }

        
    public string GetHtml(string url, Encoding ed)
{
    string Html = string.Empty;//初始化新的webRequst
    HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(url);

    Request.KeepAlive = true;
    Request.ProtocolVersion = HttpVersion.Version11;
    Request.Method = "GET";
    Request.Accept = "*/* ";
    Request.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/536.5 (KHTML, like Gecko) Chrome/19.0.1084.56 Safari/536.5";
    Request.Referer = url;

    HttpWebResponse htmlResponse = (HttpWebResponse)Request.GetResponse();

    //从Internet资源返回数据流
    Stream htmlStream = htmlResponse.GetResponseStream();
    //读取数据流
    StreamReader weatherStreamReader = new StreamReader(htmlStream, ed);
    //读取数据
    Html = weatherStreamReader.ReadToEnd();
    weatherStreamReader.Close();
    htmlStream.Close();
    htmlResponse.Close();
    //针对不同的网站查看html源文件
    return Html;
}
    }
}
