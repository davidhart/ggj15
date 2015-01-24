public class GameOverState : BaseState
{
	public override void Enter()
	{
		UIRoot.Instance.LoadScreen("GameOver");
	}

	public override void Exit()
	{
		UIRoot.Instance.DestroyScreen("GameOver");
	}

	public override void Update()
	{
		if ( InputManager.activeDevice.GetButtonDown(ButtonType.Action1) )
		{
			StateMachine.Instance.SetState(new SignInState());
		}
	}
}
