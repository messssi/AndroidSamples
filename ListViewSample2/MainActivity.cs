using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.Collections.Generic;
using IList = System.Collections.IList;

namespace ListViewSample2
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

            var lvMenu = this.FindViewById<ListView>(Resource.Id.lv_menu);
            //adaptor生成
            var adaptor = new ArrayAdapter(this.ApplicationContext,
                Android.Resource.Layout.SimpleListItem1, createListForAdapter());
            lvMenu.Adapter = adaptor;
            //イベントハンドラ設定
            lvMenu.ItemClick += OnItemClick;
        }

        /// <summary>
        /// リスト表示用データを生成する
        /// </summary>
        /// <returns></returns>
        private IList createListForAdapter()
        {
            return new List<string>()
            {
                "から揚げ定食",
                "ハンバーグ定食",
                "生姜焼き定食",
                "ステーキ定食",
                "野菜炒め定食",
                "とんかつ定食",
                "ミンチかつ定食",
                "チキンカツ定食",
                "コロッケ定食",
                "焼き魚定食",
                "焼肉定食",
            };
        }

        private void OnItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //ダイアログ表示
            var dialogFragment = new OrderConfirmDialogFragment();
            //tag(第2引数)の使い道がいまいちよく分かっていない…
            dialogFragment.Show(this.SupportFragmentManager, "OrderConfirmDialogFragment");
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}