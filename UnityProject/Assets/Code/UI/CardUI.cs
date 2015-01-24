using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CardUI : MonoBehaviour
{
	public Image BaseImage;
	public Image ImageDesaturated;
	public Animator Animator;
	public Text Text;

	public List<Sprite> Sprites;
	public List<Sprite> SpritesDesaturated;

	public void SetupForAction(ActionBase action)
	{
		Animator.SetBool("SelectionLockedIn", false);
		Animator.SetBool("Selected", false);

		int iconIndex = 0;
		string text = "??";

		if (action != null)
		{
			iconIndex = action.IconIndex();
			text = action.Name();
		}

		BaseImage.sprite = Sprites[iconIndex];
		ImageDesaturated.sprite = SpritesDesaturated[iconIndex];
		Text.text = text;
	}

	public void SetSelected(bool selected)
	{
		Animator.SetBool("Selected", selected);
	}

	public void SetSelectionLocked(bool locked)
	{
		Animator.SetBool("SelectionLockedIn", locked);
	}

	public void SetSelectionLockedOut(bool locked)
	{
		Animator.SetBool ("SelectionLockedOut", locked);
	}
}

