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
    class Game
    {
        private static Activity _activity = (Activity)MainActivity.Context;
        public static void Save()
        {
            
            System.Collections.ArrayList gameArrayList = new System.Collections.ArrayList { Element.LeftPlayer.ShallowClone(), Element.OppositePlayer.ShallowClone(), Element.RightPlayer.ShallowClone(), Element.MePlayer.ShallowClone(), Element.Session.ShallowClone() };
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
                    Element.Session.BenChang = cloneSession.BenChang;
                    Element.Session.QianBang = cloneSession.QianBang;
                    Element.Session.NowSession = cloneSession.NowSession;
                    MainActivity.NowSessionNum = targetSession;
                    
                }
            }
            catch
            {
            }
        }
    }
}