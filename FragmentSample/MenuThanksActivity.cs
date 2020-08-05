using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using static FragmentSample.Constants;

namespace FragmentSample
{
    [Activity(Label = "MenuThanksActivuty")]
    public class MenuThanksActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // レイアウトの定義
            SetContentView(Resource.Layout.activity_menu_thanks);
        }
    }
}