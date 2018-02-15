using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    [SerializeField] private GameObject _bullet;
    [SerializeField] private float _speed;
    [SerializeField] private float _deviation;
    [SerializeField] private float _rotationSpeed = 10;

    [SerializeField] private float _fireOffset;
    [SerializeField] private ParticleSystem _particle;

    //[SerializeField] private int _maxBullets;
    //private int _bulletIndex;

    private Transform _transform;
    //private GameObject[] _bullets;

	// Use this for initialization
	void Start ()
	{
	    _transform = transform;
     //   _bullets = new GameObject[_maxBullets];

	    //for (var i = 0; i < _bullets.Length; i++)
	    //{
	    //    _bullets[i] = Instantiate(_bullet);
     //       _bullets[i].SetActive(false);
	    //}
	    //_bulletIndex = 0;
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
        GameObject bullet = Instantiate(_bullet);
        Vector2 pos = _transform.position + _transform.up * _fireOffset;
        bullet.transform.position = pos;
        bullet.transform.up = _transform.up;
        Vector2 vel = _transform.up;
        float angle = Vector2.Angle(Vector2.zero, vel);
        angle += Random.Range(-_deviation, _deviation);
        Quaternion r = Quaternion.AngleAxis(angle, _transform.forward);
        vel = ((Vector2) (r * _transform.right)).normalized * speed;
        Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();
        bulletRigid.velocity = vel;
        bulletRigid.angularVelocity = Random.value * _rotationSpeed;

        Instantiate(_particle).transform.position = pos;

        Debug.DrawRay(_transform.position, _transform.up, Color.blue, 1.0f);
    }
}
