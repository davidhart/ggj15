using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Camera : MonoBehaviour
{
	public Vector3 deltaFromPlayer;
	public float levelWeight;
	
	void Update()
	{
		if( Level.Instance == null )
			return;

		Vector3 levelMidPoint = new Vector3( Level.Instance.BoundsX, 0.0f, Level.Instance.BoundsZ ) * 0.5f;

		Vector3 midPoint = Vector3.Lerp( Character.Instance.transform.position, levelMidPoint, levelWeight );

		Vector3 targetPosition = midPoint + deltaFromPlayer;

		transform.position = targetPosition;
	}
}