using UnityEngine;
using System.Collections;

public enum eTileType
{
	Floor,
	Wall
}

[ExecuteInEditMode]
public class TileDefinition : MonoBehaviour
{
	public eTileType Type;

	public int X { get { return Mathf.FloorToInt( gameObject.transform.position.x ); } }
	public int Z { get { return Mathf.FloorToInt( gameObject.transform.position.z ); } }

	void SnapToGrid()
	{
		Vector3 newPosition = new Vector3( X, 0.0f, Z );

		if( newPosition.x < 0 )
			newPosition.x = 0;

		if( newPosition.z < 0 )
			newPosition.z = 0;

		gameObject.transform.position = newPosition;
	}

	void Update()
	{
		if( !Application.isPlaying )
			SnapToGrid();
	}
}
