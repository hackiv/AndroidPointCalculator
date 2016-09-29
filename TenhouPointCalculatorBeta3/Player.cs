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
                    UpdateText.Set(_activity.FindViewById<Button>(Btn),ToString());
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
                    UpdateText.Set(_activity.FindViewById<Button>(Btn), ToString());
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
                        if (!_isReach && value && !Element.Session.IsNagareMode)
                        {
                            Point -= 1000;
                            Element.Session.QianBang++;
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
        public Player ShallowClone()
        {
            return Clone() as Player;
        }
    }
}