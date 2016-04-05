using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class DificultyManager : MonoBehaviour {

    public Button normal;
    public Button hard;
    public Color disabledColor;

    private Color normalColor;

	public static DificultyManager shared;
	// Use this for initialization
	void Start () {

		shared = this;

        normalColor = normal.GetComponent<Image>().color;
        UpdateColor();

	}

    /*void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            UpdateColor();
        }
    }*/

    public void UpdateColor()
    {
		print ("diffic - "+Game.dificulty);
		
        if (Game.dificulty == Difficulty.Normal)
        {
			normal.GetComponent<Image>().color = disabledColor;
			hard.GetComponent<Image>().color = normalColor;
        }
        else if (Game.dificulty == Difficulty.Hard)
        {
			hard.GetComponent<Image>().color = disabledColor;
			normal.GetComponent<Image>().color = normalColor;
        }
    }
}
