using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionQueue
{
	public static ActionQueue Instance { get; private set; }
	public System.Action OnQueueChanged;

	List< ActionBase > actions = new List< ActionBase >();

	public List < ActionBase > Actions
	{
		get { return actions; }
	}

	private int currentActionIndex = 0;

	public ActionQueue()
	{
		Instance = this;
	}

	public void AddToQueue( ActionBase newAction )
	{
		actions.Add( newAction );

		if (OnQueueChanged != null)
			OnQueueChanged();
	}
	
	public void Update()
	{
		if (currentActionIndex >= actions.Count)
			return;

		var currentAction = actions[currentActionIndex];
		if( !currentAction.Started )
		{
			currentAction.Start();
		}

		if( currentAction.IsDone() )
		{
			currentActionIndex++;
		}
	}

	public void Reset()
	{
		currentActionIndex = 0;
		actions.Clear();

		if (OnQueueChanged != null)
			OnQueueChanged();
	}

	public bool Done()
	{
		return currentActionIndex >= actions.Count;
	}
}
