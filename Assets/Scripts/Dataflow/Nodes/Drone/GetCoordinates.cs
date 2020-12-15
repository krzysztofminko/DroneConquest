using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataflowNodes.Drone
{
	public class GetCoordinates : DataflowNode
	{
		public GetCoordinates()
		{
			AddOutput("x", Dataflow.IOType.Number);
			AddOutput("y", Dataflow.IOType.Number);
		}

		public override void Run()
		{
			base.Run();

			outputs[0].Send(dataflow.drone.transform.position.x);
			outputs[1].Send(dataflow.drone.transform.position.y);

			Finish();
		}
	}
}