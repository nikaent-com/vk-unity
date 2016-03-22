package com.nikaent.unity.vk;

import java.util.Map;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;
import com.unity3d.player.UnityPlayer;
import com.vk.sdk.api.VKError;
import com.vk.sdk.api.VKParameters;
import com.vk.sdk.api.VKRequest;
import com.vk.sdk.api.VKRequest.VKRequestListener;
import com.vk.sdk.api.VKResponse;

import android.app.Activity;

public class ApiCall {

	private int requestId = 0;
	static private int counterId = 0;

	private VKRequestListener _requestListener = new VKRequestListener() {
		@Override
		public void onComplete(VKResponse response) {
			String requestData = response.responseString;

			VK.log("onComplete");
			VK.log("request in: " + requestId);
			VK.log(requestData);

			UnityPlayer.UnitySendMessage("VkPlugin", "call", "{\"response"+requestId+"\":"+requestData+"}");
			response.request.unregisterObject();
		}

		@Override
		public void onError(VKError error) {
			VK.log("onError");
			VK.log("request in: " + requestId);
			VK.log(error.toString());
			
			error.request.unregisterObject();
			
			int errorCode = 0;
			String errorMessage = "";
			try {
				errorCode = error.apiError.errorCode;
			} catch(Error e){
				errorCode = -1;
			}
			try {
				errorMessage = error.apiError.errorMessage;
			} catch(Error e){
				errorMessage = "error";
			}
			
			try {
				UnityPlayer.UnitySendMessage("VkPlugin", "call", "{\"responseError"+requestId+"\":"+String.format("{\"vkErrorCode\":%d, \"message\":\"%s\"}", errorCode, errorMessage)+"}");
				
			} catch(Error e){
				VK.log("error getContext()");
			}
		}

		@Override
		public void onProgress(VKRequest.VKProgressType progressType, long bytesLoaded, long bytesTotal) {
			// you can show progress of the request if you want
		}

		@Override
		public void attemptFailed(VKRequest request, int attemptNumber, int totalAttempts) {
			VK.log("attemptFailed");
			VK.log("request in: " + requestId);

			UnityPlayer.UnitySendMessage("VkPlugin", "call", "{\"responseFailed"+requestId+"\":"+String.format("Attempt %d/%d failed\n", attemptNumber, totalAttempts)+"}");
			request.unregisterObject();
		}
	};

	static public String call(Activity context, String method, String params) {
		ApiCall apiCall = new ApiCall();
		return apiCall.callMethod(context, method, params);
	};

	
	public String callMethod(Activity context, String method, String params) {
		String result = null;

		VKRequest request = null;
		if(params.length()>1){
			Map<String, Object> map = new Gson().fromJson(params, new TypeToken<Map<String, Object>>(){}.getType());
			request = new VKRequest(method, new VKParameters(map), null);
		}else{
			request = new VKRequest(method, null, null);
		}
		request.secure = false;
		request.useSystemLanguage = false;
		
		requestId = counterId++;
		
		result = doRequest(request);

		return result;
	}

	private String doRequest(VKRequest request) {
		request.executeWithListener(_requestListener);
		try {
			request.registerObject();
			VK.log("send request id: " + requestId);
			return ""+requestId;
		} catch (Error e) {
			e.printStackTrace();
		}
		return null;
	}
}
