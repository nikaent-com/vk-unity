using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace com.nikaent.vk {
	public enum Scopes{
		notify,
		friends,
		photos,
		audio,
		video,
		docs,
		notes,
		pages,
		status,
		offers,
		questions,
		wall,
		groups,
		messages,
		notifications,
		stats,
		ads,
		offline,
		nohttps,
		email
	}
	public class Scope {
		public const string NOTIFY = "notify";
		public const string FRIENDS = "friends";
		public const string PHOTOS = "photos";
		public const string AUDIO = "audio";
		public const string VIDEO = "video";
		public const string DOCS = "docs";
		public const string NOTES = "notes";
		public const string PAGES = "pages";
		public const string STATUS = "status";
		public const string OFFERS = "offers";
		public const string QUESTIONS = "questions";
		public const string WALL = "wall";
		public const string GROUPS = "groups";
		public const string MESSAGES = "messages";
		public const string NOTIFICATIONS = "notifications";
		public const string STATS = "stats";
		public const string ADS = "ads";
		public const string OFFLINE = "offline";
		public const string NOHTTPS = "nohttps";
		public const string EMAIL = "email";

		public static string listToStringJson(List<Scopes> listScopes){
			return JsonFx.Json.JsonWriter.Serialize (listScopes);
		}
	}
}
