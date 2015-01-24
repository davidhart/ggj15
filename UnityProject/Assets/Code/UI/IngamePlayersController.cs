using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class IngamePlayersController : MonoBehaviour
{
	public List<PlayerCardSelectUI> PlayerCardUI = new List<PlayerCardSelectUI>();

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
		}
	}
}
