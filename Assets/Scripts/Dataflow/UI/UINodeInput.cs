using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UINodeInput : MonoBehaviour, IPointerClickHandler {

	public DataflowInput input;

	public void OnPointerClick(PointerEventData eventData)
	{
		if (UIDataflowEditor.instance.newLinkOutput != null)
		{
			if (eventData.button == PointerEventData.InputButton.Left)
				UIDataflowEditor.instance.EndNewLink(input);
		}
		else if(eventData.button == PointerEventData.InputButton.Right)
		{
			UIDataflowEditor.instance.DeleteLinkFromInput(input);
		}
	}

	public void EditValue()
	{
		if (input != null)
		{
			InputField ipf = transform.GetChild(0).GetComponent<InputField>();

			if (input.type == Dataflow.IOType.Boolean)
			{
				if (ipf.text.ToLower().StartsWith("t"))
					ipf.text = "True";
				else
					ipf.text = "False";
			}

			switch (input.type)
			{
				case Dataflow.IOType.Number:	input.valueFloat = float.Parse(ipf.text); break;
				case Dataflow.IOType.Boolean:	input.valueBoolean = bool.Parse(ipf.text); break;
			}
		}
	}
}
