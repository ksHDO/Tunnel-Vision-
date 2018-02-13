using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Soundbank : MonoBehaviour
{
    private AudioSource _audio;

    [SerializeField] private List<AudioClip> _clips;

    [SerializeField] private bool _playOnWake;
	// Use this for initialization
	void Start ()
	{
	    _audio = GetComponent<AudioSource>();
	    if (_playOnWake)
	        Play();
	}


    public void Play()
    {
        _audio.clip = _clips[Random.Range(0, _clips.Count)];
        _audio.Play();
    }
}
