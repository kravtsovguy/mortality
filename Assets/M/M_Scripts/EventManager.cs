using UnityEngine;
using System.Collections;
using System;

public static class EventManager {

	public static event Action GameStartedEvent=delegate{};
	public static event Action GameEndedEvent=delegate{};
	public static event Action GameReadyToStartEvent=delegate{};
	public static event Action GamePausedEvent=delegate{};
	public static event Action GameResumedEvent=delegate{};
	public static bool IsGameStarted;
	public static bool IsGamePaused;
	public static void GameStarted()
	{
		Debug.Log("GameStarted");
		GameStartedEvent ();
		IsGameStarted = true;
	}
	public static void GameEnded()
	{
		Debug.Log("GameEnded");
		GameEndedEvent ();
		IsGameStarted = false;
	}
	public static void GameReadyToStart()
	{
		Debug.Log("GameReadyToStart");
		GameReadyToStartEvent();
	}
	public static void GamePaused()
	{
		Debug.Log ("GamePaused");
		GamePausedEvent ();
		IsGamePaused = true;
	}
	public static void GameResumed()
	{
		Debug.Log ("GameResumed");
		GameResumedEvent ();
		IsGamePaused = false;
	}

}
