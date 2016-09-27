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
        public int Fu { get; set; }
        public int Fan { get; set; }
        public int KoAgareTotalPoint { get; set; }
        public int OyaAgareTotalPoint { get; set; }
        public int OyaTsumoTotalPoint { get; set; }
        public int KoTsumoTotalPoint { get; set; }
        public int KoTsumoLostPoint { get; set; }
        public int OyaTsumoLostPoint { get; set; }

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