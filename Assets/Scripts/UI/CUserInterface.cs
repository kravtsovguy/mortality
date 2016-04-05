//Written By Amr Zewail
//Copyright 2015-2016
//Kalponic Games
//-----------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using CMethods;
using CUIEngine;
using UnityEngine.UI;

namespace CUIEngine
{
    public class Button
    {
        public string name;
        public Sprite normal;
        public Sprite clicked;
        public GameObject gameObject;
        public bool isClicked;
        public bool Switch = false;
    }

    public class SwitchButton : Button
    {
        public Sprite switchNormal=null;
        public Sprite switchClicked=null;
        public bool on = true;
        public void Switch()
        {
            on = !on;
        }
    }

    public class HorizontalSlider
    {
        public string name;
        public Sprite slider_sprite;
        public Sprite valuer_sprite;
        public GameObject slider;
        public GameObject valuer;
        public Vector2 dimensions;
        public Vector2 left;
        public Vector2 right;
        public float value;
        public bool drag = false;
    }
    public class HorizontalSlider3D
    {
        public string name;
        public Sprite slider_sprite;
        public Sprite valuer_sprite;
        public GameObject slider;
        public GameObject valuer;
        public Vector2 dimensions;
        public Vector2 left;
        public Vector2 right;
        public float value;
        public bool drag = false;
    }

    public class Background
    {
        public string name;
        public Sprite sprite;
        public GameObject gameObject;
        public GameObject buttonsparent;
    }

    public class Label
    {
        public string name;
        public string text;
        public GameObject gameObject;
        public GameObject canvas;
        public Vector3 position;
        public Transform target;
        public bool keepInPosition = false;
    }

    public class CUserInterface
    {

        public List<Button> buttons = new List<Button>();
        public List<SwitchButton> switchButtons = new List<SwitchButton>();
        public List<Background> backgrounds = new List<Background>();
        public List<HorizontalSlider> horizontalSliders = new List<HorizontalSlider>();
        public List<Label> labels = new List<Label>();
        public bool run = true;

        GameObject UI= null;

        public void Clear()
        {
            buttons.Clear();
            backgrounds.Clear();
            horizontalSliders.Clear();
        }

        void RunButtons(RaycastHit2D hit)
        {
            foreach (Button b in buttons)
            {
                b.isClicked = false;
                if (hit && hit.transform.gameObject == b.gameObject && CInput.MouseClick(0))
                {
                    b.isClicked = true;
                }
                if (hit && hit.transform.gameObject == b.gameObject && Input.GetMouseButton(0) && b.clicked)
                {
                    b.gameObject.GetComponent<SpriteRenderer>().sprite = b.clicked;
                }
                else if (b.gameObject)
                {
                    b.gameObject.GetComponent<SpriteRenderer>().sprite = b.normal;
                }
            }

            foreach (SwitchButton b in switchButtons)
            {
                b.isClicked = false;
                if (hit && hit.transform.gameObject == b.gameObject && CInput.MouseClick(0))
                {
                    b.isClicked = true;
                    b.on = !b.on;
                }
                if (hit && hit.transform.gameObject == b.gameObject && Input.GetMouseButton(0) && b.clicked)
                {
                    if (b.on)
                    {
                        b.gameObject.GetComponent<SpriteRenderer>().sprite = b.clicked;
                    }
                    else
                    {
                        b.gameObject.GetComponent<SpriteRenderer>().sprite = b.switchClicked;
                    }
                }
                else if (b.gameObject)
                {
                    if (b.on)
                    {
                        b.gameObject.GetComponent<SpriteRenderer>().sprite = b.normal;
                    }
                    else
                    {
                        b.gameObject.GetComponent<SpriteRenderer>().sprite = b.switchNormal;
                    }
                }
            }
            
        }

