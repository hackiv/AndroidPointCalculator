using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TenhouPointCalculatorBeta3
{
    class MessageBox
    {
        Activity _activity = (Activity) MainActivity.Context;

        public void Show(string str)
        {
            AlertDialog.Builder adb = new AlertDialog.Builder(MainActivity.Context).SetMessage(str);
            
        }
    }
}