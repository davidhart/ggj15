using UnityEngine;
using System.Collections;

public class ActionBackward : ActionBase
{
	protected override void Execute()
	{
		Character.Instance.MoveBackward();
	}

	public override string Name()
	{
		return "Backward";
	}

	public override int IconIndex ()
	{
		return -1;
	}

	public override bool IsDone()
	{
		return !Character.Instance.IsAnimating();
	}
}
