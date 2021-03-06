﻿using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.Collections.Generic;
using static MenuSample.Constants;
using Android.Content;
using Android.Views;
using Android;

namespace MenuSample
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        /// <summary>
        /// 表示メニューリスト
        /// </summary>
        private List<IDictionary<string, object>> _menuList = null;

        /// <summary>
        /// アダプターに渡すための定義(表示用)
        /// </summary>
        private string[] _from;

        /// <summary>
        /// アダプターに渡すための定義(表示用)
        /// </summary>
        private int[] _to;

        /// <summary>
        /// アクティビティ生成時の処理
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            var lvMenu = this.FindViewById<ListView>(Resource.Id.lvMenu);
            // リストデータ生成
            _menuList = CreateTeishokuList();

            // adapter生成
            _from = new string[] { NAME_KEY, PRICE_KEY };
            _to = new int[] { Resource.Id.tvMenuName, Resource.Id.tvMenuPrice };
            //↓は error CS0117: 'Resource.Id' に 'text1' の定義がありません と言われてエラーになった
            //var to = new int[] { Resource.Id.text1, Resource.Id.text2 };
            var adapter = new SimpleAdapter(this.ApplicationContext, _menuList,
                Resource.Layout.row, _from, _to);
            lvMenu.Adapter = adapter;

            // クリックイベントハンドラ登録
            lvMenu.ItemClick += OnListItemClick;

            // コンテキストメニューの登録
            this.RegisterForContextMenu(lvMenu);
        }

        /// <summary>
        /// データ生成(定食)
        /// </summary>
        /// <returns></returns>
        private static List<IDictionary<string, object>> CreateTeishokuList()
        {
            var data = new List<(string name, decimal price, string desc)>()
            {
                ("から揚げ定食", 800, "若鶏のから揚げにサラダ、ごはんとお味噌汁が付きます。"),
                ("ハンバーグ定食", 850, "手ごねハンバーグにサラダ、ごはんとお味噌汁が付きます。"),
                ("親子丼", 600, "鶏肉と玉ねぎを卵で閉じたどんぶりです。"),
            };
            var menuList = new List<IDictionary<string, object>>();
            foreach (var d in data)
            {
                menuList.Add(new JavaDictionary<string, object>()
                {
                    { NAME_KEY, d.name },
                    { PRICE_KEY, d.price },
                    { DESC_KEY, d.desc},
                });
            }
            return menuList;
        }

        /// <summary>
        /// データ生成(カレー)
        /// </summary>
        /// <returns></returns>
        private static List<IDictionary<string, object>> CreateCurryList()
        {
            var data = new List<(string name, decimal price, string desc)>()
            {
                ("ビーフカレー", 520, "特選スパイスをきかせた国産ビーフ100%のカレーです"),
                ("ポークカレー", 420, "特選スパイスをきかせた国産ポーク100%のカレーです"),
            };
            var menuList = new List<IDictionary<string, object>>();
            foreach (var d in data)
            {
                menuList.Add(new JavaDictionary<string, object>()
                {
                    { NAME_KEY, d.name },
                    { PRICE_KEY, d.price },
                    { DESC_KEY, d.desc},
                });
            }
            return menuList;
        }

        /// <summary>
        /// リストアイテム選択時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            // タップされた行のデータ取得
            var item = e.Parent.GetItemAtPosition(e.Position) as IDictionary<string, object>;
            // 注文処理
            Order(item);
        }

        /// <summary>
        /// 注文処理
        /// </summary>
        /// <param name="item"></param>
        private void Order(IDictionary<string, object> item)
        {
            var name = item[NAME_KEY] as string;
            var price = (decimal)item[PRICE_KEY];

            //インテント生成→画面起動
            var intent = new Intent(this.ApplicationContext, typeof(MenuThanksActivity));
            intent.PutExtra(NAME_KEY, name);
            intent.PutExtra(PRICE_KEY, $"{price}円");
            this.StartActivity(intent);
        }

        /// <summary>
        /// オプションメニュー生成時の処理
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            // 追加直後はResource。Menu が無い旨エラーになったが、
            // VisualStudio再起動すると問題なくビルドできるようになった
            this.MenuInflater.Inflate(Resource.Menu.menu_options_menu_list, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        /// <summary>
        /// オプションメニュー選択時の処理
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            //選択メニューIDによりデータを差し替える
            switch (item.ItemId)
            {
                case Resource.Id.menuListOptionTeishoku:
                    _menuList = CreateTeishokuList();
                    break;
                case Resource.Id.menuListOptionCurry:
                    _menuList = CreateCurryList();
                    break;
                default:
                    break;
            }
            var lvMenu = this.FindViewById<ListView>(Resource.Id.lvMenu);
            var adapter = new SimpleAdapter(this.ApplicationContext, _menuList,
                Resource.Layout.row, _from, _to);
            lvMenu.Adapter = adapter;
            return base.OnOptionsItemSelected(item);
        }

        /// <summary>
        /// コンテキストメニュー生成時の処理
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="v"></param>
        /// <param name="menuInfo"></param>
        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            base.OnCreateContextMenu(menu, v, menuInfo);

            // コンテキストメニューのレイアウトファイルをインフレ―ト、タイトル設定
            this.MenuInflater.Inflate(Resource.Menu.menu_context_menu_list, menu);
            menu.SetHeaderTitle(Resource.String.menu_list_context_header);
        }

        /// <summary>
        /// コンテキストメニュー選択時の処理
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool OnContextItemSelected(IMenuItem item)
        {
            //情報格納オブジェクト取得
            var info = item.MenuInfo as AdapterView.AdapterContextMenuInfo;
            //長押しされたポジション取得→情報取得
            var listPosition = info.Position;
            var isDefined = this._menuList?.IsDefinedAt(listPosition) ?? false;
            var menu = isDefined ? _menuList[listPosition] : null;
            if (menu == null)
            {
                return false;
            }

            switch (item.ItemId)
            {
                case Resource.Id.menuListContextDesc:
                    // 説明メニュー
                    var desc = menu[DESC_KEY] as string;
                    Toast.MakeText(this.ApplicationContext, desc, ToastLength.Long).Show();
                    break;
                case Resource.Id.menuListContextOrder:
                    // 注文メニュー
                    this.Order(menu);
                    break;
                default:
                    break;
            }

            return base.OnContextItemSelected(item);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}