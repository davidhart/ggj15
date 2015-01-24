using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NewFire
{
	public NewFire( int X, int Z )
	{
		this.X = X;
		this.Z = Z;
	}

	public int X;
	public int Z;
}

public class FireManager : MonoBehaviour
{
	public static FireManager Instance { get; private set; }

	public Fire[,] Fires;

	GameObject fireResource;

	List<GameObject> fireGameObjects = new List<GameObject>();

	void Start()
	{
		Instance = this;

		fireResource = Resources.Load( "Fire" ) as GameObject;
	}

	public void OnStartLevel()
	{
		Fires = new Fire[ Level.maxSize, Level.maxSize ];

		var startingFires = GameObject.FindObjectsOfType< Fire >();

		Debug.Log( string.Format( "Found {0} fires", startingFires.Length ) );
		foreach( var fire in startingFires )
		{
			int X = Mathf.FloorToInt( fire.transform.position.x + 0.5f );
			int Z = Mathf.FloorToInt( fire.transform.position.z + 0.5f );
			
			Fires[ X, Z ] = fire;
		}
	}

	void StartFireAtLocation( int X, int Z )
	{
		var newGO = GameObject.Instantiate( fireResource ) as GameObject;
		
		newGO.transform.parent = gameObject.transform;
		
		var fire = newGO.AddComponent< Fire >();
		fire.SetPosition( X, Z );
		
		Fires[ X, Z ] = fire;
	}

	void StartRandomFire()
	{
		int X = Mathf.FloorToInt( Random.value * Level.Instance.BoundsX );
		int Z = Mathf.FloorToInt( Random.value * Level.Instance.BoundsZ );

		StartFireAtLocation( X, Z );
	}

	void Update()
	{
		if( Input.GetKeyDown( KeyCode.P ) )
			OnStartLevel();

		if( Input.GetKeyDown( KeyCode.F ) )
		{
			StartRandomFire();
		}

		if( Input.GetKeyDown( KeyCode.G ) )
		{
			FireSpreads();
		}
	}

	public bool ContainsFire( int X, int Z )
	{
		if( Fires[ X, Z ] == null )
			return false;

		return true;
	}

	public void FireSpreads()
	{
		int maxX = Level.Instance.BoundsX;
		int maxZ = Level.Instance.BoundsZ;

		bool[,] newFires = new bool[ maxX, maxZ ];

		for( int X = 0; X < maxX; X++ )
		{
			for( int Z = 0; Z < maxZ; Z++ )
			{
				bool containsFire = FireManager.Instance.ContainsFire( X, Z );

				if( containsFire )
				{
					TryNewFire( ref newFires, maxX, maxZ, X-1, Z );
					TryNewFire( ref newFires, maxX, maxZ, X+1, Z );
					TryNewFire( ref newFires, maxX, maxZ, X, Z-1 );
					TryNewFire( ref newFires, maxX, maxZ, X, Z+1 );
				}
			}
		}

		for( int X = 0; X < maxX; X++ )
		{
			for( int Z = 0; Z < maxZ; Z++ )
			{
				if( newFires[ X, Z ] )
				{
					StartFireAtLocation( X, Z );
				}
			}
		}

		Character.Instance.IsInFire();
	}

	void TryNewFire( ref bool[,] newFires, int maxX, int maxZ, int X, int Z )
	{
		if( X < 0 || X >= maxX )
			return;

		if( Z < 0 || Z >= maxZ )
			return;

		if( newFires[ X, Z ] )
			return;

		if( FireManager.Instance.ContainsFire( X, Z ) )
			return;

		var tileDefinition = Level.Instance.GetTileAtLocation( X, Z );
		if( tileDefinition == null )
			return;

		if( !tileDefinition.IsWalkable() )
			return;


		newFires[ X, Z ] = true;
	}

	public void KillallFires()
	{
		int maxX = Level.Instance.BoundsX;
		int maxZ = Level.Instance.BoundsZ;

		for( int X = 0; X < maxX; X++ )
		{
			for( int Z = 0; Z < maxZ; Z++ )
			{
				if( Fires[ X, Z ] != null )
				{
					GameObject.Destroy(Fires[X, Z].gameObject);
					Fires[X, Z] = null;
				}
			}
		}
	}
}
