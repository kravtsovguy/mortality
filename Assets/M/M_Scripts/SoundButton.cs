using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour {

	public Sprite soundON;
	public Sprite soundOFF;
	bool isOFF;
    AudioSource audio;

	void Start()
	{
        audio = GameManager.audio;
		if (!audio) return;
		isOFF = !audio.isPlaying;
		GetComponent<Image>().sprite = isOFF? soundOFF: soundON;
	}

	public void OnClick(Image img)
	{
		isOFF = !isOFF;
		img.sprite = isOFF? soundOFF: soundON;
	}
}
