using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataflowNodes
{

	public class AnalyseTarget : DataflowNode
	{
		public AnalyseTarget()
		{
			AddInput("target", Dataflow.IOType.Target);

			AddOutput("target", Dataflow.IOType.Target);
			AddOutput("pos x", Dataflow.IOType.Number);
			AddOutput("pos y", Dataflow.IOType.Number);
			AddOutput("health", Dataflow.IOType.Number);
			AddOutput("healthMax", Dataflow.IOType.Number);
			AddOutput("energy", Dataflow.IOType.Number);
			AddOutput("energyMax", Dataflow.IOType.Number);
			AddOutput("damage", Dataflow.IOType.Number);
		}

		public override void Run()
		{
			base.Run();

			Target t = inputs[0].valueTarget;

			outputs[0].Send(t);
			outputs[1].Send(t.transform.position.x);
			outputs[2].Send(t.transform.position.z);
			outputs[3].Send(t.health);
			outputs[4].Send(t.healthMax);
			outputs[5].Send(t.energy);
			outputs[6].Send(t.energyMax);
			outputs[7].Send(t.damage);

			Finish();
		}
	}
}