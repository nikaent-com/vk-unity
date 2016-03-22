package com.nikaent.unity.vk;

import com.google.gson.Gson;
import com.nikaent.unity.vk.activity.AuthActivity;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;

public class Login{
	static public String[] _scope;
	static public Context _context;
	
	static public void call(Activity context, String appVkIdStr) {
		Login login = new Login();
		login.callMethod(context, appVkIdStr);
	};

	
	public void callMethod(Activity context, String scopes) {
		VK.log(scopes);
		_scope = new Gson().fromJson(scopes, String[].class);
		_context = context;
		
		Activity activity = context;
		
		Intent intent = new Intent(activity, AuthActivity.class);
		activity.startActivityForResult(intent, 1);
	}

}
