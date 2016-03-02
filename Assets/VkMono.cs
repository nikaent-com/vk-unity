using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

public class VkMono : MonoBehaviour {

	delegate void CallBack(object obj);

	public Text text1;

	private Dictionary<string, CallBack> mapFunction = new Dictionary<string, CallBack>();

	#if UNITY_IPHONE && !UNITY_EDITOR
	[DllImport("__Internal")]
	static extern void init(string idVkApp);
	[DllImport("__Internal")]
	static extern void login(string scopes);
	[DllImport("__Internal")]
	static extern void logout();
	[DllImport("__Internal")]
	static extern string apiCall(string method, string param);
	[DllImport("__Internal")]
	static extern bool isLoggedIn();
	[DllImport("__Internal")]
	static extern void testCaptcha();
#else
	static void init (string filename) {
	}

	static void login (string scopes) {
	}

	static void logout () {
		
	}

	static string apiCall (string method, string param) {
		return "";
	}

	static bool isLoggedIn () {
		return false;
	}

	static void testCaptcha () {
	}

	#endif // if UNITY_IPHONE && !UNITY_EDITOR

	void LoadTexture () {
		init (@"5282890");
	}

	void LoginVk () {
		login ("[\"friends\",\"notifications\",\"status\",\"groups\",\"wall\",\"audio\",\"photos\",\"nohttps\",\"email\",\"messages\"]");
	}

	string UserGet () {
		return apiCall ("users.get", "{\"fields\":\"uid,first_name,last_name,photo_medium,photo_200,sex,bdate,online,country\"}");
	}

	void Start () {
		Application.stackTraceLogType = StackTraceLogType.None;
	}

	void OnGUI () {
		if (GUI.Button (new Rect (10, 0, 100, 50), "Init"))
			LoadTexture ();
		if (GUI.Button (new Rect (10, 50, 100, 50), "Login"))
			LoginVk ();
		if (GUI.Button (new Rect (10, 100, 100, 50), "logout"))
			logout ();
		if (GUI.Button (new Rect (10, 150, 100, 50), "isLoggedIn"))
			text1.text = isLoggedIn().ToString();
		if (GUI.Button (new Rect (10, 200, 100, 50), "testCaptcha"))
			testCaptcha ();
		if (GUI.Button (new Rect (10, 250, 100, 50), "user.get")) {
			string idRequare = "0";
			CallBack myFunc = (obj) => {
				text1.text = JsonFx.Json.JsonWriter.Serialize(obj);
				mapFunction.Remove (idRequare);
			};
			mapFunction [UserGet()] = myFunc;
		}
	}

	public void trace (string str) {
		Debug.Log ("from Unity : " + str);
	}
	public void call (string data) {
		Debug.Log ("data : " + data);

		Dictionary<string, object> request = JsonFx.Json.JsonReader.Deserialize(data) as Dictionary<string, object>;

		string requestName = request.Keys.ToList () [0];
		if (requestName.IndexOf ("response") == 0) {
			int indexStartId = 8;
			if (requestName.IndexOf ("responseError")==0) {
				indexStartId = 13;
			}
			string idRequare = requestName.Substring (indexStartId);
			CallBack myFunc = mapFunction[idRequare];
			mapFunction.Remove (idRequare);
			object requestData = request [requestName];
			myFunc (requestData);
		}
	}
}