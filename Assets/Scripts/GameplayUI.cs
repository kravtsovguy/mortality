//Written By Amr Zewail
//Copyright 2015-2016
//Kalponic Games
//-----------------------

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class GameplayUI : MonoBehaviour
{

    public CButton backButton;

    void Update()
    {
        if (backButton.isClicked)
        {
            DoScript.LoadScene("Menu");
        }
    }

}
