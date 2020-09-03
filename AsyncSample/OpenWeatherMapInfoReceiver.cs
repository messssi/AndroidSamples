using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Net;

namespace AsyncSample
{
    /// <summary>
    /// OpenWeatherMapから情報を取得して画目更新するクラス
    /// …の、AsyncTaskを継承する版(書籍のC#翻訳版)
    /// </summary>
    public class OpenWeatherMapInfoReceiver : AsyncTask<String, String, String>
    {
        Activity _activity = null;

        /// <summary>
        /// OpenWeatherMapのAPIにアクセスするためのキー
        /// 登録したら発行されるので、差し替えること
        /// </summary>
        private const string API_KEY = "YOUR_OWN_KEY";
        private const string BASE_URL = "http://api.openweathermap.org/data/2.5/forecast";

        public OpenWeatherMapInfoReceiver(Activity parent)
        {
            this._activity = parent;
        }

        protected override string RunInBackground(params string[] @params)
        {
            //可変長引数の1個目(インデックス0)を取得。これが都市ID
            var id = @params[0];
            //都市IDを使って接続URL文字列を作成。
            //!!! 2020/7/31でサービス終了したらしい… https://help.livedoor.com/weather/index.html 今はもう動かない(というか、情報取れない)…
            //var urlStr = "http://weather.livedoor.com/forecast/webservice/json/v1?city=${id}";
            var urlStr = $"{BASE_URL}?id={id}&APPID={API_KEY}";

            //URLオブジェクトを生成。
            var url = new URL(urlStr);
            //URLオブジェクトからHttpURLConnectionオブジェクトを取得。
            var con = url.OpenConnection() as Java.Net.HttpURLConnection;
            //http接続メソッドを設定。
            con.RequestMethod = "GET";

            //接続。
            con.Connect();

            //HttpURLConnectionオブジェクトからレスポンスデータを取得。天気情報が格納されている。
            var stream = con.InputStream;
            //レスポンスデータであるInputStreamオブジェクトを文字列(JSON文字列)に変換。
            var result = is2String(stream);
            //HttpURLConnectionオブジェクトを解放。
            con.Disconnect();
            //InputStreamオブジェクトを解放。
            stream.Close();

            //JSON文字列を返す。
            return result;
        }

        /// <summary>
        /// InputStreamオブジェクトを文字列に変換するメソッド。変換文字コードはUTF-8。
        /// </summary>
        /// <param name="stream">変換対象のInputStreamオブジェクト</param>
        /// <returns>変換された文字列</returns>
        private string is2String(System.IO.Stream stream)
        {
            var sb = new Java.Lang.StringBuilder();

            var reader = new Java.IO.BufferedReader(new Java.IO.InputStreamReader(stream, "UTF-8"));

            var line = reader.ReadLine();
            while (line != null)
            {
                sb.Append(line);
                line = reader.ReadLine();
            }
            reader.Close();
            return sb.ToString();
        }

        protected override void OnPostExecute(string result)
        {
            //JSON文字列からJSONObjectオブジェクトを生成。これをルートJSONオブジェクトとする。
            var rootJSON = new Org.Json.JSONObject(result);
            var listJSON = rootJSON.GetJSONArray("list");
            // とりあえずサンプルとして、最初の情報を取る(当日9時の情報っぽい)
            var firstJSON = listJSON.GetJSONObject(0);
            //var lastJSON = listJSON.GetJSONObject(listJSON.Length() - 1);
            var weatherJSON = firstJSON.GetJSONArray("weather").GetJSONObject(0);
            var telop = weatherJSON.GetString("main");
            var desc = weatherJSON.GetString("description");

            //天気情報用文字列をTextViewにセット。
            var tvWeatherTelop = this._activity.FindViewById<TextView>(Resource.Id.tvWeatherTelop);
            var tvWeatherDesc = this._activity.FindViewById<TextView>(Resource.Id.tvWeatherDesc);
            tvWeatherTelop.Text = telop;
            tvWeatherDesc.Text = desc;
        }
    }
}
