using UnityEngine;

public class ActionGenerator
{
	public static ActionBase GenerateAction()
	{
		switch (Random.Range(0, 5))
		{
		case 0:
			return new ActionForward();
		case 1:
			return new ActionForward();
		case 2:
			return new ActionForward();
		case 3:
			return new ActionRotateLeft();
		case 4:
			return new ActionRotateRight();
		}

		return null;
	}
}