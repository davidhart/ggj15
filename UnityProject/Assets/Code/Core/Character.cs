using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
	public static Character Instance { get; private set; }

	public int Rotation { get; private set; }

	public int X { get { return Mathf.FloorToInt( gameObject.transform.position.x ); } }
	public int Z { get { return Mathf.FloorToInt( gameObject.transform.position.z ); } }

	void Awake()
	{
		Instance = this;
	}

	public void MoveForward()
	{
		gameObject.transform.position += gameObject.transform.rotation * new Vector3( 0.0f, 0.0f, 1.0f );
	}

	public void MoveBackward()
	{
		gameObject.transform.position -= gameObject.transform.rotation * new Vector3( 0.0f, 0.0f, 1.0f );
	}

	public void RotateLeft()
	{
		Rotation--;

		UpdateRotation();
	}

	public void RotateRight()
	{
		Rotation++;

		UpdateRotation();
	}

	void UpdateRotation()
	{
		if( Rotation < 0 )
			Rotation = 3;

		if( Rotation > 3 )
			Rotation = 0;

		gameObject.transform.rotation = Quaternion.AngleAxis( Rotation * 90.0f, Vector3.up );
	}

	void Update()
	{
		if( Input.GetKeyDown( KeyCode.A ) )
			ActionQueue.Instance.AddToQueue( new ActionRotateLeft() );

		if( Input.GetKeyDown( KeyCode.D ) )
			ActionQueue.Instance.AddToQueue( new ActionRotateRight() );

		if( Input.GetKeyDown( KeyCode.W ) )
			ActionQueue.Instance.AddToQueue( new ActionForward() );

		if( Input.GetKeyDown( KeyCode.S ) )
			ActionQueue.Instance.AddToQueue( new ActionBackward() );

		if( Input.GetKeyDown( KeyCode.Space ) )
			ActionQueue.Instance.Execute();
	}
}
