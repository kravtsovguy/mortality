//Written By Amr Zewail
//Copyright 2015-2016
//Kalponic Games
//-----------------------

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using CSave;
using CUIEngine;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.UI;
public class PackController : MonoBehaviour{


    //public string pack;
    public GameObject packPrefab;
    void Awake()
    {
        Packs packs = LoadPacks.packs;
        List<GameObject> pcks = new List<GameObject>();
        for (int i = 0; i < packs.packs.Count; i++)
        {

            GameObject p = GameObject.Instantiate(packPrefab, packPrefab.transform.position, packPrefab.transform.rotation) as GameObject;
            ContentPack pack = packs.packs[packs.packs.Count-i-1];

            p.name = pack.file;
            p.GetComponent<PackCheck>().license = pack.license;
            if (Game.unlockedContentPacks.Contains(pack.name))
            {
                p.GetComponent<PackCheck>().license = License.Unlocked;
            }

			print("PackController - "+pack.name);
			if(IAPManager.shared.HasProduct(pack.name))
			{
				p.GetComponent<PackCheck>().license = License.Unlocked;
			}

            p.transform.SetParent(packPrefab.transform.parent);
            p.transform.localScale = packPrefab.transform.localScale;
            Vector3 position = packPrefab.transform.localPosition;
            position.z = 0;

            Vector2 to = new Vector2(position.x, (i - ((packs.packs.Count - 1) / 2.0f)) * packPrefab.GetComponent<RectTransform>().rect.height * 2);
            Vector2 from = p.transform.localPosition;
            p.transform.localPosition = to;

            p.transform.FindChild("Label").GetComponent<Text>().text = pack.name;
            pcks.Add(p);


            
        }
        BoxCollider bound = GameObject.FindObjectOfType<BoxCollider>();
        bound.transform.position = pcks[0].transform.position + ((pcks[pcks.Count-1].transform.position-pcks[0].transform.position)/2);
        float y = ((RectBoundsY(pcks[pcks.Count - 1]).y - RectBoundsY(pcks[0]).x) - 1705.435f/2);
        bound.transform.localScale = new Vector3(1, y);
        Destroy(packPrefab);
    }

    List<GameObject> GetOffScreen(List<GameObject> list)
    {
        List<GameObject> off = new List<GameObject>();
        foreach (GameObject g in list)
        {
            if (g.transform.position.y > GameObject.FindObjectOfType<BoxCollider>().bounds.max.y)
            {
                off.Add(g);
            }
            else if (g.transform.position.y < GameObject.FindObjectOfType<BoxCollider>().bounds.min.y)
            {
                off.Add(g);
            }
        }
        return off;
    }

    Vector2 RectBoundsY(GameObject g)
    {
        RectTransform rt = g.GetComponent<RectTransform>();
        float x = rt.transform.localPosition.y - rt.sizeDelta.y/2;
        float y = rt.transform.localPosition.y + rt.sizeDelta.y / 2;
        return new Vector2(x, y);
    }
}

public class Packs
{
    public List<ContentPack> packs = new List<ContentPack>();
}

