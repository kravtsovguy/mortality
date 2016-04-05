using UnityEngine;
using System.Collections;

public class DontDestroy : MonoBehaviour {

	static DontDestroy shared;
	void Awake () {
		if (shared) {
			DestroyImmediate (gameObject);
			return;
		}

		shared = this;
		DontDestroyOnLoad (gameObject);
	}
}
