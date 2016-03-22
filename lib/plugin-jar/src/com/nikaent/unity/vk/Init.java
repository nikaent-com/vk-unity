package com.nikaent.unity.vk;

import com.vk.sdk.VKAccessToken;
import com.vk.sdk.VKAccessTokenTracker;
import com.vk.sdk.VKSdk;
import com.vk.sdk.util.VKUtil;

import android.content.Context;

public class Init{
	VKAccessTokenTracker vkAccessTokenTracker = new VKAccessTokenTracker() {
		@Override
		public void onVKAccessTokenChanged(VKAccessToken oldToken, VKAccessToken newToken) {
			if (newToken == null) {
				VK.log("token invalid");
			}
		}
	};

	
	static public void call(Context context, String appVkIdStr) {
		VK.log("call static init");
		Init initi = new Init();
		initi.callMethod(context, appVkIdStr);
	};

	
	public void callMethod(Context context, String appVkIdStr) {
		VK.log("callMethod init");
		
		int appVkId = Integer.parseInt(appVkIdStr);
		VK.log("appVkId: "+appVkId);
		
		vkAccessTokenTracker.startTracking(); 
		VKSdk.customInitialize(context, appVkId, "5.21");

		String[] fingerprints = VKUtil.getCertificateFingerprint(context, context.getPackageName());

		VK.log("packageName: "+context.getPackageName());
		
		for (int j = 0; j < fingerprints.length; j++) {
			VK.log("fingerprint"+fingerprints[j]);
		}
	}

}
