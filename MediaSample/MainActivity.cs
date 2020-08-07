using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Media;
using Android.Net;
using Android.Util;
using Java.Interop;
using Android.Views;

namespace MediaSample
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, MediaPlayer.IOnPreparedListener, MediaPlayer.IOnCompletionListener
    {
        private MediaPlayer _player = null;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            // メディア再生準備
            this._player = new MediaPlayer();
            var mediaFileUriStr = $"android.resource://{this.PackageName}/{Resource.Raw.bath_out}";
            var mediaFileUri = Uri.Parse(mediaFileUriStr);
            try
            {
                _player?.SetDataSource(this.ApplicationContext, mediaFileUri);
                _player?.SetOnPreparedListener(this);
                _player?.SetOnCompletionListener(this);
                _player?.PrepareAsync();
            }
            catch (System.Exception ex)
            {
                Log.Error("MediaSample", "メディアプレーヤー準備時の例外発生", ex);
            }

            // スイッチのリスナ設定
            var loopSwitch = this.FindViewById<Switch>(Resource.Id.swLoop);
            loopSwitch.CheckedChange += OnLoopSwitchCheckedChanged;
        }

        /// <summary>
        /// ループスイッチチェック切り替わり時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoopSwitchCheckedChanged(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            // ↓は残念ながらコンパイルエラーになった
            // this._player?.Looping = e.IsChecked;
            if (this._player == null)
            {
                return;
            }
            this._player.Looping = e.IsChecked;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        /// <summary>
        /// 再生準備完了時の処理
        /// </summary>
        /// <param name="mp"></param>
        public void OnPrepared(MediaPlayer mp)
        {
            var btPlay = this.FindViewById<Button>(Resource.Id.btPlay);
            btPlay.Enabled = true;
            var btBack = this.FindViewById<Button>(Resource.Id.btBack);
            btBack.Enabled = true;
            var btForward = this.FindViewById<Button>(Resource.Id.btForward);
            btForward.Enabled = true;
        }

        public void OnCompletion(MediaPlayer mp)
        {
            if (this._player == null)
            {
                return;
            }
            if (!this._player.Looping)
            {
                // 再生ボタンの表示テキスト更新
                var btPlay = this.FindViewById<Button>(Resource.Id.btPlay);
                btPlay.Text = this.Resources.GetString(Resource.String.bt_play_play);
            }
        }

        /// <summary>
        /// 再生ボタンクリック時の処理
        /// </summary>
        /// <param name="view"></param>
        [Export(nameof(OnPlayButtonClick))]
        public void OnPlayButtonClick(View view)
        {
            if (this._player == null)
            {
                return;
            }

            var btPlay = this.FindViewById<Button>(Resource.Id.btPlay);
            if (_player.IsPlaying)
            {
                // 単純に再生を停止するときはPause!
                _player.Pause();
                btPlay.Text = this.Resources.GetString(Resource.String.bt_play_play);
            }
            else
            {
                _player.Start();
                btPlay.Text = this.Resources.GetString(Resource.String.bt_play_pause);
            }
        }

        /// <summary>
        /// アクティビティ破棄時の処理
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();

            // メディアプレーヤーの解放
            if (this._player == null)
            {
                return;
            }
            if (this._player.IsPlaying)
            {
                this._player.Stop();
            }
            this._player.Release();
            this._player = null;
        }

        /// <summary>
        /// 戻るボタンクリック時の処理
        /// </summary>
        /// <param name="view"></param>
        [Export(nameof(OnBackButtonClick))]
        public void OnBackButtonClick(View view)
        {
            _player?.SeekTo(0);
        }

        /// <summary>
        /// 進むボタンクリック時の処理
        /// </summary>
        /// <param name="view"></param>
        [Export(nameof(OnForwardButtonClick))]
        public void OnForwardButtonClick(View view)
        {
            if (this._player == null)
            {
                return;
            }

            // Kotlin の let 風に使うならこうなるのかな…？と思ったけど、using使うとDisposeされちゃうからダメだな
            // 単純に変数代入でいいか…
            var it = this._player;
            var duration = it.Duration;
            it.SeekTo(duration);
            if (!it.IsPlaying)
            {
                it.Start();
            }
        }
    }
}