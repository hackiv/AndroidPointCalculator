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
    class End
    {
        public static void IsOwari()
        {
            Player player = Element.Players.OrderByDescending(p => p.Point).ThenBy(p => p.OriginalWind).FirstOrDefault();
            if (player?.Point > 30000 && (int)Element.Session.NowSession > 7)
            {
                if (MainActivity.IsOyaAgare)
                {
                    //Ç×¼ÒºúÅÆ
                    if (player?.Name == Element.Session.OyaName)
                        Owari();
                }
                else
                {
                    Owari();
                }
                
            }
        }

        public static void Owari()
        {
            string txt = "";
            foreach (var player in Element.Players.OrderByDescending(p => p.Point).ThenBy(p => p.OriginalWind))
            {
                txt += player.Name + ":" + player.Point + "\n";
            }
            MessageBox.Show("Íê³¡\n" + txt);
        }
    }
}