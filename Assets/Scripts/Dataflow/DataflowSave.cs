using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DataflowSave {

	public static List<DataflowSave> list = new List<DataflowSave>();

	public string name;
	public List<Node> nodes = new List<Node>();
	public List<Link> links = new List<Link>();

	[Serializable]
	public struct Node
	{
		public string name;
		public Input activator;
		public List<Input> inputs;
		public List<Config> configs;
		public int posx;
		public int posy;
	}

	[Serializable]
	public struct Input
	{
		public float valueFloat;
		public bool valueBoolean;
		public bool ready;
	}

	[Serializable]
	public struct Config
	{
		public string name;
		public string value;		
	}

	[Serializable]
	public struct Link
	{
		public int outputNodeId;
		public int outputId;
		public int inputNodeId;
		public int inputId;
	}

	public DataflowSave(Dataflow dataflow)
	{
		name = dataflow.name;
		
		foreach(var dn in dataflow.nodes)
		{
			Node node = new Node
			{
				name = dn.name,
				posx = dn.posx,
				posy = dn.posy,
				inputs = new List<Input>(),
				configs = new List<Config>()
			};

			var dna = dn.activator;
			node.activator = new Input
			{
				valueFloat = dna.valueFloat,
				valueBoolean = dna.valueBoolean,
				ready = dna.ready
			};
			if (dna.link != null)
			{
				links.Add(new Link
				{
					inputNodeId = dataflow.nodes.IndexOf(dn),
					inputId = -1,
					outputNodeId = dataflow.nodes.IndexOf(dna.link.node),
					outputId = dna.link.node.outputs.IndexOf(dna.link)
				});
			}
			foreach (var dni in dn.inputs)
			{
				node.inputs.Add(new Input {
					valueFloat = dni.valueFloat,
					valueBoolean = dni.valueBoolean,
					ready = dni.ready
				});
				if(dni.link != null)
				{
					links.Add(new Link {
						inputNodeId = dataflow.nodes.IndexOf(dn),
						inputId = dn.inputs.IndexOf(dni),
						outputNodeId = dataflow.nodes.IndexOf(dni.link.node),
						outputId = dni.link.node.outputs.IndexOf(dni.link)
					});
				}
			}
			foreach (var dnc in dn.configs)
				node.configs.Add(new Config { name = dnc.Key, value = dnc.Value.value});
			nodes.Add(node);
		}
	}

	public Dataflow ToDataflow()
	{
		Dataflow d = new Dataflow(name);

		foreach (var n in nodes)
		{
			DataflowNode node = d.AddNode(n.name, n.posx, n.posy);
			node.activator.valueFloat = n.activator.valueFloat;
			node.activator.valueBoolean = n.activator.valueBoolean;
			node.activator.ready = n.activator.ready;
			for (int i = 0; i < node.inputs.Count; i++)
			{
				node.inputs[i].valueFloat = n.inputs[i].valueFloat;
				node.inputs[i].valueBoolean = n.inputs[i].valueBoolean;
				node.inputs[i].ready = n.inputs[i].ready;
			}
			foreach (var c in n.configs)
				node.configs[c.name].value = c.value;
		}

		foreach (var l in links)
			if(l.outputId < d.nodes[l.outputNodeId].outputs.Count && l.inputId < d.nodes[l.inputNodeId].inputs.Count)	//Check for different save versions (inputs/outputs changed)
				d.AddLink(d.nodes[l.outputNodeId].outputs[l.outputId], l.inputId < 0 ? d.nodes[l.inputNodeId].activator : d.nodes[l.inputNodeId].inputs[l.inputId]);

		return d;
	}
}
