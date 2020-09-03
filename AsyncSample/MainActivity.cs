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
                // Livedoor天気終了に伴いIDも変更
                ("大阪", "1853909"),
                ("神戸", "1859171"),
                ("豊岡", "1849831"),
                ("京都", "1857910"),
                ("舞鶴", "1857766"),
                ("奈良", "1855612"),
                ("和歌山", "1926004"),
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

            // AsyncTaskを使うサンプル
            //WeatherInfoReceiverインスタンスを生成。
            var receiver = new OpenWeatherMapInfoReceiver(this);
            //WeatherInfoReceiverを実行。
            receiver.Execute(cityId);

            // AsyncTaskを使わないサンプル
            ////↓試しにAsyncTaskを使わずに通常のTask.Runを試してみた。問題なく動く様子
            //var receiver = new OpenWeatherMapInfoReceiver2(this);
            //Task.Run(() => receiver.UpdateWeatherInfo(cityId));
        }
    }
}
