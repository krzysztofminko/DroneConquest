using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	public Transform moveToFront;

    Vector2 localMousePos;
    Vector3 globalMousePos;

	RectTransform rt;

	public void OnBeginDrag(PointerEventData eventData)
    {        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out localMousePos);
		if (moveToFront)
			moveToFront.SetAsLastSibling();
		rt = transform.parent.GetComponent<RectTransform>();
	}

    public void OnDrag(PointerEventData eventData)
    {
        SetDraggedPosition(eventData);
    }

	public void OnEndDrag(PointerEventData eventData)
	{
		//Update dragged position - for DataflwoNode only
		DataflowNode node = UIDataflowEditor.instance.dataflow.nodes.Find(n => n.transform == rt);
		if (node != null)
		{
			node.posx = (int)rt.anchoredPosition.x;
			node.posy = (int)rt.anchoredPosition.y;
		}
		rt = null;
	}

	void SetDraggedPosition(PointerEventData eventData)
    {

        if (RectTransformUtility.RectangleContainsScreenPoint(rt.parent.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera)) 
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt.parent.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out globalMousePos))
        {
            rt.position = globalMousePos - new Vector3(localMousePos.x, localMousePos.y, 0);
            rt.rotation = transform.parent.GetComponent<RectTransform>().rotation;
        }
	}
}
