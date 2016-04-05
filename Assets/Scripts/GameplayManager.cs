//Written By Amr Zewail
//Skylight
//-----------------------

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using CSave;
using System.IO;
using System.Xml.Serialization;

public class GameplayManager : MonoBehaviour{

    Controller con;

    void Awake()
    {
        if (Game.contentPacks.Count == 0)
        {
            ContentPack cp;
            TextAsset t = Resources.Load("TestPack") as TextAsset;

            using (StreamWriter s = new StreamWriter(Application.persistentDataPath + "/" + this.name + ".xml"))
            {
                s.Write(t.text);
            }

            cp = CSave.SaveEngine.LoadFromXML<ContentPack>(Application.persistentDataPath + "/" + this.name + ".xml");
            Game.contentPacks.Add(cp);
        }
        Game.stats = new Stats();
        Game.pause = false;
        Game.triesLeft = 3;
        con = Camera.main.GetComponent<Controller>();
    }

    void Update()
    {
        bool done = (con.deck.Count == 0 && !con.dragging);
        if (done || Game.triesLeft==0)
        {
            Game.stats.SetGrade();
            if (!File.Exists(Application.persistentDataPath + "/stats.xml"))
            {
                TextAsset t = Resources.Load("AllStatsTemplate") as TextAsset;
                using (StreamWriter s = new StreamWriter(Application.persistentDataPath + "/stats.xml"))
                {
                    s.Write(t.text);
                }
            }

			Game.UploadStatsToAnalytics();

            XmlSerializer xml = new XmlSerializer(typeof(AllStats));
            AllStats all = new AllStats();
            using (Stream inputStream = File.OpenRead(Application.persistentDataPath + "/stats.xml"))
            {
                all = (AllStats)xml.Deserialize(inputStream);
            }

            all.stats.Add(Game.stats);

            // Serialize.
            using (Stream outputStream = File.OpenWrite(Application.persistentDataPath + "/stats.xml"))
            {
                xml.Serialize(outputStream, all);
            }
            StartCoroutine(ShowResults());
            this.enabled = false;
            Game.pause = true;
        }
    }


    IEnumerator ShowResults()
    {
        yield return new WaitForSeconds(2);
        con.resultsWindow.SetActive(true);
    }

}
