using UnityEngine;
using System.Collections;

public class DisableIfAndroid : MonoBehaviour {

	void Awake () {
	
	#if UNITY_ANDROID
		gameObject.SetActive(false);
	#endif

	}
}
