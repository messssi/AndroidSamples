using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using DialogFragment = Android.Support.V4.App.DialogFragment;
using AlertDialog = Android.Support.V7.App.AlertDialog;

namespace ListViewSample2
{
    /// <summary>
    /// ダイアログ
    /// http://www.fineblue206.net/archives/328
    /// android.app の方のfragment は非推奨になった
    /// →android.support.v4.app の方を使う必要がある
    /// </summary>
    public class OrderConfirmDialogFragment : DialogFragment, IDialogInterfaceOnClickListener
    {
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var builder = new AlertDialog.Builder(this.Activity);
            builder.SetTitle(Resource.String.dialog_title);
            builder.SetMessage(Resource.String.dialog_msg);

            // 個別にイベントハンドラを設定してもいける
            //builder.SetPositiveButton(Resource.String.dialog_btn_ok, OnPositiveButtonClicked);

            // Fragment自身がIDialogInterfaceOnClickListenerを実装してもいける
            builder.SetPositiveButton(Resource.String.dialog_btn_ok, this);
            builder.SetNegativeButton(Resource.String.dialog_btn_ng, this);
            builder.SetNeutralButton(Resource.String.dialog_btn_nu, this);

            var dialog = builder.Create();
            return dialog;
        }

        //private void OnPositiveButtonClicked(object sender, DialogClickEventArgs e)
        //{
        //    //var msg = GetString(Resource.String.dialog_ok_toast) + _tmp;
        //    var msg = GetString(Resource.String.dialog_ok_toast);
        //    Toast.MakeText(this.Activity, msg, ToastLength.Long).Show();

        //    this.Dismiss();
        //}

        void IDialogInterfaceOnClickListener.OnClick(IDialogInterface dialog, int which)
        {
            var msg = "";
            //var whichEnum = (DialogButtonType)Enum.ToObject(typeof(DialogButtonType), which);
            switch (which)
            {
                case (int)DialogButtonType.Positive:
                    msg = GetString(Resource.String.dialog_ok_toast);
                    break;
                case (int)DialogButtonType.Negative:
                    msg = GetString(Resource.String.dialog_ng_toast);
                    break;
                case (int)DialogButtonType.Neutral:
                    msg = GetString(Resource.String.dialog_nu_toast);
                    break;
                default:
                    break;
            }

            Toast.MakeText(this.Activity, msg, ToastLength.Long).Show();
        }
    }
}