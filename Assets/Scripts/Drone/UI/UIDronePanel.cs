using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIDronePanel : MonoBehaviour {

	public static UIDronePanel instance;

	public Text titleBarText;

	public Drone selectedDrone;

	void Awake () {

		instance = this;
		Hide();
	}

	public void Hide()
	{
		selectedDrone = null;
		GetComponent<RectTransform>().localScale = Vector3.zero;
	}

	public void Show(Drone drone)
	{		
		GetComponent<RectTransform>().localScale = Vector3.one;

		selectedDrone = drone;

		titleBarText.text = drone.name;
	}
}
