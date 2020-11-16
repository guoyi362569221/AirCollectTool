using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WeatherDataSys.From_Main;

namespace WeatherDataSys.UtilTool
{
    public class Utility
    {
        public static string ConvertFieldValue(string fieldNames, string str = "'")
        {
            if (fieldNames.Contains(","))
            {
                string[] strArr = fieldNames.Split(',');
                for (int i = 0; i < strArr.Length; i++)
                {
                    strArr[i] = str + strArr[i] + str;
                }
                return string.Join(",", strArr);
            }
            else
            {
                return "'" + fieldNames + "'";
            }
        }

        public static void writeLog(string msg, ColorEnum color = ColorEnum.Green)
        {
            From_Main.rtb.Focus();
            From_Main.rtb.Select(From_Main.rtb.Text.Length, 0);
            switch (color)
            {
                case ColorEnum.Blue:
                    From_Main.rtb.SelectionColor = Color.Blue;
                    break;
                case ColorEnum.Red:
                    From_Main.rtb.SelectionColor = Color.Red;
                    break;
                case ColorEnum.Green:
                    From_Main.rtb.SelectionColor = Color.Green;
                    break;
                case ColorEnum.Black:
                    From_Main.rtb.SelectionColor = Color.Black;
                    break;
            }

            string time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            From_Main.rtb.AppendText(time + "->" + msg);
            From_Main.rtb.AppendText("\r\n");
        }
    }

    public enum ColorEnum
    {
        Black = 0,
        Blue = 1,
        Red = 2,
        Green = 3
    }


}
