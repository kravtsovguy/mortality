//Written By Amr Zewail
//Copyright 2015-2016
//Kalponic Games
//-----------------------

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class AudioSwitcher : MonoBehaviour{

    public CSwitchButton switcher;

    void Start()
    {
        if (GameManager.audio.isPlaying != switcher.on)
        {
            switcher.switchButton.Switch();
        }
    }
    void Update()
    {
        if (switcher.isClicked)
        {
            if (GameManager.audio.isPlaying)
            {
                GameManager.audio.Stop();
                return;
            }
            else
            {
                GameManager.audio.Play();
            }
        }
    }

}
