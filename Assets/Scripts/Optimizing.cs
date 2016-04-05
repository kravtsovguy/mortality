//Written By Amr Zewail
//Copyright 2015-2016
//Kalponic Games
//-----------------------

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[@RequireComponent(typeof(Controller))]
public class Optimizing : MonoBehaviour{

    Controller controller;
    void Awake()
    {
        Application.targetFrameRate = 60;
    }
    void Start()
    {
        controller = GetComponent<Controller>();
    }
    int dOrder = 0;
    void Update()
    {
        if (dOrder != controller.deck.Count)
        {
            //if (controller.deck.Count > 3)
            //{
            //    for (int i = 0; i < controller.deck.Count - 2; i++)
            //    {
            //        controller.deck[i].SetActive(false);
            //    }
            //    for (int i = controller.deck.Count - 2; i < controller.deck.Count; i++)
            //    {
            //        controller.deck[i].SetActive(true);
            //    }
            //}
            //else
            //{
            //    foreach (GameObject g in controller.deck)
            //    {
            //        g.SetActive(true);
            //    }
            //}
            if (controller.deck.Count > 0)
            {
                for (int i = 0; i < controller.deck.Count - 1; i++)
                {
                    controller.deck[i].SetActive(false);
                }
            }
            dOrder = controller.deck.Count;
        }
    }

}
