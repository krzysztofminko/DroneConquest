using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataflowNodes.Logic
{

	public class AndOperator : DataflowNode
	{
		public AndOperator()
		{
			AddInput("value 1", Dataflow.IOType.Boolean);
			AddInput("value 2", Dataflow.IOType.Boolean);

			AddOutput("result", Dataflow.IOType.Boolean);
		}

		public override void Run()
		{
			base.Run();
			Debug.Log(inputs[0].valueBoolean);
			Debug.Log(inputs[1].valueBoolean);
			Debug.Log(inputs[0].valueBoolean && inputs[1].valueBoolean);

			outputs[0].Send(inputs[0].valueBoolean && inputs[1].valueBoolean);

			Finish();
		}
	}
}