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
        public static void IsEndWest(Activity activity,AlertDialog.Builder adb)
        {
            foreach (var player in Element.Players)
            {
                if (player.Point >= 30000)
                {
                    activity?.RunOnUiThread(() =>
                    {
                        adb.SetMessage("Íê³¡\n" + PlayerPointSort.Sort());
                        adb.Show();
                    });
                }
            }
        }
    }
}