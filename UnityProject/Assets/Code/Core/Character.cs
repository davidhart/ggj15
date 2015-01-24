using UnityEngine;
using System.Collections;

public class CharacterAnimation : MonoBehaviour
{
	public enum eAnimationType
	{
		MoveForward,
		MoveBackward,
		RotateLeft,
		RotateRight,

		AttemptingToWalkOnNonWalkableTile
	}

	eAnimationType animationType;
	Character character;

	public Vector3 startPosition;
	public Vector3 endPosition;

	public Quaternion startRotation;
	public Quaternion endRotation;

	const float animationSpeed = 2.0f;

	float timer;

	public static void Setup( Character character, eAnimationType animationType, Vector3 targetPosition, Quaternion targetRotation )
	{
		var characterAnimation = character.gameObject.AddComponent< CharacterAnimation >();

		characterAnimation.character = character;
		characterAnimation.animationType = animationType;

		characterAnimation.startPosition = character.gameObject.transform.position;
		characterAnimation.endPosition = targetPosition;

		characterAnimation.startRotation = character.gameObject.transform.rotation;
		characterAnimation.endRotation = targetRotation;
	}

	void Update()
	{
		timer += Time.deltaTime * animationSpeed;

		timer = Mathf.Clamp( timer, 0.0f, 1.0f );

		character.transform.position = Vector3.Lerp( startPosition, endPosition, timer );
		character.transform.rotation = Quaternion.Lerp( startRotation, endRotation, timer );

		if( IsDone() )
		{
			GameObject.Destroy( this );
		}
	}

	bool IsDone()
	{
		if( timer >= 1.0f )
			return true;

		return false;
	}
}

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
		var targetPosition = gameObject.transform.position + ( gameObject.transform.rotation * new Vector3( 0.0f, 0.0f, 1.0f ) );

		var tileDefinition = Level.Instance.GetTileAtLocation( targetPosition );

		if( !tileDefinition.IsWalkable() )
		{
			//	CANT WALK HERE MATE!
			CharacterAnimation.Setup( this, CharacterAnimation.eAnimationType.AttemptingToWalkOnNonWalkableTile,
			                         gameObject.transform.position, gameObject.transform.rotation );

			return;
		}

		CharacterAnimation.Setup( this, CharacterAnimation.eAnimationType.MoveForward,
		                         targetPosition, gameObject.transform.rotation );
	}

	public void MoveBackward()
	{
		var targetPosition = gameObject.transform.position - ( gameObject.transform.rotation * new Vector3( 0.0f, 0.0f, 1.0f ) );

		CharacterAnimation.Setup( this, CharacterAnimation.eAnimationType.MoveBackward,
		                         targetPosition, gameObject.transform.rotation );
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

		var targetRotation = Quaternion.AngleAxis( Rotation * 90.0f, Vector3.up );
		
		CharacterAnimation.Setup( this, CharacterAnimation.eAnimationType.MoveBackward,
		                         gameObject.transform.position, targetRotation );
	}

	public bool IsAnimating()
	{
		if( gameObject.GetComponent< CharacterAnimation >() == null )
			return false;

		return true;
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
	}
}
