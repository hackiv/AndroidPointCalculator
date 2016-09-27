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
        public static int Flag;
        private static bool _isShowingDeltaPoint;
        public static int NowSessionNum = 1;
        public static bool IsOyaAgare;
        public static bool RunningOtherProgram;
        public static bool NagareMode;

        public static TextView InpuTextView;
        public static TextView ControlTextView;
        public static TextView SessionTextView;
        public static TextView ChangBangTextView;
        public static TextView QianBangTextView;
        public static CheckBox DoubleRonCheckBox;
        public static Button test;

        protected override void OnCreate(Bundle bundle)
        {
            //SetTheme(Resource.Style.HackivTheme);
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
            ////场棒图片
            var changBangImg = FindViewById<ImageView>(Resource.Id.imgViewChangBang);
            changBangImg.SetImageResource(Resource.Drawable.ChangBang);
            //千棒图片
            var qianBangImg = FindViewById<ImageView>(Resource.Id.imgViewQianBang);
            qianBangImg.SetImageResource(Resource.Drawable.QianBang);
            //场棒
            ChangBangTextView = FindViewById<TextView>(Resource.Id.textViewChangBang);
            QianBangTextView = FindViewById<TextView>(Resource.Id.textViewQianBang);
            #endregion

            #region 四家数据控件 ok
            //四家按钮
            var leftButton = FindViewById<Button>(Resource.Id.btnLeftPlayer);
            leftButton.Click += (s, e) => Flag = 1;
            var oppositeButton = FindViewById<Button>(Resource.Id.btnOppositePlayer);
            oppositeButton.Click += (s, e) => Flag = 2;
            var rightButton = FindViewById<Button>(Resource.Id.btnRightPlayer);
            rightButton.Click += (s, e) => Flag = 3;
            var meButton = FindViewById<Button>(Resource.Id.btnMePlayer);
            meButton.Click += (s, e) => Flag = 4;

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
                {
                    RunningOtherProgram = true;
                    ShowDeltaPointMethod(Element.LeftPlayer, buttons);
                    RunningOtherProgram = false;
                }
            };
            oppositeButton.LongClick += (s, e) =>
            {
                if (RunningOtherProgram == false)
                {
                    RunningOtherProgram = true;
                    ShowDeltaPointMethod(Element.OppositePlayer, buttons);
                    RunningOtherProgram = false;
                }
            };
            rightButton.LongClick += (s, e) =>
            {
                if (RunningOtherProgram == false)
                {
                    RunningOtherProgram = true;
                    ShowDeltaPointMethod(Element.RightPlayer, buttons);
                    RunningOtherProgram = false;
                }
            };
            meButton.LongClick += (s, e) =>
            {
                if (RunningOtherProgram == false)
                {
                    RunningOtherProgram = true;
                    ShowDeltaPointMethod(Element.MePlayer, buttons);
                    RunningOtherProgram = false;
                }
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
                if (RunningOtherProgram == false)
                {
                    RunningOtherProgram = true;
                    NewGame_Click();
                    RunningOtherProgram = false;
                }
            };
            //设定
            var setting = FindViewById<Button>(Resource.Id.btnSetting);
            setting.Click += (s, e) =>
            {
                if (RunningOtherProgram == false)
                {
                    RunningOtherProgram = true;
                    Setting.SettingElement(InpuTextView.Text);
                    RunningOtherProgram = false;
                }
            };
            //记录
            var log = FindViewById<Button>(Resource.Id.btnShowGameLog);
            log.Click += Log_Click;
            //上一局
            var priGame = FindViewById<Button>(Resource.Id.btnPriGame);
            priGame.Click += (s, e) =>
            {
                if (RunningOtherProgram == false)
                {
                    RunningOtherProgram = true;
                    Game.Load(NowSessionNum - 1);
                    RunningOtherProgram = false;
                }
            };
            //下一局
            var nextGame = FindViewById<Button>(Resource.Id.btnNextGame);
            nextGame.Click += (s, e) =>
            {
                if (RunningOtherProgram == false)
                {
                    RunningOtherProgram = true;
                    Game.Load(NowSessionNum + 1);
                    RunningOtherProgram = false;
                }
            };
            //双响
            DoubleRonCheckBox = FindViewById<CheckBox>(Resource.Id.checkBoxDoubleRon);
            //帮助
            var help = FindViewById<Button>(Resource.Id.btnHelp);
            help.Click += Help_Click;
            //取消立直
            var cancelReach = FindViewById<Button>(Resource.Id.btnCancelReach);
            cancelReach.Click += (s, e) => CancelReach();
            #endregion

            #region 改变点数的按钮 推99/流局/和牌 ok
            //推99
            var suddenlyNagare = FindViewById<Button>(Resource.Id.btnSuddenlyNagare);
            suddenlyNagare.Click += (s, e) =>
            {
                if (RunningOtherProgram == false)
                {
                    RunningOtherProgram = true;
                    SuddenlyNagare_Click();
                    RunningOtherProgram = false;
                }
            };
            //流局
            var nagareBtn = FindViewById<Button>(Resource.Id.btnNagare);
            nagareBtn.Click += (s, e) =>
            {
                if (RunningOtherProgram == false)
                {
                    RunningOtherProgram = true;
                    Nagare_Click();
                }
            };
            //和牌
            var agareBtn = FindViewById<Button>(Resource.Id.btnAgare);
            agareBtn.Click += (s, e) =>
            {
                if (RunningOtherProgram == false)
                {
                    RunningOtherProgram = true;
                    //agare.GetFlag();
                    AgareRefactor.Method();
                }
            };
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
            FindViewById<Button>(Resource.Id.btnBackSpace).Click += (s, e) => InpuTextView.Text = InpuTextView.Text.Substring(0, InpuTextView.Text.Length - 1);
            #endregion

            //测试用
            test = FindViewById<Button>(Resource.Id.btnForTest);
            test.Click += Test_Click;
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
            string txt = "";
            foreach (var d in Element.GameLogDictionary)
            {
                txt += d.Key + " " + d.Value[0] + " " + d.Value[1] + " " + d.Value[2] + " " + d.Value[3] + " " + d.Value[4] + " " + "\n";
            }
            MessageBox.Show(txt);
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
            UpdateText.Set(ControlTextView, "谁听牌？");
            Flag = 0;
            NagareMode = true;
            Thread th = new Thread(() =>
            {
                while (Flag == 0)
                {
                }
                Nagare nagare = new Nagare();
                nagare.NagareMethod();
                NagareMode = false;
                UpdateText.Set(ControlTextView, "(OvO)");
                Game.Save();
                End.IsOwari();
                RunningOtherProgram = false;
            })
            { IsBackground = true };
            th.Start();
        }

        private void SuddenlyNagare_Click()
        {
            Element.Session.BenChang++;
            NowSessionNum++;
            NagareMode = true;
            foreach (var player in Element.Players)
            {
                player.IsReach = false;
            }
            NagareMode = false;
            Game.Save();
        }

        private void NewGame_Click()
        {
            UpdateText.Set(InpuTextView, "");
            UpdateText.Set(ControlTextView, "谁是起家？");
            Flag = 0;
            Thread th = new Thread(() =>
            {
                //flag：上家1 对家2 下家3 自己4
                while (Flag == 0)
                {
                }
                MessageBox.Show(Element.Players[Flag - 1].Name + "东起");
                UpdateText.Set(ControlTextView, "(OvO)");
                Element.Session = new Session(0, 0, SessionEnum.东一局, (NameEnum)Flag - 1);
                NagareMode = false;
                for (int i = 0; i < 4; i++)
                {
                    Element.Players[Flag - i - 1].Point = 25000;
                    Element.Players[Flag - i - 1].IsReach = false;
                    Element.Players[Flag - i - 1].Wind = (WindEnum)i;
                    Element.Players[Flag - i - 1].OriginalWind = (WindEnum)i;
                    if (Flag - i - 2 < 0) Flag += 4;
                }
                NowSessionNum = 0;
                Game.Save();
            })
            { IsBackground = true };
            th.Start();
        }

        private void CancelReach()
        {
            foreach (var p in Element.Players)
            {
                if (p.IsReach)
                {
                    p.IsReach = false;
                    p.Point += 1000;
                    Element.Session.QianBang -= 1;
                }
            }
        }

        private void Test_Click(object sender, EventArgs e)
        {
            GetXml.GetPoint();
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