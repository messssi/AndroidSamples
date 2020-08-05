using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V4.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using static FragmentSample.Constants;

namespace FragmentSample
{
    public class MenuThanksFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // 画面をXMLからインフレ―ト
            var view = inflater.Inflate(Resource.Layout.fragment_menu_thanks, container, false);
            var intent = this.Activity?.Intent;
            // 引継ぎデータから情報取得
            var extras = intent?.Extras;
            var menuName = extras?.GetString(NAME_KEY);
            var menuPrice = extras?.GetString(PRICE_KEY);

            // Viewに表示
            var tvMenuName = view.FindViewById<TextView>(Resource.Id.tvMenuName);
            var tvMenuPrice = view.FindViewById<TextView>(Resource.Id.tvMenuPrice);
            tvMenuName.Text = menuName;
            tvMenuPrice.Text = menuPrice;

            // 戻るボタンにリスナ登録
            var btBack = view.FindViewById<Button>(Resource.Id.btBackButton);
            btBack.Click += OnBtBackClick;

            // インフレートした画面を返す
            return view;
        }

        private void OnBtBackClick(object sender, EventArgs e)
        {
            // アクティビティ終了 = 戻る
            this.Activity?.Finish();
        }
    }
}