//Written By Amr Zewail
//Copyright 2015-2016
//Kalponic Games
//-----------------------
	
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using CMonoBehaviours;
using CMethods;

using System.Security.Cryptography;


namespace CMethods
{
    public static class CSystem
    {
        static float timer;
        static int fps;
        static int fpsResult;

        public static bool ValueBetween(float value, float min, float max)
        {
            if (value > 180) { value = value - 360; }
            if (value <= max && value >= min)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static float ToPositive(float number)
        {
            if(number < 0)
            {
                number *= -1;
            }
            return number;
        }

        public static int FramesPerSecond()
        {
            timer -= Time.deltaTime;
            fps++;
            if(timer <= 0)
            {
                fpsResult = fps;
                fps = 0;
                timer = 1;
            }
            return fpsResult;
        }

        public static float MathTriangle(float a, float b, float c)
        {
            float result = (a * b) / c;
            return result;
        }

		public static Vector2 Multiply(Vector2 v1,Vector2 v2)
		{
			return new Vector2 (v1.x * v2.x, v1.y * v2.y);
		}

        public static string StringBeforeLastSlash(string name)
        {
            string[] splits = name.Split('/');
            string before = "";
            for (int i = 0; i < splits.Length - 1; i++)
            {
                before += splits[i] + "/";
            }
            before = before.Remove(before.Length - 1);
            return before;
        }

        public static string StringAfterLastSlash(string name)
        {
            string[] splits = name.Split('/');
            string after = splits[splits.Length - 1];
            return after;
        }


    }

    public static class CList
    {
        public static void Shuffle<T>(this List<T> list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

    }

    public static class CGameObject
    {
        public static GameObject FullInstantiate(GameObject original, Vector3 position, Quaternion rotation)
        {
            GameObject i = GameObject.Instantiate(original, position, rotation) as GameObject;
            i.transform.parent = original.transform.parent;
            i.name = original.name;
            i.transform.localScale = original.transform.localScale;
            return i;
        }

        public static bool IsTriggering(GameObject gameObject)
        {
            OnTrigger ot = null;
            if (gameObject.GetComponent<OnTrigger>())
            {
                ot = gameObject.GetComponent<OnTrigger>();
            }
            else
            {
                ot = gameObject.AddComponent<OnTrigger>();
            }
            if (ot.start)
            {
                if (!gameObject.GetComponent<PolygonCollider2D>())
                {
                    PolygonCollider2D pc = gameObject.AddComponent<PolygonCollider2D>();
                    pc.isTrigger = true;
                }
                else
                {
                    PolygonCollider2D pc = gameObject.GetComponent<PolygonCollider2D>();
                    pc.isTrigger = true;
                }
                ot.start = false;
            }
            if (ot.isTrigerring)
            {
                return true;
            }else
            {
                return false;
            }
        }
        public static OnTrigger MakeBoxTrigger(GameObject gameObject,Vector2 size)
        {
            OnTrigger ot = null;
            if (gameObject.GetComponent<OnTrigger>())
            {
                ot = gameObject.GetComponent<OnTrigger>();
            }
            else
            {
                ot = gameObject.AddComponent<OnTrigger>();
            }

            if (ot.start)
            {
                BoxCollider2D bc = null;
                if (!gameObject.GetComponent<BoxCollider2D>())
                {
                    bc = gameObject.AddComponent<BoxCollider2D>();
                }
                else
                {
                    bc = gameObject.GetComponent<BoxCollider2D>();
                }
                bc.size = size;
                bc.isTrigger = true;
                ot.start = false;
            }
            return ot;
        }

        public static OnTrigger GetTrigger(GameObject gameObject)
        {
            return gameObject.GetComponent<OnTrigger>();
        }
    }

    public static class CSprite
    {
        public static void ChangeColor(GameObject gameObject, Color color, float alpha = 1)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, alpha);
        }

        public static Color GetColor(GameObject gameObject)
        {
            if (gameObject.GetComponent<SpriteRenderer>()) {
                return gameObject.GetComponent<SpriteRenderer>().color;
            }
            return Color.white;
        }

