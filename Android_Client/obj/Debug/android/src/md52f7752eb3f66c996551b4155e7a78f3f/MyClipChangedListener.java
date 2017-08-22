package md52f7752eb3f66c996551b4155e7a78f3f;


public class MyClipChangedListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.content.ClipboardManager.OnPrimaryClipChangedListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onPrimaryClipChanged:()V:GetOnPrimaryClipChangedHandler:Android.Content.ClipboardManager/IOnPrimaryClipChangedListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("Android_Client.MyClipChangedListener, Android_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", MyClipChangedListener.class, __md_methods);
	}


	public MyClipChangedListener () throws java.lang.Throwable
	{
		super ();
		if (getClass () == MyClipChangedListener.class)
			mono.android.TypeManager.Activate ("Android_Client.MyClipChangedListener, Android_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onPrimaryClipChanged ()
	{
		n_onPrimaryClipChanged ();
	}

	private native void n_onPrimaryClipChanged ();

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
