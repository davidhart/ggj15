using UnityEngine;
using System.Collections;

public class ActionBackward : ActionBase
{
	protected override void Execute()
	{
		Character.Instance.MoveBackward();
	}

	public override bool IsDone()
	{
		return !Character.Instance.IsAnimating();
	}
}
