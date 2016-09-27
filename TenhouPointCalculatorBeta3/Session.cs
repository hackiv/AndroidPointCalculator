using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TenhouPointCalculatorBeta3
{
    class Session:ICloneable
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

        public Session(int benChang, int qianBang, SessionEnum session, NameEnum oyaNameEnum)
        {
            BenChang = benChang;
            QianBang = qianBang;
            NowSession = session;
            OyaName = oyaNameEnum;
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