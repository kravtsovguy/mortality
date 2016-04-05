using UnityEngine;
using System.Collections;

public class Inputs : MonoBehaviour {
    public string menu;
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DoScript.LoadScene(menu);
        }
	}
}
