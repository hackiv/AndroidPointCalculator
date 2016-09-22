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
        public static void IsOwari(Activity activity, AlertDialog.Builder adb)
        {
            Player player = Element.Players.OrderByDescending(p => p.Point).ThenBy(p => p.OriginalWind).FirstOrDefault();
            if (player?.Point > 30000 && (int)Element.Session.NowSession > 7)
            {
                if (MainActivity.IsOyaAgare)
                {
                    //Ç×¼ÒºúÅÆ
                    if (player?.Name == Element.Session.OyaName)
                        Owari(activity, adb);
                }
                else
                {
                    Owari(activity, adb);
                }
            }
        }

        public static void Owari(Activity activity, AlertDialog.Builder adb)
        {
            string txt = "";
            foreach (var player in Element.Players.OrderByDescending(p => p.Point).ThenBy(p => p.OriginalWind))
            {
                txt += player.Name + ":" + player.Point + "\n";
            }
            activity?.RunOnUiThread(() =>
            {
                adb.SetMessage("Íê³¡\n" + txt);
                adb.Show();
            });
        }
    }
}