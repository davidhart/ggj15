using UnityEngine;
using System.Collections;

public abstract class ActionBase
{
	protected abstract void Execute();
	public abstract string Name();
	public bool Started { get; private set; }

	public abstract bool IsDone();

	public void Start()
	{
		Execute();
		Started = true;
	}
}
