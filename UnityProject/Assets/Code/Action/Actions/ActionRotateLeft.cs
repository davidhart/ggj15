using UnityEngine;
using System.Collections;

public class ActionRotateLeft : ActionBase
{
	protected override void Execute()
	{
		Character.Instance.RotateLeft();
	}

	public override string Name ()
	{
		return "Left Turn";
	}

	public override int IconIndex ()
	{
		return 2;
	}

	public override bool IsDone()
	{
		return !Character.Instance.IsAnimating();
	}
}
