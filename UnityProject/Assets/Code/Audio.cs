using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum eSound
{
	Music,
	FireAlarm,
	Scream
}

public class Audio : MonoBehaviour
{
	public static Audio Instance { get; private set; }

	public AudioClip Music;
	public AudioClip FireAlarm;
	public AudioClip Scream;

	public AudioSource Source;

	void Awake()
	{
		Instance = this;
	}

	AudioClip getClip( eSound sound )
	{
		switch( sound )
		{
		case eSound.FireAlarm:
			return FireAlarm;
		case eSound.Music:
			return Music;
		case eSound.Scream:
			return Scream;
		}

		return null;
	}

	public void PlayAudio( eSound sound )
	{
		var clip = getClip( sound );
		Source.PlayOneShot( clip );
	}

	public void Stop( eSound sound )
	{

	}
}
