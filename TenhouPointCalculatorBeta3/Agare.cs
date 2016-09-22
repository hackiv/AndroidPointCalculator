using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TenhouPointCalculatorBeta3
{
    class Agare
    {
        int _flagUp;
        int _flagDown;
        private int _temp;
        private bool _afterDoubleRon;

        public void GetFlag()
        {
            //定义变量
            int changBang = Element.Session.BenChang;
            int qianBang = Element.Session.QianBang;
            bool isOyaAgare = false;
            bool isOyaAgareFirst = false;//双响时第一遍是否为亲和牌
            MainActivity.Flag = 0;

            Thread th = new Thread(() =>
            {
                //第一遍和牌运算不执行场棒++和风位变换，记录是否是亲和牌，第二遍和牌运算时场棒=0，执行完场棒恢复正常值

                DoubleRon:
                //获得铳家与和家
                UpdateText.Set(MainActivity.ControlTextView, "谁出铳？");
                while (MainActivity.Flag == 0) ;
                _flagDown = MainActivity.Flag;
                UpdateText.Set(MainActivity.ControlTextView, "谁和牌？");
                MainActivity.Flag = 0;
                while (MainActivity.Flag == 0) ;
                UpdateText.Set(MainActivity.ControlTextView, "(OvO)");
                string txt = MainActivity.InpuTextView.Text;
                _flagUp = MainActivity.Flag;
                isOyaAgare = false;
                //胡牌计算
                if (_flagUp == _flagDown)
                    TsumoMethod(txt, qianBang, changBang, ref isOyaAgare);
                else
                    AgareMethod(txt, qianBang, changBang, ref isOyaAgare);
                //双响处理
                if (MainActivity.DoubleRonCheckBox.Checked)//判断是否双响
                {
                    MainActivity.DoubleRonCheckBox.Checked = false;
                    _afterDoubleRon = true;
                    isOyaAgareFirst = isOyaAgare;
                    _temp = changBang;
                    changBang = 0;
                    qianBang = 0;
                    goto DoubleRon;
                }
                if (_afterDoubleRon)//双响时执行的额外操作
                {
                    changBang = _temp;
                    _afterDoubleRon = false;
                }
                //胡牌完后处理
                MainActivity.IsOyaAgare = isOyaAgare || isOyaAgareFirst;

                Element.Session.NagareMode = true;//收掉棒子时为流局状态
                if (isOyaAgare || isOyaAgareFirst)
                {
                    Element.Session.BenChang++;
                    foreach (var player in Element.Players)
                    {
                        player.IsReach = false;
                    }
                }
                else
                {
                    Element.Session.BenChang = 0;
                    Element.Session.NowSession++;
                    foreach (var player in Element.Players)
                    {
                        if (player.Wind == 0)
                            player.Wind = WindEnum.北;
                        else
                            player.Wind--;
                        player.IsReach = false;
                    }
                }
                Element.Session.NagareMode = false;

                Element.Session.QianBang = 0;
                UpdateText.Set(MainActivity.InpuTextView, "");
                MainActivity.NowSessionNum++;
                Game.Save();
                End.IsOwari();
                MainActivity.RunningOtherProgram = false;
            });
            th.IsBackground = true;
            th.Start();
        }

        private void IsSpecialInput(ref string txt, Dictionary<String, String> dic)
        {
            string newtxt = txt;
            if (dic.ContainsKey(txt))
            {
                txt = dic[txt];
                UpdateText.Set(MainActivity.ControlTextView, "符翻点数为" + dic[newtxt]);
            }
        }

        private void AgareMethod(string txt, int qianBang, int changBang, ref bool isOyaAgare)
        {
            Player playerUp = Element.Players[_flagUp - 1];
            Player playerDown = Element.Players[_flagDown - 1];
            if (playerUp.Name == Element.Session.OyaName) isOyaAgare = true;
            if (isOyaAgare)
                IsSpecialInput(ref txt, Dictionaries.OyaAgareDictionary);
            else
                IsSpecialInput(ref txt, Dictionaries.KoAgareDictionary);

            int changePoint = 0;
            try
            {
                IsSpecialInput(ref txt, Dictionaries.KoTsumoDictionary);
                changePoint = Convert.ToInt32(txt);
            }
            catch
            {
                ThrowInputPointError();
            }
            playerUp.Point += changePoint + 300 * changBang + 1000 * qianBang;
            playerDown.Point -= changePoint + 300 * changBang;
            _flagUp = 0;
            _flagDown = 0;
            MainActivity.Flag = 0;
        }

        private void TsumoMethod(string txt, int qianBang, int changBang, ref bool isOyaAgare)
        {
            int oyaLostPoint = 0;
            int koLostPoint = 0;
            int totalChangePoint = 0;
            List<Player> playerDownKoList = new List<Player>();//自摸时子家集合
            Player playerUp = Element.Players[_flagUp - 1];//胡牌方
            Player playerDownOya = new Player();//子自摸时的亲

            #region 获取四家player
            if (playerUp.Name == Element.Session.OyaName)//亲自摸
            {
                foreach (var player in Element.Players)
                {
                    if (player.Name != playerUp.Name)
                        playerDownKoList.Add(player);
                }
                playerDownOya = null;
                isOyaAgare = true;

                #region 亲自摸时分配对应变动点数
                IsSpecialInput(ref txt, Dictionaries.OyaTsumoDictionary);
                totalChangePoint = Convert.ToInt32(txt);
                koLostPoint = totalChangePoint / 3;
                #endregion
            }
            else//子自摸
            {
                foreach (var player in Element.Players)
                {
                    if (player != playerUp && player.Name != Element.Session.OyaName)
                        playerDownKoList.Add(player);
                    else if (player != playerUp)
                        playerDownOya = player;
                }

                #region 子自摸时分配对应变动点数
                IsSpecialInput(ref txt, Dictionaries.KoTsumoDictionary);
                if (Dictionaries.OyaDictionary.ContainsKey(txt))//输入点数格式为 "7900"
                {
                    oyaLostPoint = Dictionaries.OyaDictionary[txt];
                    koLostPoint = Dictionaries.KoDictionary[txt];
                    totalChangePoint = Convert.ToInt32(txt);
                }
                else//输入点数形式为 "2000/3900" 或者 "3900/2000"
                {
                    try
                    {
                        var txtStrings = txt.Split(new char[] { '/' }, options: StringSplitOptions.RemoveEmptyEntries);
                        int point1 = Convert.ToInt32(txtStrings[0]);
                        int point2 = Convert.ToInt32(txtStrings[1]);
                        oyaLostPoint = point1 > point2 ? point1 : point2;
                        koLostPoint = oyaLostPoint == point1 ? point2 : point1;
                        totalChangePoint = oyaLostPoint + 2 * koLostPoint;
                    }
                    catch
                    {
                        ThrowInputPointError();
                    }
                }
                #endregion
            }
            #endregion

            #region 点数转移
            playerUp.Point += totalChangePoint + 300 * changBang + 1000 * qianBang;
            if (playerDownOya != null)
                playerDownOya.Point -= oyaLostPoint + 100 * changBang;
            foreach (var player in playerDownKoList)
            {
                player.Point -= koLostPoint + 100 * changBang;
            }
            #endregion
        }

        private void ThrowInputPointError()
        {
            MessageBox.Show("点数输入出错\n请重新输入");
            UpdateText.Set(MainActivity.InpuTextView,"");
            GetFlag();
        }
    }
}