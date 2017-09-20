using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Java.IO;
using Java.Lang;
using Thread = System.Threading.Thread;

namespace TenhouPointCalculatorBeta3
{
    [Activity(Label = "实麻算点beta3", MainLauncher = true, Icon = "@drawable/DIYIcon")]
    public class MainActivity : Activity
    {
        public static Context Context;
        private static bool _isShowingDeltaPoint;
        public static int NowSessionNum = 1;
        public static bool IsOyaAgare;
        public static bool RunningOtherProgram;

        public static TextView InpuTextView;
        public static TextView ControlTextView;
        public static TextView SessionTextView;
        public static TextView ChangBangTextView;
        public static TextView QianBangTextView;
        public static CheckBox DoubleRonCheckBox;
        public static Button Test;
        public static Button AgareBtn;
        public static Button nagareBtn;
        public static TextView GameLogTextView;

        protected override void OnCreate(Bundle bundle)
        {
            SetTheme(Resource.Xml.theme);
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it

            Context = this;
            GetXml.GetPoint();//加载程序时读取符翻对照表


            #region 显示用的控件 输入框/场风框 ok
            //输入框
            InpuTextView = FindViewById<TextView>(Resource.Id.textViewShowInput);
            //场风框
            SessionTextView = FindViewById<TextView>(Resource.Id.textViewSession);
            //控制框
            ControlTextView = FindViewById<TextView>(Resource.Id.textViewControl);
            //场棒图片
            var changBangImg = FindViewById<ImageView>(Resource.Id.imgViewChangBang);
            changBangImg.SetImageResource(Resource.Drawable.ChangBang);
            //千棒图片
            var qianBangImg = FindViewById<ImageView>(Resource.Id.imgViewQianBang);
            qianBangImg.SetImageResource(Resource.Drawable.QianBang);
            //场棒
            ChangBangTextView = FindViewById<TextView>(Resource.Id.textViewChangBang);
            QianBangTextView = FindViewById<TextView>(Resource.Id.textViewQianBang);
            //对局记录框
            GameLogTextView = FindViewById<TextView>(Resource.Id.textViewShowLog);
            GameLogTextView.MovementMethod= new Android.Text.Method.ScrollingMovementMethod();
            GameLogTextView.Visibility = ViewStates.Invisible;
            #endregion

            #region 四家数据控件 ok
            //四家按钮
            var leftButton = FindViewById<Button>(Resource.Id.btnLeftPlayer);
            leftButton.Click += (s, e) => Element.Session.Flag = 1;
            var oppositeButton = FindViewById<Button>(Resource.Id.btnOppositePlayer);
            oppositeButton.Click += (s, e) => Element.Session.Flag = 2;
            var rightButton = FindViewById<Button>(Resource.Id.btnRightPlayer);
            rightButton.Click += (s, e) => Element.Session.Flag = 3;
            var meButton = FindViewById<Button>(Resource.Id.btnMePlayer);
            meButton.Click += (s, e) => Element.Session.Flag = 4;

            List<Button> buttons = new List<Button>()
            {
                leftButton,
                oppositeButton,
                rightButton,
                meButton
            };

            leftButton.LongClick += (s, e) =>
            {
                if (RunningOtherProgram == false)
                    ShowDeltaPointMethod(Element.LeftPlayer, buttons);
            };
            oppositeButton.LongClick += (s, e) =>
            {
                if (RunningOtherProgram == false)
                    ShowDeltaPointMethod(Element.OppositePlayer, buttons);
            };
            rightButton.LongClick += (s, e) =>
            {
                if (RunningOtherProgram == false)
                    ShowDeltaPointMethod(Element.RightPlayer, buttons);
            };
            meButton.LongClick += (s, e) =>
            {
                if (RunningOtherProgram == false)
                    ShowDeltaPointMethod(Element.MePlayer, buttons);
            };

            //四家选项框
            var oppositeCheckBox = FindViewById<CheckBox>(Resource.Id.checkBoxOppositePlayer);
            oppositeCheckBox.CheckedChange += (s, e) => Element.OppositePlayer.IsReach = oppositeCheckBox.Checked;
            var leftCheckBox = FindViewById<CheckBox>(Resource.Id.checkBoxLeftPlayer);
            leftCheckBox.CheckedChange += (s, e) => Element.LeftPlayer.IsReach = leftCheckBox.Checked;
            var rightCheckBox = FindViewById<CheckBox>(Resource.Id.checkBoxRightPlayer);
            rightCheckBox.CheckedChange += (s, e) => Element.RightPlayer.IsReach = rightCheckBox.Checked;
            var meCheckBox = FindViewById<CheckBox>(Resource.Id.checkBoxMePlayer);
            meCheckBox.CheckedChange += (s, e) => Element.MePlayer.IsReach = meCheckBox.Checked;
            #endregion

            #region 功能性按键 新对局/设定/记录/上一局/下一局/帮助/取消立直
            //新对局
            var newGame = FindViewById<Button>(Resource.Id.btnNewGame);
            newGame.Click += (s, e) =>
            {
                if (RunningOtherProgram) return;
                RunningOtherProgram = true;
                NewGame_Click();
                RunningOtherProgram = false;
            };
            //设定
            var setting = FindViewById<Button>(Resource.Id.btnSetting);
            setting.Click += (s, e) =>
            {
                if (RunningOtherProgram) return;
                RunningOtherProgram = true;
                Setting.SettingElement(InpuTextView.Text);
                RunningOtherProgram = false;
            };
            //记录
            var log = FindViewById<Button>(Resource.Id.btnShowGameLog);
            log.Click += Log_Click;
            //上一局
            var priGame = FindViewById<Button>(Resource.Id.btnPriGame);
            priGame.Click += (s, e) =>
            {
                if (RunningOtherProgram == false)
                    Game.Load(NowSessionNum - 1);
            };
            //下一局
            var nextGame = FindViewById<Button>(Resource.Id.btnNextGame);
            nextGame.Click += (s, e) =>
            {
                if (RunningOtherProgram == false)
                    Game.Load(NowSessionNum + 1);
            };
            //双响
            DoubleRonCheckBox = FindViewById<CheckBox>(Resource.Id.checkBoxDoubleRon);
            //帮助
            var help = FindViewById<Button>(Resource.Id.btnHelp);
            help.Click += Help_Click;
            #endregion

            #region 改变点数的按钮 推99/流局/和牌 ok
            //推99
            var suddenlyNagare = FindViewById<Button>(Resource.Id.btnSuddenlyNagare);
            suddenlyNagare.Click += (s, e) =>
            {
                if (RunningOtherProgram == false)
                    SuddenlyNagare_Click();
            };
            //流局
            nagareBtn = FindViewById<Button>(Resource.Id.btnNagare);
            nagareBtn.Click += (s, e) => Nagare_Click();
            //和牌
            AgareBtn = FindViewById<Button>(Resource.Id.btnAgare);
            AgareBtn.Click += AgareBtn_Click;
            #endregion

            #region 键盘设定ok
            FindViewById<Button>(Resource.Id.btnNum0).Click += (s, e) => InpuTextView.Text += "0";
            FindViewById<Button>(Resource.Id.btnNum1).Click += (s, e) => InpuTextView.Text += "1";
            FindViewById<Button>(Resource.Id.btnNum2).Click += (s, e) => InpuTextView.Text += "2";
            FindViewById<Button>(Resource.Id.btnNum3).Click += (s, e) => InpuTextView.Text += "3";
            FindViewById<Button>(Resource.Id.btnNum4).Click += (s, e) => InpuTextView.Text += "4";
            FindViewById<Button>(Resource.Id.btnNum5).Click += (s, e) => InpuTextView.Text += "5";
            FindViewById<Button>(Resource.Id.btnNum6).Click += (s, e) => InpuTextView.Text += "6";
            FindViewById<Button>(Resource.Id.btnNum7).Click += (s, e) => InpuTextView.Text += "7";
            FindViewById<Button>(Resource.Id.btnNum8).Click += (s, e) => InpuTextView.Text += "8";
            FindViewById<Button>(Resource.Id.btnNum9).Click += (s, e) => InpuTextView.Text += "9";
            FindViewById<Button>(Resource.Id.btnNumDiv).Click += (s, e) => InpuTextView.Text += "/";
            FindViewById<Button>(Resource.Id.btnBackSpace).Click += (s, e) =>
            {
                if (InpuTextView.Text != "")
                    InpuTextView.Text = InpuTextView.Text.Substring(0, InpuTextView.Text.Length - 1);
            };
            FindViewById<Button>(Resource.Id.btnBackSpace).LongClick += (s, e) => InpuTextView.Text = "";
            #endregion

            //测试用
            Test = FindViewById<Button>(Resource.Id.btnForTest);
            Test.Click += Test_Click;
        }

