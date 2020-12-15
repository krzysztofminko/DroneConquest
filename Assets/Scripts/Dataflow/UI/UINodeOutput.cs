using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UINodeOutput : MonoBehaviour, IPointerClickHandler
{
	public DataflowOutput output;

	public void OnPointerClick(PointerEventData eventData)
	{
		if (UIDataflowEditor.instance.newLinkOutput == null && eventData.button == PointerEventData.InputButton.Left)
		{
			UIDataflowEditor.instance.newLinkOutput = output;
			UIDataflowEditor.instance.StartNewLink(output);
		}
	}
}
