package crc645ff52cfc1ab6e2ce;


public class AndroidPopupService_BackButtonKeyListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.content.DialogInterface.OnKeyListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onKey:(Landroid/content/DialogInterface;ILandroid/view/KeyEvent;)Z:GetOnKey_Landroid_content_DialogInterface_ILandroid_view_KeyEvent_Handler:Android.Content.IDialogInterfaceOnKeyListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("MauiPopup.Platforms.Android.AndroidPopupService+BackButtonKeyListener, MauiPopup", AndroidPopupService_BackButtonKeyListener.class, __md_methods);
	}

	public AndroidPopupService_BackButtonKeyListener ()
	{
		super ();
		if (getClass () == AndroidPopupService_BackButtonKeyListener.class) {
			mono.android.TypeManager.Activate ("MauiPopup.Platforms.Android.AndroidPopupService+BackButtonKeyListener, MauiPopup", "", this, new java.lang.Object[] {  });
		}
	}

	public boolean onKey (android.content.DialogInterface p0, int p1, android.view.KeyEvent p2)
	{
		return n_onKey (p0, p1, p2);
	}

	private native boolean n_onKey (android.content.DialogInterface p0, int p1, android.view.KeyEvent p2);

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
