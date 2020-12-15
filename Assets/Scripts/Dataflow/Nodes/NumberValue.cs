using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataflowNodes
{

	[System.Serializable]
	public class NumberValue : DataflowNode
	{
		public NumberValue()
		{
			AddConfig("value", "0.0", Dataflow.ConfigType.Float);

			AddOutput("value", Dataflow.IOType.Number);
		}

		public override void Run()
		{
			base.Run();

			outputs[0].Send(float.Parse(configs["value"].value));

			Finish();
		}
	}
}