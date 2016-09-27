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
    class AgareRefactor
    {
        private static int _flagUp;
        private static int _flagDown;
        private static Player _upPlayer;
        private static Player _downPlayer;
        private static IEnumerable<Player> _downPlayers;
        private static FuFanPoint _targetPoint;
        private static int? _upPoint;
        private static int? _downOyaPoint;
        private static int? _downKoPoint;
        private static bool _isOyaAgare;
        private static bool _isTsumo;
        //��Ϊ˫���������ݣ�������ʼ��
        private static int _benChangTemp;
        private static bool _isOyaAgareFirst;



        public static void Method()
        {
            #region ��ʼ��
            MainActivity.RunningOtherProgram = true;
            _flagUp = 0;
            _flagDown = 0;
            _upPlayer = null;
            _downPlayer = null;
            _downPlayers = null;
            _targetPoint = null;
            _upPoint = null;
            _downOyaPoint = null;
            _downKoPoint = null;
            _isOyaAgare = false;
            _isTsumo = false;
            #endregion

            //�������߳�
            Thread th = new Thread(() =>
            {
                //���良���ͼ�
                SelectPlayer();
                //�ж��Ƿ��׺���
                _isOyaAgare = Element.Players[_flagUp - 1].Name == Element.Session.OyaName;
                //�ж��Ƿ�����
                _isTsumo = _flagUp == _flagDown;
                try
                {
                    //��׼������
                    StandardizePoint();
                    //���е�������
                    AgareMethod();
                }
                catch (Exception)
                {
                    MessageBox.Show("�����������");
                }

                if (MainActivity.DoubleRonCheckBox.Checked)
                {
                    //˫��׼��
                    DoubleRonPrepare();
                    Method();
                }
                else
                {
                    //���ƺ���
                    AfterAgare();
                }
            });
            th.IsBackground = true;
            th.Start();
        }

        private static void SelectPlayer()//���良���ͼ�
        {
            //��ȡflag
            MainActivity.Flag = 0;
            UpdateText.Set(MainActivity.ControlTextView, "˭��北�");
            while (MainActivity.Flag == 0) { }
            _flagDown = MainActivity.Flag;

            MainActivity.Flag = 0;
            UpdateText.Set(MainActivity.ControlTextView, "˭���ƣ�");
            while (MainActivity.Flag == 0)
            {
            }
            _flagUp = MainActivity.Flag;

            UpdateText.Set(MainActivity.ControlTextView, "(OvO)");
            _upPlayer = Element.Players[_flagUp - 1];
            _downPlayer = Element.Players[_flagDown - 1];

            //�Ժ�����
            _upPlayer = Element.Players[_flagUp - 1];
            _downPlayer = Element.Players[_flagDown - 1];
            _downPlayers = Element.Players.Where(p => p.Name != _upPlayer.Name).Select(p => p);
        }

        private static void StandardizePoint()//��׼������
        {
            var txt = MainActivity.InpuTextView.Text;
            var txtStrings = txt.Split(new[] { '/' });
            int totalPoint;

            #region "7900"
            // "7900"                   length==1
            if (txtStrings.Length == 1)
            {
                totalPoint = Convert.ToInt32(txt);

                if (_isOyaAgare && _isTsumo)
                {
                    _targetPoint = Element.FuFanPoints.Where(
                        p =>
                            p.OyaTsumoTotalPoint == totalPoint)
                        .Select(p => p)
                        .FirstOrDefault();
                    _upPoint = _targetPoint?.OyaTsumoTotalPoint;
                    _downOyaPoint = null;
                    _downKoPoint = _targetPoint?.KoTsumoLostPoint;
                }
                else if (_isOyaAgare)
                {
                    _targetPoint = Element.FuFanPoints.Where(
                        p =>
                            p.OyaAgareTotalPoint == totalPoint)
                        .Select(p => p)
                        .FirstOrDefault();
                    _upPoint = _targetPoint?.OyaAgareTotalPoint;
                    _downOyaPoint = null;
                    _downKoPoint = null;
                }
                else if (_isTsumo)
                {
                    _targetPoint = Element.FuFanPoints.Where(
                        p => p.KoTsumoTotalPoint == totalPoint)
                        .Select(p => p)
                        .FirstOrDefault();
                    _upPoint = _targetPoint?.KoTsumoTotalPoint;
                    _downOyaPoint = _targetPoint?.OyaTsumoLostPoint;
                    _downKoPoint = _targetPoint?.KoTsumoLostPoint;
                }
                else
                {
                    _targetPoint = Element.FuFanPoints.Where(
                        p =>
                            p.KoAgareTotalPoint == totalPoint)
                        .Select(p => p)
                        .FirstOrDefault();
                    _upPoint = _targetPoint?.KoAgareTotalPoint;
                    _downOyaPoint = null;
                    _downKoPoint = null;
                }
            }
            #endregion

            #region "2000/3900"
            // "2000/3900" ������only   length==2
            else if (txtStrings.Length == 2)
            {
                _targetPoint = Element.FuFanPoints.Where(
                        p =>
                            p.KoTsumoLostPoint.ToString() == txtStrings[0] &&
                            p.OyaTsumoLostPoint.ToString() == txtStrings[1])
                            .Select(p => p)
                            .FirstOrDefault();
                _upPoint = _targetPoint?.KoTsumoTotalPoint;
                _downOyaPoint = _targetPoint?.OyaTsumoLostPoint;
                _downKoPoint = _targetPoint?.KoTsumoLostPoint;
            }
            #endregion

            #region "//2000"
            // "//2000"    ������only   length==3 && [0]==null && [1]==null
            else if (txtStrings.Length == 3 && txtStrings[0] == "")
            {
                _targetPoint = Element.FuFanPoints.Where(
                        p =>
                            p.OyaTsumoLostPoint.ToString() == txtStrings[2] &&
                            (p.OyaTsumoTotalPoint / 3).ToString() == txtStrings[2])
                            .Select(p => p)
                            .FirstOrDefault();
                _upPoint = _targetPoint?.OyaTsumoTotalPoint;
                _downOyaPoint = null;
                _downKoPoint = _targetPoint?.OyaTsumoLostPoint;
            }
            #endregion

            #region "30//4" 
            // "30//4"                  length==3 && [0]!=null && [1]==null
            else if (txtStrings.Length == 3 && txtStrings[0] != "")
            {
                _targetPoint = Element.FuFanPoints.Where(
                        p =>
                            p.Fu.ToString() == txtStrings[0] &&
                            p.Fan.ToString() == txtStrings[2])
                            .Select(p => p)
                            .FirstOrDefault();
                if (_isOyaAgare && _isTsumo)
                {
                    _upPoint = _targetPoint?.OyaTsumoTotalPoint;
                    _downOyaPoint = null;
                    _downKoPoint = _targetPoint?.KoTsumoLostPoint;
                }
                else if (_isOyaAgare)
                {
                    _upPoint = _targetPoint?.OyaAgareTotalPoint;
                    _downOyaPoint = null;
                    _downKoPoint = null;
                }
                else if (_isTsumo)
                {
                    _upPoint = _targetPoint?.KoTsumoTotalPoint;
                    _downOyaPoint = _targetPoint?.OyaTsumoLostPoint;
                    _downKoPoint = _targetPoint?.KoTsumoLostPoint;
                }
                else
                {
                    _upPoint = _targetPoint?.KoAgareTotalPoint;
                    _downOyaPoint = null;
                    _downKoPoint = null;
                }
            }
            #endregion



        }

        private static void AgareMethod()//�����㷨
        {
            int cb = Convert.ToInt32(MainActivity.ChangBangTextView.Text);//����
            int qb = Convert.ToInt32(MainActivity.QianBangTextView.Text);//ǧ��

            _upPlayer.Point += Convert.ToInt32(_upPoint) + cb * 300 + qb * 1000;

            if (!_isTsumo)//�ٺ�
            {
                _downPlayer.Point -= Convert.ToInt32(_upPoint) + cb * 300;
            }
            else//����
            {
                foreach (var p in _downPlayers)
                {
                    if (p.Name == Element.Session.OyaName)
                        p.Point -= Convert.ToInt32(_downOyaPoint) + cb * 100;
                    else
                        p.Point -= Convert.ToInt32(_downKoPoint) + cb * 100;
                }
            }
        }

        private static void DoubleRonPrepare()//˫��׼��
        {
            UpdateText.Set(MainActivity.DoubleRonCheckBox, false);
            _benChangTemp = Element.Session.BenChang;
            Element.Session.BenChang = 0;
            Element.Session.QianBang = 0;
            _isOyaAgareFirst = _isOyaAgare;
        }

        private static void AfterAgare()//���ƺ���
        {
            //˫��󽻻�����
            if (_benChangTemp != 0)
                Element.Session.BenChang = _benChangTemp;
            //�ж��Ƿ��׼Һ���
            if (_isOyaAgare || _isOyaAgareFirst)
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
                        player.Wind = WindEnum.��;
                    else
                        player.Wind--;
                    player.IsReach = false;
                }
            }
            Element.Session.QianBang = 0;
            UpdateText.Set(MainActivity.InpuTextView, "");
            MainActivity.NowSessionNum++;
            Game.Save();
            End.IsOwari();
            MainActivity.RunningOtherProgram = false;


            #region �Ѻ��Ʒ����ｨ���ֶγ�ʼ��
            _flagUp = 0;
            _flagDown = 0;
            _upPlayer = null;
            _downPlayer = null;
            _downPlayers = null;
            _targetPoint = null;
            _upPoint = null;
            _downOyaPoint = null;
            _downKoPoint = null;
            _isOyaAgare = false;
            _isTsumo = false;
            #endregion
        }
    }
}