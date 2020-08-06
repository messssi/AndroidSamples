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

        /// <summary>
        /// DBアクセス用クラス
        /// </summary>
        private DatabaseHelper _helper = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainActivity()
        {
            // 宣言時だと自分自身を渡せないため、コンストラクタ内に記載した
            this._helper = new DatabaseHelper(this);
        }

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

            // DBに感想データがあれば表示
            // SQL実行
            // 実際にはべた書きしない…
            var db = this._helper.WritableDatabase;
            var sql = $"SELECT * FROM cocktailmemos WHERE _id = {_cocktailId}";
            var cursor = db.RawQuery(sql, null);

            // 以下でも可
            //var param = new string[] { _cocktailId.ToString() };
            //var cursor = db.Query("cocktailmemos", null, "_id = ?", param, null, null, null);

            // データ取得　Androidではカーソルでデータ取得する(取得結果表を指すイメージ)
            var note = "";
            while (cursor.MoveToNext())
            {
                var idxNote = cursor.GetColumnIndex("note");
                note = cursor.GetString(idxNote);
            }

            // 表示
            var etNote = this.FindViewById<EditText>(Resource.Id.etNote);
            etNote.Text = note;
        }

        /// <summary>
        /// 保存ボタンクリック時の処理
        /// </summary>
        /// <param name="view"></param>
        [Export(nameof(OnSaveButtonClick))]
        public void OnSaveButtonClick(View view)
        {
            var etNote = this.FindViewById<EditText>(Resource.Id.etNote);
            var note = etNote.Text;

            var db = this._helper.WritableDatabase;
            // 削除してからインサート(実際やるときにはViewでDB処理すべきではない)
            // まず削除
            var sqlDelete = "DELETE FROM cocktailmemos WHERE _id = ?";
            var stmt = db.CompileStatement(sqlDelete);
            stmt.BindLong(1, this._cocktailId);
            // CRUD/テーブル作成削除 など用途に応じて呼び出すメソッドを分けるようだ
            stmt.ExecuteUpdateDelete();

            // インサート
            // こーゆーこともあるし、実際やるとき文字列べた書きはやめるべき…
            //var sqlInsert = "INSERT INTO cocktailmemo (_id, name, note) VALUES (?, ?, ?)";
            var sqlInsert = "INSERT INTO cocktailmemos (_id, name, note) VALUES (?, ?, ?)";
            // 使いまわさなくてもいい気はするが…
            stmt = db.CompileStatement(sqlInsert);
            stmt.BindLong(1, _cocktailId);
            stmt.BindString(2, _cocktailName);
            stmt.BindString(3, note);
            stmt.ExecuteInsert();

            // Viewのリセット

            // 感想の入力値リセット
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

        protected override void OnDestroy()
        {
            // DBヘルパーを閉じておく
            this._helper.Close();
            base.OnDestroy();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}