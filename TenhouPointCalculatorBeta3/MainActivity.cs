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
        public static bool IsOyaAgare;
        public static bool RunningOtherProgram;



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

            leftBtn.LongClick += (s, e) =>
            {
                if (RunningOtherProgram == false)
                {
                    RunningOtherProgram = true;
                    ShowDeltaPointMethod(Element.LeftPlayer, buttons);
                    RunningOtherProgram = false;
                }
            };
            oppositeBtn.LongClick += (s, e) =>
            {
                if (RunningOtherProgram == false)
                {
                    RunningOtherProgram = true;
                    ShowDeltaPointMethod(Element.OppositePlayer, buttons);
                    RunningOtherProgram = false;
                }
            };
            rightBtn.LongClick += (s, e) =>
            {
                if (RunningOtherProgram == false)
                {
                    RunningOtherProgram = true;
                    ShowDeltaPointMethod(Element.RightPlayer, buttons);
                    RunningOtherProgram = false;
                }
            };
            meBtn.LongClick += (s, e) =>
            {
                if (RunningOtherProgram == false)
                {
                    RunningOtherProgram = true;
                    ShowDeltaPointMethod(Element.MePlayer, buttons);
                    RunningOtherProgram = false;
                }
            };

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

            #region 功能性按键 新对局/设定/记录/上一局/下一局/帮助
            //新对局
            var newGame = FindViewById<Button>(Resource.Id.btnNewGame);
            newGame.Click += (s, e) =>
            {
                if (RunningOtherProgram == false)
                {
                    RunningOtherProgram = true;
                    NewGame_Click(s, e);
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
                    Setting.SettingElement(textOfInput.Text);
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
            //priGame.Click += Test_Click;
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
            //nextGame.Click += Test_Click;
            //双响
            var doubleRon = FindViewById<CheckBox>(Resource.Id.checkBoxDoubleRon);
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
                {
                    RunningOtherProgram = true;
                    SuddenlyNagare_Click(s, e);
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
                    Nagare_Click(s, e);
                }
            };
            //和牌
            var agareBtn = FindViewById<Button>(Resource.Id.btnAgare);
            agareBtn.Click += (s, e) =>
            {
                if (RunningOtherProgram == false)
                {
                    RunningOtherProgram = true;
                    agare.GetFlag();
                }
            };
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

        //帮助文档
        private void Help_Click(object sender, EventArgs e)
        {
            string txt = "";
            txt += @"<简单写个使用说明>
欢迎使用本软件(OvO)。
当前软件版本为1.2内测版。
请原谅这个简陋的界面，因为以实现功能为优先，我完全没学习关于美化界面方面的知识......
但是请期待后续版本！
顺带说下我的测试机型为魅蓝note3，版本为5.1（手上也没有别的机型测试......

<大致说下界面各部分组成>
左上角为新建对局，右上角为帮助文档。

上方四个按钮和四个选项框是对应四家状态，默认下方的按钮表示自己，其他三个按钮为对应的其他三家。
长按按钮可以显示该玩家与其他三家的点差，再长按一次就恢复原来情况（四家按钮都适用。）
请不要在显示点差的状态下进行任何其他操作，要和牌/立直/流局等请先恢复到原来点数状态。

中间是控制框和显示输入文本，中间右边为本场棒（上）和供托中的1000千点棒（下）数量。

下方中间是键盘区（“BS”为退格键BackSpace，“/”为分割符号），左右两边是功能键。

<功能键使用说明>
新对局：点新对局→点四家按钮中一个→以被按的玩家为东起开始。

和牌：输入和牌大小→点和牌→点铳家再点和家（如果自摸就点同一按钮两下）。
自摸时可以输入“2000/3900”（不分前后顺序）或者“7900” 。
在忘记符翻对应点数的时候，可以输入“70//2”（分前后顺序）来表示70符2翻，控制台还会显示70符2翻点数为多少。
但是实麻移交点数的时候别忘了还要算上本场棒，因为这个点数是没算计算本场棒的。
例1：对家铳自己3900点：键盘区输入3900→点和牌按钮→点对家按钮→点自己按钮
例2：自己自摸4000点：键盘区输入4000→点和牌按钮→点自己按钮→点自己按钮
例3：自己铳上家（子）70符2翻：键盘区输入70//2→点和牌按钮→点自己按钮→点上家按钮→控制台显示“符翻点数为4500”

双响选项框：出现双响局面时，勾选双响→输入和牌大小→点和牌→输入铳、和家→输入和牌大小→输入铳、和家
注：和牌大小只要在当次结算的输入和家之前输入完毕就可以。
注2：请从铳牌家开始逆时针方向开始输入和牌家，因为只有第一遍计算会涉及本场棒和立直棒。

立直：勾选选项框，对应玩家点数-1000。在普通状态下取消勾选就可以取消立直，点数恢复1000。
注：流局时立直选项框代表是否听牌，此时取消听牌并不会恢复1000点（当然这个也算是非法操作，将在后续版本中禁止这种操作）。

流局：点流局→勾选对应玩家选项框表示听牌→点任意玩家按钮

推99：这是中途流局用的按钮，实际上四风/四立/四杠等都可以按这个，本场直接+1。

设定：按照“上家点数/对家点数/下家点数/自己点数/本场数/供托千棒数/场次（东一为1，依次+1）”格式输入→点设定来设置。这个功能是主动设置局面，在罚满贯/中途进入计分状态的情况下可以使用。
例：上家10000点，对家20000点，下家40000点，自己30000点，1本0棒，南三局→输入10000/20000/40000/30000/1/0/7→点设定
注：记得要先点新对局确定起家再用设定！

上一局/下一局：可以用此功能返回到上一局/下一局的状态，比如在东四局输入点数错误时可以按上一局回到东三局重新输入点数。
注：用上一局进行重新输入点数后，该局以后的旧记录都会销毁。

记录：显示对局中各个时点的状态，半成品，将在后续版本中完善。

测试用：测试新功能用的按钮，无实际用途。

返回键：用返回键退回桌面再进入程序会有显示bug，所以我把返回键改成了清除显示框文本，刚好省掉一个按钮。

<结束语>
这个软件会有大量的bug，所以使用的时候请多多包涵（捂脸
建议每新记录一个半庄都先清后台重启再使用，因为有时会出现记录bug，正努力修改中......
因为软件的数据都是在内存中保存，如果被杀后台会导致数据丢失。后续版本中会加入以写入文件形式保存数据，但当前版本还请注意别杀后台......
另外请按上面的操作规范来使用，因为很多边界条件和非法操作我还没进行限制，所以很多非法操作可能会引起程序崩溃丢失记录（booooooooom

如果发现bug，欢迎提交到 919703505@qq.com
";

            AlertDialog.Builder adb = new AlertDialog.Builder(this);
            adb.SetMessage(txt);
            adb.Show();
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
            AlertDialog.Builder adb = new AlertDialog.Builder(this);
            Element.Session.NagareMode = true;
            FindViewById<TextView>(Resource.Id.textViewControl).Text = "谁听牌？";
            Flag = 0;
            Thread th = new Thread(() =>
            {
                while (Flag == 0) ;
                Nagare nagare = new Nagare();
                nagare.NagareMethod();
                RunOnUiThread(() => FindViewById<TextView>(Resource.Id.textViewControl).Text = "(OvO)");
                Game.Save();
                End.IsOwari(this, adb);
                RunningOtherProgram = false;
            });
            th.IsBackground = true;
            th.Start();
        }

        private void SuddenlyNagare_Click(object sender, EventArgs e)
        {
            Element.Session.BenChang++;
            NowSessionNum++;
            Element.Session.NagareMode = true;
            foreach (var player in Element.Players)
            {
                player.IsReach = false;
            }
            Element.Session.NagareMode = false;
            Game.Save();
        }

        private void NewGame_Click(object sender, EventArgs e)
        {
            FindViewById<TextView>(Resource.Id.textViewShowInput).Text = "";
            FindViewById<TextView>(Resource.Id.textViewControl).Text = "谁是起家？";
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
                    FindViewById<TextView>(Resource.Id.textViewControl).Text = "(OvO)";
                });
                Element.Session = new Session(0, 0, SessionEnum.东一局, (NameEnum)Flag - 1, false);
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