        public void Run()
        {
            /*RaycastHit h;
            bool hit = Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward,out h);*/
            if (run)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                RunButtons(hit);
                foreach (HorizontalSlider hs in horizontalSliders)
                {
                    hs.valuer.transform.localPosition = new Vector3(Mathf.Clamp(hs.valuer.transform.localPosition.x, hs.left.x, hs.right.x), 0, -0.01f);
                    if (hit && /*h*/hit.transform.gameObject == hs.valuer && Input.GetMouseButton(0))
                    {
                        hs.drag = true;
                    }
                    if (Input.GetMouseButtonUp(0))
                    {
                        hs.drag = false;
                    }
                    if (hs.drag)
                    {
                        hs.value = CalculateValue(hs.left, hs.right, hs.valuer.transform.localPosition);
                        Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        position.y = hs.valuer.transform.position.y;
                        Vector3 tmp = hs.valuer.transform.localPosition;
                        //tmp.x = position.x-hs.slider.transform.position.x;
                        tmp.x += CInput.MouseSpeed().x * Time.deltaTime;
                        valuerdelta = Input.mousePosition - valuerlastpos;
                        valuerlastpos = Input.mousePosition;
                        //tmp.y = position.y-hs.slider.transform.position.y;
                        hs.valuer.transform.localPosition = tmp;
                        //hs.valuer.transform.localPosition = new Vector3(hs.valuer.transform.localPosition.x, hs.valuer.transform.localPosition.y, -0.1f);
                        Debug.Log(hs.value);
                        hs.valuer.transform.localPosition = new Vector3(Mathf.Clamp(hs.valuer.transform.localPosition.x, hs.left.x, hs.right.x), 0, -0.01f);
                    }
                }
                foreach (Label l in labels)
                {
                    if (l.keepInPosition)
                    {
                        if (l.target)
                        {
                            l.gameObject.GetComponent<RectTransform>().anchoredPosition = CTransform.WorldToScreenPosition(l.target.position, l.canvas.name);
                        }
                        else
                        {
                            l.gameObject.GetComponent<RectTransform>().anchoredPosition = CTransform.WorldToScreenPosition(l.position, l.canvas.name);

                        }
                    }
                }
            }
            else
            {
                foreach (Button b in buttons)
                {
                    if (b.isClicked)
                    {
                        b.isClicked = false;
                    }
                }
            }
        }
        private float CalculateValue(Vector2 left,Vector2 right,Vector2 valuer)
        {
            Vector2 max = right - left;
            Vector2 value = valuer - left;
            return (CSystem.ToPositive(value.x / max.x));
        }
        Vector3 valuerlastpos = Vector2.zero;
        Vector3 valuerdelta = Vector2.zero;
        public void Start()
        {
            UI.transform.gameObject.SetActive(true);
            foreach (Label l in labels)
            {
                l.gameObject.SetActive(true);
            }
        }

        public void Close()
        {
            
            UI.transform.gameObject.SetActive(false);
            foreach (Label l in labels)
            {
                l.gameObject.SetActive(false);
            }
            
        }
        public bool Active()
        {
            return UI.gameObject.activeSelf;
        }

        public bool ButtonClicked(string button_name)
        {
            if (GetButton(button_name).isClicked)
            {
                return true;
            }
            return false;
        }
        public bool SwitchButtonClicked(string button_name)
        {
            if (GetSwitchButton(button_name).isClicked)
            {
                return true;
            }
            return false;
        }

       
        public Button SetButton(string button_name, string sprite_path, Rect rect, float depth,bool close_able,string clicked_sprite_path = "")
        {
            CreateUI();
            Button b = new Button();
            GameObject g = new GameObject(button_name, typeof(SpriteRenderer));
            b.normal = CSprite.LoadSprite(sprite_path);
            if (clicked_sprite_path != "")
            {
                b.clicked = CSprite.LoadSprite(clicked_sprite_path);
            }

            b.name = button_name;
            if (close_able)
            {
                g.transform.parent = UI.transform;
            }else
            {
                g.transform.parent = Camera.main.transform;
            }
            g.transform.localScale = new Vector3(rect.width, rect.height,1);
            g.transform.localPosition = new Vector3(rect.x, rect.y, depth);
            g.GetComponent<SpriteRenderer>().sprite = b.normal;
            BoxCollider2D bc = g.AddComponent<BoxCollider2D>();
            bc.isTrigger = true;
            
            b.gameObject = g;
            buttons.Add(b);
            return b;
        }

