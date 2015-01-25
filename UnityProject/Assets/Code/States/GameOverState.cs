
using UnityEngine;

public class GameOverState : BaseState
{
	private float delay = 0;
	private float delayLockOut = 2.0f;

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
		delay += Time.deltaTime;

		if (delay < delayLockOut)
			return;

		if ( InputManager.activeDevice.GetButtonDown(ButtonType.Action1) )
		{
			ProgressManager.Instance.Reset();
		}
	}
}
