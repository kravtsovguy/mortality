//Written By Amr Zewail
//Skylight
//-----------------------

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using CMethods;

public class CardController : MonoBehaviour
{
    public Card card = new Card();
    [HideInInspector]
    public Controller controller;
    [HideInInspector]
    public BoxCollider2D bc;
    void Start()
    {
        controller = Camera.main.GetComponent<Controller>();
        bc = GetComponent<BoxCollider2D>();

        if (!controller.emptySpace)
        {
            controller.emptySpace = new GameObject("Empty Space");
        }
    }
    bool remove = false;
    void Update()
    {
        if (controller.order.Contains(this.gameObject) && !remove)
        {
            this.enabled = false;
            Destroy(GetComponent<Rigidbody2D>());
            remove = true;
        }
    }
    GameObject card_g;
    BoxCollider2D cardBC;

    void OnTriggerStay2D(Collider2D col)
    {
        if (controller.dragging == this.gameObject && controller.startDragging && GetComponent<CardInformation>().inOrder)
        {
            if (col && col.gameObject && col.tag == "DCTrigger")
            {
                for (int i = 0; i < controller.order.Count; i++)
                {
                    card_g = controller.order[i];
                    cardBC = card_g.GetComponent<BoxCollider2D>();
                    if (cardBC && (CSystem.ValueBetween(cardBC.bounds.max.y, bc.bounds.min.y, bc.bounds.max.y) ||
                        CSystem.ValueBetween(cardBC.bounds.min.y, bc.bounds.min.y, bc.bounds.max.y)))
                    {

                        int ext = 0;
                        if (CSystem.ValueBetween(cardBC.bounds.max.y, bc.bounds.min.y, bc.bounds.max.y))
                        {
                            ext++;
                        }

                        if (!controller.order.Contains(controller.emptySpace))
                        {
                            controller.order.Insert(controller.order.IndexOf(card_g) + ext, controller.emptySpace);

                        }
                        else if (controller.order.IndexOf(controller.emptySpace) != controller.order.IndexOf(card_g) + ext)
                        {
                            controller.order.Remove(controller.emptySpace);
                            controller.order.Insert(controller.order.IndexOf(card_g) + ext, controller.emptySpace);
                        }

                    }

                }
            }
        }
    }



    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "DCTrigger" && controller.dragging==this.gameObject)
        {
            controller.order.Remove(controller.emptySpace);
        }
    }

}