using System.Collections.Generic;
using UnityEngine;

public enum ePlayerControlMode
{
	ControllerSelect,
	SlotMachine,
	Random
}

public class Player
{
	InputDevice device;
	List<ActionBase> availableActions = new List<ActionBase>();

	const int MaxAvailableActions = 3;
	bool selectionLockedIn;

	int selectionIndex = 0;
	int stickDirection = 0;
	float timeSinceTick = 0;
	const float controllerTickSpeed = 0.5f;
	const float slotTickSpeed = 0.25f;
	const float randomTickSpeed = 0.25f;

	public System.Action onSelectionChanged;
	public System.Action onSelectionLockedIn;
	public System.Action onSelectionUnlocked;
	public System.Action onCardsPopulated;

	private ePlayerControlMode controlMode = ePlayerControlMode.ControllerSelect;

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
			LockInSelection();
			return;
		}

		switch (controlMode)
		{
		case ePlayerControlMode.ControllerSelect:
			HandleInputPlayerSelectController();
			break;

		case ePlayerControlMode.SlotMachine:
			HandleInputSlotMachine();
			break;

		case ePlayerControlMode.Random:
			HandleInputRandom();
			break;
		}
	}

	private void HandleInputPlayerSelectController()
	{
		if (device.axes[0] < 0.0f)
		{
			if (stickDirection != -1)
			{
				MoveSelectionLeft();
				stickDirection = -1;
				timeSinceTick = 0.0f;
			}
			else
			{
				timeSinceTick += Time.deltaTime;
				
				while (timeSinceTick > controllerTickSpeed)
				{
					timeSinceTick -= controllerTickSpeed;
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
				timeSinceTick = 0.0f;
			}
			else
			{
				timeSinceTick += Time.deltaTime;
				
				while (timeSinceTick > controllerTickSpeed)
				{
					timeSinceTick -= controllerTickSpeed;
					MoveSelectionRight();
				}
			}
		}
		else
		{
			stickDirection = 0;
		}
	}

	private void HandleInputSlotMachine()
	{
		timeSinceTick += Time.deltaTime;

		while(timeSinceTick > slotTickSpeed)
		{
			MoveSelectionRight();
			timeSinceTick -= slotTickSpeed;
		}
	}

	private void HandleInputRandom()
	{
		timeSinceTick += Time.deltaTime;

		while(timeSinceTick > randomTickSpeed)
		{
			int random;

			do
			{
				random = Random.Range(0, availableActions.Count);
			} while (random == selectionIndex);
			SetSelection(random);

			timeSinceTick -= randomTickSpeed;
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
		availableActions[selectionIndex] = null;
		selectionLockedIn = false;
		onSelectionUnlocked();
	}

	public void PopulateActions()
	{
		while(availableActions.Count < MaxAvailableActions)
		{
			availableActions.Add(null);
		}

		for (int i = 0; i < MaxAvailableActions; ++i)
		{
			if (availableActions[i] == null)
			{
				availableActions[i] = ActionGenerator.GenerateAction();
			}
		}

		onCardsPopulated();
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

	private void SetSelection(int index)
	{
		selectionIndex = index;
		onSelectionChanged();
	}

	public bool SelectionLockedIn
	{
		get { return selectionLockedIn; }
	}

	public void LockInSelection()
	{
		ActionQueue.Instance.AddToQueue(SelectedAction);
		
		selectionLockedIn = true;
		
		onSelectionLockedIn();
	}
}
