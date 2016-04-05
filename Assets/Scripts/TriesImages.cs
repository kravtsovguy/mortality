using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TriesImages : MonoBehaviour {

    public float offset;
    int tries;
	// Use this for initialization
	void Start () {
        foreach (Transform child in transform)
        {
            child.GetComponent<Image>().enabled = false;
        }
        RefreshTries();
    }
	
	// Update is called once per frame
	void Update () {

        if (tries != Game.triesLeft)
        {
            RefreshTries();

        }
    }

    void RefreshTries()
    {
        tries = Game.triesLeft;
        

        for (int i = 0; i < 3-Game.triesLeft; i++)
        {
            Image img = transform.FindChild((i + 1).ToString()).GetComponent<Image>();
            if (!img.enabled)
            {
                img.enabled = true;
                StartCoroutine(PopUp(img.gameObject));
            }
        }
    }

    IEnumerator PopUp(GameObject g)
    {
        Transform tran = g.transform;
        Try tryMB = tran.gameObject.AddComponent<Try>();
        tran.position = Vector2.zero;
        tran.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        tryMB.scale = 5;
        tryMB.scaleLerpSpeed = 50;
        yield return new WaitForSeconds(1);
        tryMB.position = tryMB.startPosition;
        tryMB.scale = 1;
        tryMB.positionLerpSpeed = 20;
        tryMB.scaleLerpSpeed = 100;
    }
}

public class Try : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 position = Vector2.zero;
    public float positionLerpSpeed;
    public float scale;
    public float scaleLerpSpeed;
    void Awake()
    {
        startPosition = transform.position;
    }
    void Update()
    {
        transform.position = Vector2.Lerp(transform.position, position, positionLerpSpeed * Time.deltaTime);
        transform.localScale = Vector2.MoveTowards(transform.localScale, new Vector2(scale,scale), scaleLerpSpeed * Time.deltaTime);
    }
}
