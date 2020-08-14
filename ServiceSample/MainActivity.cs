using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views;
using System;
using Android.Content;

namespace ServiceSample
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

            //イベントハンドラ設定(XML側に定義するやり方がなんかうまくいかない…→イベントハンドラメソッドにExportタグ追加、Mono.Android.Exportを参照追加と後で知った
            var btPlay = this.FindViewById<Button>(Resource.Id.btPlay);
            var btStop = this.FindViewById<Button>(Resource.Id.btStop);
            btPlay.Click += onPlayButtonClick;
            btStop.Click += onStopButtonClick;

            //引継ぎデータ取得
            var fromNotif = Intent.GetBooleanExtra(Constants.DATA_FROM_NOTIFICATION, false);
            //通知タップからの時、再生/停止ボタンを制御する
            if (fromNotif)
            {
                btPlay.Enabled = false;
                btStop.Enabled = true;
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        /// <summary>
        /// 再生ボタンクリック時の処理
        /// </summary>
        public void onPlayButtonClick(object sender, EventArgs e)
        {
            //呼び出し方の参考
            //http://furuya02.hatenablog.com/entry/20140503/1399767382
            var intent = new Intent(ApplicationContext, typeof(SoundManageService));
            StartService(intent);
            //ボタン制御
            var btPlay = this.FindViewById<Button>(Resource.Id.btPlay);
            var btStop = this.FindViewById<Button>(Resource.Id.btStop);
            btPlay.Enabled = false;
            btStop.Enabled = true;
        }

        /// <summary>
        /// 停止ボタンクリック時の処理
        /// </summary>
        public void onStopButtonClick(object sender, EventArgs e)
        {
            var intent = new Intent(ApplicationContext, typeof(SoundManageService));
            this.StopService(intent);
            //ボタン制御
            var btPlay = this.FindViewById<Button>(Resource.Id.btPlay);
            var btStop = this.FindViewById<Button>(Resource.Id.btStop);
            btPlay.Enabled = true;
            btStop.Enabled = false;
        }
    }
}