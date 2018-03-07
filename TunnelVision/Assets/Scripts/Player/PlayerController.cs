using Assets.Scripts.Constants;
using GameSparks.RT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Turret))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(SpriteRenderer))]
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


    public UnityEvent OnFire;

	// Use this for initialization
	void Start ()
	{
	    _transform = transform;
	    _rigidbody = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
	    _turret = GetComponent<Turret>();
	    _zoomValue = _camera.orthographicSize;

        _turret.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;

	    _isWaitingForZoomOut = false;
	}

	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        if (IsPlayer)
        {
            HandleMovement();
            HandleMouse();
        }
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
                OnFire.Invoke();
                _turret.Fire();
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


    // Multiplayer
    // --------------
    #region Multiplayer

    [Header("Multiplayer")]
    [SerializeField] private bool _isPlayer = true;
    public float UpdateRate = 0.3f;
    public int PeerID;
    public bool IsPlayer
    {
        get { return _isPlayer; }
        set { _isPlayer = value; }
    }
    private GameSparksManager _gameSparksManager;
    public GameSparksManager GameSparks
    {
        get { return _gameSparksManager; }
        set { _gameSparksManager = value; }
    }

    public void SetupMultiplayer(Transform spawnPos, bool isPlayer)
    {
        IsPlayer = isPlayer;
        Transform selfTransform = transform;
        transform.position = spawnPos.position;
        if (isPlayer)
        {
            StartCoroutine(SendTransformUpdates());
            Turret turret = GetComponent<Turret>();
            OnFire.AddListener(SendTurretFireUpdate);
        }
    }

    IEnumerator SendTransformUpdates()
    {
        // To be safe
        if (!_transform)
            _transform = transform;
        if (!_rigidbody)
            _rigidbody = GetComponent<Rigidbody2D>();

        while (true)
        {
            using (RTData data = RTData.Get())
            {
                data.SetVector3(1, _transform.position);
                data.SetFloat(2, _transform.eulerAngles.z);
                data.SetVector2(3, _rigidbody.velocity);
                _gameSparksManager.RTSession.SendData(
                    MultiplayerCodes.PLAYER_POSITION.Int(),
                    GameSparksRT.DeliveryIntent.UNRELIABLE_SEQUENCED,
                    data
                    );
            }
            yield return new WaitForSeconds(UpdateRate);
        }
    }

    void SendTurretFireUpdate()
    {
        using (RTData data = RTData.Get())
        {
            data.SetInt(1, 0);
            _gameSparksManager.RTSession.SendData(
                MultiplayerCodes.PLAYER_BULLETS.Int(),
                GameSparksRT.DeliveryIntent.UNRELIABLE_SEQUENCED,
                data
                );
        }
    }

    #endregion
}
