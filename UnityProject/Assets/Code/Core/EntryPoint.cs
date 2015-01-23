using UnityEngine;
using System.Collections;

public class EntryPoint : MonoBehaviour
{
	void Start()
	{
		LoadLevel( "Level1" );
	}

	void LoadLevel( string level )
	{
		Application.LoadLevelAdditive( level );
	}
}
