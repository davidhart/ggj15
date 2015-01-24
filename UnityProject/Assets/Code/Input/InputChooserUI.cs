using UnityEngine;
using UnityEngine.UI;

public class InputChooserUI : MonoBehaviour
{
	public GameObject activeIcon;
	public Text deviceIDText;
	public InputDevice device;

	public void SetActiveForDevice(InputDevice device)
	{
		this.device = device;
		activeIcon.SetActive(true);
		deviceIDText.gameObject.SetActive(true);
		deviceIDText.text = device.deviceId.ToString();
	}

	public void SetInactive()
	{
		activeIcon.SetActive(false);
		device = null;
		deviceIDText.gameObject.SetActive(false);
	}
}