using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivePlayers
{
	static ActivePlayers instance = new ActivePlayers();

	public static ActivePlayers Instance { get { return instance; } }

	List<Player> players = new List<Player>();

	public IEnumerable<Player> Players
	{
		get { return players; }
	}

	public void Add(InputDevice device)
	{
		players.Add(new Player(device));
	}

	public void Clear()
	{
		players.Clear();
	}
}
