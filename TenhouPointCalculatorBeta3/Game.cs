using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;

namespace TenhouPointCalculatorBeta3
{
    internal static class Game
    {
        public static void Save(string situation)
        {

            System.Collections.ArrayList gameArrayList = new System.Collections.ArrayList
            {
                Element.LeftPlayer.ShallowClone(),
                Element.OppositePlayer.ShallowClone(),
                Element.RightPlayer.ShallowClone(),
                Element.MePlayer.ShallowClone(),
                Element.Session.ShallowClone(),
                situation
            };
            int i = MainActivity.NowSessionNum;
            while (true)//清除掉已有的记录
            {
                if (Element.GameLogDictionary.ContainsKey(i))
                {
                    Element.GameLogDictionary.Remove(i);
                    i++;
                }
                else
                    break;
            }

            Element.GameLogDictionary.Add(MainActivity.NowSessionNum, gameArrayList);

            //查看总分 debug用
            UpdateText.Set(MainActivity.Test, showTotalPoint());
        }

        public static void Load(int targetSession)
        {
            try
            {
                if (Element.GameLogDictionary.ContainsKey(targetSession))
                {
                    var cloneArrayList = Element.GameLogDictionary[targetSession];
                    int i = 0;
                    foreach (var player in Element.Players)
                    {
                        var clonePlayer = cloneArrayList[i] as Player;
                        if (clonePlayer != null)
                        {
                            player.Point = clonePlayer.Point;
                            player.Wind = clonePlayer.Wind;
                        }
                        i++;
                    }
                    Session cloneSession = cloneArrayList[i] as Session;
                    if (cloneSession != null)
                    {
                        Element.Session.BenChang = cloneSession.BenChang;
                        Element.Session.QianBang = cloneSession.QianBang;
                        Element.Session.NowSession = cloneSession.NowSession;
                    }
                    MainActivity.NowSessionNum = targetSession;

                }
            }
            catch
            {
                // ignored
            }
        }
        private static string showTotalPoint()
        {
            int sum = 0;
            foreach (var p in Element.Players)
            {
                sum += p.Point;
            }
            return (sum + Element.Session.QianBang * 1000).ToString();
        }
    }
}