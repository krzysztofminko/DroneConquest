using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataflowNodes
{
	public class OnStart : DataflowNode
	{
		bool started;

		public OnStart()
		{
			AddOutput("activatior", Dataflow.IOType.Activator);
		}

		public override void Run()
		{
			base.Run();

			if (!started)
			{
				started = true;
				outputs[0].Send();
			}

			Finish();
		}
	}
}