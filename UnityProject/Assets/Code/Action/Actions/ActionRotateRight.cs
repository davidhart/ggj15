using UnityEngine;
using System.Collections;

public class ActionRotateRight : ActionBase
{
	public override void Execute()
	{
		Character.Instance.RotateRight();
	}

	public override string Name ()
	{
		return "Right Turn";
	}
}
