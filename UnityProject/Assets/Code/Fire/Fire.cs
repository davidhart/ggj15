using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Fire : MonoBehaviour
{
	public void SetPosition( int X, int Z )
	{
		gameObject.transform.position = new Vector3( X, 0.0f, Z );
	}
}
