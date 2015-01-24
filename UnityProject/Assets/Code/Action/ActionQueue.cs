using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionQueue
{
	public static ActionQueue Instance { get; private set; }
	public System.Action OnCardQueued;

	List< ActionBase > actions = new List< ActionBase >();


	public ActionQueue()
	{
		Instance = this;
	}

	public void AddToQueue( ActionBase newAction )
	{
		actions.Add( newAction );

		OnCardQueued();
	}

	public void Execute()
	{
		foreach( var action in actions )
		{
			action.Execute();
		}

		actions.Clear();
	}

	public List<ActionBase> Actions
	{
		get { return actions; }
	}
}
