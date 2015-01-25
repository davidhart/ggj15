using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SelectedCardsUI : MonoBehaviour
{
	List<CardUI> CardSlots = new List<CardUI>();

	GameObject CardSlotPrefab;

	int NumberOfCardSlots = 0;
	const float ItemHeight = 100.0f;
	const float itemWidth = 100.0f;
	const float itemSpacing = 10.0f;

	public void Start()
	{
		NumberOfCardSlots = ActivePlayers.Instance.Players.Count;

		CardSlotPrefab = Resources.Load("UI/CardLarge") as GameObject;

		HorizontalLayoutGroup group = gameObject.AddComponent<HorizontalLayoutGroup>();
		group.spacing = itemSpacing;
		group.childAlignment = TextAnchor.MiddleCenter;
		RectTransform thisTransform = GetComponent<RectTransform>();
		float width = NumberOfCardSlots * itemWidth + (NumberOfCardSlots - 1) * itemSpacing;

		thisTransform.offsetMin = new Vector2(-width / 2.0f, -ItemHeight);
		thisTransform.offsetMax = new Vector2(width / 2.0f, 0.0f);

		for (int i = 0; i < NumberOfCardSlots; ++i)
		{
			GameObject card = GameObject.Instantiate(CardSlotPrefab) as GameObject;

			RectTransform cardTransform = card.GetComponent<RectTransform>();
			cardTransform.SetParent(transform, false);

			CardSlots.Add(card.GetComponent<CardUI>());
		}

		ActionQueue.Instance.OnQueueChanged += OnCardQueued;

		OnCardQueued();
	}

	public void OnDestroy()
	{
		ActionQueue.Instance.OnQueueChanged -= OnCardQueued;
	}

	public void OnCardQueued()
	{
		for(int i = 0; i < NumberOfCardSlots; ++i)
		{
			if (i < ActionQueue.Instance.Actions.Count)
			{
				CardSlots[i].SetupForAction(ActionQueue.Instance.Actions[i]);

				if ( i == ActionQueue.Instance.Actions.Count - 1 )
				{
					CardSlots[i].TriggerQueue();
				}
			}
			else
			{
				CardSlots[i].SetupForAction(null);
			}
		}
	}
}

