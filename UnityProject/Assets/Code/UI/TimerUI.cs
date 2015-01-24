using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerUI : MonoBehaviour {

	public Text TimerText;
	public Image TimerImage;

	float TimerLength = 0.0f;
	float TimerCurrent = 0.0f;

	public System.Action OnTimerCountdown;

	public void Start()
	{
		TimerCurrent = TimerLength;

		UpdateUI();
	}

	public void Update()
	{
		TimerCurrent -= Time.deltaTime;

		if (Mathf.FloorToInt(TimerCurrent + 1.0f) <= 0)
		{
			OnTimerCountdown();

			return;
		}

		UpdateUI();
	}

	public void UpdateUI()
	{	
		TimerText.text = Mathf.FloorToInt(TimerCurrent + 1.0f).ToString();
		TimerImage.fillAmount = TimerCurrent / TimerLength;
	}

	public void ResetTimer(float duration)
	{
		TimerLength = duration;
		TimerCurrent = duration;

		UpdateUI();
	}
}
