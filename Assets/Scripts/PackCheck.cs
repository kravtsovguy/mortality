using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using OnePF;

public class PackCheck : MonoBehaviour {

    public Sprite lockedSprite;
    private Sprite unlockedSprite;
    ContentPack cp;

    [HideInInspector]
    public License license;
    void Start()
    {
        unlockedSprite = transform.FindChild("Background").GetComponent<Image>().sprite;

        TextAsset t = Resources.Load(this.name) as TextAsset;

        using (StreamWriter s = new StreamWriter(Application.persistentDataPath + "/" + this.name + ".xml"))
        {
            s.Write(t.text);
        }

        cp = CSave.SaveEngine.LoadFromXML<ContentPack>(Application.persistentDataPath + "/" + this.name + ".xml");
        cp.license = license;
        if (cp.license == License.Buy)
        {
            LockPack();
        }

        CheckPack(GetComponent<Toggle>().enabled);
    }

    public void Click()
    {
        if (GetComponent<Toggle>().isOn)
        {
            Game.contentPacks.Add(cp);
        }
        else
        {
            if (Game.contentPacks.Contains(cp))
            {
                Game.contentPacks.Remove(cp);
            }
        }
    }

    public void CheckPack(bool on)
    {
        GetComponent<Toggle>().isOn = on;
    }

    private void LockPack()
    {
		GetComponent<Toggle>().enabled = false;
		transform.FindChild("Background").GetComponent<Button>().enabled = true;
		transform.FindChild("Background").GetComponent<Image>().sprite = lockedSprite;
		transform.FindChild("Label").GetComponent<Text>().color = Color.gray;

    }

    public void UnlockPack()
    {
		IAPManager.shared.queryInventorySucceeded = (Inventory inv) => {
			//DoScript.LoadScene(Application.loadedLevelName);
		};
		IAPManager.shared.purchaseSucceeded = (string sku) => {
			Debug.Log("purchaseSucceeded: "+cp.name+" - "+sku);
			//if(cp.name != sku) return;

			GetComponent<Toggle>().enabled = true;
			transform.FindChild("Background").GetComponent<Button>().enabled = false;
			transform.FindChild("Background").GetComponent<Image>().sprite = unlockedSprite;
			transform.FindChild("Label").GetComponent<Text>().color = Color.black;
			Game.unlockedContentPacks.Add(cp.name);
			cp.license = License.Unlocked;

		};
		IAPManager.shared.Buy (cp.name);
    }
}
