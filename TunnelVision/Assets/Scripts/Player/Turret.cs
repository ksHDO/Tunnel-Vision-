using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Turret : MonoBehaviour
{
    [Header("Configuration")]
    [Header("Projectile Information")]
    [SerializeField] private GameObject _bulletContainer;
    public GameObject BulletContainer {
        get { return _bulletContainer; }
        set { _bulletContainer = value; }
    }
    [SerializeField] private GameObject _particleContainer;
    public GameObject ParticleContainer
    {
        get { return _particleContainer; }
        set { _particleContainer = value; }
    }
    [SerializeField] private GameObject _bullet;
    [SerializeField] private int _maxBullets = 10;
    [SerializeField] private ParticleSystem _particle;

    [Header("Turret Properties")]
    [SerializeField] private float _speed;
    [SerializeField] private float _deviation;
    [SerializeField] private float _rotationSpeed = 10;
    [SerializeField] private float _fireOffset;

    // Pooling
    private int _bulletIndex;
    private GameObject[] _bullets;
    private ParticleSystem[] _particles;

    // Internal Information
    private Transform _transform;
    private Soundbank _soundbank;

    // Use this for initialization
    void Start ()
	{
        
	    _transform = transform;
        _soundbank = GetComponent<Soundbank>();
        
        // Pooling
        _bullets = new GameObject[_maxBullets];
        _particles = new ParticleSystem[_maxBullets];


        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

	    for (var i = 0; i < _bullets.Length; i++)
	    {
            // Instantiate item
	        _bullets[i] = Instantiate(_bullet);
            _particles[i] = Instantiate(_particle);
            _bullets[i].GetComponent<SpriteRenderer>().color = spriteRenderer.color;
            
            // Set information
            _bullets[i].SetActive(false);
            _particles[i].gameObject.SetActive(false);
            _bullets[i].transform.SetParent(_bulletContainer.transform);
            _particles[i].transform.SetParent(_particleContainer.transform);
	    }
	    _bulletIndex = -1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Fire()
    {
        Fire(_speed);
    }

    public void Fire(float speed)
    {
        // Grab object from pool
        _soundbank.Play();
        ++_bulletIndex;
        if(_bulletIndex >= _maxBullets)
        {
            _bulletIndex = 0;
        }
        GameObject bullet = _bullets[_bulletIndex];
        bullet.SetActive(true);

        // Bullet Logic
        Vector2 pos = _transform.position + _transform.up * _fireOffset;
        bullet.transform.position = pos;
        bullet.transform.up = _transform.up;
        Vector2 vel = _transform.up;
        float angle = Vector2.Angle(Vector2.zero, vel);
        angle += UnityEngine.Random.Range(-_deviation, _deviation);
        Quaternion r = Quaternion.AngleAxis(angle, _transform.forward);
        vel = ((Vector2) (r * _transform.right)).normalized * speed;
        Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();
        bulletRigid.velocity = vel;
        bulletRigid.angularVelocity = UnityEngine.Random.value * _rotationSpeed;
        
        _particles[_bulletIndex].transform.position = pos;
        _particles[_bulletIndex].gameObject.SetActive(true);
        _particles[_bulletIndex].Play();

        Debug.DrawRay(_transform.position, _transform.up, Color.blue, 1.0f);
    }
}
