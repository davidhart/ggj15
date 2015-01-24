using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Level : MonoBehaviour
{
	public const int maxSize = 128;

	public TileDefinition[,] tiles;

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
}