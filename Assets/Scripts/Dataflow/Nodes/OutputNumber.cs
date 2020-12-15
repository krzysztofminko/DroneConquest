using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataflowNodes
{

	[System.Serializable]
	public class OutputNumber : DataflowNode
	{
		public OutputNumber()
		{
			AddInput("value", Dataflow.IOType.Number);

			AddConfig("Debug?", "false", Dataflow.ConfigType.Boolean);
		}

		public override void Run()
		{
			base.Run();

			if (configs["Debug?"].value == "true")
				Debug.Log("DNOutputNumber: " + inputs[0].valueFloat, dataflow.drone);

			Finish();
		}
	}
}