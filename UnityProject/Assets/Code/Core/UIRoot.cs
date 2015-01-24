using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class UIRoot : MonoBehaviour
{
	public Canvas uiCanvasRoot;
	
	private List<GameObject> loadedScreens = new List<GameObject>();

	public static UIRoot Instance;

	public void Awake()
	{
		Instance = this;
	}

	public void LoadScreen( string screenName )
	{
		GameObject screen = GameObject.Instantiate(Resources.Load( "UI/" + screenName )) as GameObject;
		screen.name = screenName;
		loadedScreens.Add(screen);
		
		RectTransform screenTransform = screen.GetComponent<RectTransform>();
		screenTransform.SetParent(uiCanvasRoot.transform, false);
	}
	
	public void DestroyScreen( string screenid )
	{
		GameObject screen = loadedScreens.First(q=> q.name == screenid);
		
		GameObject.Destroy(screen);
		
		loadedScreens.Remove(screen);
	}
}