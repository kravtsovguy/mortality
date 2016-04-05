//Written By Amr Zewail
//Copyright 2015-2016
//Kalponic Games
//-----------------------

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class UnitySingleton : MonoBehaviour{

    private static UnitySingleton instance = null;
    public static UnitySingleton Instance
    {
        get { return instance; }
    }
    void Awake() {
     if (instance != null && instance != this) {
         Destroy(this.gameObject);
         return;
     } else {
         instance = this;
     }
     DontDestroyOnLoad(this.gameObject);
 }

}
