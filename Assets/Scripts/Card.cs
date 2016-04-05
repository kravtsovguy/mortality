//Written By Amr Zewail
//Skylight
//-----------------------

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

[Serializable]
public class Card
{
    public string name;
    public string source;
    public int mortality;
    public string definition;
    public string time_scale;
    public string location;
    public string image;

    public Card()
    {
        name = "card";
        mortality = -1;
    }
    public Card(string Name,string Source, int Mortality,string Definition,string Time_scale,string Location,string Img="")
    {
        name = Name;
        source = Source;
        mortality = Mortality;
        definition = Definition;
        time_scale = Time_scale;
        location = Location;
        image = Img;
    }

}