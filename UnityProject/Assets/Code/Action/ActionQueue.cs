using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionQueue
{
	public static ActionQueue Instance { get; private set; }

	Queue< ActionBase > Actions = new Queue< ActionBase >();

	public ActionQueue()
	{
		Instance = this;
	}

	public void AddToQueue( ActionBase newAction )
	{
		Actions.Enqueue( newAction );
	}
	
	public void Update()
	{
		if( Actions.Count == 0 )
			return;

		var currentAction = Actions.Peek();
		if( !currentAction.Started )
		{
			currentAction.Start();
		}

		if( currentAction.IsDone() )
		{
			Actions.Dequeue();
		}
	}
}
