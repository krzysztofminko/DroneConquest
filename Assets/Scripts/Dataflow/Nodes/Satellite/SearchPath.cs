using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataflowNodes.Mothership
{
	public class SearchPath : DataflowNode
	{
		AStar.FindQuery query;

		public SearchPath()
		{
			AddInput("start x", Dataflow.IOType.Number);
			AddInput("start y", Dataflow.IOType.Number);
			AddInput("end x", Dataflow.IOType.Number);
			AddInput("end y", Dataflow.IOType.Number);

			AddOutput("path", Dataflow.IOType.CoordinatesList);
			AddOutput("no path", Dataflow.IOType.Activator);
		}

		public override void Run()
		{
			base.Run();

			if ((int)inputs[0].valueFloat == (int)inputs[2].valueFloat && (int)inputs[1].valueFloat == (int)inputs[3].valueFloat)
			{
				query = null;
				Finish();
			}
			else if (query == null)
			{
				query = AStar.FindPath(dataflow.drone.planet.walkable, (int)inputs[0].valueFloat, (int)inputs[1].valueFloat, (int)inputs[2].valueFloat, (int)inputs[3].valueFloat, true, true);
			}
			else if (query.path != null)
			{
				outputs[0].Send(AStar.PathToVectors(query.path));
				query = null;
				Finish();
			}
			else if (query.notFound)
			{
				outputs[1].Send();
				query = null;
				Finish();
			}
		}
	}
}
