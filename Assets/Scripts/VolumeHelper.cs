using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeHelper : MonoBehaviour {

	private AudioSource source;

	void Start () 
	{
		source = GetComponent<AudioSource> ();
	}

	void Update () 
	{
		source.volume = StaticClass.soundLevel;
	}
}
