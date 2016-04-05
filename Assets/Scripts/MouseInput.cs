//Written By Amr Zewail
//Copyright 2015-2016
//Kalponic Games
//-----------------------

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using CMethods;

public class MouseInput{

    public bool SwipeUp()
    {
        if (CInput.MouseSpeed().y > Screen.height / 10)
        {
            return true;
        }
        return false;
    }

    public bool SwipeDown()
    {
        if (CInput.MouseSpeed().y < Screen.height / -10)
        {
            return true;
        }
        return false;
    }
}
