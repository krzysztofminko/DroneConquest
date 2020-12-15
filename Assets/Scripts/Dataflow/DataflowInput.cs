using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Do not serialize, causing linking problems
public class DataflowInput
{

	public string name;

	public Dataflow.IOType type;

	public float valueFloat;
	public bool valueBoolean;
	public Target valueTarget;
	public List<Target> valueTargetsList = new List<Target>();
	public List<Vector2> valueCoordinatesList = new List<Vector2>();

	public bool noDefaultValue;

	public string unlinkedValue;

	public bool ready;

	public DataflowOutput link;
	public DataflowNode node;

	public Transform transform;
	public UINodeLink uiLink;

	public string ValueToString()	//Only for inputs with default values
	{
		switch (type)
		{
			case Dataflow.IOType.Number:	return valueFloat.ToString();
			case Dataflow.IOType.Boolean:	return valueBoolean.ToString();
			default: return null;
		}
	}
	
}
