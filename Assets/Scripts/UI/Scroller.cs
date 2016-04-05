using UnityEngine;
using System.Collections;

public class Scroller : MonoBehaviour {

    public Transform target;
    public float scrollSpeed;
    public BoxCollider bounds;

    private Vector2 scrollTarget;
    private float deceleration = 0;

    void Start()
    {
        scrollTarget.y = bounds.bounds.min.y;
        target.position = new Vector3(target.position.x, scrollTarget.y, target.position.z);
    }

	// Update is called once per frame
	void Update () {

        Vector3 tmp = target.position;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                lastY = Camera.main.ScreenToWorldPoint(touch.position).y;
            }
            if (touch.phase == TouchPhase.Moved)
            {
                deceleration = GetDelta(Camera.main.ScreenToWorldPoint(touch.position), 0);
                scrollTarget.y += deceleration;
                tmp.y = scrollTarget.y;
            }
        }
        else
        {
            scrollTarget.y += deceleration;
            deceleration = Mathf.Lerp(deceleration, 0, scrollSpeed * Time.deltaTime);
            tmp.y = Mathf.Lerp(tmp.y, scrollTarget.y, scrollSpeed * Time.deltaTime);
        }
        tmp.z = target.position.z;
        tmp.y = Mathf.Clamp(tmp.y, bounds.bounds.min.y, bounds.bounds.max.y);
        scrollTarget.y = Mathf.Clamp(scrollTarget.y, bounds.bounds.min.y, bounds.bounds.max.y);
        target.position = tmp;

	}

    float lastY = 0;
    float delta = 0;
    public float GetDelta(Vector2 t, float dead)
    {
        delta = t.y - lastY;
        lastY = t.y;
        if (delta > dead || delta < -dead)
        {
            return delta;
        }
        return 0;
    }
}
