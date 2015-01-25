using UnityEngine;
using UnityEngine.UI;

public class InputChooserUI : MonoBehaviour
{
	public GameObject activeIcon;
	public GameObject inativeIcon;
	public Text deviceIDText;
	public InputDevice device;
	public Text playerText;

	public void SetActiveForDevice(InputDevice device)
	{
		this.device = device;
		activeIcon.SetActive(true);
		deviceIDText.text = device.deviceId.ToString();
		inativeIcon.SetActive(false);
	}

	public void SetInactive()
	{
		activeIcon.SetActive(false);
		device = null;
		deviceIDText.text = "";
		inativeIcon.SetActive(true);
	}

	public void SetPlayerIndex(int index)
	{
		playerText.text = "P" + index;
	}
}