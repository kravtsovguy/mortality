using UnityEngine;
using System.Collections;

public class FlipController : MonoBehaviour {

    public GameObject flippedCard;
    public Vector3 flipPosition;
    public float flipScale;
    public float flipSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (flippedCard)
        {
            Vector3 tmp = flippedCard.transform.localScale;
            tmp.x = Mathf.MoveTowards(tmp.x, -flipScale, flipSpeed * Time.deltaTime);
            tmp.y = Mathf.MoveTowards(tmp.y, flipScale, flipSpeed * Time.deltaTime);
            tmp.z = 1;
            flippedCard.transform.localScale = tmp;
            tmp = Vector2.Lerp(flippedCard.transform.position, flipPosition, flipSpeed * Time.deltaTime);
            tmp.z = flipPosition.z;
            flippedCard.transform.position = tmp;

        }
	}
}