        private void AgareBtn_Click(object sender, EventArgs e)
        {
            if (AgareBtn.Text == "和牌")
            {
                if (RunningOtherProgram) return;
                Element.Session.IsAgareMode = true;
                PlayerCondition.AgareJudge();
                Element.Session.IsNewAgare = true;
                Element.Session.Save = 0;
                UpdateText.Set(ControlTextView, "谁出铳？");
                RunningOtherProgram = true;
                UpdateText.Set(AgareBtn, "取消和牌");
            }
            else
            {
                Element.Session.IsAgareMode = false;
                UpdateText.Set(ControlTextView, "(OvO)");
                UpdateText.Set(InpuTextView, "");
                UpdateText.Set(AgareBtn, "和牌");
                RunningOtherProgram = false;
            }
        }

        //帮助文档
        private void Help_Click(object sender, EventArgs e)
        {
            var fs = Resources.OpenRawResource(Resource.Raw.Help);
            InputStreamReader read = new InputStreamReader(fs, "gbk");
            BufferedReader reader = new BufferedReader(read);
            StringBuffer sb = new StringBuffer("");
            while (true)
            {
                var line = reader.ReadLine();
                if (line == null)
                    break;
                sb.Append(line);
                sb.Append("\n");
            }

            read.Close();
            fs.Close();
            reader.Close();

            MessageBox.Show(sb.ToString());
        }

