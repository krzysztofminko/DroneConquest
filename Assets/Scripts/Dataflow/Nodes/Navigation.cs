using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataflowNodes
{
	public class Navigation : DataflowNode
	{
		bool navigating;

		public Navigation()
		{
			AddInput("path", Dataflow.IOType.CoordinatesList);

			AddOutput("finished", Dataflow.IOType.Activator);
			AddOutput("obstacle", Dataflow.IOType.Activator);
		}

		public override void Run()
		{
			base.Run();

			if (!navigating)
			{
				navigating = true;
				dataflow.drone.path = inputs[0].valueCoordinatesList;
			}
			else if (dataflow.drone.path.Count == 0)
			{
				navigating = false;
				outputs[0].Send();
				Finish();
			}
			else if (dataflow.drone.obstacleAhead)
			{
				navigating = false;
				outputs[1].Send();
				Finish();
			}
		}
	}
}