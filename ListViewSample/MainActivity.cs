using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views;
using Java.Lang;

namespace ListViewSample
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

            //イベントハンドラ設定
            var lvMenu = this.FindViewById<ListView>(Resource.Id.lv_menu);
            lvMenu.ItemClick += OnItemClick;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        /// <summary>
        /// クリックイベントを拾う処理(Listenerの代わり)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            // ItemClickEventArgs に java/kotlinで言うところの
            // AdapterView.OnItemClickListener の onItemClick の引数一式が入っているようだ
            var item = e.Parent.GetItemAtPosition(e.Position) as String;
            var show = "あなたが選んだメニュー: " + item;
            Toast.MakeText(this.ApplicationContext, show, ToastLength.Long).Show();
        }
    }
}