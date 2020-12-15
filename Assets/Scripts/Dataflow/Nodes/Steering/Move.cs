using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataflowNodes.Steering
{
	public class Move : DataflowNode
	{
		public Move()
		{
			AddInput("distance", Dataflow.IOType.Number);
		}

		public override void Run()
		{
			base.Run();

			if(dataflow.drone.Move(inputs[0].valueFloat))
				Finish();
		}
	}
}