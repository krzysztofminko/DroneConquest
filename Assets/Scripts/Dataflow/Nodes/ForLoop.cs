using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataflowNodes
{
	public class ForLoop : DataflowNode
	{
		bool started;
		int i;

		public ForLoop()
		{
			AddInput("list", Dataflow.IOType.CoordinatesList);
			AddInput("start", Dataflow.IOType.Number);
			AddInput("count", Dataflow.IOType.Number);

			AddOutput("item x", Dataflow.IOType.Number);
			AddOutput("item y", Dataflow.IOType.Number);
			AddOutput("finished", Dataflow.IOType.Activator);

		}

		public override void Run()
		{
			base.Run();

			if (!started)
			{
				started = true;
				i = (int)inputs[1].valueFloat;
			}
			else
			{
				i++;
			}

			if (i < (inputs[2].valueFloat == 0 ? inputs[0].valueCoordinatesList.Count : inputs[2].valueFloat))
			{
				outputs[0].Send(inputs[0].valueCoordinatesList[i].x);
				outputs[1].Send(inputs[0].valueCoordinatesList[i].y);
			}
			else
			{
				outputs[2].Send();
				started = false;
				Finish();
			}

		}
	}
}