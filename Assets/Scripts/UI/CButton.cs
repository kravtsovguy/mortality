//Written By Amr Zewail
//Copyright 2015-2016
//Kalponic Games
//-----------------------

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using CUIEngine;

[@RequireComponent(typeof(SpriteRenderer))]
public class CButton : MonoBehaviour{

    CUserInterface ui =new CUserInterface();
    public Sprite clickedSprite;
    //[HideInInspector]
    public bool isClicked;
    public bool Switch = false;
    public bool disable = false;
    void Start()
    {

        ui.SetButton(this.name, GetComponent<SpriteRenderer>().sprite, this.gameObject,clickedSprite);
    }

    void Update()
    {
        if (!disable)
        {
            ui.Run();
            isClicked = ui.ButtonClicked(this.name);
        }
    }

}