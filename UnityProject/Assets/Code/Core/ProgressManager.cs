using System;
using System.Collections.Generic;

using UnityEngine;

public class ProgressManager
{
	public static ProgressManager Instance { get; private set; }

	List<string> levels = new List<string>
	{
		"None",
		"FTUE",
		"Level1",
		"Level2"
	};

	int currentLevel = 0;

	public ProgressManager()
	{
		Instance = this;
	}

	public void Reset()
	{
		currentLevel = 0;
		StateMachine.Instance.SetState( new SignInState() );
	}

	public void NextLevel()
	{
		ActionGenerator.ResetDeck();

		currentLevel++;

		if( currentLevel >= levels.Count )
		{
			StateMachine.Instance.SetState( new SignInState() );
		}
		else
		{
			StateMachine.Instance.SetState( new GameState( levels[ currentLevel ] ) );
		}
	}

	public bool IsOnLastLevel()
	{
		return currentLevel >= levels.Count - 1;
	}
}

