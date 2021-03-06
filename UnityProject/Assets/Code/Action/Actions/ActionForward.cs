using UnityEngine;
using System.Collections;

public class ActionForward : ActionBase
{
	protected override void Execute()
	{
		Character.Instance.MoveForward();
	}

	public override string Name ()
	{
		return "Forward";
	}

	public override int IconIndex ()
	{
		return 1;
	}

	public override bool IsDone()
	{
		return !Character.Instance.IsAnimating();
	}
}
