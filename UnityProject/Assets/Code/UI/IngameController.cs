using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class IngameController : MonoBehaviour
{
	public static IngameController Instance { get; private set; }

	public List<RectTransform> PlayerCardUIParent = new List<RectTransform>();
	private List<PlayerCardSelectUI> PlayerCardUI = new List<PlayerCardSelectUI>();
	private GameObject PlayerPrefab;
	public TimerUI Timer;

	float timerDuration = 10.0f;
	float timerDecayRate = 1.0f;
	float timerNaturalDecayRate = 0.25f;
	float timerMinDuration = 5.0f;

	public void Start()
	{
		Instance = this;

		PlayerPrefab = Resources.Load("UI/PlayerCardSelectUI") as GameObject;

		for (int i = 0; i < PlayerCardUIParent.Count; ++i)
		{
			GameObject instance = GameObject.Instantiate(PlayerPrefab) as GameObject;
			RectTransform instanceTransform = instance.GetComponent<RectTransform>();
			instanceTransform.SetParent(PlayerCardUIParent[i]);
			instanceTransform.anchorMin = Vector2.zero;
			instanceTransform.anchorMax = Vector2.one;
			instanceTransform.offsetMin = Vector2.zero;
			instanceTransform.offsetMax = Vector2.zero;

			PlayerCardSelectUI cardUI = instance.GetComponent<PlayerCardSelectUI>();

			if (i < ActivePlayers.Instance.Players.Count)
			{
				cardUI.SetupForPlayer(ActivePlayers.Instance.Players[i], i + 1);
			}
			else
			{
				cardUI.gameObject.SetActive(false);
			}

			PlayerCardUI.Add( cardUI );
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
			foreach(Player player in ActivePlayers.Instance.Players)
			{
				if (player.SelectionLockedIn)
					player.ConsumeSelectedAction();
			}

			UIRoot.Instance.StartCoroutine( VictoryTransition() );


		}
	}

	public IEnumerator VictoryTransition()
	{
		UIRoot.Instance.LoadScreen("Victory");

		yield return new WaitForSeconds(2.0f);

		StateMachine.instance.SetState( new VictoryState() );
	}

	public void Update()
	{
		foreach(Player player in ActivePlayers.Instance.Players)
		{
			player.UpdateInput();
		}

		foreach(PlayerCardSelectUI cardUI in PlayerCardUI)
		{
			cardUI.TimeIsLow = Timer.TimeIsLow;
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
