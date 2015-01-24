
public class StateMachine
{
	public static StateMachine instance = new StateMachine();

	public static StateMachine Instance
	{
		get { return instance; }
	}

	private BaseState currentState;

	public void SetState(BaseState state)
	{
		if (currentState != null)
		{
			currentState.Exit();
		}

		state.Enter();
		currentState = state;
	}

	public void Update()
	{
		if (currentState != null)
			currentState.Update();
	}
}

