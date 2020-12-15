using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataflowNodes.ScannerData
{

	public class GetNearest : DataflowNode
	{
		public GetNearest()
		{
			AddInput("data", Dataflow.IOType.TargetsList);

			AddOutput("target", Dataflow.IOType.Target);
			AddOutput("distance", Dataflow.IOType.Number);
		}

		public override void Run()
		{
			base.Run();

			Target nearest = null;
			float min = Mathf.Infinity;
			float tmpDistance = min;
			for (int i = 0; i < inputs[0].valueTargetsList.Count; i++)
			{
				tmpDistance = (inputs[0].valueTargetsList[i].transform.position - dataflow.drone.transform.position).sqrMagnitude;
				if (tmpDistance < min)
				{
					min = tmpDistance;
					nearest = inputs[0].valueTargetsList[i];
				}
			}

			outputs[0].Send(nearest);
			outputs[1].Send(tmpDistance);

			Finish();
		}
	}
}