using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class Fire : MonoBehaviour
{
	public int X { get { return Mathf.FloorToInt( gameObject.transform.position.x + 0.5f ); } }
	public int Z { get { return Mathf.FloorToInt( gameObject.transform.position.z + 0.5f ); } }


	public void SetPosition( int X, int Z )
	{
		gameObject.transform.position = new Vector3( X, 0.0f, Z );
	}

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
		{
			SnapToGrid();
		}
	}
}
