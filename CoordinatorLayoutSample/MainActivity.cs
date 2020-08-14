using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.Design.Widget;
using Android.Graphics;

namespace CoordinatorLayoutSample
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
            toolbar.SetLogo(Resource.Mipmap.ic_launcher);
            //タイトル設定は↓どちらでもOK
            //toolbar.Title = Resources.GetText(Resource.String.toolbar_title);
            toolbar.SetTitle(Resource.String.toolbar_title);
            toolbar.SetSubtitle(Resource.String.toolbar_subtitle);
            SetSupportActionBar(toolbar);

            // CollapsingToolbarLayoutに設定
            var toolbarLayout = FindViewById<CollapsingToolbarLayout>(Resource.Id.toolbarLayout);
            toolbarLayout.Title = Resources.GetText(Resource.String.toolbar_title);
            //引数はintのはずなのだが、なぜか↓でコンパイルエラーにならないし動く　
            toolbarLayout.SetExpandedTitleColor(Color.White);
            toolbarLayout.SetCollapsedTitleTextColor(Color.Gray);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}