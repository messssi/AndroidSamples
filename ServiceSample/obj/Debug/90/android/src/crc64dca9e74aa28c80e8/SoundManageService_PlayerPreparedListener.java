package crc64dca9e74aa28c80e8;


public class SoundManageService_PlayerPreparedListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.media.MediaPlayer.OnPreparedListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onPrepared:(Landroid/media/MediaPlayer;)V:GetOnPrepared_Landroid_media_MediaPlayer_Handler:Android.Media.MediaPlayer/IOnPreparedListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("ServiceSample.SoundManageService+PlayerPreparedListener, ServiceSample", SoundManageService_PlayerPreparedListener.class, __md_methods);
	}


	public SoundManageService_PlayerPreparedListener ()
	{
		super ();
		if (getClass () == SoundManageService_PlayerPreparedListener.class)
			mono.android.TypeManager.Activate ("ServiceSample.SoundManageService+PlayerPreparedListener, ServiceSample", "", this, new java.lang.Object[] {  });
	}


	public void onPrepared (android.media.MediaPlayer p0)
	{
		n_onPrepared (p0);
	}

	private native void n_onPrepared (android.media.MediaPlayer p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
