using UnityEngine;
using System.Collections;

public class ActionRotateLeft : ActionBase
{
	protected override void Execute()
	{
		Character.Instance.RotateLeft();
	}

	public override bool IsDone()
	{
		return !Character.Instance.IsAnimating();
	}
}
