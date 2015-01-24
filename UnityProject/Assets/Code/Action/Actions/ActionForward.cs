using UnityEngine;
using System.Collections;

public class ActionForward : ActionBase
{
	public override void Execute()
	{
		Character.Instance.MoveForward();
	}
}
