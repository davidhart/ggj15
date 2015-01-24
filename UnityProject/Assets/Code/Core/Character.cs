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

	void MoveForward()
	{
		gameObject.transform.position += gameObject.transform.rotation * new Vector3( 0.0f, 0.0f, 1.0f );
	}

	void MoveBackward()
	{
		gameObject.transform.position -= gameObject.transform.rotation * new Vector3( 0.0f, 0.0f, 1.0f );
	}

	void RotateLeft()
	{
		Rotation++;

		UpdateRotation();
	}

	void RotateRight()
	{
		Rotation--;

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
			RotateLeft();

		if( Input.GetKeyDown( KeyCode.D ) )
			RotateRight();

		if( Input.GetKeyDown( KeyCode.W ) )
			MoveForward();

		if( Input.GetKeyDown( KeyCode.S ) )
			MoveBackward();
	}
}
