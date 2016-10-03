using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TenhouPointCalculatorBeta3
{
    internal static class Setting
    {
        public static void SettingElement(string txt)
        {
            var txtStrings = txt.Split('/');
            int i = 0;
            try
            {
                foreach (var player in Element.Players)
                {
                    i++;
                    if (txtStrings[i - 1] == "") continue;
                    player.Point = Convert.ToInt32(txtStrings[i - 1]);
                }

                if (txtStrings[i] != "")
                    Element.Session.BenChang = Convert.ToInt32(txtStrings[i]);

                if (txtStrings[i + 1] != "")
                    Element.Session.QianBang = Convert.ToInt32(txtStrings[i + 1]);

                if (txtStrings[i + 2] != "")
                    Element.Session.NowSession = (SessionEnum)Convert.ToInt32(txtStrings[i + 2]);
                //设定玩家风位
                int j = (Convert.ToInt32(txtStrings[i + 2]) - 1) % 4;
                foreach (var player in Element.Players)
                {
                    if ((int)player.OriginalWind - j >= 0)
                        player.Wind = player.OriginalWind - j;
                    else
                        player.Wind = player.OriginalWind - j + 4;
                }
                Game.Save("中途设定");

            }
            catch
            {
                // ignored
            }
        }
    }
}