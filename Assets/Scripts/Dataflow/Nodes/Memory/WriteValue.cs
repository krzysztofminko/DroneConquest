using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataflowNodes.Memory
{
	public class WriteValue : DataflowNode
	{
		public WriteValue()
		{
			AddInput("name", Dataflow.IOType.Number);
			AddInput("value", Dataflow.IOType.Number);
		}

		public override void Run()
		{
			base.Run();

			if (dataflow.drone.memory.ContainsKey(inputs[0].valueFloat.ToString()))
				dataflow.drone.memory[inputs[0].valueFloat.ToString()] = inputs[1].valueFloat;
			else
				dataflow.drone.memory.Add(inputs[0].valueFloat.ToString(), inputs[1].valueFloat);

			Finish();
		}
	}
}