        private void Log_Click(object sender, EventArgs e)
        {
            GameLogTextView.Visibility = GameLogTextView.Visibility == ViewStates.Invisible ? ViewStates.Visible : ViewStates.Invisible;
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

        private void Nagare_Click()
        {
            if (nagareBtn.Text == "流局")
            {
                if (RunningOtherProgram) return;
                RunningOtherProgram = true;
                UpdateText.Set(ControlTextView, "谁听牌？");
                Element.Session.IsNagareMode = true;
                foreach (var player in Element.Players)
                {
                    if (player.IsReach)
                        player.IsReachLockOn = true;
                }
                UpdateText.Set(nagareBtn, "取消流局");
            }
            else
            {
                foreach (var player in Element.Players)
                {
                    player.IsReach = false;
                    player.IsReachLockOn = false;
                }
                Element.Session.IsNagareMode = false;
                UpdateText.Set(ControlTextView, "(OvO)");
                UpdateText.Set(nagareBtn, "流局");
                RunningOtherProgram = false;
            }
        }

        private void SuddenlyNagare_Click()
        {
            Element.Session.BenChang++;
            NowSessionNum++;
            Element.Session.IsNagareMode = true;
            foreach (var player in Element.Players)
            {
                player.IsReach = false;
            }
            Element.Session.IsNagareMode = false;
            Game.Save(Element.Session.ToString() + Element.Session.BenChang + "本 " + "中途流局");
        }

        private void NewGame_Click()
        {
            RunningOtherProgram = true;
            UpdateText.Set(InpuTextView, "");
            UpdateText.Set(ControlTextView, "谁是起家？");
            Element.Session.Flag = 0;
            Element.Session.IsNewGame = true;
        }

        private void Test_Click(object sender, EventArgs e)
        {
            if (Element.Session.IsAgareMode)
            {
                Element.Session.IsAgareMode = false;
                UpdateText.Set(ControlTextView, "(OvO)");
                UpdateText.Set(InpuTextView, "");
                RunningOtherProgram = false;
            }
        }

        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)//重写返回键
        {
            if (keyCode == Keycode.Back)
            {
                UpdateText.Set(InpuTextView, "");
                return true;
            }
            return base.OnKeyDown(keyCode, e);
        }
    }
}