using UnityEngine;
using System.Collections;

public class GameManager: MonoBehaviour {
	
	public static GameManager shared;

	void Awake () {
		shared = this;

		Application.targetFrameRate = 60;

	}

	void Start(){
        PlayAudio();

		EventManager.GameStartedEvent += GameStarted;
		EventManager.GameEndedEvent += GameEnded;
		EventManager.GameReadyToStartEvent += GameReadyToStart;

		EventManager.GameReadyToStart ();

	}

	void OnDestroy()
	{
		EventManager.GameStartedEvent -= GameStarted;
		EventManager.GameEndedEvent -= GameEnded;
		EventManager.GameReadyToStartEvent -= GameReadyToStart;

	}

	void GameStarted()
	{
	}
	void GameEnded()
	{	
	}

	void GameReadyToStart()
	{
	}

    public static bool soundEffects = true;
	public static AudioSource audio;
	void PlayAudio()
	{
        //audio = GameObject.FindObjectOfType<AudioSource>();
        audio = GameObject.Find("Music").GetComponent<AudioSource>();
        audio.Play();
	}

	public void SwitchAudio()
	{
        if (audio.isPlaying)
        {
            audio.Pause();
        }
        else
        {
            audio.Play();
        }
	}


}
