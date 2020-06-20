﻿using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Support.V7.Widget;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using static Android.Resource;

namespace ToolbarSample
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

            //ツールバー設定
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            //このロゴ設定はちゃんと効く
            toolbar.SetLogo(Resource.Mipmap.ic_launcher);
            //↓どちらでもOK
            //toolbar.Title = Resources.GetText(Resource.String.toolbar_title);
            toolbar.SetTitle(Resource.String.toolbar_title);
            //↓これがよくない…？ -> 一旦除外
            //toolbar.SetTitleTextColor(Color.White);
            toolbar.SetSubtitle(Resource.String.toolbar_subtitle);            
            ////書籍では LTGRAY だったが、無いようだ
            ///↓もなんかダメっぽい -> 一旦除外
            //toolbar.SetSubtitleTextColor(Color.DarkerGray);
            SetSupportActionBar(toolbar);

            //ActionBar.Title = Resources.GetText(Resource.String.toolbar_title);
            //SupportActionBar.Title = Resources.GetText(Resource.String.toolbar_title);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}