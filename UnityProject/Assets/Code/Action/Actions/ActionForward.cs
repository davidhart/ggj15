using UnityEngine;
using System.Collections;

public class ActionForward : ActionBase
{
	protected override void Execute()
	{
		Character.Instance.MoveForward();
	}

	public override bool IsDone()
	{
		return !Character.Instance.IsAnimating();
	}
}
