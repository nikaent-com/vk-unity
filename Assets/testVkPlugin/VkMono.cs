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
		void Start () {
			Application.stackTraceLogType = StackTraceLogType.None;
		}

		void OnGUI () {
			if (GUI.Button (new Rect (10, 50, 100, 50), "Login"))
				VKontakte.login ();
			if (GUI.Button (new Rect (10, 100, 100, 50), "logout"))
				VKontakte.logout ();
			if (GUI.Button (new Rect (10, 150, 100, 50), "isLoggedIn"))
				Debug.Log ("isLoggedIn: " + VKontakte.isLoggedIn ());
			if (GUI.Button (new Rect (10, 200, 100, 50), "testCaptcha"))
				VKontakte.testCaptcha ();
			if (GUI.Button (new Rect (10, 250, 100, 50), "user.get")) {
				VKontakte.CallBack myFunc = (obj) => {
					Debug.Log ("callback: " + JsonFx.Json.JsonWriter.Serialize (obj));
				};
				Dictionary<string, object> obj1 = new Dictionary<string, object> ();
				obj1.Add ("fields", "uid,first_name,last_name,photo_medium,photo_200,sex,bdate,online,country");
				VKontakte.Api ("users.get", obj1, myFunc);

			}
			if (GUI.Button (new Rect (10, 300, 100, 50), "log")) {
				VKontakte.log ("log test!!!!!!!!!!!!!!!!!!!!!!");
			}
		}
	}
}