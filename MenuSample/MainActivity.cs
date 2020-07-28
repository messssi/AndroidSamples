using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.Collections.Generic;
using static MenuSample.Constants;
using Android.Content;

namespace MenuSample
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            var lvMenu = this.FindViewById<ListView>(Resource.Id.lvMenu);
            // リストデータ生成
            var menuList = CreateMenuList();

            // adapter生成
            var from = new string[] { NAME_KEY, PRICE_KEY };
            var to = new int[] { Resource.Id.tvMenuName, Resource.Id.tvMenuPrice };
            //↓は error CS0117: 'Resource.Id' に 'text1' の定義がありません と言われてエラーになった
            //var to = new int[] { Resource.Id.text1, Resource.Id.text2 };
            var adapter = new SimpleAdapter(this.ApplicationContext, menuList,
                Resource.Layout.row, from, to);
            lvMenu.Adapter = adapter;

            // クリックイベントハンドラ登録
            lvMenu.ItemClick += OnListItemClick;
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
        private void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //タップされた行のデータ取得
            var item = e.Parent.GetItemAtPosition(e.Position) as IDictionary<string, object>;
            var name = item[NAME_KEY] as string;
            var price = (decimal)item[PRICE_KEY];

            //インテント生成→画面起動
            var intent = new Intent(this.ApplicationContext, typeof(MenuThanksActivity));
            intent.PutExtra(NAME_KEY, name);
            intent.PutExtra(PRICE_KEY, $"{price}円");
            this.StartActivity(intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}