        public Button SetButton(string button_name, Sprite normal, GameObject g,Sprite clicked = null)
        {
            Button b = new Button();
            b.normal = normal;
            if (clicked != null)
            {
                b.clicked = clicked;
            }

            b.name = button_name;

            BoxCollider2D bc = g.AddComponent<BoxCollider2D>();
            bc.isTrigger = true;

            b.gameObject = g;
            buttons.Add(b);
            return b;
        }
        public SwitchButton SetSwitchButton(string button_name, Sprite normal,Sprite switchNormal, GameObject g, Sprite clicked = null,Sprite switchClicked=null)
        {
            SwitchButton sb = new SwitchButton();
            sb.name = button_name;
            sb.normal = normal;
            sb.switchNormal = switchNormal;
            sb.switchNormal = switchNormal;
            sb.gameObject = g;
            sb.clicked = clicked;
            sb.switchClicked = switchClicked;
            BoxCollider2D bc = g.AddComponent<BoxCollider2D>();
            bc.isTrigger = true;
            switchButtons.Add(sb);
            return sb;
        }

        public void SetHorizontalSlider(string name , string slider_path , string valuer_path , Rect rect , float depth , bool close_able = true)
        {
            CreateUI();
            HorizontalSlider hs = new HorizontalSlider();
            GameObject slider = new GameObject("Slider", typeof(SpriteRenderer));
            GameObject valuer = new GameObject("Valuer", typeof(SpriteRenderer));
            valuer.transform.parent = slider.transform;
            if (close_able)
            {
                slider.transform.parent = UI.transform;
            }
            else
            {
                slider.transform.parent = Camera.main.transform;
            }
            hs.slider_sprite = CSprite.LoadSprite(slider_path);
            hs.valuer_sprite = CSprite.LoadSprite(valuer_path);
            slider.GetComponent<SpriteRenderer>().sprite = hs.slider_sprite;
            valuer.GetComponent<SpriteRenderer>().sprite = hs.valuer_sprite;
            valuer.AddComponent<BoxCollider>();
            slider.transform.localPosition = new Vector3(rect.x, rect.y, depth);
            valuer.transform.localPosition = new Vector3(0, 0, -0.1f);
            slider.transform.localScale = new Vector3(rect.width, rect.height, 1);
            hs.dimensions = CTransform.GetDimensions(slider);
            hs.left = new Vector2(valuer.transform.localPosition.x - (CTransform.GetDimensions(slider).x / 2), valuer.transform.localPosition.y);
            hs.right = new Vector2(valuer.transform.localPosition.x + (CTransform.GetDimensions(slider).x / 2), valuer.transform.localPosition.y);

            hs.slider = slider;
            hs.valuer = valuer;
            horizontalSliders.Add(hs);
        }

