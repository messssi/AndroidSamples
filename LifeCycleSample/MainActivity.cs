using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Util;
using Xamarin.Essentials;
using Android.Views;
using Java.Interop;
using Android.Content;

namespace LifeCycleSample
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Log.Info(AppInfo.Name, $"{nameof(MainActivity)} {nameof(OnCreate)} called.");

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        protected override void OnStart()
        {
            Log.Info(AppInfo.Name, $"{nameof(MainActivity)} {nameof(OnStart)} called.");
            base.OnStart();
        }

        protected override void OnRestart()
        {
            Log.Info(AppInfo.Name, $"{nameof(MainActivity)} {nameof(OnRestart)} called.");
            base.OnRestart();
        }

        protected override void OnResume()
        {
            Log.Info(AppInfo.Name, $"{nameof(MainActivity)} {nameof(OnResume)} called.");
            base.OnResume();
        }

        protected override void OnPause()
        {
            Log.Info(AppInfo.Name, $"{nameof(MainActivity)} {nameof(OnPause)} called.");
            base.OnPause();
        }

        protected override void OnStop()
        {
            Log.Info(AppInfo.Name, $"{nameof(MainActivity)} {nameof(OnStop)} called.");
            base.OnStop();
        }

        protected override void OnDestroy()
        {
            Log.Info(AppInfo.Name, $"{nameof(MainActivity)} {nameof(OnDestroy)} called.");
            base.OnDestroy();
        }

        /// <summary>
        /// 次の画面表示ボタンタップ時の動作
        /// </summary>
        /// <param name="view"></param>
        [Export(nameof(OnButtonClick))]
        public void OnButtonClick(View view)
        {
            var intent = new Intent(this.ApplicationContext, typeof(SubActivity));
            this.StartActivity(intent);
        }
    }
}