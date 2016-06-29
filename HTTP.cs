using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class HTTP : MonoBehaviour {

	public delegate void HTTPResponse (int code, WWW www);		
	public const int OK = 100;														
	public const int ERROR = 101;													
	private static HTTP _http;					

	public static HTTP instance {
		get {
			if (!_http) {
				_http = FindObjectOfType (typeof (HTTP)) as HTTP;
				if (!_http) 
					Debug.LogError ("There needs to be one active HTTP script on a GameObject in your scene.");
			}
			return _http;
		}
	}

	public static void GET (string url, HTTPResponse callback) {
		instance.StartCoroutine (instance.WaitForRequest (new WWW (url), callback));
	}

	public static void POST (string url, Dictionary<string, string> postData, HTTPResponse callBack) {
		WWWForm form = new WWWForm();
		foreach (KeyValuePair<String, String> param in postData)
			form.AddField(param.Key, param.Value);
		instance.StartCoroutine (instance.WaitForRequest (new WWW (url, form), callBack));
	}

	IEnumerator WaitForRequest (WWW www, HTTPResponse callback) {
		yield return www;
		if (www.error == null)
			callback (OK, www);
		else 
			callback (ERROR, www);
	}
}