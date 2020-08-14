using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Interop;
using Java.IO;
using Java.Lang;
using Java.Net;

namespace ServiceSample
{
    [Service]
    public class SoundManageService : Service, MediaPlayer.IOnPreparedListener
    {
        /// <summary>
        /// (継承の都合上実装しないといけないメソッド)
        /// </summary>
        /// <param name="intent"></param>
        /// <returns></returns>
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        /// <summary>
        /// 音楽再生のためのインスタンス
        /// </summary>
        private MediaPlayer _player;

        /// <summary>
        /// 通知チャネルID
        /// </summary>
        const string NOTIF_CHANNEL_ID = "soundmanageservice_notification_channel";

        public override void OnCreate()
        {
            //base.OnCreate();
            this._player = new MediaPlayer();
            CreateNotificationChannel();
        }

        /// <summary>
        /// 通知チャネルを作成する
        /// </summary>
        private void CreateNotificationChannel()
        {
            // 通知チャネルを作成する
            var id = NOTIF_CHANNEL_ID;
            // var name = Resource.String.notification_channel_name;
            var name = Resources.GetText(Resource.String.notification_channel_name);
            // var importance = NotificationManager.ImportanceDefault;
            var importance = Android.App.NotificationImportance.Default;
            var channel = new NotificationChannel(id, name, importance);
            // 通知チャネルを設定する
            var manager = this.GetSystemService(Context.NotificationService) as NotificationManager;
            manager.CreateNotificationChannel(channel);
        }

        /// <summary>
        /// 開始コマンド処理
        /// </summary>
        /// <param name="intent"></param>
        /// <param name="flags"></param>
        /// <param name="startId"></param>
        /// <returns></returns>
        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            // 音声ファイルのURI文字列を作成
            var mediaFileUriStr = $"android.resource://{PackageName}/{Resource.Raw.jungle}";
            // URIオブジェクト生成
            var mediaFileUri = Android.Net.Uri.Parse(mediaFileUriStr);
            try
            {
                // メディアプレイヤーに音声ファイルを指定
                this._player?.SetDataSource(ApplicationContext, mediaFileUri);
                // 非同期での再生準備完了時のリスナ設定
                // _player?.SetOnPreparedListener(new PlayerPreparedListener());
                _player?.SetOnPreparedListener(this);

                // 再生終了時のリスナ設定
                // 参考：https://qiita.com/amay077/items/51c36e8ff65c90a7f43e
                // _player?.SetOnCompletionListener(new PlayerCompletionListener());
                // _player?.Completion だとコンパイルエラーになった…
                if (_player != null)
                {
                    _player.Completion += OnPlayerCompletion;
                }
                // メディア再生準備
                _player?.PrepareAsync();
            }
            catch (IllegalArgumentException ex)
            {
                Log.Error("ServiceSample", "メディアプレイヤー準備時の例外発生", ex);
            }
            catch (IOException ex)
            {
                Log.Error("ServiceSample", "メディアプレイヤー準備時の例外発生", ex);
            }
            //定数を返す(XamarinだとEnumになってた)
            return StartCommandResult.NotSticky;
        }

        private void OnPlayerCompletion(object sender, EventArgs e)
        {
            // 通知を作成するBuilderを生成
            // Xamarinでは以下のような感じ
            // https://docs.microsoft.com/ja-jp/xamarin/android/app-fundamentals/notifications/local-notifications-walkthrough
            var builder = new NotificationCompat.Builder(ApplicationContext, NOTIF_CHANNEL_ID)
                // 通知エリアに表示されるアイコン設定(アイコンが無いと通知を出そうとしたとき死ぬ)
                .SetSmallIcon(Resource.Mipmap.ic_launcher)
                // 通知ドロワーでの表示タイトルを設定
                .SetContentTitle(Resources.GetText(Resource.String.msg_notification_title_finish))
                // 通知ドロワーでの表示メッセージを設定
                .SetContentText(Resources.GetText(Resource.String.msg_notification_text_finish));
            // 通知
            Notify(builder, 0);

            // サービスを止める
            this.StopSelf();
        }

        public override void OnDestroy()
        {
            var target = _player;
            if (target == null)
            {
                return;
            }
            if (target.IsPlaying)
            {
                target.Stop();
            }
            target.Release();
            //インスタンス変数にもセットしておく
            _player = null;
        }

        public void OnPrepared(MediaPlayer mp)
        {
            // メディア再生
            mp.Start();

            // 再生開始通知の処理(終了通知と結構一緒だな…)
            var builder = new NotificationCompat.Builder(ApplicationContext, NOTIF_CHANNEL_ID)
                .SetSmallIcon(Resource.Mipmap.ic_launcher)
                .SetContentTitle(Resources.GetText(Resource.String.msg_notification_title_start))
                .SetContentText(Resources.GetText(Resource.String.msg_notification_text_start));

            // インテントnew する際は、Classを渡すためにtypeofを使う。起動先Activityクラスを指定する
            var intent = new Intent(this.ApplicationContext, typeof(MainActivity));
            // 引継ぎデータの格納
            intent.PutExtra(Constants.DATA_FROM_NOTIFICATION, true);

            // PendingIntent：指定されたタイミングで何かを起動するインテント
            // 第2引数：複数画面部品からPendingIntentを利用する際に、それらを区別するための番号
            var stopServiceIntent = PendingIntent.GetActivity(this.ApplicationContext, 0, intent, PendingIntentFlags.CancelCurrent);
            builder.SetContentIntent(stopServiceIntent);
            builder.SetAutoCancel(true);

            // 通知
            Notify(builder, 1);
        }

        private void Notify(NotificationCompat.Builder builder, int id)
        {
            var notification = builder.Build();
            var manager = this.GetSystemService(Context.NotificationService) as NotificationManager;
            manager.Notify(id, notification);
        }

        /// <summary>
        /// 再生準備完了時のリスナ
        /// 
        /// Java.Lang.Object継承についての参考：
        /// https://stackoverflow.com/questions/61129398/how-to-set-the-volume-to-0-in-xamarin-native-video-player
        /// </summary>
        private class PlayerPreparedListener : Java.Lang.Object, MediaPlayer.IOnPreparedListener
        {
            public void OnPrepared(MediaPlayer mp)
            {
                //メディア再生
                mp.Start();

                ////再生開始通知の処理
                //var builder = NotificationCompat.Builder(applicationcon)
            }
        }

        //XamarinというかC#では使わなくてもよかった
        ///// <summary>
        ///// 再生終了時のリスナ
        ///// </summary>
        //private class PlayerCompletionListener : Java.Lang.Object, MediaPlayer.IOnCompletionListener
        //{
        //    public void OnCompletion(MediaPlayer mp)
        //    {
        //        //自分自身を終了…したいが、このクラスだと無理じゃないか？                
        //    }
        //}
    }
}