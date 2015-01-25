using UnityEngine;
using System.Collections.Generic;

public class ActionGenerator
{
	static List<ActionBase> currentDeck = new List<ActionBase>();

	public static ActionBase GenerateAction()
	{
		if( currentDeck.Count == 0 )
			AddCards();

		int topOfDeckIndex = currentDeck.Count - 1;
		var drawCard = currentDeck[ topOfDeckIndex ];
		currentDeck.RemoveAt( topOfDeckIndex );

		return drawCard;
	}

	public static void ResetDeck()
	{
		currentDeck.Clear();
	}
	
	static void AddCards()
	{
		const int numForwards = 12;
		const int numLeftTurns = 6;
		const int numRightTurns = 6;

		for( int index = 0; index < numForwards; index++ )
			currentDeck.Add( new ActionForward() );

		for( int index = 0; index < numLeftTurns; index++ )
			currentDeck.Add( new ActionRotateLeft() );

		for( int index = 0; index < numRightTurns; index++ )
			currentDeck.Add( new ActionRotateRight() );

		Shuffle();

		DebugOutput();
	}

	static void Shuffle()
	{
		const int iterations = 200;

		for( int index = 0; index < iterations; index++ )
		{
			int cardIndex0 = (int)( Random.value * currentDeck.Count );
			int cardIndex1 = (int)( Random.value * currentDeck.Count );

			//	Swap!
			var card0 = currentDeck[ cardIndex0 ];

			currentDeck[ cardIndex0 ] = currentDeck[ cardIndex1 ];
			currentDeck[ cardIndex1 ] = card0;
		}
	}

	static void DebugOutput()
	{
		for( int index = 0; index < currentDeck.Count; index++ )
		{
			Debug.Log ( currentDeck[ index ] );
		}
	}
}
