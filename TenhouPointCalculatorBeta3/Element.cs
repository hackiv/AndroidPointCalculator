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
        ��,
        ��,
        ��,
        ��
    }

    public enum NameEnum
    {
        �ϼ�,
        �¼�,
        �Լ�,
        �Լ�
    }

    public enum SessionEnum
    {
        ��һ�� = 1,
        ������,
        ������,
        ���ľ�,
        ��һ��,
        �϶���,
        ������,
        ���ľ�,
        ��һ��,
        ������,
        ������,
        ���ľ�,
    }

    static class Element
    {
        public static readonly Player OppositePlayer = new Player()
        {
            Btn = Resource.Id.btnOppositePlayer,
            Ckb = Resource.Id.checkBoxOppositePlayer,
            Name = NameEnum.�Լ�,
            RealName = "�Լ�"
        };

        public static readonly Player LeftPlayer = new Player()
        {
            Btn = Resource.Id.btnLeftPlayer,
            Ckb = Resource.Id.checkBoxLeftPlayer,
            Name = NameEnum.�ϼ�,
            RealName = "�ϼ�"
        };

        public static readonly Player RightPlayer = new Player()
        {
            Btn = Resource.Id.btnRightPlayer,
            Ckb = Resource.Id.checkBoxRightPlayer,
            Name = NameEnum.�¼�,
            RealName = "�¼�"
        };

        public static readonly Player MePlayer = new Player()
        {
            Btn = Resource.Id.btnMePlayer,
            Ckb = Resource.Id.checkBoxMePlayer,
            Name = NameEnum.�Լ�,
            RealName = "�Լ�"
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