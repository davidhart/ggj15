using UnityEngine;
using System.Collections;

public class Thanks : MonoBehaviour
{

	void Awake()
	{
		if (ProgressManager.Instance.IsFinished() == false)
			gameObject.SetActive(false);
	}
}
