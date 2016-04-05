//Written By Amr Zewail
//Skylight
//-----------------------

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

public class ContentPack{
	
	public string file;
	public string name;
	public License license = License.Buy;
	public List<Card> deck = new List<Card>();
	public int longestStreak=0;
	public int correctCards=0;
	
	public ContentPack()
	{
		name = "ContentPack";
	}
	public ContentPack(string N)
	{
		name = N;
	}
	
	public void Save()
	{
		using (Stream s = File.Create(Application.persistentDataPath+"/"+this.name+".xml"))
		{
			XmlSerializer x = new XmlSerializer(this.GetType());
			x.Serialize(s, this);
			
		}
	}
	
	public static ContentPack LoadContentPack(string ContentPackname)
	{
		if (File.Exists(Application.persistentDataPath+"/"+ContentPackname+".xml"))
		{
			using (Stream s = File.Open(Application.persistentDataPath +"/"+ContentPackname + ".xml", FileMode.Open))
			{
				var obj = Activator.CreateInstance(typeof(ContentPack));
				XmlSerializer x = new XmlSerializer(obj.GetType());
				
				return (ContentPack)x.Deserialize(s);
			}
		}
		else
		{
			return (ContentPack)Activator.CreateInstance(typeof(ContentPack));
		}
	}
}

public static class Game
{
	public static List<ContentPack> contentPacks = new List<ContentPack>();
	public static List<string> unlockedContentPacks = new List<string>();
    public static GameObject firstCard;
	public static Stats stats;
	public static bool pause = false;
	public static int triesLeft = 3;
	public static Difficulty dificulty = Difficulty.Normal;
	
	public static void RemoveDuplicates(this List<GameObject> list)
	{
		if (list[0].GetComponent<CardController>())
		{
			for (int i = 0; i < list.Count; i++)
			{
				for (int ii = 0; ii < list.Count; ii++)
				{
					Card cardi = list[i].GetComponent<CardController>().card;
					Card cardii = list[ii].GetComponent<CardController>().card;
					
					if (list[i] != list[ii])
					{
						if (cardi.name == cardii.name)
						{
							GameObject des = cardi.mortality >= cardii.mortality ? list[ii] : list[i];
							list.Remove(des);
							GameObject.Destroy(des);
						}
					}
				}
			}
		}
	}

    public static void ApplyFirstCardFilter(this List<GameObject> list)
    {
        List<GameObject> alls = new List<GameObject>();
        foreach (ContentPack cp in contentPacks)
        {
            int highestCard = 0;
            for (int i = 0; i < cp.deck.Count; i++)
            {
                if (cp.deck[i].mortality > cp.deck[highestCard].mortality)
                {
                    highestCard = i;
                }
            }
            foreach (GameObject g in list)
            {
                if (g.GetComponent<CardController>().card == cp.deck[highestCard])
                {
                    alls.Add(g);
                    break;
                }
            }
        }
        Debug.Log(alls.Count);
        foreach (GameObject g in alls)
        {
            if (g.GetComponent<CardController>().card.location == "Global")
                firstCard = g;
        }
        foreach (GameObject g in alls)
        {
            if (g.GetComponent<CardController>().card.location == "US")
                firstCard = g;
        }
    }
	
	//new functions of IDs start here
	
	private static string GetID(Card card)
	{
		string deck = "";
		foreach (ContentPack cp in contentPacks)
		{
			if (cp.deck.Contains(card))
			{
				deck = cp.name;
			}
		}
		string str = deck + ":" + card.name;
		return str;
	}
	
	public static string[] GetFullID(int cardPosition, List<GameObject> list)
	{
		string[] IDs = new string[3];
		IDs[1] = GetID(list[cardPosition].GetComponent<CardController>().card);
		if (cardPosition < list.Count - 1)
		{
			IDs[0] = GetID(list[cardPosition + 1].GetComponent<CardController>().card);
		}
		if (cardPosition > 0)
		{
			IDs[2] = GetID(list[cardPosition-1].GetComponent<CardController>().card);
			Debug.Log("works");
		}
		return IDs;
	}
	
	//ends here
	
	public static void UploadStatsToAnalytics(){
		AnalyticsManager.TrackCustomEvent("Game:Stats:cardsPlaced", stats.cardsPlaced);
		AnalyticsManager.TrackCustomEvent("Game:Stats:correct", stats.correct);
		AnalyticsManager.TrackCustomEvent("Game:Stats:longestStreak", stats.longestStreak);
		AnalyticsManager.TrackCustomEvent("Game:Stats:timeElapsed", stats.timeElapsed);
		AnalyticsManager.TrackCustomEvent("Game:Stats:grade:" + stats.grade);
		AnalyticsManager.TrackCustomEvent("Game:Stats:dificulty:" + dificulty.ToString());
		
		
		Debug.Log ("Stats uploaded!");
	}
	
}

public class AllStats
{
	public List<Stats> stats = new List<Stats>();
}

public class Stats
{
	public int cardsPlaced = 0;
	public int correct;
	public int longestStreak = 0;
	public float timeElapsed = 0.0f;
	public string grade = "";
	
	private int streak;
	
	public void SetGrade()
	{
		int percent = (correct * 100) / AllCards();
		
		if (percent > 99) { grade = "Perfect"; return; }else
		if (percent > 89) { grade = "Nearly Perfect"; return; }else
		if (percent > 79) { grade = "Amazing"; return; }else
		if (percent > 69) { grade = "Great"; return; }else
		if (percent > 59) { grade = "Very Good"; return; }else
		if (percent > 49) { grade = "Good"; return; }else
		if (percent > 29) { grade = "Not Bad"; return; }else
			grade = "Try Again";
		
		
	}
	
	public void CorrectCard()
	{
		cardsPlaced++;
		correct++;
		streak++;
		if (streak > longestStreak)
		{
			longestStreak = streak;
		}
	}
	
	public void CloseEnoughCard()
	{
		cardsPlaced++;
		streak = 0;
	}
	
	public void WrongCard()
	{
		cardsPlaced++;
		streak = 0;
	}
	
	public void CountTime()
	{
		timeElapsed += Time.deltaTime;
	}
	
	public int AllCards()
	{
		int all = 0;
		foreach (ContentPack cp in Game.contentPacks)
		{
			all += cp.deck.Count;
		}
		return all;
	}
	
}


public enum License
{
	Free,
	Buy,
	Unlocked
};

public enum Difficulty
{
	Normal=0,
	Hard=1
};