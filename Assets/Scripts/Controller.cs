//Written By Amr Zewail
//Skylight
//-----------------------

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using CMethods;
using UnityEngine.UI;
using TouchInput;

public class Controller : MonoBehaviour{

    public Transform deckParent;
    public Transform orderParent;
    public GameObject card;

    public float offset;

    public float dragSpeed;
    public float scrollSpeed;
    public float arrangeSpeed;
    public List<GameObject> order = new List<GameObject>();

    public Sprite highlight;
    [HideInInspector]
    public GameObject emptySpace;

    [HideInInspector]
    public GameObject dragging;

    public AudioSource correctSound;
    public AudioSource wrongSound;
    public AudioSource ladderScrollSound;
    public AudioSource dealSound;

    public GameObject resultsWindow;
    public GameObject closeEnough;

    [HideInInspector]
    public List<GameObject> deck = new List<GameObject>();
    private float orderParentStartY = 0;
    MouseInput mouseInput = new MouseInput();
    TInput touchInput = new TInput();

    private int oCount = 0;

    private float cameraHalfHeight;

    void Start()
    {
        orderParentStartY = orderParent.transform.position.y;
        cameraHalfHeight = Camera.main.GetComponent<Camera>().orthographicSize;
        if (Game.contentPacks != null)
        {
            foreach (ContentPack cp in Game.contentPacks)
            {
                foreach (Card c in cp.deck)
                {
                    GameObject g = Instantiate(card) as GameObject;
                    g.GetComponent<CardController>().card = c;
                    g.name = c.name+"<"+c.mortality+">";
                    deck.Add(g);
                    g.transform.parent = deckParent.transform;
                }
            }
        }
        deck.RemoveDuplicates();
        deck.Shuffle();

        deck.ApplyFirstCardFilter();
        GameObject cc = Game.firstCard;
        deck.Remove(cc);
        order.Add(cc);
        

        foreach (GameObject card in deck)
        {
            card.transform.localPosition = Vector2.zero;
        }
        RefreshOrderParenting();
        StartCoroutine(CUpdateDeck());
        StartCoroutine(CUpdateOrder());
    }
    public static float mouseDistance;

    void Update()
    {
        mouseDistance = GetMouseDistanceOnClick();
        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (!Game.pause)
        {
            CardControl();
            //Debug.Log("correct " + Game.stats.correct + " all " + Game.stats.cardsPlaced + " streak " + Game.stats.longestStreak + " time "+Game.stats.timeElapsed);
            Game.stats.CountTime();
        }
#if UNITY_EDITOR || UNITY_STANDALONE
        Scrolling();
#endif
#if UNITY_ANDROID || UNITY_WP8 || UNITY_WP8_1 || UNITY_IOS
        Swiping();
#endif
    }

    IEnumerator CUpdateDeck()
    {
        DeckOrdering();
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(CUpdateDeck());
    }

    IEnumerator CUpdateOrder()
    {
        OrderArrange();
        DeckArrange();
        yield return new WaitForSeconds(0.015f);
        StartCoroutine(CUpdateOrder());
    }

    void RefreshOrderParenting()
    {
        foreach (GameObject g in order)
        {
            if (g.transform.parent != orderParent.transform)
            {
                if (g.GetComponent<CardInformation>())
                {
                    g.GetComponent<CardInformation>().scale /= 1.2f;
                    //Debug.Log(g.GetComponent<CardInformation>().scale);
                }
                g.transform.parent = orderParent.transform;
            }
        }
        OrderBounding();
        visibleCards = GetVisibleCards();
    }

    Vector2 scrollTarget;
    float boundHalfRange;

