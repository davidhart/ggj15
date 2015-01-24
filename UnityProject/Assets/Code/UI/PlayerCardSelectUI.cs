using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerCardSelectUI : MonoBehaviour
{
	private List<CardUI> Cards = new List<CardUI>();
	private GameObject CardSlotPrefab;
	public RectTransform CardTransformParent;

	private Player player;

	const int NumberOfCardSlots = 3;
	const float ItemHeight = 80.0f;
	const float itemWidth = 80.0f;
	const float itemSpacing = 5.0f;

	public void SetupForPlayer(Player player)
	{
		this.player = player;
		player.onSelectionChanged += OnSelectionChanged;
		player.onSelectionLockedIn += OnSelectionLockedIn;
		player.onSelectionUnlocked += OnSelectionUnlocked;
		player.onCardsPopulated += OnCardsPopulated;

		CardSlotPrefab = Resources.Load("UI/CardLarge") as GameObject;
		
		HorizontalLayoutGroup group = CardTransformParent.gameObject.AddComponent<HorizontalLayoutGroup>();
		group.spacing = itemSpacing;
		group.childAlignment = TextAnchor.MiddleCenter;
		float width = NumberOfCardSlots * itemWidth + (NumberOfCardSlots - 1) * itemSpacing;
		
		CardTransformParent.offsetMin = new Vector2(-width / 2.0f, -ItemHeight);
		CardTransformParent.offsetMax = new Vector2(width / 2.0f, 0.0f);
		
		for (int i = 0; i < NumberOfCardSlots; ++i)
		{
			GameObject card = GameObject.Instantiate(CardSlotPrefab) as GameObject;
			
			RectTransform cardTransform = card.GetComponent<RectTransform>();
			cardTransform.SetParent(CardTransformParent, false);
			
			Cards.Add(card.GetComponent<CardUI>());
		}
		OnSelectionChanged();
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
				Cards[i].SetSelectionLockedOut(player.SelectedAction != action);
			}
		}
	}

	public void OnSelectionUnlocked()
	{
		for (int i = 0; i < Cards.Count; ++i)
		{
			Cards[i].SetSelectionLocked(false);
			Cards[i].SetSelectionLockedOut(false);
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

