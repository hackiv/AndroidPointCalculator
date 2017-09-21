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
        public static readonly Player OppositePlayer = new Player()
        {
            Btn = Resource.Id.btnOppositePlayer,
            Ckb = Resource.Id.checkBoxOppositePlayer,
            Name = NameEnum.对家,
            RealName = "对家"
        };

        public static readonly Player LeftPlayer = new Player()
        {
            Btn = Resource.Id.btnLeftPlayer,
            Ckb = Resource.Id.checkBoxLeftPlayer,
            Name = NameEnum.上家,
            RealName = "上家"
        };

        public static readonly Player RightPlayer = new Player()
        {
            Btn = Resource.Id.btnRightPlayer,
            Ckb = Resource.Id.checkBoxRightPlayer,
            Name = NameEnum.下家,
            RealName = "下家"
        };

        public static readonly Player MePlayer = new Player()
        {
            Btn = Resource.Id.btnMePlayer,
            Ckb = Resource.Id.checkBoxMePlayer,
            Name = NameEnum.自己,
            RealName = "自己"
        };

        public static Session Session = new Session();

        public static Dictionary<int, ArrayList> GameLogDictionary;

        public static List<FuFanPoint> FuFanPoints = new List<FuFanPoint>();

        public static readonly List<Player> Players = new List<Player>()
        {
            LeftPlayer,
            OppositePlayer,
            RightPlayer,
            MePlayer
        };
    }

}