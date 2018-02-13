using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    private Transform _transform;

    private Camera _camera;
	// Use this for initialization
	void Start ()
	{
	    _transform = transform;
	    _camera = Camera.main;
	    
	}
	
	// Update is called once per frame
	void Update ()
	{
	    Vector2 mousePos = Input.mousePosition;
	    _transform.position = _camera.ScreenToWorldPoint(mousePos);
	}
}
