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
                    //�׼Һ���
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
            //����һ���ı��ļ�,������ж�һ�� 
            StreamWriter sw;
            string path = Android.OS.Environment.ExternalStorageDirectory.ToString();
            if (!File.Exists(path+"/GameLog.txt"))
            {
                //�����ھ��½�һ���ı��ļ�,��д��һЩ���� 
                sw = File.CreateText(path + "/GameLog.txt");
                sw.WriteLine("��ǰ������:"+ DateTime.Now);
                sw.Write(totalLog);
            }
            else
            {
                //������ھ����һЩ�ı����� 
                sw = File.AppendText(path + "/GameLog.txt");
                sw.WriteLine("\n");
                sw.WriteLine("--------------------");
                sw.WriteLine("\n");
                sw.WriteLine("��ǰ������:" + DateTime.Now);
                sw.Write(totalLog);
            }
            sw.Close();
        }
    }
}