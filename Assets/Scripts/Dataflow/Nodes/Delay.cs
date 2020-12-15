using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataflowNodes
{

	public class Delay : DataflowNode
	{
		public Delay()
		{
			AddInput("delay", Dataflow.IOType.Number);

			AddOutput("activator", Dataflow.IOType.Activator);
		}

		public override void Run()
		{
			base.Run();

			delay = inputs[0].valueFloat;

			outputs[0].Send();

			Finish();
		}
	}
}
