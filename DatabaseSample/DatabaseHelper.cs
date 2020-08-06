using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Database.Sqlite;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DatabaseSample
{
    /// <summary>
    /// DBヘルパークラス
    /// 参考： https://rksoftware.hatenablog.com/entry/2018/01/01/024717
    /// </summary>
    public class DatabaseHelper : SQLiteOpenHelper
    {
        private const string DATABASE_NAME = "cocktailmemo.db";
        private const int DATABASE_VERSION = 1;

        /// <summary>
        /// コンストラクタ
        /// ※基底クラス処理のうちバージョンについて
        /// 　端末内部のDBのバージョンの方が若い番号だったらOnUpgradeが実行されるようになっている
        /// </summary>
        /// <param name="context"></param>
        public DatabaseHelper(Context context) : base(context, DATABASE_NAME, null, DATABASE_VERSION)
        {

        }

        /// <summary>
        /// デバイス内に端末が存在しないとき1回だけ実行される処理
        /// </summary>
        /// <param name="db"></param>
        public override void OnCreate(SQLiteDatabase db)
        {
            // テーブル作成用SQL文字列作成(実際やるときはこんなべた書きしない…はず)
            var sb = new StringBuilder();
            sb.Append("CREATE TABLE cocktailmemos (");
            sb.Append("_id INTEGER PRIMARY KEY,");
            sb.Append("name TEXT,");
            sb.Append("note TEXT");
            sb.Append(");");
            var sql = sb.ToString();

            // SQL実行
            db.ExecSQL(sql);
        }

        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
        }
    }
}