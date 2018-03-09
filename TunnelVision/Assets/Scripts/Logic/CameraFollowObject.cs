using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{

    [SerializeField] private Transform _objectToFollow;
    public Transform ObjectToFollow {
        get { return _objectToFollow; }
        set { _objectToFollow = value; }
    }
    [SerializeField] private float _smoothTime = 1;
    [SerializeField] private float _maxSpeed = 1;
    [SerializeField] private Bounds _followBounds;
    private Transform _transform;
    private Vector3 _currentVelocity;
    
	// Use this for initialization
	void Start ()
	{
	    _transform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        if (_objectToFollow)
        {
            Vector3 dest = _objectToFollow.position;
            _followBounds.center = _transform.position;
            Vector3 newTransform = Vector3.SmoothDamp(_transform.position, dest, ref _currentVelocity, _smoothTime, _maxSpeed);
            newTransform.z = _transform.position.z;

            _transform.position = newTransform;
        }




    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(80, 80, 80);
        Gizmos.DrawWireCube(_followBounds.center, _followBounds.size);
    }
}
