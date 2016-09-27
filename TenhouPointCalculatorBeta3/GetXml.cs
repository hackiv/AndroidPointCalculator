using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Xml;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TenhouPointCalculatorBeta3;

namespace TenhouPointCalculatorBeta3
{
    static class GetXml
    {
        public static void GetPoint()
        {
            XmlDocument xd = new XmlDocument();
            XmlReader xmlfile = MainActivity.Context.Resources.GetXml(Resource.Xml.database);
            xd.Load(xmlfile);
            XmlNodeList node = xd.SelectNodes("/resources/record");
            if (node != null)
            {
                foreach (XmlNode data in node)
                {
                    if (data.Attributes != null)
                    {
                        var fu = Convert.ToInt32(data.SelectSingleNode("Fu")?.InnerText);
                        var fan = Convert.ToInt32(data.SelectSingleNode("Fan")?.InnerText);
                        var koAgareTotalPoint = Convert.ToInt32(data.SelectSingleNode("KoAgareTotalPoint")?.InnerText);
                        var oyaAgareTotalPoint = Convert.ToInt32(data.SelectSingleNode("OyaAgareTotalPoint")?.InnerText);
                        var oyaTsumoTotalPoint = Convert.ToInt32(data.SelectSingleNode("OyaTsumoTotalPoint")?.InnerText);
                        var koTsumoTotalPoint = Convert.ToInt32(data.SelectSingleNode("KoTsumoTotalPoint")?.InnerText);
                        var koTsumoLostPoint = Convert.ToInt32(data.SelectSingleNode("KoTsumoLostPoint")?.InnerText);
                        var oyaTsumoLostPoint = Convert.ToInt32(data.SelectSingleNode("OyaTsumoLostPoint")?.InnerText);

                        FuFanPoint point = new FuFanPoint(fu, fan, koAgareTotalPoint, oyaAgareTotalPoint, oyaTsumoTotalPoint, koTsumoTotalPoint, koTsumoLostPoint, oyaTsumoLostPoint);

                        Element.FuFanPoints.Add(point);
                    }
                }
            }
        }

    }
}