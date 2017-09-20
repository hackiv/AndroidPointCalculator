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
    public static class PlayerCondition
    {
        public static string[] PlayersCondition = new string[4] { "对", "局", "开", "始" };
        public static void AgareJudge()
        {
            int i = 0;
            foreach (var player in Element.Players)
            {
                if (player.IsReach == true)
                {
                    if (player.IsReachLockOn == true)
                        PlayersCondition[i] = "立";
                    else
                        PlayersCondition[i] = "听";
                }
                else PlayersCondition[i] = "无";
                i++;
            }
        }
        public static void NagareJudge()
        {
            string[] before = PlayersCondition;
            AgareJudge();
            string[] now = PlayersCondition;
            string[] newCondition = new string[4];
            for (int i = 0; i < now.Length; i++)
            {
                if (now[i] != before[i])
                {
                    PlayersCondition[i] = "听";
                }
            }
        }
        public static string GetCondition()
        {
            string txt = "";
            for (int i = 0; i < PlayersCondition.Length; i++)
            {
                txt += PlayersCondition[i];
            }
            return txt;
        }
    }
}