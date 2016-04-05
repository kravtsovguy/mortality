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
public class CSwitchButton : MonoBehaviour{

    CUserInterface ui =new CUserInterface();
    public SwitchButton switchButton;
    public Sprite clickedSprite;
    public Sprite switchNormal;
    public Sprite switchClicked;
    //[HideInInspector]
    public bool isClicked;
    public bool disable = false;
    [HideInInspector]
    public bool on=true;
    void Awake()
    {

        switchButton = ui.SetSwitchButton(this.name, GetComponent<SpriteRenderer>().sprite,switchNormal, this.gameObject,clickedSprite,switchClicked);
        isClicked = switchButton.isClicked;
        on = switchButton.on;
    }

    void Update()
    {
        if (!disable)
        {
            ui.Run();
            isClicked = switchButton.isClicked;
            on = switchButton.on;
        }
    }

}