        public Label SetLabelWorldPosition(string name, string text, Rect rect, float depth, Font font,int fontSize, string canv = "Canvas")
        {
            Label l = new Label();
            l.name = name;
            l.canvas = GameObject.Find(canv);
            l.gameObject = new GameObject(name);
            l.gameObject.transform.SetParent(l.canvas.transform);
            l.gameObject.transform.localPosition = new Vector3(rect.x, rect.y, depth);
            l.gameObject.transform.position = new Vector3(rect.x, rect.y, l.gameObject.transform.position.z);
            l.gameObject.transform.localScale = new Vector2(rect.width, rect.height);
            l.position = new Vector3(rect.x, rect.y, l.gameObject.transform.position.z);
            Text t = l.gameObject.AddComponent<Text>();
            t.text = text;
            t.alignment = TextAnchor.MiddleCenter;
            t.horizontalOverflow = HorizontalWrapMode.Overflow;
            t.verticalOverflow = VerticalWrapMode.Overflow;
            t.resizeTextForBestFit = true;
            t.font = font;
            t.fontSize = fontSize;
            labels.Add(l);
            return l;
        }
        public Label SetLabelWorldPosition(string name, string text, Transform target,Vector2 scale, float depth, Font font,int fontSize, string canv = "Canvas")
        {
            Label l = new Label();
            l.name = name;
            l.canvas = GameObject.Find(canv);
            l.target = target;
            l.gameObject = new GameObject(name);
            l.gameObject.transform.SetParent(l.canvas.transform);
            l.gameObject.transform.localPosition = new Vector3(target.position.x, target.position.y, depth);
            l.gameObject.transform.position = new Vector3(target.position.x, target.position.y, l.gameObject.transform.position.z);
            l.gameObject.transform.localScale = scale;
            l.position = new Vector3(target.position.x, target.position.y, l.gameObject.transform.position.z);
            Text t = l.gameObject.AddComponent<Text>();
            t.text = text;
            t.alignment = TextAnchor.MiddleCenter;
            t.horizontalOverflow = HorizontalWrapMode.Overflow;
            t.verticalOverflow = VerticalWrapMode.Overflow;
            t.resizeTextForBestFit = true;
            t.font = font;
            t.fontSize = fontSize;
            labels.Add(l);
            return l;
        }
        public Label SetLabel(string name, string text, Rect rect, float depth, Font font,int fontSize, string canv = "Canvas")
        {
            Label l = new Label();
            l.name = name;
            l.canvas = GameObject.Find(canv);
            l.gameObject = new GameObject(name);
            Text t = l.gameObject.AddComponent<Text>();
            l.gameObject.transform.SetParent(l.canvas.transform);
            l.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(rect.x, rect.y, l.gameObject.transform.position.z);
            //l.gameObject.transform.position = new Vector3(rect.x, rect.y, l.gameObject.transform.position.z);
            l.gameObject.transform.localScale = new Vector2(rect.width, rect.height);
            l.position = new Vector3(rect.x, rect.y, l.gameObject.transform.position.z);

            t.text = text;
            t.alignment = TextAnchor.MiddleCenter;
            t.horizontalOverflow = HorizontalWrapMode.Overflow;
            t.verticalOverflow = VerticalWrapMode.Overflow;
            t.resizeTextForBestFit = true;
            t.font = font;
            t.fontSize = fontSize;
            labels.Add(l);
            return l;
        }

       
        private void CreateUI()
        {
            if (!UI)
            {
                UI = new GameObject("UI");
                UI.transform.parent = Camera.main.transform;
                UI.transform.localScale = new Vector3(1, 1, 1);
                UI.transform.localPosition = Vector3.zero;
            }
            
        }

        public Button GetButton(string button_name)
        {

            foreach (Button b in buttons)
            {
                if (b.name == button_name)
                {
                    return b;
                }
            }
            return null;
        }

        public SwitchButton GetSwitchButton(string name)
        {
            foreach (SwitchButton b in switchButtons)
            {
                if (b.name == name)
                {
                    return b;
                }
            }
            return null;
        }

        public Label GetLabel(string label_name)
        {
            foreach (Label l in labels)
            {
                if (l.name == label_name)
                {
                    return l;
                }
            }
            return null;
        }

        public void RemoveButton(string button_name)
        {
            Button b = GetButton(button_name);
            GameObject.Destroy(b.gameObject);
            b.gameObject = null;
            buttons.Remove(b);
        }

        public Background GetBackground(string back_name)
        {
            foreach (Background b in backgrounds)
            {
                if (b.name == back_name)
                {
                    return b;
                }
            }
            return null;
        }

        public void Switch(string button_name)
        {
            if (GetButton(button_name).Switch)
            {
                GetButton(button_name).Switch = false;
            }else
            {
                GetButton(button_name).Switch = true;
            }
        }

