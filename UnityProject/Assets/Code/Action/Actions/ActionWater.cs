using UnityEngine;
using System.Collections;

public class ActionWater : ActionBase
{
	int frameCountAtStart;

	protected override void Execute()
	{
		Character.Instance.WaterParticleEmitter.emit = true;
		frameCountAtStart = Time.frameCount;
	}
	
	public override string Name ()
	{
		return "Water";
	}
	
	public override int IconIndex ()
	{
		return 4;
	}
	
	public override bool IsDone()
	{
		if( Time.frameCount - frameCountAtStart > 100 )
		{
			Character.Instance.WaterParticleEmitter.emit = false;

			FireManager.Instance.WaterAction( Character.Instance.gameObject.transform.position,
				Character.Instance.gameObject.transform.rotation );

			return true;
		}

		return false;
	}
}
