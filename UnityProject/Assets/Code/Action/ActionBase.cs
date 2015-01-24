using UnityEngine;
using System.Collections;

public abstract class ActionBase
{
	public bool Started { get; private set; }
	
	protected abstract void Execute();

	public abstract bool IsDone();

	public void Start()
	{
		Execute();
		Started = true;
	}
}
