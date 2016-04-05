using UnityEngine;
using System.Collections;
using OnePF;

public class DoScript : MonoBehaviour {


	public void Restore()
	{
		#if UNITY_IOS
		
		IAPManager.shared.queryInventorySucceeded = (Inventory inv) => {
			LoadScene(Application.loadedLevelName);
		};
		IAPManager.shared.Restore ();
		#endif
	}

	public void LoadLevel(string name)
	{
		LoadScene (name);
	}

	public static void LoadScene(string name){

		AnalyticsManager.TrackCustomEvent ("UI:Screen:" + name);
        if (GameManager.soundEffects && GameObject.Find("ButtonSound"))
        {
            GameObject.Find("ButtonSound").GetComponent<AudioSource>().Play();
        }
		Debug.Log ("Log screen: "+name);
		Application.LoadLevel (name);
	
	}

    public void SwitchGameObject(GameObject g)
    {
        g.SetActive(!g.activeSelf);
    }

    public void StartGame(string name)
    {
        if (Game.contentPacks.Count > 0)
        {
			AnalyticsManager.TrackCustomEvent ("Game:PacksCount",Game.contentPacks.Count);
			foreach(var item in Game.contentPacks){
				AnalyticsManager.TrackCustomEvent ("Game:Packs:"+item.name);
			}
			
            LoadLevel(name);
        }
    }

    public void PauseGameplay(bool value)
    {
        Game.pause = value;
    }

	public void SwitchAudio(){
		GameManager.shared.SwitchAudio ();
	}

    public void SwitchSoundEffects()
    {
        GameManager.soundEffects = !GameManager.soundEffects;
    }

    public void SetDifficulty(int difficulty)
    {
        Game.dificulty = (Difficulty)difficulty;
		DificultyManager.shared.UpdateColor ();
    }
}
