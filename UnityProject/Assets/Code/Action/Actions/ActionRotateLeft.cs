using UnityEngine;
using System.Collections;

public class ActionRotateLeft : ActionBase
{
	public override void Execute()
	{
		Character.Instance.RotateLeft();
	}
}
