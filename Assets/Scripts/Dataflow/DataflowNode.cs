using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataflowNode
{
	public string name;

	public Dataflow dataflow;

	public float delay;// = 1.0f;
	public float lastRun;
	public bool running;

	public DataflowInput activator;
	public List<DataflowInput> inputs = new List<DataflowInput>();
    public List<DataflowOutput> outputs = new List<DataflowOutput>();
	public Dictionary<string, DataflowConfig> configs = new Dictionary<string, DataflowConfig>();

	public int posx;
	public int posy;

	public Transform transform;

	public bool RunIfReady()
	{
		//Check delay
		if (Time.time - lastRun < delay)
			return false;
		//Check linked activator
		if (activator.link != null)
			if (!running && !activator.ready)
				return false;
		//Check linked inputs
		for (int i = 0; i < inputs.Count; i++)
		{
			if (inputs[i].link != null) {
				if (!running && !inputs[i].ready)
					return false;			
			}
			if (inputs[i].link == null && inputs[i].noDefaultValue)
				return false;
		}
		//Check linked activator
		/*
		if (!running)
		{
			bool active = true;
			for (int i = 0; i < inputs.Count; i++)
				if (inputs[i].type == Dataflow.IOType.Activator && inputs[i].link != null)
				{
					active = inputs[i].valueBoolean;
					inputs[i].valueBoolean = false;
				}
			if (!active)
				return false;
		}*/
		//Run
		Run();
		return true;
    }

    public virtual void Run()
    {
		if (Dataflow.debugFlow) Debug.Log("Run: " + GetType().ToString());
		running = true;
		lastRun = Time.time;
	}

	public void Finish()
	{
		running = false;
		activator.ready = false;
		for (int i = 0; i < inputs.Count; i++)
			inputs[i].ready = false;
	}

	protected DataflowInput AddInput(string name, Dataflow.IOType type)
	{
		DataflowInput di = new DataflowInput() { name = name, type = type, node = this };
		if (type == Dataflow.IOType.TargetsList || type == Dataflow.IOType.Target || type == Dataflow.IOType.CoordinatesList)
			di.noDefaultValue = true;
		inputs.Add(di);
		return di;
	}

	protected DataflowOutput AddOutput(string name, Dataflow.IOType type)
	{
		outputs.Add(new DataflowOutput() { name = name, type = type, node = this });
		return outputs[outputs.Count - 1];
	}

	protected DataflowConfig AddConfig(string name, string value, Dataflow.ConfigType type, List<string> values = null)
	{
		DataflowConfig config = new DataflowConfig { value = value, type = type, values = values };
		configs.Add(name, config);
		return config;
	}

}
