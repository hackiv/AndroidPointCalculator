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
    internal class Agare
    {
        private int _flagUp;
        private int _flagDown;
        private int _temp;
        private bool _afterDoubleRon;

        public void GetFlag()
        {
            //�������
            int changBang = Element.Session.BenChang;
            int qianBang = Element.Session.QianBang;
            bool isOyaAgare;
            bool isOyaAgareFirst = false;//˫��ʱ��һ���Ƿ�Ϊ�׺���
            MainActivity.Flag = 0;

            Thread th = new Thread(() =>
            {
                //��һ��������㲻ִ�г���++�ͷ�λ�任����¼�Ƿ����׺��ƣ��ڶ����������ʱ����=0��ִ���곡���ָ�����ֵ

                DoubleRon:
                //ѡ��良���ͼ�
                SelectPlayer(out isOyaAgare);
                //���Ƽ���
                if (_flagUp == _flagDown)
                    TsumoMethod(MainActivity.InpuTextView.Text, qianBang, changBang, ref isOyaAgare);
                else
                    AgareMethod(MainActivity.InpuTextView.Text, qianBang, changBang, ref isOyaAgare);
                //˫�촦��
                if (MainActivity.DoubleRonCheckBox.Checked)//�ж��Ƿ�˫��
                {
                    UpdateText.Set(MainActivity.DoubleRonCheckBox, false);
                    _afterDoubleRon = true;
                    isOyaAgareFirst = isOyaAgare;
                    _temp = changBang;
                    changBang = 0;
                    qianBang = 0;
                    goto DoubleRon;
                }
                if (_afterDoubleRon)//˫��ʱִ�еĶ������
                {
                    changBang = _temp;
                    _afterDoubleRon = false;
                }
                //���ƺ���
                AfterAgare(isOyaAgare, isOyaAgareFirst);
            });
            th.IsBackground = true;
            th.Start();
        }

        private void SelectPlayer(out bool isOyaAgare)//���良���ͼ�
        {
            MainActivity.Flag = 0;
            UpdateText.Set(MainActivity.ControlTextView, "˭��北�");
            while (MainActivity.Flag == 0)
            {
            }
            _flagDown = MainActivity.Flag;

            MainActivity.Flag = 0;
            UpdateText.Set(MainActivity.ControlTextView, "˭���ƣ�");
            while (MainActivity.Flag == 0)
            {
            }
            _flagUp = MainActivity.Flag;

            UpdateText.Set(MainActivity.ControlTextView, "(OvO)");
            isOyaAgare = false;
        }


        private static void AfterAgare(bool isOyaAgare, bool isOyaAgareFirst)//���������
        {
            MainActivity.IsOyaAgare = isOyaAgare || isOyaAgareFirst;

            if (isOyaAgare || isOyaAgareFirst)
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
        }

        private static bool IsSpecialInput(ref string txt, IReadOnlyDictionary<string, string> dic)
        {
            if (dic.ContainsKey(txt))
            {
                UpdateText.Set(MainActivity.ControlTextView, "��������Ϊ" + dic[txt]);
                return true;
            }
            return false;
        }

        private void AgareMethod(string txt, int qianBang, int changBang, ref bool isOyaAgare)
        {
            Player playerUp = Element.Players[_flagUp - 1];
            Player playerDown = Element.Players[_flagDown - 1];
            if (playerUp.Name == Element.Session.OyaName) isOyaAgare = true;
            if (isOyaAgare)
                IsSpecialInput(ref txt, Dictionaries.OyaAgareDictionary);
            else
                IsSpecialInput(ref txt, Dictionaries.KoAgareDictionary);

            int changePoint = 0;
            try
            {
                IsSpecialInput(ref txt, Dictionaries.KoTsumoDictionary);
                changePoint = Convert.ToInt32(txt);
            }
            catch
            {
                ThrowInputPointError();
            }
            playerUp.Point += changePoint + 300 * changBang + 1000 * qianBang;
            playerDown.Point -= changePoint + 300 * changBang;
            _flagUp = 0;
            _flagDown = 0;
            MainActivity.Flag = 0;
        }

        private void TsumoMethod(string txt, int qianBang, int changBang, ref bool isOyaAgare)
        {
            int oyaLostPoint = 0;
            int koLostPoint = 0;
            int totalChangePoint = 0;
            List<Player> playerDownKoList = new List<Player>();//����ʱ�ӼҼ���
            Player playerUp = Element.Players[_flagUp - 1];//���Ʒ�
            Player playerDownOya = new Player();//������ʱ����

            #region ��ȡ�ļ�player
            if (playerUp.Name == Element.Session.OyaName)//������
            {
                foreach (var player in Element.Players)
                {
                    if (player.Name != playerUp.Name)
                        playerDownKoList.Add(player);
                }
                playerDownOya = null;
                isOyaAgare = true;

                #region ������ʱ�����Ӧ�䶯����
                IsSpecialInput(ref txt, Dictionaries.OyaTsumoDictionary);
                totalChangePoint = Convert.ToInt32(txt);
                koLostPoint = totalChangePoint / 3;
                #endregion
            }
            else//������
            {
                foreach (var player in Element.Players)
                {
                    if (player != playerUp && player.Name != Element.Session.OyaName)
                        playerDownKoList.Add(player);
                    else if (player != playerUp)
                        playerDownOya = player;
                }
                IsSpecialInput(ref txt, Dictionaries.KoTsumoDictionary);
                #region ������ʱ�����Ӧ�䶯���� ����ok
                if (!IsDivInput(txt))
                {
                    var point =
                        Element.FuFanPoints.Where(p => p.KoTsumoTotalPoint.ToString() == txt)
                            .Select(p => p)
                            .FirstOrDefault();
                    if (point != null)
                    {
                        oyaLostPoint = point.OyaTsumoLostPoint;
                        koLostPoint = point.KoTsumoLostPoint;
                        totalChangePoint = point.KoTsumoTotalPoint;
                    }
                    else
                    {
                        ThrowInputPointError();
                    }
                }
                else//���������ʽΪ "2000/3900" ���� "3900/2000"
                {
                    try
                    {
                        var txtStrings = txt.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                        int point1 = Convert.ToInt32(txtStrings[0]);
                        int point2 = Convert.ToInt32(txtStrings[1]);
                        oyaLostPoint = point1 > point2 ? point1 : point2;
                        koLostPoint = oyaLostPoint == point1 ? point2 : point1;

                        var point =
                            Element.FuFanPoints.Where(
                                p => p.KoTsumoLostPoint == koLostPoint && p.OyaTsumoLostPoint == oyaLostPoint)
                                .Select(p => p)
                                .FirstOrDefault();
                        if (point == null)
                            ThrowInputPointError();
                        else
                        {
                            totalChangePoint = point.KoTsumoTotalPoint;
                        }
                    }
                    catch
                    {
                        ThrowInputPointError();
                    }
                }
                #endregion
            }
            #endregion

            #region ����ת��
            playerUp.Point += totalChangePoint + 300 * changBang + 1000 * qianBang;
            if (playerDownOya != null)
                playerDownOya.Point -= oyaLostPoint + 100 * changBang;
            foreach (var player in playerDownKoList)
            {
                player.Point -= koLostPoint + 100 * changBang;
            }
            #endregion
        }

        private void ThrowInputPointError()
        {
            MessageBox.Show("�����������\n����������");
            UpdateText.Set(MainActivity.InpuTextView, "");
            GetFlag();
        }

        private static bool IsDivInput(string txt)
        {
            try
            {
                return txt.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries)[1] != null;
            }
            catch
            {
                return true;
            }
        }
    }
}