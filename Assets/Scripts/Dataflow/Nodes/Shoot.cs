using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataflowNodes
{
	public class Shoot : DataflowNode
	{
		public Shoot()
		{
			delay = 0.5f;

			AddInput("target", Dataflow.IOType.Target);
		}

		public override void Run()
		{
			base.Run();

			dataflow.drone.Shoot(inputs[0].valueTarget);

			Finish();
		}
	}
}