using UnityEngine;
using System.Collections;

public class Thanks : MonoBehaviour
{

	void Awake()
	{
		if (ProgressManager.Instance.IsOnLastLevel() == false)
			gameObject.SetActive(false);
	}
}
