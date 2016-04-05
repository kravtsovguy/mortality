using UnityEngine;
using System.Collections;
using GameAnalyticsSDK;
using GameAnalyticsSDK.Events;
using System.Collections.Generic;

public static class AnalyticsManager {
	
	public static void SetGanderAndDate(GAGender gender, int year){

		GameAnalytics.SetGender (gender);
		GameAnalytics.SetBirthYear (year);
		
		Debug.Log (string.Format("Analytics: Gender - {0}, birthyear - {1}",gender.ToString(),year));
		
	}
	
	public static void TrackBusinessEvent(string currency, int amount, string itemType, string itemId, string cartType){

		GameAnalytics.NewBusinessEvent (currency, amount, itemType, itemId, cartType);

	}

	public static void TrackVirtualEconomy(GAResourceFlowType flowType, string currency, float amount, string itemType, string itemId){
		GameAnalytics.NewResourceEvent (flowType, currency, amount, itemType, itemId);
	}

	public static void TrackProgression(GAProgressionStatus progressionStatus, string progression, int score){
		GameAnalytics.NewProgressionEvent (progressionStatus, progression, score);
	}

	public static void TrackCustomEvent(string eventName){
		GameAnalytics.NewDesignEvent (eventName);
	}

	public static void TrackCustomEvent(string eventName, float eventValue){
		GameAnalytics.NewDesignEvent (eventName, eventValue);
	}

	public static void TrackErrorEvent(GAErrorSeverity severity, string message = null){
		GameAnalytics.NewErrorEvent (severity, message);
	}
}
