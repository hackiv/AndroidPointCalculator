using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace TenhouPointCalculatorBeta3
{
    [Serializable]
    [Activity(Label = "实麻算点beta3", MainLauncher = true, Icon = "@drawable/DIYIcon")]
    public class MainActivity : Activity
    {
        public static Context Context;
        public static int Flag;
        private static bool _isShowingDeltaPoint;
        public static int NowSessionNum = 1;
        public static bool IsReadyToSave;



        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it

            Context = this;
            Agare agare = new Agare();


            #region 显示用的控件 输入框/场风框 ok
            //输入框
            var textOfInput = FindViewById<TextView>(Resource.Id.textViewShowInput);
            //场风框
            var session = FindViewById<TextView>(Resource.Id.textViewSession);
            //控制框
            var control = FindViewById<TextView>(Resource.Id.textViewControl);
            ////场棒图片
            var changBangImg = FindViewById<ImageView>(Resource.Id.imgViewChangBang);
            //changBangImg.SetImageDrawable(GetDrawable(Resource.Drawable.ChangBang));
            changBangImg.SetImageResource(Resource.Drawable.ChangBang);
            //千棒图片
            var qianBangImg = FindViewById<ImageView>(Resource.Id.imgViewQianBang);
            //qianBangImg.SetImageDrawable(GetDrawable(Resource.Drawable.QianBang));
            qianBangImg.SetImageResource(Resource.Drawable.QianBang);
            #endregion

            #region 四家数据控件 ok
            //四家按钮
            var leftBtn = FindViewById<Button>(Resource.Id.btnLeftPlayer);
            leftBtn.Click += (s, e) => Flag = 1;
            var oppositeBtn = FindViewById<Button>(Resource.Id.btnOppositePlayer);
            oppositeBtn.Click += (s, e) => Flag = 2;
            var rightBtn = FindViewById<Button>(Resource.Id.btnRightPlayer);
            rightBtn.Click += (s, e) => Flag = 3;
            var meBtn = FindViewById<Button>(Resource.Id.btnMePlayer);
            meBtn.Click += (s, e) => Flag = 4;

            List<Button> buttons = new List<Button>()
            {
                leftBtn,
                oppositeBtn,
                rightBtn,
                meBtn
            };

            leftBtn.LongClick += (s, e) => ShowDeltaPointMethod(Element.LeftPlayer, buttons);
            oppositeBtn.LongClick += (s, e) => ShowDeltaPointMethod(Element.OppositePlayer, buttons);
            rightBtn.LongClick += (s, e) => ShowDeltaPointMethod(Element.RightPlayer, buttons);
            meBtn.LongClick += (s, e) => ShowDeltaPointMethod(Element.MePlayer, buttons);

            //四家选项框
            var oppositeCkb = FindViewById<CheckBox>(Resource.Id.checkBoxOppositePlayer);
            oppositeCkb.CheckedChange += (s, e) => Element.OppositePlayer.IsReach = oppositeCkb.Checked;
            var leftCkb = FindViewById<CheckBox>(Resource.Id.checkBoxLeftPlayer);
            leftCkb.CheckedChange += (s, e) => Element.LeftPlayer.IsReach = leftCkb.Checked;
            var rightCkb = FindViewById<CheckBox>(Resource.Id.checkBoxRightPlayer);
            rightCkb.CheckedChange += (s, e) => Element.RightPlayer.IsReach = rightCkb.Checked;
            var meCkb = FindViewById<CheckBox>(Resource.Id.checkBoxMePlayer);
            meCkb.CheckedChange += (s, e) => Element.MePlayer.IsReach = meCkb.Checked;
            #endregion

            #region 功能性按键 新对局/设定/记录/上一局/下一局
            //新对局
            var newGame = FindViewById<Button>(Resource.Id.btnNewGame);
            newGame.Click += NewGame_Click;
            //设定
            var setting = FindViewById<Button>(Resource.Id.btnSetting);
            setting.Click += (s, e) => Setting.SettingElement(textOfInput.Text);
            //记录
            var log = FindViewById<Button>(Resource.Id.btnShowGameLog);
            log.Click += Log_Click;
            //上一局
            var priGame = FindViewById<Button>(Resource.Id.btnPriGame);
            priGame.Click += (s, e) => Game.Load(NowSessionNum - 1);
            //priGame.Click += Test_Click;
            //下一局
            var nextGame = FindViewById<Button>(Resource.Id.btnNextGame);
            nextGame.Click += (s, e) => Game.Load(NowSessionNum + 1);
            //nextGame.Click += Test_Click;
            //双响
            var doubleRon = FindViewById<CheckBox>(Resource.Id.checkBoxDoubleRon);
            #endregion

            #region 改变点数的按钮 推99/流局/和牌 ok
            //推99
            var suddenlyNagare = FindViewById<Button>(Resource.Id.btnSuddenlyNagare);
            suddenlyNagare.Click += SuddenlyNagare_Click;
            //流局
            var nagareBtn = FindViewById<Button>(Resource.Id.btnNagare);
            nagareBtn.Click += Nagare_Click;
            //和牌
            var agareBtn = FindViewById<Button>(Resource.Id.btnAgare);
            agareBtn.Click += (s, e) => agare.GetFlag();
            #endregion

            #region 键盘设定ok
            FindViewById<Button>(Resource.Id.btnNum0).Click += (s, e) => textOfInput.Text += "0";
            FindViewById<Button>(Resource.Id.btnNum1).Click += (s, e) => textOfInput.Text += "1";
            FindViewById<Button>(Resource.Id.btnNum2).Click += (s, e) => textOfInput.Text += "2";
            FindViewById<Button>(Resource.Id.btnNum3).Click += (s, e) => textOfInput.Text += "3";
            FindViewById<Button>(Resource.Id.btnNum4).Click += (s, e) => textOfInput.Text += "4";
            FindViewById<Button>(Resource.Id.btnNum5).Click += (s, e) => textOfInput.Text += "5";
            FindViewById<Button>(Resource.Id.btnNum6).Click += (s, e) => textOfInput.Text += "6";
            FindViewById<Button>(Resource.Id.btnNum7).Click += (s, e) => textOfInput.Text += "7";
            FindViewById<Button>(Resource.Id.btnNum8).Click += (s, e) => textOfInput.Text += "8";
            FindViewById<Button>(Resource.Id.btnNum9).Click += (s, e) => textOfInput.Text += "9";
            FindViewById<Button>(Resource.Id.btnNumDiv).Click += (s, e) => textOfInput.Text += "/";
            FindViewById<Button>(Resource.Id.btnBackSpace).Click += (s, e) => textOfInput.Text = textOfInput.Text.Substring(0, textOfInput.Text.Length - 1);
            #endregion

            //测试用
            var test = FindViewById<Button>(Resource.Id.btnForTest);
            test.Click += Test_Click;
        }

        private void Log_Click(object sender, EventArgs e)
        {
            string txt = "";
            foreach (var d in Element.GameLogDictionary)
            {
                txt += d.Key + " " + d.Value[0] + " " + d.Value[1] + " " + d.Value[2] + " " + d.Value[3] + " " + d.Value[4] + " " + "\n";
            }

            AlertDialog.Builder adb = new AlertDialog.Builder(this);
            adb.SetMessage(txt);
            adb.Show();
        }

        private void ShowDeltaPointMethod(Player player, List<Button> buttons)
        {
            if (_isShowingDeltaPoint)
            {
                foreach (var eplayer in Element.Players)
                {
                    FindViewById<Button>(eplayer.Btn).Text = eplayer.ToString();
                }
                _isShowingDeltaPoint = false;
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    if (player.Btn != buttons[i].Id)
                        buttons[i].Text = (Element.Players[i].Point - player.Point).ToString();
                }
                _isShowingDeltaPoint = true;
            }
        }

        private void Nagare_Click(object sender, EventArgs e)
        {
            Element.Session.NagareMode = true;
            FindViewById<TextView>(Resource.Id.textViewShowInput).Text = "谁听牌？";
            Flag = 0;
            Thread th = new Thread(() =>
            {
                while (Flag == 0) ;
                Nagare nagare = new Nagare();
                nagare.NagareMethod();
                RunOnUiThread(() => FindViewById<TextView>(Resource.Id.textViewShowInput).Text = "");
                Element.Session.NagareMode = false;
                Game.Save();
            });
            th.IsBackground = true;
            th.Start();
        }

        private void SuddenlyNagare_Click(object sender, EventArgs e)
        {
            Element.Session.BenChang++;
            NowSessionNum++;
            Element.Session.NagareMode = true;
            Game.Save();
        }

        private void NewGame_Click(object sender, EventArgs e)
        {
            FindViewById<TextView>(Resource.Id.textViewShowInput).Text = "";
            //先用foreach运行一遍，执行静态构造函数，为子线程for循环输入数据做准备
            foreach (var player in Element.Players)
            {
                player.Point = 25000;
                player.IsReach = false;
            }
            Flag = 0;
            Thread th = new Thread(() =>
            {
                //flag：上家1 对家2 下家3 自己4
                while (Flag == 0) ;
                AlertDialog.Builder adb = new AlertDialog.Builder(Context);
                adb.SetMessage(Element.Players[Flag - 1].Name + "东起");
                RunOnUiThread(() =>
                {
                    adb.Show();
                });
                Element.Session = new Session(0, 0, SessionEnum.东一局, (NameEnum)Flag - 1, false);
                for (int i = 0; i < 4; i++)
                {
                    Element.Players[Flag - i - 1].Wind = (WindEnum)i;
                    Element.Players[Flag - i - 1].OriginalWind = (WindEnum)i;
                    if (Flag - i - 2 < 0) Flag += 4;
                }
                NowSessionNum = 0;
                //Element.ProcessLogs = new List<ProcessLog>
                //{
                //    new ProcessLog() {Players = Element.Players,Session = Element.Session,SessionNum = NowSessionNum}
                //};
                Game.Save();
            });
            th.IsBackground = true;
            th.Start();
        }

        private void Test_Click(object sender, EventArgs e)
        {
            string txt = "";
            foreach (var p in Element.Players)
            {
                txt += p.Wind.ToString() + p.Point.ToString() + "\n";
            }
            txt += Element.Session.NowSession.ToString() + "\n";
            txt += "本场：" + Element.Session.BenChang.ToString() + "\n";
            txt += "千棒：" + Element.Session.QianBang.ToString() + "\n";
            txt += "当前局数：" + NowSessionNum;

            AlertDialog.Builder adb = new AlertDialog.Builder(this);
            adb.SetMessage(txt);
            adb.Show();
        }

        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)//重写返回键
        {
            if (keyCode == Keycode.Back)
            {
                FindViewById<TextView>(Resource.Id.textViewShowInput).Text = "";
                return true;
            }
            return base.OnKeyDown(keyCode, e);
        }
    }
}