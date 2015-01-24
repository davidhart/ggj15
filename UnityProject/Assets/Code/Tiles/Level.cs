using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Level : MonoBehaviour
{
	public const int maxSize = 128;

	public TileDefinition[,] tiles;
	List<EndGame> endGames = new List<EndGame>();

	List<StartGame> startGames = new List<StartGame>();

	public static Level Instance { get; private set; }

	public int BoundsX { get; private set; }
	public int BoundsZ { get; private set; }

	void Awake()
	{
		Instance = this;

		var tileObjects = GameObject.FindObjectsOfType< TileDefinition >();

		Debug.Log( string.Format ( "{0} tiles found in level.", tileObjects.Length ) );

		tiles = new TileDefinition[ maxSize, maxSize ];

		foreach( var tileObject in tileObjects )
		{
			if( tiles[ tileObject.X, tileObject.Z ] != null )
				Debug.LogError( string.Format( "MULTIPLE TILES EXIST AT LOCATION {0},{1}", tileObject.X, tileObject.Z ) );

			tiles[ tileObject.X, tileObject.Z ] = tileObject;

			if( tileObject.X > BoundsX )
				BoundsX = tileObject.X;

			if( tileObject.Z > BoundsZ )
				BoundsZ = tileObject.Z;
		}

		BoundsX++;
		BoundsZ++;

		Debug.Log( string.Format ( "Level Bounds = X={0} Z={1}", BoundsX, BoundsZ ) );

		var endGameObjects = GameObject.FindObjectsOfType< EndGame >();
		foreach( var endGame in endGameObjects )
		{
			endGames.Add ( endGame );
		}

		var startGameObjects = GameObject.FindObjectsOfType< StartGame >();
		foreach( var startGame in startGameObjects )
		{
			startGames.Add ( startGame );
		}

		StartGame myGame = startGames[ Random.Range( 0, startGames.Count ) ];
		Character.Instance.gameObject.transform.position = myGame.transform.position;
		Character.Instance.gameObject.transform.rotation = myGame.transform.rotation;
		Character.Instance.Rotation = Mathf.FloorToInt( myGame.transform.rotation.eulerAngles.y + 0.5f ) / 90;

		FireManager.Instance.OnStartLevel();
	}
	
	public TileDefinition GetTileAtLocation( Vector3 worldPosition )
	{
		int X = Mathf.FloorToInt( worldPosition.x + 0.5f );
		int Z = Mathf.FloorToInt( worldPosition.z + 0.5f );

		return GetTileAtLocation( X, Z );
	}

	public TileDefinition GetTileAtLocation( int X, int Z )
	{
		return tiles[ X, Z ];
	}

	public bool ContainsFire( Vector3 worldPosition )
	{
		int X = Mathf.FloorToInt( worldPosition.x + 0.5f );
		int Z = Mathf.FloorToInt( worldPosition.z + 0.5f );

		return FireManager.Instance.ContainsFire( X, Z );
	}

	public bool ContainsEndGame( Vector3 worldPosition )
	{
		int X = Mathf.FloorToInt( worldPosition.x + 0.5f );
		int Z = Mathf.FloorToInt( worldPosition.z + 0.5f );

		foreach( var endGame in endGames )
		{
			if( endGame.X == X && endGame.Z == Z )
				return true;
		}

		return false;
	}

	public void UnloadLevel()
	{
		GameObject.Destroy(gameObject);

	}
}