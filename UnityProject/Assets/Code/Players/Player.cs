using System.Collections.Generic;
using UnityEngine;

public class Player
{
	InputDevice device;
	List<ActionBase> availableActions = new List<ActionBase>();

	const int MaxAvailableActions = 3;
	bool selectionLockedIn;

	int selectionIndex = 0;
	int stickDirection = 0;
	float timeInDirection = 0;
	const float tickTime = 0.2f;

	public System.Action onSelectionChanged;
	public System.Action onSelectionLockedIn;
	public System.Action onSelectionUnlocked;
	public System.Action onCardsPopulated;

	public Player(InputDevice device)
	{
		this.device = device;
	}

	public void UpdateInput()
	{
		if (selectionLockedIn)
			return;

		if ( device.GetButtonDown( ButtonType.Action1 ) )
		{
			ActionQueue.Instance.AddToQueue(SelectedAction);

			selectionLockedIn = true;

			onSelectionLockedIn();
			return;
		}

		if (device.axes[0] < 0.0f)
		{
			if (stickDirection != -1)
			{
				MoveSelectionLeft();
				stickDirection = -1;
				timeInDirection = 0.0f;
			}
			else
			{
				timeInDirection += Time.deltaTime;

				while (timeInDirection > tickTime)
				{
					timeInDirection -= tickTime;
					MoveSelectionLeft();
				}
			}
		}
		else if (device.axes[0] > 0.0f)
		{
			if (stickDirection != 1)
			{
				MoveSelectionRight();
				stickDirection = 1;
				timeInDirection = 0.0f;
			}
			else
			{
				timeInDirection += Time.deltaTime;

				while (timeInDirection > tickTime)
				{
					timeInDirection -= tickTime;
					MoveSelectionRight();
				}
			}
		}
		else
		{
			stickDirection = 0;
		}
	}

	public ActionBase SelectedAction
	{
		get { return availableActions[selectionIndex]; }
	}

	public List<ActionBase> AvailableActions
	{
		get { return availableActions; }
	}

	public void ConsumeSelectedAction()
	{
		availableActions.RemoveAt(selectionIndex);
		selectionIndex = 0;

		onSelectionUnlocked();
	}

	public void PopulateActions()
	{
		while(availableActions.Count < MaxAvailableActions)
		{
			availableActions.Add( ActionGenerator.GenerateAction() );
		}

		onCardsPopulated();

		selectionIndex = 0;
		onSelectionChanged();
	}

	private void MoveSelectionLeft()
	{
		selectionIndex--;

		if (selectionIndex < 0)
		{
			selectionIndex = availableActions.Count - 1;
		}

		onSelectionChanged();
	}

	private void MoveSelectionRight()
	{
		selectionIndex++;

		if (selectionIndex >= availableActions.Count)
		{
			selectionIndex = 0;
		}

		onSelectionChanged();
	}

	public bool SelectionLockedIn
	{
		get { return selectionLockedIn; }
	}
}
