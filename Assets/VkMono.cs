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
	#elif UNITY_ANDROID && !UNITY_EDITOR
	public static AndroidJavaClass unityActivityClassLeft;
	public static AndroidJavaObject unityActivityClass;
	public static AndroidJavaClass sdk;
	public static AndroidJavaClass unityActivityClassInit;
	static void init(string idVkApp){
		var activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
		unityActivityClass.CallStatic("init", activity, idVkApp);
	}
	static void login(string scopes){
		unityActivityClass.Call<string>("call", "login", scopes);
	}
	static void logout(){
		unityActivityClass.Call<string>("call", "logout", "");
	}
	static string apiCall(string method, string param){
		return unityActivityClass.Call<string>("call", "apiCall", method, param);
	}
	static bool isLoggedIn(){
		return unityActivityClass.Call<string>("call", "isLoggedIn", "")=="1";
	}
	static void testCaptcha(){
		unityActivityClass.Call<string>("call", "testCaptcha", "");
	}
	static void log(string str){
		unityActivityClass.CallStatic("log", str);
	}
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

	static void log(string str){
		Debug.Log ("piiiuuuuuu");
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
		#if UNITY_ANDROID && !UNITY_EDITOR
		unityActivityClass = new AndroidJavaClass("com.nikaent.unity.vk.VK");
		unityActivityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");

		#endif
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
		if (GUI.Button (new Rect (10, 300, 100, 50), "log")) {
			log ("log test!!!!!!!!!!!!!!!!!!!!!!");
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