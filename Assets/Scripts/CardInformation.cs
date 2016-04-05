//Written By Amr Zewail
//Copyright 2015-2016
//Kalponic Games
//-----------------------

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using CMethods;
[@RequireComponent(typeof(CardController))]
public class CardInformation : MonoBehaviour{

    public Text number;
    public Text nameText;
    public Text definition;
    public Text source;
    public Text timeScale;
    public Image location;
    public Image image;
    public GameObject front;
    public GameObject back;
    CardController cc;
    public float flipSpeed;
    public float scale;
    public float horizontalOffset=-2;
    [HideInInspector]
    public float hOffset=0;
    [HideInInspector]
    public bool disableFlip = false;

    void Start()
    {
        cc = GetComponent<CardController>();
        //scale = transform.localScale.x;
        number.text = string.Format("{0:#,###0.#}", cc.card.mortality);
        nameText.text = cc.card.name;
        number.enabled = false;
        timeScale.text = cc.card.time_scale;
        definition.text = cc.card.definition;
        source.text = cc.card.source;
        location.sprite = CSprite.LoadSprite("M_Icons/" + cc.card.location);
        image.sprite = CSprite.LoadSprite("M_Icons/"+cc.card.image);
    }
    void Update()
    {
        if (cc.controller.order.Contains(this.gameObject))
        {
            number.enabled = true;
        }
        FlipAnimation();

        if (!Game.pause)
        {
            if (Input.GetMouseButtonDown(0))
            {
                disableFlip = false;
            }
            if ((Input.GetMouseButton(0) && Controller.mouseDistance > 1) || (Camera.main.GetComponent<FlipController>().flippedCard && Camera.main.GetComponent<FlipController>().flippedCard != this.gameObject))
            {
                disableFlip = true;
            }
            if (Input.GetMouseButtonUp(0) && cc.controller.hit && cc.controller.hit.collider.gameObject == this.gameObject && !disableFlip)
            {
                Flip();
            }
        }
            
    }

    public void Flip()
    {

        front.SetActive(!front.activeSelf);
        back.SetActive(!back.activeSelf);
        hOffset = hOffset == horizontalOffset ? 0 : horizontalOffset;
    }

    public bool inOrder = true;
    void FlipAnimation()
    {
        if (front.activeSelf && transform.localScale.x != scale)
        {
            Vector2 tmp = transform.localScale;
            tmp.x = Mathf.MoveTowards(tmp.x, scale, flipSpeed * Time.deltaTime);
            tmp.y = Mathf.MoveTowards(tmp.y, scale, flipSpeed * Time.deltaTime);
            transform.localScale = tmp;
            inOrder = true;
            if (Camera.main.GetComponent<FlipController>().flippedCard == this.gameObject)
                Camera.main.GetComponent<FlipController>().flippedCard = null;
        }
        else if(!front.activeSelf && transform.localScale.x != -scale)
        {
            inOrder = false;
            Camera.main.GetComponent<FlipController>().flippedCard = this.gameObject;
        }
    }

    public void ShowNumber()
    {
        number.enabled = true;
    }
}