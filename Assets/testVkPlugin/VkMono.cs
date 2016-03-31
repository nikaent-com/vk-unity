using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;
using com.nikaent.vk;

namespace test {
	public class VkMono : MonoBehaviour {
		private Text mText = null;
		void Start () {
			Application.stackTraceLogType = StackTraceLogType.None;
			mText = GameObject.Find ("Text").GetComponent<Text>();
			mText.text = "Start";
		}

		void OnGUI () {
			if (GUI.Button (new Rect (10, 50, 100, 50), "Login"))
				VKontakte.login ();
			if (GUI.Button (new Rect (10, 100, 100, 50), "logout"))
				VKontakte.logout ();
			if (GUI.Button (new Rect (10, 150, 100, 50), "isLoggedIn"))
				mText.text = "isLoggedIn: " + VKontakte.isLoggedIn ();
			if (GUI.Button (new Rect (10, 200, 100, 50), "testCaptcha"))
				VKontakte.testCaptcha ();
			if (GUI.Button (new Rect (10, 250, 100, 50), "users.get")) {
				VKontakte.CallBack myFunc = (obj) => {
					mText.text = "users.get: " + JsonFx.Json.JsonWriter.Serialize (obj);
				};
				Dictionary<string, object> obj1 = new Dictionary<string, object> ();
				obj1.Add ("fields", "uid,first_name,last_name,photo_medium,photo_200,sex,bdate,online,country");
				VKontakte.Api ("users.get", obj1, myFunc);
			}
			if (GUI.Button (new Rect (10, 300, 100, 50), "friends.get")) {
				VKontakte.CallBack myFunc = (obj) => {
					mText.text = "friends.get: " + JsonFx.Json.JsonWriter.Serialize (obj);
				};
				Dictionary<string, object> obj1 = new Dictionary<string, object> ();
				VKontakte.Api ("friends.get", obj1, myFunc);
			}
			if (GUI.Button (new Rect (10, 350, 100, 50), "wall.post")) {
				VKontakte.CallBack myFunc = (obj) => {
					mText.text = "wall.post: " + JsonFx.Json.JsonWriter.Serialize (obj);
				};
				Dictionary<string, object> obj1 = new Dictionary<string, object> ();
				obj1.Add ("message", "Unity text message");
				VKontakte.Api ("wall.post", obj1, myFunc);
			}
			if (GUI.Button (new Rect (10, 400, 100, 50), "photos.get")) {
				VKontakte.CallBack myFunc = (obj) => {
					mText.text = "photos.get: " + JsonFx.Json.JsonWriter.Serialize (obj);
				};
				Dictionary<string, object> obj1 = new Dictionary<string, object> ();
				VKontakte.Api ("photos.get", obj1, myFunc);
			}


			if (GUI.Button (new Rect (10, 450, 100, 50), "log")) {
				VKontakte.log ("log test!!!!!!!!!!!!!!!!!!!!!!");
			}
		}
	}
}