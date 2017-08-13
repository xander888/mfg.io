using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeFXHelper : MonoBehaviour {

	private AudioSource source;

	void Start () 
	{
		source = GetComponent<AudioSource> ();
	}

	void Update () 
	{
		source.volume = StaticClass.fxLevel;
	}
}
