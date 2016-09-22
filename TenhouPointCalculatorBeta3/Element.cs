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
        public static Player OppositePlayer = new Player()
        {
            Btn = Resource.Id.btnOppositePlayer,
            Ckb = Resource.Id.checkBoxOppositePlayer,
            Point = 23000,
            Name = NameEnum.�Լ�,
            IsReach = false
        };

        public static Player LeftPlayer = new Player()
        {
            Btn = Resource.Id.btnLeftPlayer,
            Ckb = Resource.Id.checkBoxLeftPlayer,
            Point = 24000,
            Name = NameEnum.�ϼ�,
            IsReach = false
        };

        public static Player RightPlayer = new Player()
        {
            Btn = Resource.Id.btnRightPlayer,
            Ckb = Resource.Id.checkBoxRightPlayer,
            Point = 25000,
            Name = NameEnum.�¼�,
            IsReach = false
        };

        public static Player MePlayer = new Player()
        {
            Btn = Resource.Id.btnMePlayer,
            Ckb = Resource.Id.checkBoxMePlayer,
            Point = 26000,
            Name = NameEnum.�Լ�,
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