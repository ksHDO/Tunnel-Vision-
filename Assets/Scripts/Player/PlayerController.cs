using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Turret))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 10;
    [SerializeField] private float _rotationSpeed = 5;
    [SerializeField] private float _fireSpeed = 1.0f;

    [Header("Camera")]
    [SerializeField] private float _zoomMax = 2f;
    [SerializeField] private float _zoomMin = 6f;
    [SerializeField] private float _timeToZoomOut = 1f;
    [SerializeField] private float _zoomInSpeed = 0.01f;
    [SerializeField] private float _zoomOutSpeed = 0.01f;
    [SerializeField] private float _smoothTime = 1f;
    [SerializeField] private float _maxSpeed = 1f;
    private float _zoomValue;
    private float _dtZoom;


    private float _fireCooldown;

    private bool _isWaitingForZoomOut;
    private float _zoomOutCooldown;

    private Transform _transform;
    private Rigidbody2D _rigidbody;
    private Camera _camera;
    private Turret _turret;
    private Soundbank _soundbank;

	// Use this for initialization
	void Start ()
	{
	    _transform = transform;
	    _rigidbody = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
	    _turret = GetComponent<Turret>();
	    _zoomValue = _camera.orthographicSize;
	    _soundbank = GetComponent<Soundbank>();

	    _isWaitingForZoomOut = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        HandleMovement();
        HandleMouse();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");



        Vector2 movement = new Vector2(horizontal, vertical) * _movementSpeed;

        _rigidbody.AddForce(movement);
    }

    void HandleMouse()
    {
        Vector2 mouse = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 pos = _transform.position;

        Vector2 dir = pos - mouse;
        Quaternion rotation = Quaternion.LookRotation(dir, Vector3.forward);
        rotation.x = 0;
        rotation.y = 0;
        Quaternion slerp = Quaternion.Slerp(_transform.rotation, rotation, Time.deltaTime * _rotationSpeed);

        _transform.rotation = slerp;

        Debug.DrawRay(_transform.position, -dir, Color.red);


        if (Input.GetButton("Fire1"))
        {
            if (_fireCooldown <= 0.0f)
            {
                _turret.Fire();
                _soundbank.Play();
                _fireCooldown = _fireSpeed;
                _zoomValue -= _zoomInSpeed;
                _zoomValue = Mathf.Max(_zoomValue, _zoomMax);
                _isWaitingForZoomOut = true;
                _zoomOutCooldown = _timeToZoomOut;
            }
            else
            {
                _fireCooldown -= Time.deltaTime;
            }
        }
        else
        {
            if (_isWaitingForZoomOut)
            {

                if (_zoomOutCooldown <= 0.0f)
                {
                    _isWaitingForZoomOut = false;
                }
                else
                {
                    _zoomOutCooldown -= Time.deltaTime;
                }
            }
            else
            {
                _zoomValue += _zoomOutSpeed;
                _zoomValue = Mathf.Min(_zoomValue, _zoomMin);
            }
        }

        _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, _zoomValue, ref _dtZoom, _smoothTime, _maxSpeed);
    }
}
