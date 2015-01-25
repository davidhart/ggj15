public class VictoryState : BaseState
{
	public override void Enter()
	{
		// Already loaded :D
		//UIRoot.Instance.LoadScreen("Victory");
	}
	
	public override void Exit()
	{
		UIRoot.Instance.DestroyScreen("Victory");
	}
	
	public override void Update()
	{
		if ( InputManager.activeDevice.GetButtonDown(ButtonType.Action1) )
		{
			//StateMachine.Instance.SetState(new SignInState());
			ProgressManager.Instance.NextLevel();
		}
	}
}
