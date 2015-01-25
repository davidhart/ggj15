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

	public int Rotation { get; set; }

	public int X { get { return Mathf.FloorToInt( gameObject.transform.position.x + 0.5f ); } }
	public int Z { get { return Mathf.FloorToInt( gameObject.transform.position.z + 0.5f ); } }

	const bool enableDebug = false;

	void Awake()
	{
		Instance = this;
	}

	public void RemoveAnimation()
	{
		var comp = GetComponent<CharacterAnimation>();

		if( comp == null )
			return;

		GameObject.DestroyImmediate( comp );
	}

	public void MoveForward()
	{
		var targetPosition = gameObject.transform.position + ( gameObject.transform.rotation * new Vector3( 0.0f, 0.0f, 1.0f ) );

		if( !PreMoveChecks( targetPosition ) )
			return;

		CharacterAnimation.Setup( this, CharacterAnimation.eAnimationType.MoveForward,
		                         targetPosition, gameObject.transform.rotation );
	}

	public void MoveBackward()
	{
		var targetPosition = gameObject.transform.position - ( gameObject.transform.rotation * new Vector3( 0.0f, 0.0f, 1.0f ) );

		if( !PreMoveChecks( targetPosition ) )
			return;

		CharacterAnimation.Setup( this, CharacterAnimation.eAnimationType.MoveBackward,
		                         targetPosition, gameObject.transform.rotation );
	}

	public bool PreMoveChecks( Vector3 targetPosition )
	{
		if( targetPosition.x < 0.0f )
			return false;

		if( targetPosition.z < 0.0f )
			return false;

		var tileDefinition = Level.Instance.GetTileAtLocation( targetPosition );

		if( tileDefinition == null || !tileDefinition.IsWalkable() )
		{
			//	CANT WALK HERE MATE!
			CharacterAnimation.Setup( this, CharacterAnimation.eAnimationType.AttemptingToWalkOnNonWalkableTile,
			                         gameObject.transform.position, gameObject.transform.rotation );
			
			return false;
		}

		if( Level.Instance.ContainsFire( targetPosition ) )
		{
			Debug.LogError( "Mmmmm Toasted Player, Lovely!" );
			return false;
		}

		return true;
	}

	public bool IsInFire()
	{
		if( Level.Instance.ContainsFire( gameObject.transform.position ) )
		{
			Debug.LogError( "Mmmmm Toasted Player, Lovely!" );
			return true;
		}

		return false;
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
		if( !enableDebug )
			return;

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
