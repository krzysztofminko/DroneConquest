using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataflowNodes.Logic
{

	public class NotOperator : DataflowNode
	{
		public NotOperator()
		{
			AddInput("value", Dataflow.IOType.Boolean);

			AddOutput("result", Dataflow.IOType.Boolean);
		}

		public override void Run()
		{
			base.Run();

			outputs[0].Send(!inputs[0].valueBoolean);

			Finish();
		}
	}
}