using UnityEngine;
using System.Collections.Generic;

public class PlayerCardSelectUI : MonoBehaviour
{
	public List<CardUI> Cards = new List<CardUI>();

	private Player player;

	public void SetupForPlayer(Player player)
	{
		this.player = player;
		player.onSelectionChanged += OnSelectionChanged;
		player.onSelectionLockedIn += OnSelectionLockedIn;
		player.onSelectionUnlocked += OnSelectionUnlocked;
		player.onCardsPopulated += OnCardsPopulated;
	}

	public void OnDestroy()
	{
		if (player != null)
		{
			player.onSelectionChanged -= OnSelectionChanged;
			player.onSelectionLockedIn -= OnSelectionLockedIn;
			player.onSelectionUnlocked -= OnSelectionUnlocked;
			player.onCardsPopulated -= OnCardsPopulated;
		}
	}

	public void OnSelectionChanged()
	{
		for (int i = 0; i < Cards.Count; ++i)
		{
			if (i < player.AvailableActions.Count)
			{
				ActionBase action = player.AvailableActions[i];

				Cards[i].SetSelected(player.SelectedAction == action);
			}
		}
	}

	public void OnSelectionLockedIn()
	{
		for (int i = 0; i < Cards.Count; ++i)
		{
			if (i < player.AvailableActions.Count)
			{
				ActionBase action = player.AvailableActions[i];
				
				Cards[i].SetSelectionLocked(player.SelectedAction == action);
			}
		}
	}

	public void OnSelectionUnlocked()
	{
		for (int i = 0; i < Cards.Count; ++i)
		{
			Cards[i].SetSelectionLocked(false);
		}
	}

	public void OnCardsPopulated()
	{
		for (int i = 0; i < Cards.Count; ++i)
		{
			if (i < player.AvailableActions.Count)
			{
				
				ActionBase action = player.AvailableActions[i];

				Cards[i].SetupForAction(action);

				Cards[i].SetSelected(player.SelectedAction == action);
			}
			else
			{
				Cards[i].SetupForAction(null);
			}
		}
	}
}

