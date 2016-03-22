package com.nikaent.unity.vk.activity;

import com.nikaent.unity.vk.VK;
import com.nikaent.unity.vk.Login;
import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;
import com.vk.sdk.VKAccessToken;
import com.vk.sdk.VKCallback;
import com.vk.sdk.VKSdk;
import com.vk.sdk.api.VKError;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;


public class AuthActivity extends Activity {
	
	private static String AUTH_SUCCESSFUL 	= "AUTH_SUCCESSFUL";
	private static String AUTH_FAILED 		= "FAILED";
	
	protected void onCreate(Bundle savedInstanceState) {
		VK.log("onCreate ApiCallActivity");
		super.onCreate(savedInstanceState);
		Activity activity = this;
		VKSdk.login(activity, Login._scope);
	}

	protected void onResume() {
		super.onResume();
	}

	protected void onActivityResult(int requestCode, int resultCode, Intent data) {
		super.onActivityResult(requestCode, resultCode, data);
		if (!VKSdk.onActivityResult(requestCode, resultCode, data, new VKCallback<VKAccessToken>() {
			@Override
			public void onResult(VKAccessToken res) {
				UnityPlayer.UnitySendMessage("VkPlugin", AUTH_SUCCESSFUL, "");
				VK.log("authorization successful");
				
			}

			@Override
			public void onError(VKError error) {
				UnityPlayer.UnitySendMessage("VkPlugin", AUTH_FAILED, "");
				VK.log("authorization failed");
			}
		})) {
			super.onActivityResult(requestCode, resultCode, data);
		}

		Intent returnIntent = new Intent();
		setResult(-1, returnIntent);
		finish();
	}

	protected void onDestroy() {
		super.onDestroy();
	}
}
