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
        public static Player OppositePlayer = new Player() { Btn = Resource.Id.btnOppositePlayer, Ckb = Resource.Id.checkBoxOppositePlayer, Point = 23000, Name = NameEnum.�Լ�, IsReach = false };
        public static Player LeftPlayer = new Player() { Btn = Resource.Id.btnLeftPlayer, Ckb = Resource.Id.checkBoxLeftPlayer, Point = 24000, Name = NameEnum.�ϼ�, IsReach = false };
        public static Player RightPlayer = new Player() { Btn = Resource.Id.btnRightPlayer, Ckb = Resource.Id.checkBoxRightPlayer, Point = 25000, Name = NameEnum.�¼�, IsReach = false };
        public static Player MePlayer = new Player() { Btn = Resource.Id.btnMePlayer, Ckb = Resource.Id.checkBoxMePlayer, Point = 26000, Name = NameEnum.�Լ�, IsReach = false };

        public static Session Session = new Session();

        public static string[] ProcesStrings = new string[20];
        public static List<ArrayList> SavingArrayLists=new List<ArrayList>();
        public static Dictionary<int,ArrayList> GameLogDictionary=new Dictionary<int, ArrayList>() ;

        public static List<Player> Players = new List<Player>()
        {
            LeftPlayer,
            OppositePlayer,
            RightPlayer,
            MePlayer
        };

        public static Dictionary<string, int> OyaDictionary = new Dictionary<string, int>()
        {
            {"1100",500},
            {"2000",1000},
            {"4000",2000},
            {"7900",3900},
            {"1500",700},
            {"2700",1300},
            {"5200",2600},
            {"1600",800},
            {"3200",1600},
            {"6400",3200},
            {"8000",4000},
            {"12000",6000},
            {"16000",8000},
            {"24000",12000},
            {"32000",16000},
            {"64000",32000},
            {"2400",1200},//70��1��
            {"4700",2300},//70��2��
            {"3100",1500},//90��1��
            {"5900",2900},//90��2��
            {"7200",3600}//110��2��
        };
        public static Dictionary<string, int> KoDictionary = new Dictionary<string, int>()
        {
            {"1100",300},
            {"2000",500},
            {"4000",1000},
            {"7900",2000},
            {"1500",400},
            {"2700",700},
            {"5200",1300},
            {"1600",400},
            {"3200",800},
            {"6400",1600},
            {"8000",2000},
            {"12000",3000},
            {"16000",4000},
            {"24000",6000},
            {"32000",8000},
            {"64000",16000},
            {"2400",600},
            {"4700",1200},
            {"3100",800},
            {"5900",1500},
            {"7200",1800}
        };
    }
}