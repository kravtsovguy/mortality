using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using System;
public class LoadPacks : MonoBehaviour {

    public static Packs packs;

	// Use this for initialization
    void Awake()
    {
        Game.contentPacks.Clear();
        Game.stats = null;
        StreamWriter file = null;
        TextAsset ps = Resources.Load("Packs") as TextAsset;
        using (file = new StreamWriter(Application.persistentDataPath + "/Packs.xml"))
        {
            file.Write(ps.text);
        }

        packs = new Packs();

        using (Stream s = File.Open(Application.persistentDataPath + "/Packs.xml", FileMode.Open))
        {
            var obj = Activator.CreateInstance(typeof(Packs));
            XmlSerializer x = new XmlSerializer(obj.GetType());
            packs = (Packs)x.Deserialize(s);
        }
    }
}
