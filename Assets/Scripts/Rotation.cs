using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{

    [SerializeField] private float Speed;

    private Transform _transform;

	// Use this for initialization
	void Start ()
	{
	    _transform = transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		_transform.Rotate(0, 0, Speed);
	}
}
