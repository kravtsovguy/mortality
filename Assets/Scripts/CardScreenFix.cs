using UnityEngine;
using System.Collections;

public class CardScreenFix : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        float sw = Screen.width;
        float sh = Screen.height;
        float cameraHalfWidth = Camera.main.orthographicSize * (sw / sh);
        float cameraMinX = Camera.main.transform.position.x - cameraHalfWidth;
        float cameraMaxX = Camera.main.transform.position.x + cameraHalfWidth;

        Controller con = GetComponent<Controller>();
        con.card.transform.localScale = new Vector2(cameraHalfWidth / 15,cameraHalfWidth/15);
        FlipController fc = GetComponent<FlipController>();
        fc.flipScale = con.card.transform.localScale.x * 2;
        con.card.GetComponent<CardInformation>().scale = con.card.transform.localScale.x;
        con.deckParent.transform.position = new Vector2(cameraMinX+(con.card.GetComponent<SpriteRenderer>().bounds.extents.x+(cameraHalfWidth/10)),0);
        con.orderParent.transform.position = new Vector2(cameraMaxX - (con.card.GetComponent<SpriteRenderer>().bounds.extents.x ), 0);
        GameObject.FindGameObjectWithTag("DCTrigger").transform.position = con.orderParent.transform.position;
        GameObject.FindGameObjectWithTag("DCTrigger").GetComponent<BoxCollider2D>().size = new Vector2(cameraHalfWidth / 5,cameraHalfWidth/4);
        con.offset = con.card.GetComponent<SpriteRenderer>().bounds.extents.y*2.3f;
	}
	
}
