using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataflowNodes.Logic
{

	public class OrOperator : DataflowNode
	{
		public OrOperator()
		{
			AddInput("value 1", Dataflow.IOType.Boolean);
			AddInput("value 2", Dataflow.IOType.Boolean);

			AddOutput("result", Dataflow.IOType.Boolean);
		}

		public override void Run()
		{
			base.Run();

			outputs[0].Send(inputs[0].valueBoolean || inputs[1].valueBoolean);

			Finish();
		}
	}
}