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
using Java.Util;
using ArrayList = System.Collections.ArrayList;

namespace TenhouPointCalculatorBeta3
{
    public enum WindEnum
    {
        东,
        南,
        西,
        北
    }

    public enum NameEnum
    {
        上家,
        下家,
        对家,
        自己
    }

    public enum SessionEnum
    {
        东一局 = 1,
        东二局,
        东三局,
        东四局,
        南一局,
        南二局,
        南三局,
        南四局,
        西一局,
        西二局,
        西三局,
        西四局,
    }

    static class Element
    {
        public static Player OppositePlayer = new Player()
        {
            Btn = Resource.Id.btnOppositePlayer,
            Ckb = Resource.Id.checkBoxOppositePlayer,
            Point = 23000,
            Name = NameEnum.对家,
            IsReach = false
        };

        public static Player LeftPlayer = new Player()
        {
            Btn = Resource.Id.btnLeftPlayer,
            Ckb = Resource.Id.checkBoxLeftPlayer,
            Point = 24000,
            Name = NameEnum.上家,
            IsReach = false
        };

        public static Player RightPlayer = new Player()
        {
            Btn = Resource.Id.btnRightPlayer,
            Ckb = Resource.Id.checkBoxRightPlayer,
            Point = 25000,
            Name = NameEnum.下家,
            IsReach = false
        };

        public static Player MePlayer = new Player()
        {
            Btn = Resource.Id.btnMePlayer,
            Ckb = Resource.Id.checkBoxMePlayer,
            Point = 26000,
            Name = NameEnum.自己,
            IsReach = false
        };

        public static Session Session = new Session();

        public static Dictionary<int, ArrayList> GameLogDictionary = new Dictionary<int, ArrayList>();

        public static List<Player> Players = new List<Player>()
        {
            LeftPlayer,
            OppositePlayer,
            RightPlayer,
            MePlayer
        };
    }

}