
public class SignInState : BaseState
{
	public override void Enter()
	{
		UIRoot.Instance.LoadScreen( "PlayerConnect" );
	}

	public override void Exit()
	{
		UIRoot.Instance.DestroyScreen( "PlayerConnect" );
	}

	public override void Update()
	{

	}
}

