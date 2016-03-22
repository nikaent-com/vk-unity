package com.nikaent.unity.vk;

import android.util.Log;
import android.app.Activity;
import android.content.Context;
import android.os.Bundle;

import com.unity3d.player.UnityPlayerActivity;
import com.vk.sdk.VKSdk;

public class VK extends UnityPlayerActivity{

	public static String TAG = "PLUGIN VK";
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		log("onCreate");
		super.onCreate(savedInstanceState);
	}
	
	public static void log(String message){
		Log.i(VK.TAG, message);
	}
	public static void init(Activity activity, String appVkIdStr){
		Init.call(activity, appVkIdStr);
	}
	
	
	public String call(String method, String data){
		log("call1"+method+data);
		if(method.indexOf("init")==0){
			Init.call(this, data);
		}else if(method.indexOf("login")==0){
			Login.call(this, data);
		}else if(method.indexOf("logout")==0){
			Logout.call(this);
		}else if(method.indexOf("isLoggedIn")==0){
			return VKSdk.isLoggedIn()?"1":"0";
		}else if(method.indexOf("testCaptcha")==0){
			TestCaptcha.call(this);
		}else{
			log("call not found: "+method);
		}
		return "";
	}
	
	public String call(String method, String apiMethod, String data){
		log("call2"+method+apiMethod+data);
		if(method.indexOf("apiCall")==0){
			return ApiCall.call(this, apiMethod, data);
		}
		return "";
	}

}