        public void ChangeSprite(string button_name,string sprite_path)
        {
            GameObject g = GetButton(button_name).gameObject;
            GetButton(button_name).normal = CSprite.LoadSprite(sprite_path);
            g.GetComponent<SpriteRenderer>().sprite = GetButton(button_name).normal;
        }

        public Background SetBackground(string back_name,string back_sprite_path,Rect rect,float depth,bool close_able = true)
        {
            CreateUI();
            Background b = new Background();
            b.sprite = CSprite.LoadSprite(back_sprite_path);
            b.name = back_name;
            GameObject g = new GameObject(back_name,typeof(SpriteRenderer));
            if (close_able)
            {
                g.transform.parent = UI.transform;
            }
            else
            {
                g.transform.parent = Camera.main.transform;
            }
            g.GetComponent<SpriteRenderer>().sprite = b.sprite;
            b.gameObject = g;

            g.transform.localScale = new Vector3(rect.width, rect.height,1);
            g.transform.localPosition = new Vector3(rect.x, rect.y, depth);
            backgrounds.Add(b);
            return b;
        }

    }

    public class CMUserInterface
    {

        public List<Button> buttons = new List<Button>();
        public List<Background> backgrounds = new List<Background>();
        public List<HorizontalSlider> horizontalSliders = new List<HorizontalSlider>();
        public List<Label> labels = new List<Label>();
        public bool run = true;
        private int index;

        GameObject UI = null;

        public CMUserInterface(int touch_index)
        {
            index = touch_index;
        }

        public void Clear()
        {
            buttons.Clear();
            backgrounds.Clear();
            horizontalSliders.Clear();
        }

        public void Run()
        {
            /*RaycastHit h;
            bool hit = Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward,out h);*/
            if (run && Input.touchCount > index)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(index).position), Vector2.zero);
                foreach (Button b in buttons)
                {
                    if (hit && hit.transform.gameObject == b.gameObject && CInput.TouchClick(index))
                    {
                        b.isClicked = true;
                    }
                    else
                    {
                        b.isClicked = false;
                    }
                    if (hit && hit.transform.gameObject == b.gameObject && (Input.GetTouch(index).phase == TouchPhase.Stationary || Input.GetTouch(index).phase == TouchPhase.Moved) && b.clicked)
                    {
                        b.gameObject.GetComponent<SpriteRenderer>().sprite = b.clicked;
                    }
                    else if (b.gameObject)
                    {
                        b.gameObject.GetComponent<SpriteRenderer>().sprite = b.normal;
                    }
                }
                foreach (HorizontalSlider hs in horizontalSliders)
                {
                    hs.valuer.transform.localPosition = new Vector3(Mathf.Clamp(hs.valuer.transform.localPosition.x, hs.left.x, hs.right.x), 0, -0.01f);
                    if (hit && /*h*/hit.transform.gameObject == hs.valuer && Input.GetMouseButton(0))
                    {
                        hs.drag = true;
                    }
                    if (Input.GetMouseButtonUp(0))
                    {
                        hs.drag = false;
                    }
                    if (hs.drag)
                    {
                        hs.value = CalculateValue(hs.left, hs.right, hs.valuer.transform.localPosition);
                        Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        position.y = hs.valuer.transform.position.y;
                        Vector3 tmp = hs.valuer.transform.localPosition;
                        //tmp.x = position.x-hs.slider.transform.position.x;
                        tmp.x += CInput.MouseSpeed().x * Time.deltaTime;
                        valuerdelta = Input.mousePosition - valuerlastpos;
                        valuerlastpos = Input.mousePosition;
                        //tmp.y = position.y-hs.slider.transform.position.y;
                        hs.valuer.transform.localPosition = tmp;
                        //hs.valuer.transform.localPosition = new Vector3(hs.valuer.transform.localPosition.x, hs.valuer.transform.localPosition.y, -0.1f);
                        Debug.Log(hs.value);
                        hs.valuer.transform.localPosition = new Vector3(Mathf.Clamp(hs.valuer.transform.localPosition.x, hs.left.x, hs.right.x), 0, -0.01f);
                    }
                }
            }
            else
            {
                foreach (Button b in buttons)
                {
                    if (b.isClicked)
                    {
                        b.isClicked = false;
                    }
                }
            }
        }
        private float CalculateValue(Vector2 left, Vector2 right, Vector2 valuer)
        {
            Vector2 max = right - left;
            Vector2 value = valuer - left;
            return (CSystem.ToPositive(value.x / max.x));
        }
        Vector3 valuerlastpos = Vector2.zero;
        Vector3 valuerdelta = Vector2.zero;
        public void Start()
        {
            UI.transform.gameObject.SetActive(true);
            foreach (Label l in labels)
            {
                l.gameObject.SetActive(true);
            }
        }

        public void Close()
        {

            UI.transform.gameObject.SetActive(false);
            foreach (Label l in labels)
            {
                l.gameObject.SetActive(false);
            }

        }
        public bool Active()
        {
            return UI.gameObject.activeSelf;
        }

        public bool ButtonClicked(string button_name)
        {
            if (GetButton(button_name).isClicked)
            {
                return true;
            }
            return false;
        }

        public Button SetButton(string button_name, string sprite_path, Rect rect, float depth, bool close_able, string clicked_sprite_path = "")
        {
            CreateUI();
            Button b = new Button();
            GameObject g = new GameObject(button_name, typeof(SpriteRenderer));
            b.normal = CSprite.LoadSprite(sprite_path);
            if (clicked_sprite_path != "")
            {
                b.clicked = CSprite.LoadSprite(clicked_sprite_path);
            }

            b.name = button_name;
            if (close_able)
            {
                g.transform.parent = UI.transform;
            }
            else
            {
                g.transform.parent = Camera.main.transform;
            }
            g.transform.localScale = new Vector3(rect.width, rect.height, 1);
            g.transform.localPosition = new Vector3(rect.x, rect.y, depth);
            g.GetComponent<SpriteRenderer>().sprite = b.normal;
            BoxCollider2D bc = g.AddComponent<BoxCollider2D>();
            bc.isTrigger = true;

            b.gameObject = g;
            buttons.Add(b);
            return b;
        }
        public Button SetButton(string button_name, Sprite normal, Rect rect, float depth, bool close_able,Sprite clicked = null)
        {
            Button b = new Button();
            GameObject g = new GameObject(button_name, typeof(SpriteRenderer));
            b.normal = normal;
            if (clicked != null)
            {
                b.clicked = clicked;
            }

            b.name = button_name;

            g.transform.localScale = new Vector3(rect.width, rect.height, 1);
            g.transform.localPosition = new Vector3(rect.x, rect.y, depth);
            g.GetComponent<SpriteRenderer>().sprite = b.normal;
            BoxCollider2D bc = g.AddComponent<BoxCollider2D>();
            bc.isTrigger = true;

            b.gameObject = g;
            buttons.Add(b);
            return b;
        }

        public void SetHorizontalSlider(string name, string slider_path, string valuer_path, Rect rect, float depth, bool close_able = true)
        {
            CreateUI();
            HorizontalSlider hs = new HorizontalSlider();
            GameObject slider = new GameObject("Slider", typeof(SpriteRenderer));
            GameObject valuer = new GameObject("Valuer", typeof(SpriteRenderer));
            valuer.transform.parent = slider.transform;
            if (close_able)
            {
                slider.transform.parent = UI.transform;
            }
            else
            {
                slider.transform.parent = Camera.main.transform;
            }
            hs.slider_sprite = CSprite.LoadSprite(slider_path);
            hs.valuer_sprite = CSprite.LoadSprite(valuer_path);
            slider.GetComponent<SpriteRenderer>().sprite = hs.slider_sprite;
            valuer.GetComponent<SpriteRenderer>().sprite = hs.valuer_sprite;
            valuer.AddComponent<BoxCollider>();
            slider.transform.localPosition = new Vector3(rect.x, rect.y, depth);
            valuer.transform.localPosition = new Vector3(0, 0, -0.1f);
            slider.transform.localScale = new Vector3(rect.width, rect.height, 1);
            hs.dimensions = CTransform.GetDimensions(slider);
            hs.left = new Vector2(valuer.transform.localPosition.x - (CTransform.GetDimensions(slider).x / 2), valuer.transform.localPosition.y);
            hs.right = new Vector2(valuer.transform.localPosition.x + (CTransform.GetDimensions(slider).x / 2), valuer.transform.localPosition.y);

            hs.slider = slider;
            hs.valuer = valuer;
            horizontalSliders.Add(hs);
        }

        public Label SetLabel(string name, string text, Rect rect, float depth, Font font)
        {
            Label l = new Label();
            l.canvas = GameObject.Find("Canvas");
            l.gameObject = new GameObject(name, typeof(Text));
            l.gameObject.transform.SetParent(l.canvas.transform);
            l.gameObject.transform.localPosition = new Vector3(rect.x, rect.y, depth);
            l.gameObject.transform.localScale = new Vector2(rect.width, rect.height);
            Text t = l.gameObject.GetComponent<Text>();
            t.text = text;
            t.alignment = TextAnchor.MiddleCenter;
            t.horizontalOverflow = HorizontalWrapMode.Overflow;
            t.verticalOverflow = VerticalWrapMode.Overflow;
            t.resizeTextForBestFit = true;
            t.font = font;
            labels.Add(l);
            return l;
        }


        private void CreateUI()
        {
            if (!UI)
            {
                UI = new GameObject("UI");
                UI.transform.parent = Camera.main.transform;
                UI.transform.localScale = new Vector3(1, 1, 1);
                UI.transform.localPosition = Vector3.zero;
            }

        }

        public Button GetButton(string button_name)
        {

            foreach (Button b in buttons)
            {
                if (b.name == button_name)
                {
                    return b;
                }
            }
            return null;
        }

        public Label GetLabel(string label_name)
        {
            foreach (Label l in labels)
            {
                if (l.name == label_name)
                {
                    return l;
                }
            }
            return null;
        }

        public void RemoveButton(string button_name)
        {
            Button b = GetButton(button_name);
            GameObject.Destroy(b.gameObject);
            b.gameObject = null;
            buttons.Remove(b);
        }

        public Background GetBackground(string back_name)
        {
            foreach (Background b in backgrounds)
            {
                if (b.name == back_name)
                {
                    return b;
                }
            }
            return null;
        }

        public void Switch(string button_name)
        {
            if (GetButton(button_name).Switch)
            {
                GetButton(button_name).Switch = false;
            }
            else
            {
                GetButton(button_name).Switch = true;
            }
        }

        public void ChangeSprite(string button_name, string sprite_path)
        {
            GameObject g = GetButton(button_name).gameObject;
            GetButton(button_name).normal = CSprite.LoadSprite(sprite_path);
            g.GetComponent<SpriteRenderer>().sprite = GetButton(button_name).normal;
        }

        public Background SetBackground(string back_name, string back_sprite_path, Rect rect, float depth, bool close_able = true)
        {
            CreateUI();
            Background b = new Background();
            b.sprite = CSprite.LoadSprite(back_sprite_path);
            b.name = back_name;
            GameObject g = new GameObject(back_name, typeof(SpriteRenderer));
            if (close_able)
            {
                g.transform.parent = UI.transform;
            }
            else
            {
                g.transform.parent = Camera.main.transform;
            }
            g.GetComponent<SpriteRenderer>().sprite = b.sprite;
            b.gameObject = g;

            g.transform.localScale = new Vector3(rect.width, rect.height, 1);
            g.transform.localPosition = new Vector3(rect.x, rect.y, depth);
            backgrounds.Add(b);
            return b;
        }

    }
}