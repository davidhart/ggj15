using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TileEditorObject : MonoBehaviour
{
	void Start()
	{
		
	}

	void SnapToGrid()
	{
		Vector3 myPosition = gameObject.transform.localPosition;

		myPosition.y = 0.0f;

		gameObject.transform.localPosition
			= new Vector3( Mathf.Floor( myPosition.x ), Mathf.Floor( myPosition.y ), Mathf.Floor( myPosition.z ) );
	}
	
	void Update()
	{
		if( Application.isEditor )
			SnapToGrid();
	}
}
