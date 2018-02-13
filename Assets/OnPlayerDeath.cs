using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayerDeath : MonoBehaviour
{

    [SerializeField] private ParticleSystem _particleSystem;


    private Transform _transform;
	// Use this for initialization
	void Start ()
	{
	    _transform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayerDeath()
    {
        Instantiate(_particleSystem).transform.position = _transform.position;
        Destroy(gameObject);
    }
}
