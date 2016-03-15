using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;


namespace com.nikaent.vk {


	public class VKontakte : MonoBehaviour{
		public delegate void CallBack (object obj);

		static private Dictionary<string, CallBack> mapFunction = new Dictionary<string, CallBack> ();
		static private VKontakte _inst = null;

		public string APP_ID = "5282890";
		public List<Scopes> listScopes = new List<Scopes>{Scopes.notify, Scopes.friends, Scopes.wall, Scopes.groups, Scopes.email, Scopes.offline, Scopes.nohttps};

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
	private static AndroidJavaClass unityActivityClassLeft;
	private static AndroidJavaObject unityActivityClass;
	private static AndroidJavaClass sdk;
	private static AndroidJavaClass unityActivityClassInit;
	static void _init(string idVkApp){
	var activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
	unityActivityClass.CallStatic("init", activity, idVkApp);
	}
	static void _login(string scopes){
	unityActivityClass.Call<string>("call", "login", scopes);
	}
	static void _logout(){
	unityActivityClass.Call<string>("call", "logout", "");
	}
	static string _apiCall(string method, string param){
	return unityActivityClass.Call<string>("call", "apiCall", method, param);
	}
	static bool _isLoggedIn(){
	return unityActivityClass.Call<string>("call", "isLoggedIn", "")=="1";
	}
	static void _testCaptcha(){
	unityActivityClass.Call<string>("call", "testCaptcha", "");
	}
	static void _log(string str){
	unityActivityClass.CallStatic("log", str);
	}
	
#else
		static void _init (string filename) {
		}

		static void _login (string scopes) {
		}

		static void _logout () {

		}

		static string _apiCall (string method, string param) {
			return "";
		}

		static bool _isLoggedIn () {
			return false;
		}

		static void _testCaptcha () {
		}

		static void _log (string str) {
			Debug.Log (str);
		}


		#endif // if UNITY_IPHONE && !UNITY_EDITOR

		public static void log (string str) {
			_log (str);
		}

		public static void login () {
			_login (Scope.listToStringJson (_inst.listScopes));
		}
		public static void logout () {
			_logout ();
		}
		public static bool isLoggedIn () {
			return _isLoggedIn ();
		}
		public static void testCaptcha () {
			_testCaptcha ();
		}

		static public void Api(string method, Dictionary<string, object> data, CallBack onResponse){
			mapFunction [_apiCall(method, JsonFx.Json.JsonWriter.Serialize (data))] = onResponse;

		}

		void Start () {
			_inst = this;
			#if UNITY_ANDROID && !UNITY_EDITOR
				unityActivityClass = new AndroidJavaClass("com.nikaent.unity.vk.VK");
				unityActivityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
			#endif
			_init (APP_ID);
		}

		public void trace (string str) {
			Debug.Log ("from Unity : " + str);
		}

		public void call (string data) {
			Debug.Log ("data : " + data);

			Dictionary<string, object> request = JsonFx.Json.JsonReader.Deserialize (data) as Dictionary<string, object>;

			string requestName = request.Keys.ToList () [0];
			if (requestName.IndexOf ("response") == 0) {
				int indexStartId = 8;
				if (requestName.IndexOf ("responseError") == 0) {
					indexStartId = 13;
				}
				string idRequare = requestName.Substring (indexStartId);
				CallBack myFunc = mapFunction [idRequare];
				mapFunction.Remove (idRequare);

				object requestData = request [requestName];
				myFunc (requestData);
			}
		}
	}
}
