using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataflowNodes
{
	public class GetDataElement : DataflowNode
	{
		public GetDataElement()
		{
			AddInput("data", Dataflow.IOType.CoordinatesList);
			AddInput("index", Dataflow.IOType.Number);

			AddOutput("x", Dataflow.IOType.Number);
			AddOutput("y", Dataflow.IOType.Number);
		}

		public override void Run()
		{
			outputs[0].Send(inputs[0].valueCoordinatesList[(int)inputs[1].valueFloat].x);
			outputs[1].Send(inputs[0].valueCoordinatesList[(int)inputs[1].valueFloat].y);
			Finish();
		}
	}
}
