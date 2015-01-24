using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EntryPoint : MonoBehaviour
{
	public Canvas uiCanvasRoot;

	private List<GameObject> loadedScreens = new List<GameObject>();

	ActionQueue actionQueue;

	void Start()
	{
		//LoadScreen( "PlayerConnect" );
		LoadLevel( "Level1" );

		Initialise();
	}

	void LoadLevel( string level )
	{
		Application.LoadLevelAdditive( level );
	}

	void LoadScreen( string screenName )
	{
		GameObject screen = GameObject.Instantiate(Resources.Load( "UI/" + screenName )) as GameObject;
		screen.name = screenName;
		loadedScreens.Add(screen);

		RectTransform screenTransform = screen.GetComponent<RectTransform>();
		screenTransform.SetParent(uiCanvasRoot.transform, false);
	}

	void DestroyScreen( string screenid )
	{
		GameObject screen = loadedScreens.First(q=> q.name == screenid);

		GameObject.Destroy(screen);

		loadedScreens.Remove(screen);
	}

	void Initialise()
	{
		actionQueue = new ActionQueue();

		var fireManagerGO = new GameObject( "FireManager" );
		fireManagerGO.AddComponent< FireManager >();
	}

	void Update()
	{
		actionQueue.Update();
	}
}
