using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SoundEffectsButton : MonoBehaviour {

	public Sprite soundON;
	public Sprite soundOFF;
	bool isOFF;

	void Start()
	{

		isOFF = !GameManager.soundEffects;
		GetComponent<Image>().sprite = isOFF? soundOFF: soundON;
	}

	public void OnClick(Image img)
	{
		isOFF = !isOFF;
		img.sprite = isOFF? soundOFF: soundON;
	}
}
