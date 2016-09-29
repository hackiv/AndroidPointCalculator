using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TenhouPointCalculatorBeta3
{
    class FuFanPoint
    {
        public int Fu { get; }
        public int Fan { get; }
        public int KoAgareTotalPoint { get; }
        public int OyaAgareTotalPoint { get; }
        public int OyaTsumoTotalPoint { get; }
        public int KoTsumoTotalPoint { get; }
        public int KoTsumoLostPoint { get; }
        public int OyaTsumoLostPoint { get; }

        public FuFanPoint()
        {

        }

        public FuFanPoint(int fu, int fan, int koAgareTotalPoint, int oyaAgareTotalPoint, int oyaTsumoTotalPoint, int koTsumoTotalPoint, int koTsumoLostPoint, int oyaTsumoLostPoint)
        {
            Fu = fu;
            Fan = fan;
            KoAgareTotalPoint = koAgareTotalPoint;
            OyaAgareTotalPoint = oyaAgareTotalPoint;
            OyaTsumoTotalPoint = oyaTsumoTotalPoint;
            KoTsumoTotalPoint = koTsumoTotalPoint;
            KoTsumoLostPoint = koTsumoLostPoint;
            OyaTsumoLostPoint = oyaTsumoLostPoint;
        }

        public override string ToString()
        {
            return Fu + " " + Fan + " " + KoAgareTotalPoint + " " + OyaAgareTotalPoint + " " + OyaTsumoTotalPoint + " " + KoTsumoTotalPoint + " " + KoTsumoLostPoint + " " + OyaTsumoLostPoint + "\n";
        }
    }
}