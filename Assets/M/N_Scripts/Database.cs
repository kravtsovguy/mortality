using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Database: Singleton<Database> {

	protected Database(){}

	string addinfoURL = "http://www.calculated-risk-llc.com/addInfo.php";
	string displayURL = "http://www.calculated-risk-llc.com/display.php";

	//string addinfoURL = "http://localhost:8000/info/addinfo.php";
	//string displayURL = "http://localhost:8000/info/display.php";
	string secretKey = "secret"; 

	public void AddInfo(string incorrect_card_ID, string lower_card_ID, string higher_card_ID, Action<string> result=null){
		var url = AddParamsToString (addinfoURL, new Dictionary<string, string> {
			{"id1",incorrect_card_ID},
			{"id2",lower_card_ID},
			{"id3",higher_card_ID}
		});
		StartCoroutine (RequestAsync(url,(res)=>{
			if (result != null)
				result(res);
		}));
	}

	public void DisplayInfo(Action<string> result=null){
		StartCoroutine (RequestAsync(displayURL,(res)=>{
			if (result != null)
				result(res);
		}));
	}

	public string AddParamsToString(string url, Dictionary<string, string> values=null){
		if (values == null || values.Count == 0) {
			return url;
		} else {
			string all = secretKey;
			url+="?";
			foreach(var pair in values){
				url+=pair.Key+"="+WWW.EscapeURL(pair.Value)+"&";
				all+=pair.Value;
			}
			url+="hash="+Md5Sum(all);
			//url.Remove(url.Length-1);
			//url = WWW.EscapeURL(url);
			return url;
		}
	}

	private IEnumerator RequestAsync(string url, Action<string> result = null){
		Debug.Log (url);
		WWW www = new WWW(url);
		yield return www;
		if (result != null)
			result (www.error == null ? www.text: "Error: no result");
	}

	public  string Md5Sum(string strToEncrypt)
	{
		System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
		byte[] bytes = ue.GetBytes(strToEncrypt);
		
		// encrypt bytes
		System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
		byte[] hashBytes = md5.ComputeHash(bytes);
		
		// Convert the encrypted bytes back to a string (base 16)
		string hashString = "";
		
		for (int i = 0; i < hashBytes.Length; i++)
		{
			hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
		}
		
		return hashString.PadLeft(32, '0');
	}
}
