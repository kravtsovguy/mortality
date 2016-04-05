//Written By Amr Zewail
//Copyright 2015-2016
//Kalponic Games
//-----------------------

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MenuUI : MonoBehaviour
{
    public CButton newGame;
    public CButton badPack;
    public CButton stupidPack;

    void Update()
    {
        if (newGame.isClicked)
        {
            DoScript.LoadScene("Programming");
        }
    }
}