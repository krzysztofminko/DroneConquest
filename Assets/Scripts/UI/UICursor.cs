using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICursor : MonoBehaviour {
	private void OnGUI()
	{
		GetComponent<RectTransform>().anchoredPosition = Input.mousePosition - new Vector3(transform.parent.GetComponent<RectTransform>().sizeDelta.x * transform.parent.GetComponent<RectTransform>().pivot.x, transform.parent.GetComponent<RectTransform>().sizeDelta.y * transform.parent.GetComponent<RectTransform>().pivot.y, 0);
	}
}
