﻿using System;
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
    /// リスト表示フラグメント
    /// 
    /// https://soncho.mydns.jp/2019/10/27/fragment%E3%81%AF%E6%97%A7%E5%9E%8B%E5%BC%8F%E3%81%A7%E3%81%99-this-class-is-obsoleted-in-this-android-platform/
    /// Fragmentは旧型式と警告が出る
    /// using Android.App;
    /// ↓
    /// using Android.Support.V4.App;
    /// </summary>
    public class MenuListFragment : Fragment
    {
        /// <summary>
        /// 大画面か否か
        /// </summary>
        private bool _isLayoutXLarge = true;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // 画面をXMLからインフレ―ト
            var view = inflater.Inflate(Resource.Layout.fragment_menu_list, container, false);
            var lvMenu = view.FindViewById<ListView>(Resource.Id.lvMenu);

            // リストデータ生成
            var menuList = CreateMenuList();

            // adapter生成
            var from = new string[] { NAME_KEY, PRICE_KEY };
            var to = new int[] { Android.Resource.Id.Text1, Android.Resource.Id.Text2 };

            var adapter = new SimpleAdapter(this.Activity, menuList,
                Android.Resource.Layout.SimpleListItem2, from, to);
            lvMenu.Adapter = adapter;

            // リスナ登録
            lvMenu.ItemClick += OnListItemClick;

            // インフレートした画面を返す
            return view;
        }

        /// <summary>
        /// メニューデータ生成
        /// </summary>
        /// <returns></returns>
        private static List<IDictionary<string, object>> CreateMenuList()
        {
            var data = new List<(string name, string priceStr)>()
            {
                ("から揚げ定食", "800円"),
                ("ハンバーグ定食", "850円"),
                ("親子丼", "600円"),
            };
            var menuList = new List<IDictionary<string, object>>();
            foreach (var d in data)
            {
                menuList.Add(new JavaDictionary<string, object>()
                {
                    { NAME_KEY, d.name },
                    { PRICE_KEY, d.priceStr },
                });
            }
            return menuList;
        }

        /// <summary>
        /// リスト表示アイテムクリック時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //タップされた行のデータ取得
            var item = e.Parent.GetItemAtPosition(e.Position) as IDictionary<string, object>;
            var name = item[NAME_KEY] as string;
            var price = item[PRICE_KEY] as string;

            // 引継ぎデータを取りまとめるオブジェクト生成
            var bundle = new Bundle();
            bundle.PutString(NAME_KEY, name);
            bundle.PutString(PRICE_KEY, price);

            // 画面サイズにより処理を分ける
            if (this._isLayoutXLarge)
            {
                //
                // 大画面：同じアクティビティ内のフラグメントを更新
                // 

                var transaction = this.FragmentManager?.BeginTransaction();
                // フラグメント生成→引継ぎデータ格納→置き換え
                var menuThanksFramgment = new MenuThanksFragment();
                menuThanksFramgment.Arguments = bundle;
                transaction?.Replace(Resource.Id.menuThanksFrame, menuThanksFramgment);
                // トランザクション単位で更新する様子
                transaction?.Commit();
            }
            else
            {
                //
                // 通常画面：別画面起動
                //

                //インテント生成→画面起動
                var intent = new Intent(this.Activity, typeof(MenuThanksActivity));
                intent.PutExtra(NAME_KEY, name);
                intent.PutExtra(PRICE_KEY, price);
                this.StartActivity(intent);
            }
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            // 所属アクティビティにmenuThanksFrame が存在する = 大画面 と判断する
            var menuThanksFrame = this.Activity?.FindViewById<View>(Resource.Id.menuThanksFrame);
            this._isLayoutXLarge = menuThanksFrame != null;
        }
    }
}