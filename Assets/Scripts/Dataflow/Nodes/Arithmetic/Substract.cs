using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataflowNodes.Arithmetic
{
	public class Substract : DataflowNode
	{
		public Substract()
		{
			AddInput("a", Dataflow.IOType.Number);
			AddInput("b", Dataflow.IOType.Number);

			AddOutput("a - b", Dataflow.IOType.Number);
		}

		public override void Run()
		{
			outputs[0].Send(inputs[0].valueFloat - inputs[1].valueFloat);
			Finish();
		}
	}
}