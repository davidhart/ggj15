using UnityEngine;

public class ActionGenerator
{
	public static ActionBase GenerateAction()
	{
		switch (Random.Range(0, 4))
		{
		case 0:
			return new ActionBackward();
		case 1:
			return new ActionForward();
		case 2:
			return new ActionRotateLeft();
		case 3:
			return new ActionRotateRight();
		}

		return null;
	}
}