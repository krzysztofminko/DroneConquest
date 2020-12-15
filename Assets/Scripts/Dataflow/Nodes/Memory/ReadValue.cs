using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataflowNodes.Memory
{
	public class ReadValue : DataflowNode
	{
		public ReadValue()
		{
			AddInput("name", Dataflow.IOType.Number);

			AddOutput("value", Dataflow.IOType.Number);
		}

		public override void Run()
		{
			base.Run();

			if (dataflow.drone.memory.ContainsKey(inputs[0].valueFloat.ToString()))
				outputs[0].Send(dataflow.drone.memory[inputs[0].valueFloat.ToString()]);

			Finish();
		}
	}
}
