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
        public static Player OppositePlayer = new Player() { Btn = Resource.Id.btnOppositePlayer, Ckb = Resource.Id.checkBoxOppositePlayer, Point = 23000, Name = NameEnum.对家, IsReach = false };
        public static Player LeftPlayer = new Player() { Btn = Resource.Id.btnLeftPlayer, Ckb = Resource.Id.checkBoxLeftPlayer, Point = 24000, Name = NameEnum.上家, IsReach = false };
        public static Player RightPlayer = new Player() { Btn = Resource.Id.btnRightPlayer, Ckb = Resource.Id.checkBoxRightPlayer, Point = 25000, Name = NameEnum.下家, IsReach = false };
        public static Player MePlayer = new Player() { Btn = Resource.Id.btnMePlayer, Ckb = Resource.Id.checkBoxMePlayer, Point = 26000, Name = NameEnum.自己, IsReach = false };

        public static Session Session = new Session();

        public static string[] ProcesStrings = new string[20];
        public static List<ArrayList> SavingArrayLists = new List<ArrayList>();
        public static Dictionary<int, ArrayList> GameLogDictionary = new Dictionary<int, ArrayList>();

        public static List<Player> Players = new List<Player>()
        {
            LeftPlayer,
            OppositePlayer,
            RightPlayer,
            MePlayer
        };

        //所有符数4翻以下的子家荣和点数
        public static Dictionary<string, string> KoAgareDictionary = new Dictionary<string, string>()
        {
            {"30//1","1000"},
            {"30//2","2000"},
            {"30//3","3900"},
            {"30//4","7700"},
            {"40//1","1300"},
            {"40//2","2600"},
            {"40//3","5200"},
            {"40//4","8000"},
            {"50//1","1600"},
            {"50//2","3200"},
            {"50//3","6400"},
            {"50//4","8000"},
            {"60//1","2000"},
            {"60//2","3900"},
            {"60//3","7700"},
            {"60//4","8000"},
            {"70//1","2300"},
            {"70//2","4500"},
            {"70//3","8000"},
            {"70//4","8000"},
            {"80//1","2600"},
            {"80//2","5200"},
            {"80//3","8000"},
            {"80//4","8000"},
            {"90//1","2900"},
            {"90//2","5800"},
            {"90//3","8000"},
            {"90//4","8000"},
            {"100//1","3200"},
            {"100//2","6400"},
            {"100//3","8000"},
            {"100//4","8000"},
            {"110//1","3600"},
            {"110//2","7100"},
            {"110//3","8000"},
            {"110//4","8000"},
        };

        //所有符数4翻以下的子家自摸点数
        public static Dictionary<string, string> KoTsumoDictionary = new Dictionary<string, string>()
        {
            {"30//1","1100"},
            {"30//2","2000"},
            {"30//3","4000"},
            {"30//4","7900"},
            {"40//1","1500"},
            {"40//2","2700"},
            {"40//3","5200"},
            {"40//4","8000"},
            {"50//1","1600"},
            {"50//2","3200"},
            {"50//3","6400"},
            {"50//4","8000"},
            {"60//1","2000"},
            {"60//2","4000"},
            {"60//3","7900"},
            {"60//4","8000"},
            {"70//1","2400"},
            {"70//2","4700"},
            {"70//3","8000"},
            {"70//4","8000"},
            {"80//1","2700"},
            {"80//2","5200"},
            {"80//3","8000"},
            {"80//4","8000"},
            {"90//1","3100"},
            {"90//2","5900"},
            {"90//3","8000"},
            {"90//4","8000"},
            {"100//1","3200"},
            {"100//2","6400"},
            {"100//3","8000"},
            {"100//4","8000"},
            {"110//2","7200"},
            {"110//3","8000"},
            {"110//4","8000"},
        };

        //所有符数4翻以下的亲家荣和点数
        public static Dictionary<string, string> OyaAgareDictionary = new Dictionary<string, string>()
        {
            {"30//1","1500"},
            {"30//2","2900"},
            {"30//3","5800"},
            {"30//4","11600"},
            {"40//1","2000"},
            {"40//2","3900"},
            {"40//3","7700"},
            {"40//4","12000"},
            {"50//1","2400"},
            {"50//2","4800"},
            {"50//3","9600"},
            {"50//4","12000"},
            {"60//1","2900"},
            {"60//2","5800"},
            {"60//3","11600"},
            {"60//4","12000"},
            {"70//1","3400"},
            {"70//2","6800"},
            {"70//3","12000"},
            {"70//4","12000"},
            {"80//1","3900"},
            {"80//2","7700"},
            {"80//3","12000"},
            {"80//4","12000"},
            {"90//1","4400"},
            {"90//2","8700"},
            {"90//3","12000"},
            {"90//4","12000"},
            {"100//1","4800"},
            {"100//2","9600"},
            {"100//3","12000"},
            {"100//4","12000"},
            {"110//1","5300"},
            {"110//2","10600"},
            {"110//3","12000"},
            {"110//4","12000"},
        };

        //所有符数4翻以下的亲家自摸点数
        public static Dictionary<string, string> OyaTsumoDictionary = new Dictionary<string, string>()
        {
            {"30//1","1500"},
            {"30//2","3000"},
            {"30//3","6000"},
            {"30//4","11700"},
            {"40//1","2100"},
            {"40//2","3900"},
            {"40//3","7800"},
            {"40//4","12000"},
            {"50//1","2400"},
            {"50//2","4800"},
            {"50//3","9600"},
            {"50//4","12000"},
            {"60//1","3000"},
            {"60//2","6000"},
            {"60//3","11700"},
            {"60//4","12000"},
            {"70//1","3600"},
            {"70//2","6900"},
            {"70//3","12000"},
            {"70//4","12000"},
            {"80//1","3900"},
            {"80//2","7800"},
            {"80//3","12000"},
            {"80//4","12000"},
            {"90//1","4500"},
            {"90//2","8700"},
            {"90//3","12000"},
            {"90//4","12000"},
            {"100//1","4800"},
            {"100//2","9600"},
            {"100//3","12000"},
            {"100//4","12000"},
            {"110//2","10800"},
            {"110//3","12000"},
            {"110//4","12000"},
        };

        //子家自摸时亲减少的点数
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
            {"2400",1200},//70符1翻
            {"4700",2300},//70符2翻
            {"3100",1500},//90符1翻
            {"5900",2900},//90符2翻
            {"7200",3600}//110符2翻
        };

        //子家自摸时子减少的点数
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