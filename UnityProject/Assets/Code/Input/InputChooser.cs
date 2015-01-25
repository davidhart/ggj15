using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InputChooser : MonoBehaviour
{
	public int deviceID;

	public List<int> availableDeviceIDs = new List<int>();
	public List<InputChooserUI> inputChooserUI = new List<InputChooserUI>();

	public GameObject readyToGoObject;

	void Start ()
	{
		ActivePlayers.Instance.Clear();

		foreach(InputChooserUI ui in inputChooserUI)
		{
			ui.SetInactive();
			ui.gameObject.SetActive(false);
		}
	}
	
	void Update ()
	{
		int connectedDevices = 0;

		foreach (InputDevice device in InputManager.inputDevices)
		{
			if (availableDeviceIDs.Contains(device.deviceId))
				connectedDevices++;
		}


		for (int i = 0; i < inputChooserUI.Count; ++i)
		{
			bool active = i < connectedDevices;

			inputChooserUI[i].gameObject.SetActive( active );
		}

	
		foreach(InputDevice device in InputManager.inputDevices)
		{
			if (device.GetButtonDown(ButtonType.Action1))
			{
				InputChooserUI alreadyActiveUI = inputChooserUI.FirstOrDefault(q=>q.device == device);

				if (alreadyActiveUI != null)
				{
					alreadyActiveUI.SetInactive();
				}
				else
				{
					InputChooserUI freeUI = inputChooserUI.FirstOrDefault(q=>q.device == null);

					if (freeUI != null)
					{
						freeUI.SetActiveForDevice(device);
					}
				}
			}
		}

		foreach(InputChooserUI ui in inputChooserUI)
		{
			if (ui.device != null && ui.device.GetButtonDown(ButtonType.Start))
			{
				StartTheGame();
			}
		}

		bool anyLockedIn = inputChooserUI.Any(q=>q.device != null);

		readyToGoObject.SetActive(anyLockedIn);
	}

	void StartTheGame()
	{
		foreach(InputChooserUI ui in inputChooserUI)
		{
			if (ui.device != null)
			{
				ActivePlayers.Instance.Add(ui.device);
			}
		}

		ProgressManager.Instance.NextLevel();
	}
}
