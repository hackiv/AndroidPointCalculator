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
        static readonly PlayerComparerByPoint Comparer = new PlayerComparerByPoint();

        public static string Sort()
        {
            List<Player> clonePlayers = new List<Player>
            {
                Element.LeftPlayer.ShallowClone(),
                Element.OppositePlayer.ShallowClone(),
                Element.RightPlayer.ShallowClone(),
                Element.MePlayer.ShallowClone()
            };
            clonePlayers.Sort(Comparer);
            string sortString = "";
            foreach (var player in clonePlayers)
            {
                sortString += player.Name + ":" + player.Point + "\n";
            }
            return sortString;
        }

        //完场条件：南四开始 亲和牌？→亲点数最高&&亲点数>3w？
        //                           →某家点数>3w?
        public static void IsEndWest(Activity activity, AlertDialog.Builder adb)
        {
            foreach (var player in Element.Players)
            {
                if (player.Point >= 30000)
                {
                    activity?.RunOnUiThread(() =>
                    {
                        adb.SetMessage("完场\n" + Sort());
                        adb.Show();
                    });
                }
            }
        }
    }
}