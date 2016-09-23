using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
    class Player : ICloneable
    {
        private readonly Activity _activity = MainActivity.Context as Activity;
        public int Btn { get; set; } //对应的按钮ID

        public int Ckb { private get; set; } //对应的选项框

        private int _point;

        public int Point //玩家点数
        {
            get
            {
                try
                {
                    return _point;
                }
                catch
                {
                    MessageBox.Show("获取点数出错啦");
                    return _point;
                }
            }
            set
            {
                try
                {
                    _point = value;

                    _activity?.RunOnUiThread(
                        () => _activity.FindViewById<Button>(Btn).Text = ToString());
                    if (value < 0)
                    {
                        End.Owari();
                    }
                }
                catch
                {
                    MessageBox.Show("设置点数出错啦");
                }
            }
        }

        public NameEnum Name { get; set; } //玩家名称，上家left 下家right 自己me 对家opposite

        public WindEnum OriginalWind { get; set; } //最开始时的风位

        private WindEnum _wind;

        public WindEnum Wind //玩家风位
        {
            get
            {
                try
                {
                    return _wind;
                }
                catch
                {
                    MessageBox.Show("获取风位出错啦");
                    return WindEnum.东;
                }
            }
            set
            {
                try
                {
                    _wind = value;
                    if (value == WindEnum.东) Element.Session.OyaName = Name;
                    _activity?.RunOnUiThread(
                        () => _activity.FindViewById<Button>(Btn).Text = ToString());
                }
                catch
                {
                    MessageBox.Show("设置风位出错啦");
                }
            }
        }

        private bool _isReach;

        public bool IsReach //立直状态判断
        {
            get
            {
                try
                {
                    return _isReach;
                }
                catch
                {
                    MessageBox.Show("获取立直状态出错啦");
                    return false;
                }
            }
            set
            {
                try
                {
                    _activity?.RunOnUiThread(() =>
                    {
                        if (!Element.Session.NagareMode)
                        {
                            if (!_isReach && value)
                            {
                                Point -= 1000;
                                Element.Session.QianBang++;
                            }
                            if (_isReach && !value)
                            {
                                Point += 1000;
                                Element.Session.QianBang--;
                            }
                        }
                        _isReach = value;
                        _activity.FindViewById<CheckBox>(Ckb).Checked = _isReach;
                    });
                }
                catch
                {
                    MessageBox.Show("设置立直状态出错啦");
                }
            }
        }

        public Player(int btn, int ckb, int point, NameEnum name, WindEnum wind, bool isreach)
        {
            Btn = btn;
            Ckb = ckb;
            Point = point;
            Name = name;
            Wind = wind;
            IsReach = isreach;
        }

        public Player()
        {
        }

        public override string ToString()
        {
            return Wind + Point.ToString();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        //暂时无法使用深复制，报错说android.app.activity也要加[Serializable]，但无法操作
        public Player DeepClone()
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
                return cloneObject as Player;
            }
        }

        public Player ShallowClone()
        {
            return Clone() as Player;
        }
    }

    //使用了orderby+thenby基本取代这个排序
    class PlayerComparerByPoint : IComparer<Player>
    {
        public int Compare(Player x, Player y)
        {
            if (x.Point == y.Point)
                return x.OriginalWind < y.OriginalWind ? -1 : 1; //-1：排在上方 1：排在下方
            return x.Point > y.Point ? -1 : 1;
        }
    }
}