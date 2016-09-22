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
                if (Convert.ToInt32(value) < 13) _nowSession = value;//不超出索引范围
                UpdateText.Set(MainActivity.SessionTextView, _nowSession.ToString());
            }
        } //当前场节 东一局到西四局

        public NameEnum OyaName { get; set; }//亲家

        public bool NagareMode { get; set; }//流局模式


        public Session(int benChang, int qianBang, SessionEnum session, NameEnum oyaNameEnum, bool nagareMode)
        {
            BenChang = benChang;
            QianBang = qianBang;
            NowSession = session;
            OyaName = oyaNameEnum;
            NagareMode = nagareMode;
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

        public Session DeepClone()
        {
            using (MemoryStream ms = new MemoryStream(1000))
            {
                BinaryFormatter bf = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
                bf.Serialize(ms, this);
                ms.Seek(0, SeekOrigin.Begin);
                // 反序列化至另一个对象(即创建了一个原对象的深表副本) 
                var cloneObject = bf.Deserialize(ms);
                // 关闭流 
                ms.Close();
                return cloneObject as Session;
            }
        }

        public Session ShallowClone()
        {
            return Clone() as Session;
        }
    }
}