        public static void ChangeAlpha(GameObject gameObject, float alpha)
        {
            if (gameObject.GetComponent<SpriteRenderer>())
            {
                SpriteRenderer spriteRend = gameObject.GetComponent<SpriteRenderer>();
                gameObject.GetComponent<SpriteRenderer>().color = new Color(spriteRend.color.r, spriteRend.color.g, spriteRend.color.b, alpha);
            }
        }

        public static Sprite LoadSprite(string sprite_path)
        {
            string path = CSystem.StringBeforeLastSlash(sprite_path);
            string name = CSystem.StringAfterLastSlash(sprite_path);
            Sprite[] sprites = Resources.LoadAll<Sprite>(path);
            return GetSprite(name, sprites);
        }

        private static Sprite GetSprite(string sprite_name, Array sprites)
        {
            foreach (Sprite s in sprites)
            {
                if (s.name == sprite_name)
                {
                    return s;
                }
            }
            return null;
        }

        public static Texture2D SpriteToTexture(Sprite sprite)
        {
            var croppedTexture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            var pixels = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                    (int)sprite.textureRect.y,
                                                    (int)sprite.textureRect.width,
                                                    (int)sprite.textureRect.height);
            croppedTexture.SetPixels(pixels);
            croppedTexture.Apply();
            return croppedTexture;
        }

    }

    public static class CAnimation
    {
        public class Animation
        {
            public string sprite_path;
            public float frames = 1;
            public bool loop = true;
            public bool done = false;
            public List<Sprite> sprites = new List<Sprite>();

            public Animation(string S, float F, bool L)
            {
                sprite_path = S;
                frames = F;
                loop = L;
            }
        }

        public static void PlaySpriteSheet(GameObject gameObject, string resources_sprite_path, float frames,bool loop = true)
        {
            Animating a = null;
            Animation anim = new Animation(resources_sprite_path,frames,loop);
            if (!gameObject.GetComponent<Animating>())
            {
                a = gameObject.AddComponent<Animating>();
            }
            else
            {
                a = gameObject.GetComponent<Animating>();
            }
            if (a.sprite_sheet != anim.sprite_path)
            {
                Sprite[] spr = Resources.LoadAll<Sprite>(resources_sprite_path);
                foreach (Sprite s in spr)
                {
                    anim.sprites.Add(s);
                }
                a.i = 0;
                a.anim = anim;
            }
        }

        public static void PlaySpriteSheet(GameObject gameObject, string resources_sprite_path, float frames,int frame_load ,bool loop = true)
        {
            Animating a = null;
            Animation anim = new Animation(resources_sprite_path, frames, loop);
            if (!gameObject.GetComponent<Animating>())
            {
                a = gameObject.AddComponent<Animating>();
            }
            else
            {
                a = gameObject.GetComponent<Animating>();
            }
            if (a.sprite_sheet != anim.sprite_path)
            {
                Sprite[] spr = Resources.LoadAll<Sprite>(resources_sprite_path);
                foreach (Sprite s in spr)
                {
                    anim.sprites.Add(s);
                }
                a.i = 0;
                a.anim = anim;
                a.frame_load = frame_load;
            }
        }

        public static bool SpriteSheetDone(GameObject gameObject,string sprite_path)
        {
            if (GetAnimating(gameObject) != null && GetAnimating(gameObject).sprite_sheet == sprite_path &&GetAnimating(gameObject).anim.done)
            {
                //GameObject.Destroy(GetAnimating(gameObject));
                return true;
            }
            return false;
        }

        public static bool OnFrame(GameObject gameObject, int frame)
        {
            if (gameObject.GetComponent<Animating>())
            {
                Animating a = gameObject.GetComponent<Animating>();
                if (a.i == frame)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public static void PlayAfter(GameObject gameObject, string before_path, string after_path, int frames, bool loop = true)
        {
            if (SpriteSheetDone(gameObject,before_path))
            {
                PlaySpriteSheet(gameObject, after_path, frames, loop);
            }
        }

        public static string GetSpriteSheet(GameObject gameObject)
        {
            if (gameObject.GetComponent<Animating>())
            {
                Animating a = gameObject.GetComponent<Animating>();
                return a.anim.sprite_path;
            }else
            {
                return null;
            }
        }

        public static Animating GetAnimating(GameObject gameObject)
        {
            if (gameObject.GetComponent<Animating>())
            {
                return gameObject.GetComponent<Animating>();
            }
            return null;
        }
    }

    public static class CInput
    {
        static KeyHold keyHold = new KeyHold();
        static MouseCountSpeed mouseCntSpd = new MouseCountSpeed();
        static Mouse mouse = new Mouse();

        public static bool HoldKey(KeyCode keyCode, float seconds, bool continuously = false)
        {

            if (Input.GetKeyDown(keyCode))
            {
                keyHold.seconds = seconds;
            }
            if (Input.GetKey(keyCode) && keyHold.seconds > 0 && keyHold.canDecrease)
            {
                keyHold.seconds -= Time.deltaTime;
            }
            if (Input.GetKeyUp(keyCode))
            {
                keyHold.canDecrease = true;
                keyHold.seconds = 1;
                return false;
            }
            if (keyHold.seconds <= 0)
            {
                if (!continuously)
                {
                    keyHold.canDecrease = false;
                    keyHold.seconds = 1;
                }
                mouse.clickDown = null;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Vector2 MouseSpeed()
        {

            if (Input.GetMouseButtonDown(0))
            {
                mouseCntSpd.lastPos = Input.mousePosition;
            }
            if (Input.GetMouseButton(0))
            {
                mouseCntSpd.delta = (Vector2)Input.mousePosition - mouseCntSpd.lastPos;
                mouseCntSpd.lastPos = Input.mousePosition;
                return mouseCntSpd.delta;
            }
            else
            {
                return Vector2.zero;
            }
        }

        private static bool beginTouchSpeed = false;
        private static Vector2 delta = Vector2.zero;
        private static Vector2 lastPos = Vector2.zero;
        private static float distanceZoomIn;
        private static float distanceZoomOut;
        public static Vector2 swipeLastPos = Vector2.zero;

        public static void ResetTouchSpeed()
        {
            beginTouchSpeed = false;
        }

        public static Vector2 TouchSpeed(int touchIndex = 0)
        {

            if (Input.touchCount > touchIndex &&Input.GetTouch(touchIndex).phase == TouchPhase.Began)
            {
                beginTouchSpeed = true;
                lastPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(touchIndex).position);
            }
            if (Input.touchCount > touchIndex && Input.GetTouch(touchIndex).phase == TouchPhase.Moved && beginTouchSpeed)
            {
                delta = (Vector2)Camera.main.ScreenToWorldPoint(Input.GetTouch(touchIndex).position) - lastPos;
                lastPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(touchIndex).position);
                return delta;
            }
            else
            {
                return Vector2.zero;
            }
        }

        public static bool TouchZoomIn(int index, int index1, float max = 0)
        {
            if (Input.touchCount > 1)
            {
                Touch touch1 = Input.GetTouch(index);
                Touch touch2 = Input.GetTouch(index1);
                if (touch1.phase == TouchPhase.Stationary && touch2.phase == TouchPhase.Stationary)
                {
                    distanceZoomIn = Vector2.Distance(touch1.position, touch2.position);
                    return false;
                }
                else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
                {
                    float nowdistance = Vector2.Distance(touch1.position, touch2.position);
                    if (nowdistance > (distanceZoomIn + max))
                    {
                        distanceZoomIn = Vector2.Distance(touch1.position, touch2.position);
                        return true;
                    }
                    else
                    {
                        distanceZoomIn = Vector2.Distance(touch1.position, touch2.position);
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }
        public static bool TouchZoomOut(int index, int index1, float max = 0)
        {
            if (Input.touchCount > 1)
            {
                Touch touch1 = Input.GetTouch(index);
                Touch touch2 = Input.GetTouch(index1);
                if (touch1.phase == TouchPhase.Stationary && touch2.phase == TouchPhase.Stationary)
                {
                    distanceZoomOut = Vector2.Distance(touch1.position, touch2.position);
                    return false;
                }
                else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
                {
                    float nowdistance = Vector2.Distance(touch1.position, touch2.position);
                    if (nowdistance < (distanceZoomOut - max))
                    {
                        distanceZoomOut = Vector2.Distance(touch1.position, touch2.position);
                        return true;
                    }
                    else
                    {
                        distanceZoomOut = Vector2.Distance(touch1.position, touch2.position);
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        private static bool touching = false;

        public static bool TouchClick(int index = 0)
        {
            if (Input.touchCount > index)
            {
                Touch touch = Input.GetTouch(index);
                if (touch.phase == TouchPhase.Began)
                {
                    touching = true;
                }
                if (touch.phase == TouchPhase.Moved && touching)
                {
                    touching = false;
                }
                if (touch.phase == TouchPhase.Ended && touching)
                {
                    touching = false;
                    return true;
                }
            }
            return false;
        }

        public static bool MouseClick(int button)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero);

            if (Input.GetMouseButtonDown(button) && hit && hit.transform.gameObject)
            {
                mouse.clickDown = hit.transform.gameObject;
            }
            if (mouse.clickDown && (!hit || (hit && hit.transform.gameObject != mouse.clickDown)))
            {
                mouse.clickDown = null;
                return false;
            }
            if (mouse.clickDown && Input.GetMouseButtonUp(button))
            {
                if (hit && hit.transform.gameObject == mouse.clickDown)
                {
                    mouse.clickDown = null;
                    return true;
                }
                else
                {
                    mouse.clickDown = null;
                    return false;
                }
            }
            return false;
        }
    }

    public static class CTransform
    {
        //static ChangingRot changRot = new ChangingRot();
        static Moving moving = new Moving();

        public static Vector2 GetPosition2D(Transform gameObject)
        {
            return new Vector2(gameObject.position.x, gameObject.position.y);
        }

        public static Vector3 Get2DVector3(Transform gameObject, float z)
        {
            return new Vector3(gameObject.position.x, gameObject.position.y, z);
        }

        public static bool IsChangingRot(GameObject gameObject)
        {
            Vector3 rot = gameObject.transform.eulerAngles;

            if (gameObject.transform.eulerAngles != rot)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsMoving2D(GameObject gameObject, float max = 0)
        {
            if (moving.firstTime)
            {
                moving.lastPos = gameObject.transform.position;
                moving.firstTime = false;
            }
            Vector3 position = gameObject.transform.position;

            if (gameObject.transform.position.x >= moving.lastPos.x + max || gameObject.transform.position.x <= moving.lastPos.x - max)
            {
                moving.lastPos = position;
                return true;
            }
            else
            {
                moving.lastPos = position;
                return false;
            }
        }

        public static Vector2 GetDimensions(GameObject gameObject)
        {
            Vector2 tmp;
            tmp.x = gameObject.transform.localScale.x * gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
            tmp.y = gameObject.transform.localScale.y * gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
            return tmp;
        }
        public static Transform PositionOffset(Transform transform,Vector3 offset)
        {
            Transform t = transform;
            t.transform.position += offset;
            return t;
        }

        public static Transform PositionOffset(Transform transform,float x = 0,float y = 0,float z = 0)
        {
            Vector3 offset = new Vector3(x, y, z);
            transform.position += offset;
            return transform;
        }

        public static Vector2 WorldToScreenPosition(Vector3 position, string canvas)
        {
            Vector2 viewportPos = Camera.main.WorldToViewportPoint(position);
            GameObject Canvas = GameObject.Find(canvas);
            Vector2 WorldObject_ScreenPosition = new Vector2(
            ((viewportPos.x * Canvas.GetComponent<RectTransform>().sizeDelta.x) - (Canvas.GetComponent<RectTransform>().sizeDelta.x * 0.5f)),
            ((viewportPos.y * Canvas.GetComponent<RectTransform>().sizeDelta.y) - (Canvas.GetComponent<RectTransform>().sizeDelta.y * 0.5f)));
            return WorldObject_ScreenPosition;
        }
    }

    public static class CPhysics
    {
        public static void Explode(float power, float radius, float timer, Vector2 position, string sprite_path,int frames)
        {
            GameObject go = new GameObject();
            go.AddComponent<SpriteRenderer>();
            Explosion e = go.AddComponent<Explosion>();
            e.power = power;
            e.radius = radius;
            e.timer = timer;
            go.transform.position = position;
            go.transform.localScale *= radius * 2;

            CircleCollider2D cc = go.AddComponent<CircleCollider2D>();
            cc.isTrigger = true;
            CAnimation.PlaySpriteSheet(go, sprite_path, frames,false);
        }
    }

    public class CoolDown
    {
        public float c;
        public float coolDown;
        public bool done = false;

        public CoolDown(float cd)
        {
            c = cd;
            Reset();
        }


        public void Reset()
        {
            coolDown = c;
            Run();
        }

        public void Run(bool autoReset = false)
        {
            if (coolDown > 0)
            {
                coolDown -= Time.deltaTime;
                done = false;
            }
            else
            {
                if (autoReset)
                {
                    Reset();
                }
                else
                {
                    coolDown = 0;
                }
                done = true;
            }
        }

    }
    

    public class KeyHold
    {
        public float seconds = 1;
        public bool canDecrease = true;
    }

    public class MouseCountSpeed
    {
        public Vector2 delta = Vector2.zero;
        public Vector2 lastPos = Vector2.zero;
    }

    public class Mouse
    {
        public GameObject clickDown;
    }

    public class ChangingRot
    {
        public float timer;
        public Vector3 rot;
    }
    public class Moving
    {
        public Vector3 lastPos;
        public bool firstTime = true;
    }
}

namespace CMonoBehaviours
{
    public class Explosion : MonoBehaviour
    {
        [HideInInspector]
        public float power;
        [HideInInspector]
        public float radius;
        [HideInInspector]
        public float timer;

        // Update is called once per frame
        void Update()
        {
            Destroy(this.gameObject, timer);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            Rigidbody2D r = other.GetComponent<Rigidbody2D>();
            Vector2 dir = new Vector2(other.transform.position.x - this.transform.position.x, other.transform.position.y - this.transform.position.y);
            r.AddForce(new Vector2((1 / dir.x) * power, (1 / dir.y) * power), ForceMode2D.Impulse);
        }
    }

    public class OnTrigger : MonoBehaviour
    {
        public bool isTrigerring = false;
        public List<Collider2D> colliders = new List<Collider2D>();
        public List<Collider2D> outColliders = new List<Collider2D>();
        [HideInInspector]
        public bool start = true;

        void OnTriggerStay2D(Collider2D other)
        {
            if (!colliders.Contains(other))
            {
                colliders.Add(other);
                if (outColliders.Contains(other))
                {
                    outColliders.Remove(other);
                }
            }
            isTrigerring = true;
        }
        void OnTriggerExit2D(Collider2D other)
        {
            if (colliders.Contains(other))
            {
                outColliders.Add(other);
                colliders.Remove(other);
            }
            if (colliders.Count == 0)
            {
                isTrigerring = false;
            }
        }
    }

    public class Animating : MonoBehaviour
    {

        public CAnimation.Animation anim;
        float nowframes = 0;
        public int i;
        public string sprite_sheet;
        public int frame_load = 1;

        void Update()
        {
            if (anim != null)
            {
                sprite_sheet = anim.sprite_path;
                if (i < anim.sprites.Count)
                {
                    if (nowframes > 0)
                    {
                        nowframes -= (1 * Time.timeScale);
                    }
                    else
                    {
                        gameObject.GetComponent<SpriteRenderer>().sprite = anim.sprites[i];
                        nowframes = anim.frames;
                        i+=frame_load;

                    }
                }
                else
                {
                    if (anim.loop)
                    {
                        i = 0;
                    }
                    else
                    {
                        anim.done = true;
                    }

                }
            }
        }
    }
}


namespace CSave
{
    public static class SaveEngine
    {

        public static void SaveToXML<T>(string path, T obj)
        {
            using (Stream s = File.Create(path))
            {
                XmlSerializer x = new XmlSerializer(obj.GetType());
                x.Serialize(s, obj);

            }
        }

        public static T LoadFromXML<T>(string path)
        {
            if (File.Exists(path))
            {
                using (Stream s = File.Open(path, FileMode.Open))
                {
                    var obj = (T)Activator.CreateInstance(typeof(T));
                    XmlSerializer x = new XmlSerializer(obj.GetType());
                    return (T)x.Deserialize(s);
                }
            }
            else
            {
                return (T)Activator.CreateInstance(typeof(T));
            }
        }

    }
}