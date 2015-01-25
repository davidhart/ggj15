using UnityEngine;
using System.Collections;

public class ActionRotateRight : ActionBase
{
	protected override void Execute()
	{
		Character.Instance.RotateRight();
	}

	public override string Name ()
	{
		return "Right Turn";
	}

	public override int IconIndex ()
	{
		return 3;
	}

	public override bool IsDone()
	{
		return !Character.Instance.IsAnimating();
	}
}
