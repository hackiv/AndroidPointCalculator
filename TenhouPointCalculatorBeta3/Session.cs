using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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
    class Session : ICloneable
    {
        private int _benChang;

        public int BenChang
        {
            get { return _benChang; }
            set
            {
                _benChang = value;
                UpdateText.Set(MainActivity.ChangBangTextView, _benChang.ToString());
            }
        } //本场数

        private int _qianBang;

        public int QianBang
        {
            get { return _qianBang; }
            set
            {
                _qianBang = value;
                UpdateText.Set(MainActivity.QianBangTextView, _qianBang.ToString());
            }
        } //供托中千棒数

        private SessionEnum _nowSession;
        public SessionEnum NowSession
        {
            get { return _nowSession; }
            set
            {
                _nowSession = Convert.ToInt32(value) < 13 ? value : _nowSession;
                UpdateText.Set(MainActivity.SessionTextView, _nowSession.ToString());
            }
        } //当前场节 东一局到西四局

        public NameEnum OyaName { get; set; }//亲家

        public bool IsAgareMode { get; set; }//胡牌模式
        public bool IsNewAgare { get; set; }//是否为新一次胡牌（双响第二次不算新一次胡牌
        public bool IsNagareMode { get; set; }//流局模式
        public bool IsNewGame { get; set; }

        private int _save;
        private int _flag;
        public int Flag
        {
            get { return _flag; }
            set
            {
                _flag = value;
                if (IsAgareMode == true)
                {
                    if (_save == 0)
                    {
                        _save = value;
                        UpdateText.Set(MainActivity.ControlTextView, "谁和牌？");
                    }
                    else
                    {
                        if (MainActivity.DoubleRonCheckBox.Checked)
                            UpdateText.Set(MainActivity.ControlTextView, "谁出铳");
                        if (IsNewAgare)
                        {
                            AgareRefactor.BenChangTemp = 0;
                            AgareRefactor.IsOyaAgareFirst = false;
                            IsNewAgare = false;
                        }
                        AgareRefactor.Method(value, _save);
                        _save = 0;
                    }
                }
                if (IsNagareMode)
                {
                    Nagare nagare = new Nagare();
                    nagare.NagareMethod();
                    IsNagareMode = false;
                    UpdateText.Set(MainActivity.ControlTextView, "(OvO)");
                    Game.Save("流局");
                    End.IsOwari();
                    MainActivity.RunningOtherProgram = false;
                }
                if (IsNewGame)
                {
                    int a = value;
                    MessageBox.Show(Element.Players[a - 1].Name + "东起");
                    UpdateText.Set(MainActivity.ControlTextView, "(OvO)");
                    Element.Session = new Session(0, 0, SessionEnum.东一局, (NameEnum)a - 1, 0) { IsNagareMode = false };
                    for (int i = 0; i < 4; i++)
                    {
                        Element.Players[a - i - 1].Point = 25000;
                        Element.Players[a - i - 1].IsReach = false;
                        Element.Players[a - i - 1].Wind = (WindEnum)i;
                        Element.Players[a - i - 1].OriginalWind = (WindEnum)i;
                        if (a - i - 2 < 0) a += 4;
                    }
                    MainActivity.NowSessionNum = 0;
                    Game.Save("对局开始：");
                    MainActivity.RunningOtherProgram = false;
                    IsNewGame = false;
                }
            }
        }

        public Session(int benChang, int qianBang, SessionEnum session, NameEnum oyaNameEnum, int flag)
        {
            BenChang = benChang;
            QianBang = qianBang;
            NowSession = session;
            OyaName = oyaNameEnum;
            Flag = flag;
        }

        public Session() { }

        public override string ToString()
        {
            return NowSession.ToString();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public Session ShallowClone()
        {
            return Clone() as Session;
        }
    }
}