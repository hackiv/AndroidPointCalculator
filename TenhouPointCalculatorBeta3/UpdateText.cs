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
    static class UpdateText
    {
        public static void Set(Object control, string txt)
        {
            Activity activity = MainActivity.Context as Activity;
            activity?.RunOnUiThread(() =>
            {
                if (control is Button)
                {
                    Button btn = (Button) control;
                    btn.Text = txt;
                }
                else if (control is TextView)
                {
                    TextView tv = (TextView) control;
                    tv.Text = txt;
                }
            });
        }

        public static void Set(Object control, bool checkValue)
        {
            Activity activity = MainActivity.Context as Activity;
            activity?.RunOnUiThread(() =>
            {
                if (control is CheckBox)
                {
                    CheckBox ckb = (CheckBox) control;
                    ckb.Checked = checkValue;
                }
            });
        }
    }
}