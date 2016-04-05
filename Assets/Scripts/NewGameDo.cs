using UnityEngine;
using System.Collections;
using System.IO;
//
public class NewGameDo : MonoBehaviour {

    public void SelectAllDecks()
    {
        foreach (ContentPack pack in LoadPacks.packs.packs)
        {
            ContentPack cp;
            TextAsset t = Resources.Load(pack.file) as TextAsset;

            using (StreamWriter s = new StreamWriter(Application.persistentDataPath + "/" + pack.file+ ".xml"))
            {
                s.Write(t.text);
            }

            cp = CSave.SaveEngine.LoadFromXML<ContentPack>(Application.persistentDataPath + "/" + pack.file + ".xml");

			print("Selecting - "+pack.name);
			if (pack.license == License.Buy && !IAPManager.shared.HasProduct(pack.name)) continue;

            Game.contentPacks.Add(cp);
        }
    }
}
