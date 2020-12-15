using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TODO: value as object, type casting by nodes, add type enum
public class Dataflow
{
	public static bool debugFlow;

	public enum IOType { Activator, Number, Boolean, TargetsList, Target, CoordinatesList }
	public enum ConfigType { String, Integer, Float, Boolean }

	public string name;
	public Drone drone;
    public List<DataflowNode> nodes = new List<DataflowNode>();

	public DataflowSave save;
	
	public Dataflow(string name)
	{
		this.name = name;
	}
		
	public void Run()
    {
		bool any = false;
		for (int i = 0; i < nodes.Count; i++)
			if (nodes[i].RunIfReady())
				any = true;
				
		if (debugFlow && any) Debug.Log("-- Dataflow tick end --");
	}

	public DataflowNode AddNode(string name, int posx = 0, int posy = 0)
	{
		string[] path = name.Split('.');
		DataflowNode n = System.Reflection.Assembly.GetExecutingAssembly().CreateInstance((path[0] == "DataflowNodes" ? "" : "DataflowNodes.") + name) as DataflowNode;
		n.name = name;
		n.dataflow = this;
		n.posx = posx;
		n.posy = posy;
		n.activator = new DataflowInput() { name = "activator", type = IOType.Activator, node = n };
		nodes.Add(n);		
		return n;
	}

	public void AddLink(DataflowOutput output, DataflowInput input)
	{
		output.links.Add(input);
		input.link = output;
		input.unlinkedValue = input.ValueToString();
	}

	public void DeleteNode(DataflowNode n)
	{
		for (int i = 0; i < n.inputs.Count; i++)
			if (n.inputs[i].link != null)
				DeleteLink(n.inputs[i]);
		for (int i = 0; i < n.outputs.Count; i++)
			for (int l = 0; l < n.outputs[i].links.Count; l++)
				DeleteLink(n.outputs[i].links[l]);
		nodes.Remove(n);
	}

	public void DeleteLink(DataflowInput input)
	{
		input.link = null;
		switch (input.type)
		{
			case IOType.Number: input.valueFloat = float.Parse(input.unlinkedValue); break;
			case IOType.Boolean: input.valueBoolean = bool.Parse(input.unlinkedValue); break;
		}
	}

}
