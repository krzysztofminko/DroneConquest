using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using UnityEngine.EventSystems;

public class UINodeLink : MonoBehaviour{
    
    public RectTransform outputAnchor;
    public RectTransform inputAnchor;

    private UILineRenderer line;
	
	void Awake()
    {
        line = GetComponent<UILineRenderer>();
        line.Points = new Vector2[4];
    }    

    void OnGUI()
    {
		//TODO: dont call GetComponent every update
		Vector2 pos1 = GetComponent<RectTransform>().GetParentCanvas().GetComponent<RectTransform>().InverseTransformPoint(outputAnchor.TransformPoint(GetComponent<RectTransform>().pivot));
        Vector2 pos2 = GetComponent<RectTransform>().GetParentCanvas().GetComponent<RectTransform>().InverseTransformPoint(inputAnchor.TransformPoint(GetComponent<RectTransform>().pivot));
        Vector2[] points;
        //if (pos1.x + 40 < pos2.x - 40)
        //{
        points = new Vector2[] {
            pos1,
            new Vector2(pos1.x + 40, pos1.y),
            new Vector2(pos2.x - 40, pos2.y),
            pos2
        };
        /*}
        else
        {
            points = new Vector2[] {
                pos1,
                new Vector2(pos1.x + 40, pos1.y),
                new Vector2(pos1.x + 40, (pos1.y + pos2.y) / 2),
                new Vector2(pos2.x - 40, (pos1.y + pos2.y) / 2),
                new Vector2(pos2.x - 40, pos2.y),
                pos2
            };
        }*/
        line.Points = points;
        line.RelativeSize = false;
        line.drivenExternally = true;
    }
}
