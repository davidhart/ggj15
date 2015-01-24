using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
	public Text CommandName;
	public Animator Animator;

	public void SetupForAction(ActionBase action)
	{
		Animator.SetBool("SelectionLockedIn", false);
		Animator.SetBool("Selected", false);
		
		if (action == null)
		{
			CommandName.text = "???";

		}
		else
		{
			CommandName.text = action.Name();
		}
	}

	public void SetSelected(bool selected)
	{
		Animator.SetBool("Selected", selected);
	}

	public void SetSelectionLocked(bool locked)
	{
		Animator.SetBool("SelectionLockedIn", locked);
	}
}

