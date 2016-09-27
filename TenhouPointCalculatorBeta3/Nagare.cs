using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TenhouPointCalculatorBeta3
{
    class Nagare
    {
        public void NagareMethod()
        {
            int count = 0;
            bool isOyaTenpai = false;
            List<Player> upPlayers = new List<Player>();
            //统计有几家听牌
            foreach (var player in Element.Players)
            {
                if (player.IsReach)
                {
                    count++;
                    upPlayers.Add(player);
                    if (player.Name == Element.Session.OyaName)
                        isOyaTenpai = true;
                }
            }
            //移交点数
            if (!(count == 0 || count == 4))
            {
                foreach (var player in Element.Players)
                {
                    if (upPlayers.Contains(player))
                        player.Point += 3000 / count;
                    else
                        player.Point -= 3000 / (4 - count);
                    player.IsReach = false;
                }
            }
            //流局完后处理
            if (!isOyaTenpai)
            {
                Element.Session.NowSession++;
                foreach (var player in Element.Players)
                {
                    if (player.Wind == 0)
                        player.Wind = WindEnum.北;
                    else
                        player.Wind--;
                }
            }
            Element.Session.BenChang++;
            MainActivity.NowSessionNum++;
            MainActivity.IsOyaAgare = isOyaTenpai;
        }
    }
}