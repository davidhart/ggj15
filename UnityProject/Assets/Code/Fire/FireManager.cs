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

	List<Fire> fireObjects = new List<Fire>();

	public FireAnimations fireAnimations;

	private bool isInGameOverAnim = true;
	private float gameOverAnimTimer = 0.0f;
	private float gameOverAnimRate = 0.5f;

	bool debugEnabled = false;

	void Start()
	{
		Instance = this;

		fireResource = Resources.Load( "Fire" ) as GameObject;

		fireAnimations = GameObject.Find( "FireAnimations" ).GetComponent< FireAnimations >();
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

			fireObjects.Add( fire );
		}
	}

	void StartFireAtLocation( int X, int Z )
	{
		var newGO = GameObject.Instantiate( fireResource ) as GameObject;

		newGO.transform.parent = gameObject.transform;
		
		var fire = newGO.GetComponent< Fire >();
		fire.SetPosition( X, Z );

		Fires[ X, Z ] = fire;

		fireObjects.Add( fire );
	}

	void StartRandomFire()
	{
		int X = Mathf.FloorToInt( Random.value * Level.Instance.BoundsX );
		int Z = Mathf.FloorToInt( Random.value * Level.Instance.BoundsZ );

		StartFireAtLocation( X, Z );
	}

	void Update()
	{
		if( debugEnabled )
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

		if( IngameController.Instance == null )
			return;



		if (isInGameOverAnim)
		{
			gameOverAnimTimer += Time.deltaTime * gameOverAnimRate;
		}

		float gameOverRatio = gameOverAnimTimer;

		int index = 0;
		foreach( var fireLight in fireObjects )
		{
			float ratio = IngameController.Instance.Timer.Ratio();
			float multiplier = fireAnimations.TimerIntensity.Evaluate( ratio ) + 0.3f;

			multiplier += fireAnimations.GameOverIntensity.Evaluate( gameOverRatio );

			fireLight.FireLight.intensity = ( ( ( Mathf.PerlinNoise( index, Time.timeSinceLevelLoad * 3.0f ) * 0.3f ) ) + 0.2f ) * multiplier;

			index++;
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

		fireObjects.Clear();

		isInGameOverAnim = false;
		gameOverAnimTimer = 0.0f;
	}

	public void StartGameOverFire()
	{
		isInGameOverAnim = true;
		gameOverAnimTimer = 0.0f;
	}

	public void WaterAction( Vector3 worldPosition, Quaternion direction )
	{
		int X = Mathf.FloorToInt( worldPosition.x + 0.5f );
		int Z = Mathf.FloorToInt( worldPosition.z + 0.5f );

		int maxX = Level.Instance.BoundsX;
		int maxZ = Level.Instance.BoundsZ;

		for( int depth = 0; depth < 5; depth++ )
		{
			Vector3 dir = ( (float)depth * ( direction * new Vector3( 0.0f, 0.0f, 1.0f ) ) );

			for( int width = -depth - 1; width <= depth + 1; width++ )
			{
				int dirX = Mathf.FloorToInt( dir.x + 0.5f );
				int dirZ = Mathf.FloorToInt( dir.z + 0.5f );

				int newX = X;
				int newZ = Z;
				if( dirX == 0 )
				{
					newX += width;
					newZ += dirZ;
				}
				else
				{
					newX += dirX;
					newZ += width;
				}

				if( newX < 0 || newX >= maxX )
					continue;
				if( newZ < 0 || newZ >= maxZ )
					continue;
				if( Fires[ newX, newZ ] == null )
					continue;

				Debug.Log ( string.Format( "water @ {0} {1}", newX, newZ ) );

				fireObjects.Remove( Fires[ newX, newZ ] );
				GameObject.Destroy( Fires[ newX, newZ ].gameObject );
				Fires[ newX, newZ ] = null;
			}
		}

		Debug.Log( "Fires remaining: " + fireObjects.Count );
	}
}
