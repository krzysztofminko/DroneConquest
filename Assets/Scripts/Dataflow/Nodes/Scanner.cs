using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataflowNodes
{
	public class ResourceScanner : DataflowNode
	{
		public ResourceScanner()
		{
			delay = 0.5f;
			
			AddConfig("scan for", "Resource", Dataflow.ConfigType.String, new List<string> { "Resource" });
			AddConfig("range", "10", Dataflow.ConfigType.Integer);

			AddOutput("data", Dataflow.IOType.CoordinatesList);
			AddOutput("onNoData", Dataflow.IOType.Activator);
		}

		public override void Run()
		{
			base.Run();

			List<Vector2> positions = new List<Vector2>();

			for (int i = 0; i < dataflow.drone.planet.sources.Count; i++)
				if (dataflow.drone.planet.sources[i].position.x >= Mathf.Max(0, (int)dataflow.drone.transform.position.x - int.Parse(configs["range"].value)) &&
					dataflow.drone.planet.sources[i].position.x < Mathf.Min(dataflow.drone.planet.size, (int)dataflow.drone.transform.position.x + int.Parse(configs["range"].value)) &&
					dataflow.drone.planet.sources[i].position.y >= Mathf.Max(0, (int)dataflow.drone.transform.position.y - int.Parse(configs["range"].value)) &&
					dataflow.drone.planet.sources[i].position.y < Mathf.Min(dataflow.drone.planet.size, (int)dataflow.drone.transform.position.y + int.Parse(configs["range"].value))
					)
					positions.Add(dataflow.drone.planet.sources[i].position);
			
			if (positions.Count > 0)
				outputs[0].Send(positions);
			else
				outputs[1].Send();

			Finish();
		}
	}
}