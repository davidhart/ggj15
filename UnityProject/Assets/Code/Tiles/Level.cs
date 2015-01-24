using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Level : MonoBehaviour
{
	const int maxSize = 128;

	public TileDefinition[,] tiles;

	public static Level Instance { get; private set; }

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
		}
	}

	public TileDefinition GetTileAtLocation( Vector3 worldPosition )
	{
		int X = Mathf.FloorToInt( worldPosition.x );
		int Z = Mathf.FloorToInt( worldPosition.z );

		return GetTileAtLocation( X, Z );
	}

	public TileDefinition GetTileAtLocation( int X, int Z )
	{
		return tiles[ X, Z ];
	}
}