package com.nikaent.unity.vk;

import com.vk.sdk.VKSdk;

import android.app.Activity;

public class Logout {

	static public void call(Activity context) {
		Logout logout = new Logout();
		logout.callMethod(context);
	};

	
	public void callMethod(Activity context) {
        VKSdk.logout();
        VK.log("VK logout");
	}

}
