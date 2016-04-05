using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SmoothAppear : MonoBehaviour {

    public float startScale;
    public float appearSpeed;
    public float idleTime;
    public float fadeSpeed;

    private float time;

	// Use this for initialization
	void OnEnable () {

        transform.localScale = new Vector3(startScale, startScale, startScale);

        time = idleTime;
	}
	
	// Update is called once per frame
	void Update () {

        transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(1, 1, 1), appearSpeed * Time.deltaTime);
        if (transform.localScale == new Vector3(1, 1, 1))
        {
            if (time > 0)
            {
                time -= Time.deltaTime;

            }
            else
            {
                Color col = GetComponent<Text>().color;
                col.a = Mathf.MoveTowards(col.a, 0, fadeSpeed * Time.deltaTime);
                GetComponent<Text>().color = col;
            }

            if (GetComponent<Text>().color.a <= 0)
            {
                gameObject.SetActive(false);
                Color col = GetComponent<Text>().color;
                col.a = 1;
                GetComponent<Text>().color = col;
            }
        }
	}
}
