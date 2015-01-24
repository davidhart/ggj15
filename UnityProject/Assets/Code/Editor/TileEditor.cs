using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(TileDefinition))]
public class TileEditor : Editor
{
	void OnSceneGUI()
	{
		Event e = Event.current;

		var lookup = new Dictionary< KeyCode, string >
		{
			{ KeyCode.Alpha1, "InsideFloor" },
			{ KeyCode.Alpha2, "InsideDoor" },
			{ KeyCode.Alpha3, "InsideWall" },
			{ KeyCode.Alpha4, "InsideWallCorner" },
			{ KeyCode.Alpha5, "InsideWallT" },
			{ KeyCode.Alpha6, "InsideWallQuad" }
		};

		switch (e.type)
		{
		case EventType.KeyDown:
			switch( e.keyCode )
			{
			case KeyCode.R:
				Selection.activeGameObject.transform.rotation *= Quaternion.AngleAxis( 90.0f, Vector3.up );
				Event.current.Use();
				break;
			case KeyCode.UpArrow:
				CreateNewInDirection( e, new Vector3( 0.0f, 0.0f, 1.0f ) );
				break;
			case KeyCode.DownArrow:
				CreateNewInDirection( e, new Vector3( 0.0f, 0.0f, -1.0f ) );
				break;
			case KeyCode.LeftArrow:
				CreateNewInDirection( e, new Vector3( -1.0f, 0.0f, 0.0f ) );
				break;
			case KeyCode.RightArrow:
				CreateNewInDirection( e, new Vector3( 1.0f, 0.0f, 0.0f ) );
				break;
			}
			break;
		}

		if( e.type == EventType.KeyDown )
		{
			string output;
			if( lookup.TryGetValue( e.keyCode, out output ) )
			{
				var old = Selection.activeGameObject;

				Create( output );

				GameObject.DestroyImmediate( old );

				e.Use ();
			}
		}
	}

	GameObject Create( string name )
	{
		var newGO = PrefabUtility.InstantiatePrefab( Resources.Load( "Tiles/" + name ) ) as GameObject;
		newGO.name = name;
		newGO.transform.parent = Selection.activeGameObject.transform.parent;
		newGO.transform.position = Selection.activeGameObject.transform.position;
		newGO.transform.rotation = Selection.activeGameObject.transform.rotation;
		
		Selection.activeGameObject = newGO;

		return newGO;
	}

	void CreateNewInDirection( Event e, Vector3 direction )
	{
		//	Duplicate in Direction.
		if( e.alt )
		{
			var name = Selection.activeGameObject.name;

			Create( name );

			Selection.activeGameObject.transform.position += direction;
		}
		//	Move Tile in Direction.
		else if( e.shift )
		{
			Selection.activeGameObject.transform.position += direction;
		}
		else
		{
			Vector3 targetLocation = Selection.activeGameObject.transform.position + direction;

			var allTiles = GameObject.FindObjectsOfType< TileDefinition >();
			float closestDistance = 0.0f;
			TileDefinition closest = null;
			foreach( var tile in allTiles )
			{
				float distanceToTile = ( targetLocation - tile.transform.position ).sqrMagnitude;
				if( closest == null || distanceToTile < closestDistance )
				{
					closestDistance = distanceToTile;
					closest = tile;
				}
			}

			Selection.activeGameObject = closest.gameObject;
		}

		Event.current.Use();
	}

	void OnGUI()
	{
		GUILayout.Label( "TILE EDITOR" );
	}
}
