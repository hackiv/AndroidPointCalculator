using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TenhouPointCalculatorBeta3
{
    class End
    {
        public static void IsOwari()
        {
            Player highestplayer = Element.Players.OrderByDescending(p => p.Point).ThenBy(p => p.OriginalWind).FirstOrDefault();
            Player lowestPlayer= Element.Players.OrderBy(p => p.Point).FirstOrDefault();
            if(lowestPlayer != null && lowestPlayer.Point<0)
                Owari();
            if (highestplayer?.Point > 30000 && (int)Element.Session.NowSession > 8)
            {
                if (MainActivity.IsOyaAgare)
                {
                    //亲家胡牌
                    if (highestplayer.Name == Element.Session.OyaName)
                        Owari();
                }
                else
                {
                    Owari();
                }
            }
        }

        public static void Owari()
        {
            string result = "";
            string gameLog = "";
            string totalLog = "";
            foreach (var player in Element.Players.OrderByDescending(p => p.Point).ThenBy(p => p.OriginalWind))
            {
                result += player.Name + ":" + player.Point + "\n";
            }
            foreach (var d in Element.GameLogDictionary)
            {
                gameLog += d.Value[5] + "\n";
            }
            totalLog = gameLog + result;
            MessageBox.Show(totalLog+ Application.Context.FilesDir.ToString());
            CreatGameLogFile(totalLog);
        }

        public static void CreatGameLogFile(string totalLog)
        {
            //创建一个文本文件,最好先判断一下 
            StreamWriter sw;
            string path = Android.OS.Environment.ExternalStorageDirectory.ToString();
            if (!File.Exists(path+"/GameLog.txt"))
            {
                //不存在就新建一个文本文件,并写入一些内容 
                sw = File.CreateText(path + "/GameLog.txt");
                sw.WriteLine("当前日期是:"+ DateTime.Now);
                sw.Write(totalLog);
            }
            else
            {
                //如果存在就添加一些文本内容 
                sw = File.AppendText(path + "/GameLog.txt");
                sw.WriteLine("\n");
                sw.WriteLine("--------------------");
                sw.WriteLine("\n");
                sw.WriteLine("当前日期是:" + DateTime.Now);
                sw.Write(totalLog);
            }
            sw.Close();
        }
    }
}