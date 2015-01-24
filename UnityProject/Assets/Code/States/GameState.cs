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
	}

	public override void Update()
	{
		ActionQueue.Instance.Update();
	}
}