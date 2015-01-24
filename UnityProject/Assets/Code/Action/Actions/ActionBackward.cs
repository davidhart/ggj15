using UnityEngine;
using System.Collections;

public class ActionBackward : ActionBase
{
	public override void Execute()
	{
		Character.Instance.MoveBackward();
	}

	public override string Name()
	{
		return "Backward";
	}
}
