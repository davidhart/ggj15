using UnityEngine;
using System.Collections.Generic;

public class SelectedCardsUI : MonoBehaviour
{
	public List<CardUI> CardSlots;

	public void Start()
	{
		ActionQueue.Instance.OnCardQueued += OnCardQueued;

		for (int i = 0; i < CardSlots.Count; ++i)
		{
			CardSlots[i].gameObject.SetActive(i < ActivePlayers.Instance.Players.Count);
		}
	}

	public void OnDestroy()
	{
		ActionQueue.Instance.OnCardQueued -= OnCardQueued;
	}

	public void OnCardQueued()
	{
		for(int i = 0; i < ActivePlayers.Instance.Players.Count; ++i)
		{
			if (i < ActionQueue.Instance.Actions.Count)
			{
				CardSlots[i].SetupForAction(ActionQueue.Instance.Actions[i]);
			}
			else
			{
				CardSlots[i].SetupForAction(null);
			}
		}
	}
}

