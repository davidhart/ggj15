using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class IngameController : MonoBehaviour
{
	public static IngameController Instance { get; private set; }

	public List<PlayerCardSelectUI> PlayerCardUI = new List<PlayerCardSelectUI>();
	public TimerUI Timer;

	float timerDuration = 10.0f;
	float timerDecayRate = 1.0f;
	float timerNaturalDecayRate = 0.25f;
	float timerMinDuration = 5.0f;

	public void Start()
	{
		Instance = this;

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

		ActionQueue.Instance.Reset();
		Character.Instance.RemoveAnimation();

		foreach(Player player in ActivePlayers.Instance.Players)
			player.PopulateActions();
	}

	void CheckForVictory()
	{
		bool isFinished = Level.Instance.ContainsEndGame( Character.Instance.gameObject.transform.position );

		if( isFinished )
		{
			StateMachine.instance.SetState( new VictoryState() );
		}
	}

	public void Update()
	{
		foreach(Player player in ActivePlayers.Instance.Players)
		{
			player.UpdateInput();
		}

		CheckForVictory();

		if (ActivePlayers.Instance.Players.All(q=>q.SelectionLockedIn) && ActionQueue.Instance.Done())
		{
			ActionQueue.Instance.Reset();

			foreach(Player player in ActivePlayers.Instance.Players)
			{
				player.ConsumeSelectedAction();
				player.PopulateActions();
			}

			FireManager.Instance.FireSpreads();

			timerDuration -= timerNaturalDecayRate;
			timerDuration = Mathf.Max(timerMinDuration, timerDuration);

			Timer.ResetTimer(timerDuration);
		}
	}

	private void OnTimerCountdown()
	{
		ForceRandomInputsForRemainingPlayers();
		//FireManager.Instance.FireSpreads();
		timerDuration -= timerDecayRate;
		timerDuration = Mathf.Max(timerMinDuration, timerDuration);
		//Timer.ResetTimer(timerDuration);
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