    void Scrolling()
    {
        float axis = Input.GetAxis("Mouse ScrollWheel");
        if (axis!=0 && !Game.pause)
        {
            scrollTarget.y -= axis * scrollSpeed * Time.deltaTime;
            StartLadderSound();

        }
        else
        {
            StopLadderSound();
        }
        Vector2 tmp = orderParent.transform.position;
        tmp.y = Mathf.Lerp(tmp.y, scrollTarget.y, arrangeSpeed * Time.deltaTime);
        orderParent.transform.position = tmp;
        scrollTarget.y = Mathf.Clamp(scrollTarget.y, -boundHalfRange, boundHalfRange);
    }
    float deceleration = 0;
    void Swiping()
    {
        Vector2 tmp = orderParent.transform.position;
        if (!dragging)
        {
            if (Input.touchCount > 0 && !Game.pause)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    lastY = Camera.main.ScreenToWorldPoint(touch.position).y;
                }
                if (touch.phase == TouchPhase.Moved)
                {
                    deceleration = GetDelta(Camera.main.ScreenToWorldPoint(touch.position),0);
                    scrollTarget.y += deceleration;
                    tmp.y = scrollTarget.y;
                    StartLadderSound();
                }
                else
                {
                    StopLadderSound();
                }
            }
            else
            {
                scrollTarget.y += deceleration;
                deceleration = Mathf.MoveTowards(deceleration, 0, arrangeSpeed * Time.deltaTime);
                tmp.y = Mathf.Lerp(tmp.y, scrollTarget.y, arrangeSpeed * Time.deltaTime);
                if (GameManager.soundEffects)
                {
                    StartLadderSound();
                    if (deceleration <= 0)
                    {
                        StopLadderSound();
                    }
                }
            }
        }
        else
        {
            if (touchInput.UpBorder())
            {
                scrollTarget.y -= arrangeSpeed * Time.deltaTime;
                tmp.y = Mathf.Lerp(tmp.y, scrollTarget.y, arrangeSpeed * Time.deltaTime);
                StartLadderSound();
            }
            else if (touchInput.DownBorder())
            {
                scrollTarget.y += arrangeSpeed * Time.deltaTime;
                tmp.y = Mathf.Lerp(tmp.y, scrollTarget.y, arrangeSpeed * Time.deltaTime);
                StartLadderSound();
            }
            else
            {
                StopLadderSound();
            }
        }
        tmp.y = Mathf.Clamp(tmp.y, -boundHalfRange, boundHalfRange);
        scrollTarget.y = Mathf.Clamp(scrollTarget.y, -boundHalfRange, boundHalfRange);
        orderParent.transform.position = tmp;
    }
    float lastY = 0;
    float delta = 0;
    public float GetDelta(Vector2 t,float dead)
    {
        delta = t.y - lastY;
        lastY = t.y;
        if (delta > dead || delta < -dead) 
        { 
            return delta; 
        } 
        return 0;
    }
    void OrderBounding()
    {

        boundHalfRange = ((offset) * (order.Count / 2));
        Vector3 tmp = orderParent.transform.position;
        tmp.y = Mathf.Clamp(tmp.y, -boundHalfRange, boundHalfRange);
        orderParent.transform.position = tmp;
        oCount = order.Count;
        
    }
    int dCount = 0;
    void DeckOrdering()
    {
        if (dCount != deck.Count)
        {
            if (deck.Count > 0)
            {
                Vector3 tmp;
                deck[deck.Count - 1].GetComponent<BoxCollider2D>().enabled = true;
                if (deck.Count > 1)
                {
                    tmp = deck[deck.Count - 1].transform.localPosition;
                    tmp.z = -0.2f;
                    deck[deck.Count - 1].transform.localPosition = tmp;
                    tmp = deck[deck.Count - 2].transform.localPosition;
                    tmp.z = -0.1f;
                    deck[deck.Count - 2].transform.localPosition = tmp;
                    for (int i = deck.Count - 2; i >= 0; i--)
                    {
                        deck[i].GetComponent<BoxCollider2D>().enabled = false;
                    }
                }
            }
            dCount = deck.Count;
        }
    }

    void MoveCard(GameObject card, Vector2 position,float speed)
    {
        Vector3 tmp = card.transform.position;
        tmp = Vector2.Lerp(tmp, position, speed * Time.deltaTime);
        if (tmp.z != -5)
        {
            tmp.z = -5;
        }
        card.transform.position = tmp;
    }
    public RaycastHit2D hit;
    [HideInInspector]
    public bool startDragging = false;
    void CardControl()
    {
        if (hit && deck.Contains(hit.collider.gameObject) && Input.GetMouseButtonDown(0))
        {
            dragging = hit.collider.gameObject;
        }
        if (dragging && Input.GetMouseButton(0) && dragging.GetComponent<CardInformation>().inOrder)
        {
            if (mouseDistance > 1 && !startDragging)
            {
                deck.Remove(dragging);
                startDragging = true;
            }
            if (startDragging)
            {
                MoveCard(dragging, Camera.main.ScreenToWorldPoint(Input.mousePosition), dragSpeed);
            }
        }
        if (Input.GetMouseButtonUp(0) && dragging && startDragging)
        {
            if (order.Contains(emptySpace))
            {
                bool exactPosition = GetCorrectPosition(dragging.GetComponent<CardController>().card).Contains(order.IndexOf(emptySpace)+1);
                bool relativePosition = false;
                int insert=0;

                if(Game.dificulty == Difficulty.Normal && !exactPosition){
                    Vector2 range = new Vector2(order.IndexOf(emptySpace)-1,order.IndexOf(emptySpace)+2);
                    range.x = Mathf.Clamp(range.x,0,order.Count);
                    range.y = Mathf.Clamp(range.y,0,order.Count);

                    foreach (int i in GetCorrectPosition(dragging.GetComponent<CardController>().card))
                    {
                        if (i >= 0 && i<=order.Count)
                        {
                            if (i >= range.x && i <= range.y)
                            {
                                insert = GetCorrectPosition(dragging.GetComponent<CardController>().card)[0];
                                relativePosition = true;
                                closeEnough.SetActive(true);
                                break;
                            }
                        }
                    }
                    
                }
                if(exactPosition)
                {
                    insert = order.IndexOf(emptySpace);
                    Correct(insert);
                }else
                if (relativePosition)
                {
                    CloseEnough(insert);
                }
                else
                {
                    Wrong();
                }
            }
            else
            {
                if (!deck.Contains(dragging))
                {
                    deck.Insert(deck.Count, dragging);
                }
                dragging = null;
            }

            startDragging = false;
        }
    }

    void Correct(int insert)
    {
        if (GameManager.soundEffects)
        {
            correctSound.Play();
        }
        order.Insert(insert, dragging);
        order.Remove(emptySpace);
        dragging = null;
        Game.stats.CorrectCard();
        RefreshOrderParenting();
        StartCoroutine(DealCard(0.3f));
        //Debug.Log("correct");
    }

    void CloseEnough(int insert)
    {
        if (GameManager.soundEffects)
        {
            wrongSound.Play();
        }
        order.Insert(insert, dragging);
        order.Remove(emptySpace);
        dragging = null;
        Game.stats.CloseEnoughCard();
        RefreshOrderParenting();
        StartCoroutine(DealCard(0.5f));
    }

    void Wrong()
    {
        if (GameManager.soundEffects)
        {
            wrongSound.Play();
        }

        StartCoroutine(WrongCoroutine());
        Game.stats.WrongCard();
        //Debug.Log("wrong");
    }
    IEnumerator WrongCoroutine()
    {
        GameObject dragtmp = dragging;
        dragging = null;
        dragtmp.GetComponent<CardInformation>().ShowNumber();
        yield return new WaitForSeconds(0.5f);
        order.Remove(emptySpace);

        Card draggingCard = dragtmp.GetComponent<CardController>().card;



        order.Insert(GetCorrectPosition(draggingCard)[0], dragtmp);

        //ids[0] is the upper card , ids[1] is the current card(wrong one) , ids[2] is the lower card
        //make sure to run this function exactly here or there will be errors
        string[] ids = Game.GetFullID(order.IndexOf(dragtmp), order);
        Debug.Log(ids[0] + " / " + ids[1] + " / " + ids[2]);
		Database.Shared.AddInfo (ids[1],ids[2],ids[0],(str)=>print(str));
        ///////////////////////////////////////////////////////////////////



        RefreshOrderParenting();
        FollowWrongCard(dragtmp);
        StartCoroutine(DealCard(0.5f));
        Game.triesLeft--;
        if (dragging == dragtmp) dragging = null;
        yield break ;
    }

    void FollowWrongCard(GameObject card)
    {
        scrollTarget.y = -GetPositionInList(order, order.IndexOf(card), offset).y;

    }

    void DeckArrange()
    {
        if (deck.Count > 0)
        {
            GameObject card = deck[deck.Count - 1];
            if (card.GetComponent<CardInformation>().inOrder)
            {
                card.transform.localPosition = Vector3.Lerp(card.transform.localPosition, new Vector3(0, 0, -0.8f), arrangeSpeed * Time.deltaTime);
                if (deck.Count > 2)
                {
                    card = deck[deck.Count - 2];
                    card.transform.localPosition = new Vector3(0, 0, -0.2f);
                }
            }
        }
    }

    List<GameObject> visibleCards = new List<GameObject>();
    bool startArrange = true;
    void OrderArrange()
    {

        for (int i = 0; i < order.Count; i++)
        {
            bool moveable = order[i].GetComponent<CardInformation>() ? order[i].GetComponent<CardInformation>().inOrder : true;
            Vector2 to = new Vector2(0, (i - ((order.Count - 1) / 2.0f)) * offset);
            if (order[i].transform.localPosition != new Vector3(to.x,to.y,0) && moveable)
            {
                if (visibleCards.Contains(order[i]))
                {
                    Vector2 from = order[i].transform.localPosition;
                    order[i].transform.localPosition = Vector2.Lerp(from, to, arrangeSpeed * Time.deltaTime);

                }
                else
                {
                    order[i].transform.localPosition = to;
                }
            }
        }
        
    }

    void StartLadderSound()
    {
        if (!ladderScrollSound.isPlaying && GameManager.soundEffects)
        {
            ladderScrollSound.Play();
        }
    }

    void StopLadderSound()
    {
        if (ladderScrollSound.isPlaying && GameManager.soundEffects)
        {
            ladderScrollSound.Stop();
        }
    }

    Vector2 GetPositionInList(List<GameObject> list, int index,float off)
    {
        return new Vector2(0, (index - ((list.Count - 1) / 2.0f)) * off);
    }

    public List<int> GetCorrectPosition(Card draggingCard)
    {
        List<int> correctPositions = new List<int>();
        for (int i = 0; i < order.Count; i++)
        {
            if (order[i].GetComponent<CardController>())
            {
                Card card = order[i].GetComponent<CardController>().card;

                if (draggingCard.mortality == card.mortality)
                {
                    correctPositions.Add(i);
                }
                if (draggingCard.mortality < card.mortality)
                {
                    correctPositions.Add(i);
                    break;
                }
                if (i == order.Count - 1 && draggingCard.mortality > card.mortality)
                {
                    correctPositions.Add(i + 1);
                }
            }
            else
            {
                if (i == order.Count - 1 && draggingCard.mortality >= order[i-1].GetComponent<CardController>().card.mortality)
                {
                    correctPositions.Add(i+1);
                }
            }
        }
        return correctPositions;
    }

    List<GameObject> GetVisibleCards()
    {
        List<GameObject> visibles = new List<GameObject>();
        float cameraMaxBoundY = Camera.main.transform.position.y + cameraHalfHeight+offset;
        float cameraMinBoundY = Camera.main.transform.position.y - (cameraHalfHeight+offset);
        foreach (GameObject g in order)
        {
            if (g.transform.position.y < cameraMaxBoundY && g.transform.position.y > cameraMinBoundY)
            {
                visibles.Add(g);
            }
        }
        return visibles;
    }

    private static Vector2 startLocation;
    public static float GetMouseDistanceOnClick()
    {
        float distance = 0;
        if (Input.GetMouseButtonDown(0))
        {
            startLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(0))
        {
            distance = Vector2.Distance(startLocation, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        return distance;
    }

    IEnumerator DealCard(float time)
    {
        yield return new WaitForSeconds(time);
        if (deck.Count > 0)
        {
            deck[deck.Count - 1].SetActive(true);
            if (GameManager.soundEffects)
            {
                dealSound.Play();
            }
        }
    }

}
