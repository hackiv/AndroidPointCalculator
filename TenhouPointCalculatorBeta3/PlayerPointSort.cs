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
    class PlayerPointSort
    {
        static readonly PlayerComparerByPoint Comparer = new PlayerComparerByPoint();
        public static string Sort()
        {
            Element.Players.Sort(Comparer);
            string sortString="";
            foreach (var player in Element.Players)
            {
                sortString += player.Name + ":" + player.Point+"\n";
            }
            return sortString;
        }
    }
}