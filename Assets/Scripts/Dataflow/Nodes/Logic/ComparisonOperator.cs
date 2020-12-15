using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataflowNodes.Logic
{

	public class ComparisonOperator : DataflowNode
	{
		public ComparisonOperator()
		{
			AddInput("value 1", Dataflow.IOType.Number);
			AddInput("value 2", Dataflow.IOType.Number);

			AddConfig("operator", "=", Dataflow.ConfigType.String, new List<string> { "=", "<", ">", "<=", ">=", "!=" });

			AddOutput("result", Dataflow.IOType.Boolean);
		}

		public override void Run()
		{
			base.Run();

			switch (configs["operator"].value)
			{
				case "=": outputs[0].Send(inputs[0].valueFloat == inputs[1].valueFloat); break;
				case "<": outputs[0].Send(inputs[0].valueFloat < inputs[1].valueFloat); break;
				case ">": outputs[0].Send(inputs[0].valueFloat > inputs[1].valueFloat); break;
				case "<=": outputs[0].Send(inputs[0].valueFloat <= inputs[1].valueFloat); break;
				case ">=": outputs[0].Send(inputs[0].valueFloat >= inputs[1].valueFloat); break;
				case "!=": outputs[0].Send(inputs[0].valueFloat != inputs[1].valueFloat); break;
			}

			Finish();
		}
	}
}