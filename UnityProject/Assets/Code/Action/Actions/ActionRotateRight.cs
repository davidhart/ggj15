using UnityEngine;
using System.Collections;

public class ActionRotateRight : ActionBase
{
	public override void Execute()
	{
		Character.Instance.RotateRight();
	}
}
