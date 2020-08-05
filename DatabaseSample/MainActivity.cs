using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Java.Interop;
using Android.Views;
using Java.Lang;

namespace DatabaseSample
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        /// <summary>
        /// 選択されたカクテルの主キーID
        /// </summary>
        private int _cocktailId = -1;

        /// <summary>
        /// 選択されたカクテル名
        /// </summary>
        private string _cocktailName = "";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            var lvCocktail = this.FindViewById<ListView>(Resource.Id.lvCocktail);
            lvCocktail.ItemClick += OnCocktailListItemClicked;
        }

        /// <summary>
        /// カクテルリストアイテムクリック時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCocktailListItemClicked(object sender, AdapterView.ItemClickEventArgs e)
        {
            // タップされた行のデータを保持
            this._cocktailId = e.Position;
            // Java.Lang.String ≠ string
            // 変換はToStringでいいっぽい
            var name = e.Parent.GetItemAtPosition(e.Position) as String;
            this._cocktailName = name.ToString();

            // カクテル名表示
            var tvCocktailName = this.FindViewById<TextView>(Resource.Id.tvCocktailName);
            tvCocktailName.Text = _cocktailName;

            // 保存できるように
            var btnSave = this.FindViewById<Button>(Resource.Id.btnSave);
            btnSave.Enabled = true;
        }

        /// <summary>
        /// 保存ボタンクリック時の処理
        /// </summary>
        /// <param name="view"></param>
        [Export(nameof(OnSaveButtonClick))]
        public void OnSaveButtonClick(View view)
        {
            // 感想の入力値リセット
            var etNote = this.FindViewById<EditText>(Resource.Id.etNote);
            // コンパイルエラー回避
            // https://stackoverflow.com/questions/37062756/xamarin-setting-text-for-a-textview-programmatically
            // etNote.SetText("");
            etNote.Text = "";

            // カクテル名を「未選択」に変更
            var tvCocktailName = this.FindViewById<TextView>(Resource.Id.tvCocktailName);
            tvCocktailName.Text = this.GetString(Resource.String.tv_name);

            // 保存ボタン押せないように
            var btnSave = this.FindViewById<Button>(Resource.Id.btnSave);
            btnSave.Enabled = false;
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}