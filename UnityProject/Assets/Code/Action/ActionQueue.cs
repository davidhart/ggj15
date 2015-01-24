using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionQueue
{
	public static ActionQueue Instance { get; private set; }

	List< ActionBase > Actions = new List< ActionBase >();

	public ActionQueue()
	{
		Instance = this;
	}

	public void AddToQueue( ActionBase newAction )
	{
		Actions.Add( newAction );
	}

	public void Execute()
	{
		foreach( var action in Actions )
		{
			action.Execute();
		}

		Actions.Clear();
	}
}
