﻿using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views;

namespace HelloSample
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, View.IOnClickListener
    {
        /// <summary>
        /// 出力時につけるサフィックス
        /// これ自体多言語対応してないが…今は気にしない
        /// </summary>
        private const string OUTPUT_SUFFIX = "さん、こんにちは！";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            //イベントハンドラ設定
            var btClick = this.FindViewById<Button>(Resource.Id.btClick);
            btClick.SetOnClickListener(this);

            var btClear = this.FindViewById<Button>(Resource.Id.btClear);
            btClear.SetOnClickListener(this);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        /// <summary>
        /// クリックイベントを拾う処理
        /// ※privateクラス等で View.IOnClickListener を実装してもできるかもしれないが、大量のメソッド実装を要求される
        /// →Activity自身で実装した方が楽
        /// </summary>
        /// <param name="v"></param>
        void View.IOnClickListener.OnClick(View v)
        {
            var input = this.FindViewById<EditText>(Resource.Id.etName);
            var output = this.FindViewById<TextView>(Resource.Id.tvOutput);

            switch (v.Id)
            {
                case Resource.Id.btClick:
                    //表示ボタン
                    //メッセージ表示
                    output.Text = input?.Text.ToString() + OUTPUT_SUFFIX;
                    break;
                case Resource.Id.btClear:
                    //クリアボタン
                    input.Text = "";
                    output.Text = "";
                    //↓一見クリアしてくれそうなメソッドがあったが、クリアされなかった
                    //input.ClearComposingText();
                    //output.ClearComposingText();
                    break;
                default:
                    break;
            }
        }
    }
}