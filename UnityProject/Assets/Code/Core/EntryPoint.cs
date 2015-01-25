using UnityEngine;

public class EntryPoint : MonoBehaviour
{
	ActionQueue actionQueue;

	void Start()
	{
		Initialise();

		StateMachine.Instance.SetState(new SignInState());
	}

	void Initialise()
	{
		var fireManagerGO = new GameObject( "FireManager" );
		fireManagerGO.AddComponent< FireManager >();

		new ProgressManager();
	}

	void Update()
	{
		StateMachine.instance.Update();
	}
}
