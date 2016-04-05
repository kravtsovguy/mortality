using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using CMethods;

namespace TouchInput
{
    //this is the class that contains all mobile input methods..
    public class TInput
    {
        public bool UpBorder()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.position.y > (Screen.height - Screen.height / 5))
                {
                    return true;
                }
            }
            return false;
        }

        public bool DownBorder()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.position.y < (Screen.height / 5))
                {
                    return true;
                }
            }
            return false;
        }

    }

}
