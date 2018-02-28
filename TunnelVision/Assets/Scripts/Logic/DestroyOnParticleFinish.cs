using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DestroyOnParticleFinish : MonoBehaviour
{
    private ParticleSystem _system;
	// Use this for initialization
	void Start ()
	{
	    _system = GetComponent<ParticleSystem>();
        gameObject.transform.parent = GlobalInfo.ParticleContainer.transform;
    }
	
	// Update is called once per frame
	void Update () {
	    if (!_system.IsAlive())
	    {
            gameObject.SetActive(false);
            //Destroy(gameObject);
	    }
	}
}
