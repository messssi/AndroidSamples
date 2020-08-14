using System;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Java.Interop;

namespace CameraIntentSample
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        /// <summary>
        /// カメラアプリ起動リクエストコード
        /// </summary>
        private const int REQUEST_CODE_CAMERA = 200;

        /// <summary>
        /// 保存された画像のUri
        /// ※Uriと名の付くクラスはいくつかあるが、Android固有の処理のためにはAndroid.NetのUriを使う必要がある
        /// </summary>
        private Android.Net.Uri? _imageUri = null;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            // カメラアプリで撮影成功→反映する
            if (requestCode == REQUEST_CODE_CAMERA && resultCode == Result.Ok)
            {
                //https://qiita.com/amay077/items/8752e7e5db233f5cc73f
                //GetParcelableExtra の使い方はAndroid本家とちょっと違う
                //var bitmap = data?.GetParcelableExtra<Bitmap>("data");
                //↑はNG、↓が正しい
                //var bitmap = data?.GetParcelableExtra("data") as Bitmap;
                // ただし、こうして取得できるBitMapは非常に小さいものしか取得できない
                // →一旦保存したものを使うようにする

                var ivCamera = this.FindViewById<ImageView>(Resource.Id.ivCamera);
                //ivCamera.SetImageBitmap(bitmap);
                ivCamera.SetImageURI(this._imageUri);
            }
        }

        [Export(nameof(OnCameraImageClick))]
        public void OnCameraImageClick(View view)
        {
            string permissionTarget = Manifest.Permission.WriteExternalStorage;
            if (!CheckPermission(this, permissionTarget))
            {
                //許可がない場合ダイアログを出して許可を求める
                var permissions = new string[] { permissionTarget };
                ActivityCompat.RequestPermissions(this, permissions, REQUEST_CODE_CAMERA);
                return;
            }

            var now = DateTime.Now;
            // 現在時間をフォーマットする
            // https://csharp.sql55.com/cookbook/format-datetime.php
            var nowStr = now.ToString("yyyyMMdd_HHmmss");
            // ファイル名定義
            var fileName = $"UseCameraActivityPhoto_{nowStr}";
            // 画像ファイル名、種類を設定
            var values = new ContentValues();
            values.Put(MediaStore.Images.Media.InterfaceConsts.Title, fileName);
            values.Put(MediaStore.Images.Media.InterfaceConsts.MimeType, "image/jpeg");
            //URI 生成
            _imageUri = ContentResolver.Insert(MediaStore.Images.Media.ExternalContentUri, values);

            var intent = new Intent(MediaStore.ActionImageCapture);
            //インテントに対してURIを設定
            intent.PutExtra(MediaStore.ExtraOutput, _imageUri);
            this.StartActivityForResult(intent, REQUEST_CODE_CAMERA);
        }

        private static bool CheckPermission(Activity activity, string target)
        {
            bool res = ActivityCompat.CheckSelfPermission(activity.ApplicationContext, target)
                == Permission.Granted;
            return res;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            //リクエストに関する処理
            if (requestCode == REQUEST_CODE_CAMERA && grantResults[0] == Permission.Granted)
            {
                // 許可とれていたら、再度カメラアプリを起動
                var ivCamera = this.FindViewById<ImageView>(Resource.Id.ivCamera);
                this.OnCameraImageClick(ivCamera);
            }
        }
    }
}
