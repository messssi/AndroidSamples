using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.Design.Widget;
using Android.Graphics;
using System.Collections.Generic;
using Android.Widget;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;

namespace RecyclerViewSample
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        const string NAME_KEY = "name";
        const string PRICE_KEY = "price";
        const string DESC_KEY = "desc";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            //
            //ツールバー設定
            //
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            //このロゴ設定はちゃんと効く
            toolbar.SetLogo(Resource.Mipmap.ic_launcher);
            //↓どちらでもOK
            //toolbar.Title = Resources.GetText(Resource.String.toolbar_title);
            toolbar.SetTitle(Resource.String.toolbar_title);
            SetSupportActionBar(toolbar);

            // CollapsingToolbarLayoutに設定
            var toolbarLayout = FindViewById<CollapsingToolbarLayout>(Resource.Id.toolbarLayout);
            toolbarLayout.Title = Resources.GetText(Resource.String.toolbar_title);
            //引数はintのはずなのだが、なぜかこれでエラーにならないし動く　
            toolbarLayout.SetExpandedTitleColor(Color.White);
            toolbarLayout.SetCollapsedTitleTextColor(Color.Gray);

            //
            //リスト表示
            //
            var lvMenuList = this.FindViewById(Resource.Id.lvMenu) as RecyclerView;
            var layout = new LinearLayoutManager(this.ApplicationContext);
            //var layout = new GridLayoutManager(this.ApplicationContext, 2);
            //var layout = new StaggeredGridLayoutManager(2, StaggeredGridLayoutManager.Vertical);

            lvMenuList.SetLayoutManager(layout);

            // リストデータ生成
            var menuList = CreateMenuList();

            // アダプタを生成して諸設定
            var adapter = new RecyclerListAdaptor(this.ApplicationContext, menuList);
            lvMenuList.SetAdapter(adapter);

            // 区切り線は手動設定が必要
            var decorator = new DividerItemDecoration(this.ApplicationContext, layout.Orientation);
            lvMenuList.AddItemDecoration(decorator);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private static List<IDictionary<string, object>> CreateMenuList()
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
        /// ビューホルダクラス(17.2.2 手順3)
        /// </summary>
        private class RecyclerListViewHolder : RecyclerView.ViewHolder
        {
            public TextView TvMenuName { get; set; }

            public TextView TvMenuPrice { get; set; }

            public RecyclerListViewHolder(View itemView) : base(itemView)
            {
                TvMenuName = itemView.FindViewById(Resource.Id.tvMenuName) as TextView;
                TvMenuPrice = itemView.FindViewById(Resource.Id.tvMenuPrice) as TextView;
            }
        }

        /// <summary>
        /// アダプタクラス(17.2.2 手順4)
        /// </summary>
        private class RecyclerListAdaptor : RecyclerView.Adapter
        {
            List<IDictionary<string, object>> _listData { get; set; }

            /// <summary>
            /// 親クラスのContext
            /// 
            /// inner class はC#だと存在しない
            /// →親クラスのインスタンスのうち必要なものは渡してやる必要あり
            /// </summary>
            Context _context { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="context"></param>
            /// <param name="listData"></param>
            public RecyclerListAdaptor(Context context, List<IDictionary<string, object>> listData)
            {
                this._context = context;
                this._listData = listData;
            }

            /// <summary>
            /// Create new views (invoked by the layout manager)
            /// </summary>
            /// <param name="parent"></param>
            /// <param name="viewType"></param>
            /// <returns></returns>
            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                // レイアウトインフレ―タ取得
                var inflater = LayoutInflater.From(this._context);

                // 1行分の画面部品 row.xmlをインフレ―とする(？)
                // インフレートする = XML定義を実際のオブジェクトに膨らませる の意
                var view = inflater.Inflate(Resource.Layout.row, parent, false);

                // クリックされた時の処理を追加しておく
                view.Click += OnViewClick;

                var holder = new RecyclerListViewHolder(view);
                return holder;
            }

            private void OnViewClick(object sender, System.EventArgs e)
            {
                // メッセージ表示する
                var view = sender as View;
                var tvMenuName = view.FindViewById<TextView>(Resource.Id.tvMenuName);
                var menuName = tvMenuName.Text.ToString();
                var msg = _context.GetString(Resource.String.msg_header) + menuName;
                Toast.MakeText(_context, msg, ToastLength.Short).Show();
            }

            /// <summary>
            /// Replace the contents of a view (invoked by the layout manager)
            /// </summary>
            /// <param name="viewHolder"></param>
            /// <param name="position"></param>
            public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
            {
                // リストデータから該当1行分のデータ取得
                var item = _listData[position];

                // 情報取得
                var menuName = item[NAME_KEY] as string;
                var menuPrice = item[PRICE_KEY] as decimal?;
                var menuPriceStr = menuPrice?.ToString();

                // viewholder に設定
                var holder = viewHolder as RecyclerListViewHolder;
                holder.TvMenuName.Text = menuName;
                holder.TvMenuPrice.Text = menuPriceStr;

                //// クリックされた時の処理を追加しておく
                //// https://sodocumentation.net/ja/xamarin-android/topic/3452/recyclerview
                //holder.ItemView.Click -= OnViewClick;
                //holder.ItemView.Click += OnViewClick;
            }

            public override int ItemCount
            {
                get
                {
                    return _listData.Count;
                }
            }
        }
    }
}