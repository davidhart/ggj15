using UnityEngine;
using System.Collections;

public class GameState : BaseState
{
	private string level;

	private bool isExiting = false;

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

		if (isExiting == false)
		{	
			if (Character.Instance.IsInFire())
			{
				UIRoot.Instance.StartCoroutine(GameOverCoroutine());
				isExiting = true;
			}
		}
	}

	public IEnumerator GameOverCoroutine()
	{
		FireManager.Instance.StartGameOverFire();

		yield return new WaitForSeconds(1.0f);

		UIRoot.Instance.LoadScreen("GameOver");

		yield return new WaitForSeconds(2.0f);

		StateMachine.Instance.SetState(new GameOverState());
	}


}