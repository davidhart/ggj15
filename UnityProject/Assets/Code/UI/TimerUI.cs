using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerUI : MonoBehaviour {

	public Text TimerText;
	public Image TimerImage;
	public Animator Animator;

	float TimerLength = 0.0f;
	float TimerCurrent = 0.0f;

	float TimeLowFraction = 0.5f;

	public System.Action OnTimerCountdown;

	bool lockOutTimerCountdown = false;
	public bool timerEnabled = true;

	public float Ratio()
	{
		return 1.0f - ( TimerCurrent / TimerLength );
	}

	public void Start()
	{
		TimerCurrent = TimerLength;

		UpdateUI();
	}

	int lastSecond = 0;
	float lastPlay = 0.0f;

	public void Update()
	{
		if( !timerEnabled )
			return;

		TimerCurrent -= Time.deltaTime;

		if (Mathf.FloorToInt(TimerCurrent + 1.0f) <= 0 && lockOutTimerCountdown == false)
		{
			OnTimerCountdown();
			lockOutTimerCountdown = true;
		}

		lastPlay += Time.deltaTime;
		if( TimeIsLow )
		{

			if( lastPlay > 1.0f )
			//if( lastSecond != Mathf.FloorToInt( TimerCurrent ) )
			{
				//lastSecond = Mathf.FloorToInt( TimerCurrent );
				lastPlay -= 1.0f;
				Audio.Instance.Start( eSound.FireAlarm );
			}
		}


		UpdateUI();
	}

	public void UpdateUI()
	{	
		TimerText.text = Mathf.FloorToInt(Mathf.Max(0.0f, TimerCurrent + 1.0f)).ToString();
		TimerImage.fillAmount = TimerCurrent / TimerLength;

		Animator.SetBool("TimeLow", TimeIsLow);
	}

	public void ResetTimer(float duration)
	{
		TimerLength = duration;
		TimerCurrent = duration;

		lockOutTimerCountdown = false;

		UpdateUI();
	}

	public bool TimeIsLow
	{
		get
		{
			return TimerCurrent / TimerLength < TimeLowFraction;
		}

	}
}
