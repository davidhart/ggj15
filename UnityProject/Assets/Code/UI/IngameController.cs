using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class IngameController : MonoBehaviour
{
	public List<PlayerCardSelectUI> PlayerCardUI = new List<PlayerCardSelectUI>();
	public TimerUI Timer;

	float timerDuration = 10.0f;
	float timerDecayRate = 1.0f;
	float timerMinDuration = 5.0f;

	public void Start()
	{
		for (int i = 0; i < PlayerCardUI.Count; ++i)
		{
			if (i < ActivePlayers.Instance.Players.Count)
			{
				PlayerCardUI[i].SetupForPlayer(ActivePlayers.Instance.Players[i]);
			}
			else
			{
				PlayerCardUI[i].gameObject.SetActive(false);
			}
		}

		foreach(Player player in ActivePlayers.Instance.Players)
		{
			player.PopulateActions();
		}

		Timer.OnTimerCountdown += OnTimerCountdown;

		Timer.ResetTimer(timerDuration);
	}

	public void OnDestroy()
	{
		Timer.OnTimerCountdown -= OnTimerCountdown;
	}

	public void Update()
	{
		foreach(Player player in ActivePlayers.Instance.Players)
		{
			player.UpdateInput();
		}

		if (ActivePlayers.Instance.Players.All(q=>q.SelectionLockedIn) && ActionQueue.Instance.Done())
		{
			ActionQueue.Instance.Reset();

			foreach(Player player in ActivePlayers.Instance.Players)
			{
				player.ConsumeSelectedAction();
				player.PopulateActions();
			}

			FireManager.Instance.FireSpreads();
		}
	}

	private void OnTimerCountdown()
	{
		ForceRandomInputsForRemainingPlayers();
		//FireManager.Instance.FireSpreads();
		timerDuration -= timerDecayRate;
		timerDuration = Mathf.Max(timerMinDuration, timerDuration);
		Timer.ResetTimer(timerDuration);
	}

	private void ForceRandomInputsForRemainingPlayers()
	{
		foreach(Player player in ActivePlayers.Instance.Players)
		{
			if (player.SelectionLockedIn == false)
			{
				player.LockInSelection();
			}
		}
	}
}
