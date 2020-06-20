using System;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Locations;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Java.Interop;
using Java.Net;

namespace ImplicitIntentSample
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, ILocationListener
    {
        /// <summary>
        /// 緯度
        /// </summary>
        private double _latitude = 0.0;

        /// <summary>
        /// 経度
        /// </summary>
        private double _longitude = 0.0;

        /// <summary>
        /// 位置情報取得許可リクエストコード
        /// </summary>
        private const int REQUEST_CODE_LOCATION = 1000;

        /// <summary>
        /// アクティビティ生成時の処理
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            //許可を確認する
            //Android版とは定数定義の場所が微妙に違う
            //https://forums.xamarin.com/discussion/86250/packagemanger-does-not-contain-definition-of-permission-granted
            if (!CanAccessLocation())
            {
                //許可がない場合ダイアログを出して許可を求める
                var permissions = new string[] { Manifest.Permission.AccessFineLocation };
                ActivityCompat.RequestPermissions(this, permissions, REQUEST_CODE_LOCATION);
                return;
            }
            //位置情報の追跡を開始する
            StartWatchLocationChange();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            //位置情報取得リクエストに関する処理
            if (requestCode == REQUEST_CODE_LOCATION && grantResults[0] == Permission.Granted)
            {
                if(!CanAccessLocation())
                {
                    return;
                }
                StartWatchLocationChange();
            }
        }

        private bool CanAccessLocation()
        {
            bool res = (ActivityCompat.CheckSelfPermission(this.ApplicationContext, Manifest.Permission.AccessFineLocation))
                == Permission.Granted;
            return res;
        }

        private void StartWatchLocationChange()
        {
            var locationManager = this.GetSystemService(Context.LocationService) as LocationManager;

            //GpsProvider 指定だと、GPSが効かない室内で試した時ほとんどOnLocationChangedイベントが発生しない
            //また、第2，3引数が両方0だとやはりなぜかOnLocationChangedイベントが発生しない(原因未調査。0=常に取得 だと、電池消費が激しくなるからダメなのかもしれない)
            //そもそもFusedLocationProviderClient を使うべきなのかもしれない
            //参考：https://tono-n-chi.com/blog/2017/10/locationmanager-onlocationchanged-is-not-called/
            //https://docs.microsoft.com/ja-jp/xamarin/android/platform/maps-and-location/location

            //エミュレータだとProviderが無いせいか、ここで落ちる
            //locationManager.RequestLocationUpdates(LocationManager.GpsProvider, 0, 0f, this);
            //locationManager.RequestLocationUpdates(LocationManager.NetworkProvider, 0, 0f, this);
            locationManager.RequestLocationUpdates(LocationManager.NetworkProvider, 2000, 1, this);
        }

        /// <summary>
        /// 地図検索ボタンクリック時の処理
        /// ※XMLからのイベントハンドラになるためにはExportタグを書く必要がある！
        /// https://stackoverflow.com/questions/37800931/xamarin-xml-androidonclick-callback-method
        /// </summary>
        /// <param name="view"></param>
        [Export(nameof(OnMapSearchButtonClick))]
        public void OnMapSearchButtonClick(View view)
        {
            //キーワード文字列取得→URLエンコード
            var etSearchWord = this.FindViewById<EditText>(Resource.Id.etSearchWord);
            var searchWord = etSearchWord.Text.ToString();
            searchWord = URLEncoder.Encode(searchWord, "UTF-8");

            //地図アプリと連携するURIオブジェクトをインテントに渡す
            var uriStr = $"geo:0,0?q={searchWord}";
            var uri = Android.Net.Uri.Parse(uriStr);
            //第1引数のEnumがアクション種別を表している。ActionViewは画面表示。
            var intent = new Intent(Intent.ActionView, uri);
            this.StartActivity(intent);
        }

        /// <summary>
        /// 地図表示ボタンタップ時の処理
        /// </summary>
        /// <param name="view"></param>
        [Export(nameof(OnMapShowCurrentButtonClick))]
        public void OnMapShowCurrentButtonClick(View view)
        {
            //保持済みの緯度経度からURIを作成
            //0.0 の時素直に文字列にすると0 になった → この状態でURI作成しても地図アプリ側で反応してくれなかった…(Android7.0)
            //var uriStr = $"geo:{_latitude},{_longitude}";
            var uriStr = $"geo:{_latitude.ToString("0.00000")},{_longitude.ToString("0.00000")}";
            var uri = Android.Net.Uri.Parse(uriStr);
            //インテント発行して地図アプリを起動する
            var intent = new Intent(Intent.ActionView, uri);
            this.StartActivity(intent);
        }

        /// <summary>
        /// 位置情報変化時の処理
        /// </summary>
        /// <param name="location"></param>
        void ILocationListener.OnLocationChanged(Location location)
        {
            //緯度経度取得し、セット
            _latitude = location.Latitude;
            _longitude = location.Longitude;
            //表示に反映
            var tvLatitude = this.FindViewById<TextView>(Resource.Id.tvLatitude);
            tvLatitude.Text = _latitude.ToString();
            var tvLongitude = this.FindViewById<TextView>(Resource.Id.tvLongitude);
            tvLongitude.Text = _longitude.ToString();
        }

        void ILocationListener.OnProviderDisabled(string provider)
        {
        }

        void ILocationListener.OnProviderEnabled(string provider)
        {
        }

        void ILocationListener.OnStatusChanged(string provider, Availability status, Bundle extras)
        {
        }
    }
}
