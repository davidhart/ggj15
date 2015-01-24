using UnityEngine;

public class GameState : BaseState
{
	private string level;

	public GameState(string level)
	{
		this.level = level;
	}

	public override void Enter()
	{
		Application.LoadLevelAdditive( level );

		UIRoot.Instance.LoadScreen("InGame");
	}

	public override void Exit()
	{
		UIRoot.Instance.DestroyScreen("InGame");

		Level.Instance.UnloadLevel();

		FireManager.Instance.KillallFires();
	}

	public override void Update()
	{
		ActionQueue.Instance.Update();

		if (Character.Instance.IsInFire())
		{
			StateMachine.Instance.SetState(new GameOverState());
		}
	}
}