using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Do not serialize, causing linking problems
public class DataflowOutput
{
	public string name;
	public Dataflow.IOType type;
	public List<DataflowInput> links = new List<DataflowInput>();
	public DataflowNode node;

	public Transform transform;

	public void Send(float valueFloat)
	{		
		for (int i = 0; i < links.Count; i++)
			if (!links[i].node.running)
			{
				//links[i].type = type;
				if (Dataflow.debugFlow) Debug.Log("Send: " + valueFloat + " to: " + links[i].node.GetType().ToString());
				links[i].valueFloat = valueFloat;
				links[i].ready = true;
			}
	}

	public void Send(bool valueBoolean)
	{
		for (int i = 0; i < links.Count; i++)
			if (!links[i].node.running)
			{
				if (Dataflow.debugFlow) Debug.Log("Send: " + valueBoolean + " to: " + links[i].node.GetType().ToString());
				links[i].valueBoolean = valueBoolean;
				links[i].ready = true;
			}
	}

	public void Send(List<Target> valueList)
	{
		for (int i = 0; i < links.Count; i++)
			if (!links[i].node.running)
			{
				if (Dataflow.debugFlow) Debug.Log("Send: list of " + valueList.Count + " Targets to: " + links[i].node.GetType().ToString());
				links[i].valueTargetsList = valueList;
				links[i].ready = true;
			}
	}

	public void Send(List<Vector2> valueList)
	{
		for (int i = 0; i < links.Count; i++)
			if (!links[i].node.running)
			{
				if (Dataflow.debugFlow) Debug.Log("Send: list of " + valueList.Count + " Coordinates to: " + links[i].node.GetType().ToString());
				links[i].valueCoordinatesList = valueList;
				links[i].ready = true;
			}
	}

	public void Send(Target valueTarget)
	{
		for (int i = 0; i < links.Count; i++)
			if (!links[i].node.running)
			{
				if (Dataflow.debugFlow) Debug.Log("Send: " + valueTarget + " to: " + links[i].node.GetType().ToString());
				links[i].valueTarget = valueTarget;
				links[i].ready = true;
			}
	}

	public void Send()  //Activator
	{
		for (int i = 0; i < links.Count; i++)
			if (!links[i].node.running)
			{
				if (Dataflow.debugFlow) Debug.Log("Send: true (activate) to: " + links[i].node.GetType().ToString());
				links[i].valueBoolean = true;
				links[i].ready = true;
			}
	}
}