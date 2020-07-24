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
using Java.Interop;
using static IntentSample.Constants;

namespace IntentSample
{
    [Activity(Label = "MenuThanksActivity")]
    public class MenuThanksActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // レイアウトの定義
            // https://kuxumarin.hatenablog.com/entry/2016/12/30/Xamarin.Android_%E3%81%A7_Intent_%E3%82%92%E4%BD%BF%E3%81%84%E7%94%BB%E9%9D%A2%E9%81%B7%E7%A7%BB%E3%81%99%E3%82%8B
            SetContentView(Resource.Layout.activity_menu_thanks);

            // データを取り出してセットする
            var name = Intent.GetStringExtra(NAME_KEY);
            var price = Intent.GetStringExtra(PRICE_KEY);

            var tvMenuName = this.FindViewById<TextView>(Resource.Id.tvMenuName);
            var tvMenuPrice = this.FindViewById<TextView>(Resource.Id.tvMenuPrice);

            tvMenuName.Text = name;
            tvMenuPrice.Text = price;
        }

        [Export(nameof(OnBackButtonClick))]
        public void OnBackButtonClick(View view)
        {
            // アクティビティを終了することで結果的には戻ることになる
            this.Finish();
        }
    }
}