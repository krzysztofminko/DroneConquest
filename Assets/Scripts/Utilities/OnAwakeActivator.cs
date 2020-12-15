using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAwakeActivator : MonoBehaviour {

	public GameObject[] activateOnAwake;

	void Awake () {
		foreach (var a in activateOnAwake)
			a.SetActive(true);
	}
}
