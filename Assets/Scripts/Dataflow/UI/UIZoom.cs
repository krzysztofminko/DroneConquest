using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	bool pointerInside;
	RectTransform rectTransform;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		pointerInside = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		pointerInside = false;
	}

	private void OnGUI()
	{
		if (pointerInside)
		{
			float delta = Input.mouseScrollDelta.y * 0.01f;			
			rectTransform.localScale += new Vector3(delta, delta, 0);
		}
	}
}
