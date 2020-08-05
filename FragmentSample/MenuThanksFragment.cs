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
    /// <summary>
    /// 注文完了フラグメント
    /// </summary>
    public class MenuThanksFragment : Fragment
    {
        /// <summary>
        /// 大画面か否か
        /// </summary>
        private bool _isLayoutXLarge = true;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // menuListFragment が存在する = 大画面 と判断する
            var menuListFragment = this.FragmentManager?.FindFragmentById(Resource.Id.fragmentMenuList);
            this._isLayoutXLarge = (menuListFragment != null);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // 画面をXMLからインフレ―ト
            var view = inflater.Inflate(Resource.Layout.fragment_menu_thanks, container, false);

            // 大画面か否かで情報取得元が異なる
            Bundle extras;
            if (this._isLayoutXLarge)
            {
                extras = this.Arguments;
            }
            else
            {
                var intent = this.Activity?.Intent;
                // 引継ぎデータから情報取得
                extras = intent?.Extras;
            }

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
            // 大画面か否かで画面遷移が異なる
            if (this._isLayoutXLarge)
            {
                // 自分を削除してコミット
                var transaction = this.FragmentManager?.BeginTransaction();
                transaction?.Remove(this);
                transaction?.Commit();
            }
            else
            {
                // アクティビティ終了 = 戻る
                this.Activity?.Finish();
            }
        }
    }
}