using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using CMethods;
public class ResultsManager : MonoBehaviour {

    public Text grade;
    public Text cardsCorrect;
    public Text longestStreak;
    public Text time;
    public Button returnButton;
    public GameObject main;
    public GameObject window;
    public GameObject cover;
    public Transform gradeFinalPosition;

    TextWriter gradeTW;
    TextWriter correctTW;
    TextWriter streakTW;
    TextWriter timeTW;


    CoolDown windowShowCD;
    bool showStats = false;
    bool coverAnimationPlayed = false;
    //bool runCorrectAnim = false;
    //bool runStreakAnim = false;
    //bool runTimeAnim = false;

    private Vector2 windowEndPosition;

	// Use this for initialization
	void Start () {

        gradeTW = new TextWriter(Game.stats.grade, 0.1f);
        correctTW = new TextWriter(Game.stats.correct.ToString()+" / "+Game.stats.AllCards(), 0.1f);
        streakTW = new TextWriter(Game.stats.longestStreak.ToString(), 0.1f);
        timeTW = new TextWriter(ConvertSecondsToDate((int)Game.stats.timeElapsed), 0.1f);


        //windowEndPosition = backWindow.transform.position;
        windowShowCD = new CoolDown(2);
        //StartCoroutine(ShowStats());

        grade.text = "";
        cardsCorrect.text = "";
        longestStreak.text = "";
        time.text = "";
        returnButton.gameObject.SetActive(false);
        //window.transform.localScale = new Vector3(1f, 0.1f, 1f);
        //grade.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        //WindowVisibility(false);
	}

    float waitTimer = 0.3f;

	// Update is called once per frame
	void Update () {
        windowShowCD.Run();
        //if (!windowShowCD.done)
        //{
        //    grade.transform.localScale = Vector3.MoveTowards(grade.transform.localScale, new Vector3(1.5f,1.5f,1.5f),10 * Time.deltaTime);
        //}
        //if (windowShowCD.done)
        //{
        //    grade.transform.position = Vector3.MoveTowards(grade.transform.position,gradeFinalPosition.position, 750 * Time.deltaTime);
        //    grade.transform.localScale = Vector3.MoveTowards(grade.transform.localScale, gradeFinalPosition.localScale,3 * Time.deltaTime);
        //}
        //if (grade.transform.position == gradeFinalPosition.position)
        //{
        //    WindowVisibility(true);
        //    window.transform.localScale = Vector3.MoveTowards(window.transform.localScale, new Vector3(1, 1, 1), 7 * Time.deltaTime);
        //}
        //if (window.transform.localScale == new Vector3(1, 1, 1) && !showStats)
        //{
        //    StartCoroutine(ShowStats());
        //    showStats = true;
        //}
        //if (runCorrectAnim)
        //{
        //    cardsCorrect.transform.localScale = Vector3.MoveTowards(cardsCorrect.transform.localScale, new Vector3(1, 1, 1), 20 * Time.deltaTime);
        //}
        //if (runStreakAnim)
        //{
        //    longestStreak.transform.localScale = Vector3.MoveTowards(longestStreak.transform.localScale, new Vector3(1, 1, 1), 20 * Time.deltaTime);
        //}
        //if (runTimeAnim)
        //{
        //    time.transform.localScale = Vector3.MoveTowards(time.transform.localScale, new Vector3(1, 1, 1), 20 * Time.deltaTime);
        //}
        if (waitTimer > 0)
        {
            waitTimer -= Time.deltaTime;
        }


        if (windowShowCD.done && !coverAnimationPlayed)
        {

            CAnimation.PlaySpriteSheet(cover, "CoverFlip", 1, false);
            coverAnimationPlayed = true;
        } 
        if (CAnimation.SpriteSheetDone(cover, "CoverFlip") && !gradeTW.Done() && waitTimer <= 0)
        {
            grade.text = gradeTW.Run();
            if (gradeTW.Done())
            {
                waitTimer = 0.3f;
            }
        }
        if (gradeTW.Done() && !correctTW.Done() && waitTimer<=0)
        {
            cardsCorrect.text = correctTW.Run();
            if(correctTW.Done()){
                waitTimer = 0.3f;    
            }
        }
        if (correctTW.Done() && !streakTW.Done() && waitTimer<=0)
        {
            longestStreak.text = streakTW.Run();
            if (streakTW.Done())
            {
                waitTimer = 0.3f;
            }

        }
        if (streakTW.Done() && !timeTW.Done() && waitTimer<=0)
        {
            time.text = timeTW.Run();
            if (timeTW.Done())
            {
                waitTimer = 0.3f;
            }
        }
        if (timeTW.Done() && waitTimer <= 0)
        {
            returnButton.gameObject.SetActive(true);
        }
        
    }

    private string ConvertSecondsToDate(int seconds)
    {
        TimeSpan ts = new TimeSpan(0, 0, seconds);
        string str = string.Format("{0:D2}:{1:D2}", ts.Minutes, ts.Seconds);
        return str;
    }

    void WindowVisibility(bool visible)
    {
        window.GetComponent<Image>().enabled = visible;
    }

}

public class TextWriter
{
    public string text="";
    public string fullText;
    private float coolDown;
    private float cd;
    private int index=0;
    public TextWriter(string full_text, float cool_down)
    {
        fullText = full_text;
        coolDown = cool_down;
        cd = coolDown;
    }

    public string Run()
    {
        cd -= Time.deltaTime;
        if (cd <= 0 && index<fullText.Length)
        {
            text += fullText[index];
            index++;
            cd = coolDown;
        }
        return text;
    }

    public bool Done()
    {
        if (text == fullText)
        {
            return true;
        }
        return false;
    }

    public string GetText()
    {
        return text;
    }
}
