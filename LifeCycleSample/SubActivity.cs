using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Interop;
using Xamarin.Essentials;

namespace LifeCycleSample
{
    [Activity(Label = "SubActivity")]
    public class SubActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Log.Info(AppInfo.Name, $"{nameof(SubActivity)} {nameof(OnCreate)} called.");

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_sub);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnStart()
        {
            Log.Info(AppInfo.Name, $"{nameof(SubActivity)} {nameof(OnStart)} called.");
            base.OnStart();
        }

        protected override void OnRestart()
        {
            Log.Info(AppInfo.Name, $"{nameof(SubActivity)} {nameof(OnRestart)} called.");
            base.OnRestart();
        }

        protected override void OnResume()
        {
            Log.Info(AppInfo.Name, $"{nameof(SubActivity)} {nameof(OnResume)} called.");
            base.OnResume();
        }

        protected override void OnPause()
        {
            Log.Info(AppInfo.Name, $"{nameof(SubActivity)} {nameof(OnPause)} called.");
            base.OnPause();
        }

        protected override void OnStop()
        {
            Log.Info(AppInfo.Name, $"{nameof(SubActivity)} {nameof(OnStop)} called.");
            base.OnStop();
        }

        protected override void OnDestroy()
        {
            Log.Info(AppInfo.Name, $"{nameof(SubActivity)} {nameof(OnDestroy)} called.");
            base.OnDestroy();
        }

        /// <summary>
        /// 前の画面表示ボタンタップ時の動作
        /// </summary>
        /// <param name="view"></param>
        [Export(nameof(OnButtonClick))]
        public void OnButtonClick(View view)
        {
            this.Finish();
        }
    }
}