package com.nikaent.unity.vk;

import com.vk.sdk.api.methods.VKApiCaptcha;

import android.app.Activity;

public class TestCaptcha{

	static public void call(Activity context) {
		TestCaptcha testCaptcha = new TestCaptcha();
		testCaptcha.callMethod(context);
	};

	
	public void callMethod(Activity context) {
		VK.log("TestCaptcha");
		new VKApiCaptcha().force().executeWithListener(null);
	}
}
