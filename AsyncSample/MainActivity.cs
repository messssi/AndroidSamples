using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.Collections.Generic;
using System;
using Java.Interop;
using Android.Views;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Linq;

namespace AsyncSample
{
    /// <summary>
    /// 非同期でお天気情報を取ってきて表示するサンプル
    /// https://wings.msn.to/index.php/-/A-03/978-4-7981-6044-3
    /// ↑の11章をXamarin.Android に翻訳しながら進めた。所要時間約3時間…
    /// </summary>
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        const string FROM_KEY = "name";
        const string ID_KEY = "id";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            var lvCityList = this.FindViewById(Resource.Id.lvCityList) as ListView;
            //var cityList = new List<(string, string)>()
            //{
            //    ("大阪", "270000"),
            //    ("神戸", "280010"),
            //};
            //なんか違う気もするが…
            //var cityList = new List<IDictionary<string, object>>()
            //List ではなくJavaList　という情報あり…だけど、落ちてるポイントはそこではないようだ
            //https://stackoverflow.com/questions/11347582/using-simpleadapter-with-mono-for-android
            //中身のDictionaryをJavaDictionaryにすれば落ちないようになった
            //https://subscription.packtpub.com/book/application_development/9781784398576/4/ch04lvl1sec44/using-a-simpleadapter
            //var cityList = new JavaList<IDictionary<string, object>>()
            //adapter生成
            var from = new string[] { FROM_KEY };
            var to = new int[] { Android.Resource.Id.Text1 };
            var adapter = new SimpleAdapter(this.ApplicationContext, createCityListForAdapter(),
                Android.Resource.Layout.SimpleExpandableListItem1, from, to);
            lvCityList.Adapter = adapter;
            //リストタップのリスナ登録 -> C#だと単にイベントハンドラ追加するだけらしい
            //https://forums.xamarin.com/discussion/7843/onitemclicklistener-convert-java-to-c
            //lvCityList.OnItemClickListener = 
            lvCityList.ItemClick += OnItemClick;
        }

        /// <summary>
        /// アダプタに渡すための都市情報リストを生成する
        /// </summary>
        /// <returns></returns>
        private static List<IDictionary<string, object>> createCityListForAdapter()
        {
            var data = new List<(string name, string id)>()
            {
                ("大阪", "270000"),
                ("神戸", "280010"),
                ("豊岡", "280020"),
                ("京都", "260010"),
                ("舞鶴", "260020"),
                ("奈良", "290010"),
                ("風屋", "290020"),
                ("和歌山", "300010"),
                ("潮岬", "300020"),
            };
            var cityList = new List<IDictionary<string, object>>();
            foreach (var d in data)
            {
                cityList.Add(new JavaDictionary<string, object>()
                {
                    { FROM_KEY, d.name },
                    { ID_KEY, d.id },
                });
            }
            return cityList;
        }

        //private void OnItemClick(object sender, AdapterView.ItemClickEventArgs e)
        private void OnItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //タップされた行の都市名とID取得
            //Xamarinでの取得参考：http://furuya02.hatenablog.com/entry/20140430/1399767387
            var item = e.Parent.GetItemAtPosition(e.Position) as IDictionary<string, object>;
            //取得した都市名を設定
            var cityName = item[FROM_KEY];
            var cityId = item[ID_KEY] as string;
            var tvCityName = this.FindViewById(Resource.Id.tvCityName) as TextView;
            tvCityName.Text = $"{cityName}の天気：";

            //情報更新
            Task.Run(() => UpdateWeatherInfo(cityId));
        }

        private class WeatherInfo
        {
            public string Telop { get; set; }
            public string Desc { get; set; }
        }

        private async Task UpdateWeatherInfo(string cityId)
        {
            //情報取得
            var info = FetchWeatherInfo(cityId);
            
            var tvWeatherTelop = this.FindViewById<TextView>(Resource.Id.tvWeatherTelop);
            var tvWeatherDesc = this.FindViewById<TextView>(Resource.Id.tvWeatherDesc);
            //tvWeatherTelop.Text = info.Telop;
            //tvWeatherDesc.Text = info.Desc;
            //Viewに反映(UIスレッド呼ばないとダメ)
            //https://docs.microsoft.com/ja-jp/xamarin/android/app-fundamentals/writing-responsive-apps
            this.RunOnUiThread(() =>
            {
                tvWeatherTelop.Text = info.Telop;
                tvWeatherDesc.Text = info.Desc;
            });
        }

        /// <summary>
        /// https://qiita.com/ronkabu/items/997c4eee40e4668951a1
        /// </summary>
        static RestSharp.RestClient client = new RestSharp.RestClient("http://weather.livedoor.com/forecast/webservice/json/v1");

        private WeatherInfo FetchWeatherInfo(string cityId)
        {
            //string urlStr = $"http://weather.livedoor.com/forecast/webservice/json/v1?city={cityId}";
            //// HttpClientの作成 
            //var httpClient = new HttpClient();
            //// 非同期でAPIからデータを取得
            //Task<string> stringAsync = httpClient.GetStringAsync(urlStr);
            //string result = await stringAsync;
            //// JSON形式のデータをデシリアライズ

            var request = new RestSharp.RestRequest();
            request.AddQueryParameter("city", cityId);
            request.RequestFormat = RestSharp.DataFormat.Json;
            var response = client.Execute<LDTenki>(request);
            var tenki = JsonConvert.DeserializeObject<LDTenki>(response.Content);
            return new WeatherInfo()
            {
                Telop = tenki.forecasts.FirstOrDefault()?.telop,
                Desc = tenki.description?.text,
            };
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}