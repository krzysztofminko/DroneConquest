using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataflowNodes.Math
{

	public class Random : DataflowNode
	{
		public Random()
		{
			AddInput("min", Dataflow.IOType.Number);
			AddInput("max", Dataflow.IOType.Number);

			AddOutput("result", Dataflow.IOType.Number);
		}

		public override void Run()
		{
			base.Run();

			outputs[0].Send(UnityEngine.Random.Range(inputs[0].valueFloat, inputs[1].valueFloat));

			Finish();
		}
	}
}