using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.Collections.Generic;
using Android.Content;
using static IntentSample.Constants;

namespace IntentSample
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

            // adaptor生成
            var from = new string[] { NAME_KEY, PRICE_KEY };
            var to = new int[] { Android.Resource.Id.Text1, Android.Resource.Id.Text2 };
            //↓は error CS0117: 'Resource.Id' に 'text1' の定義がありません と言われてエラーになった
            //var to = new int[] { Resource.Id.text1, Resource.Id.text2 };
            var adaptor = new SimpleAdapter(this.ApplicationContext, menuList,
                Android.Resource.Layout.SimpleListItem2, from, to);
            lvMenu.Adapter = adaptor;

            // クリックイベントハンドラ登録
            lvMenu.ItemClick += OnListItemClick;
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

        private void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //タップされた行のデータ取得
            var item = e.Parent.GetItemAtPosition(e.Position) as IDictionary<string, object>;
            var name = item[NAME_KEY] as string;
            var price = item[PRICE_KEY] as string;

            //インテント生成→画面起動
            var intent = new Intent(this.ApplicationContext, typeof(MenuThanksActivity));
            intent.PutExtra(NAME_KEY, name);
            intent.PutExtra(PRICE_KEY, price);
            this.StartActivity(intent);
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}