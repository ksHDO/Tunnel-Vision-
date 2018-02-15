using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitEnemy : MonoBehaviour
{

    private PlayerBullet _bullet;

    [SerializeField] private ParticleSystem _particleSystem;

	// Use this for initialization
	void Start ()
	{
	    _bullet = GetComponentInParent<PlayerBullet>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!_bullet)
            _bullet = GetComponentInParent<PlayerBullet>();
        GameObject obj = other.gameObject;
        if (obj.CompareTag("Enemy"))
        {
            EnemyInfo enemy = obj.GetComponent<EnemyInfo>();
            if (enemy)
            {
                Instantiate(_particleSystem).transform.position = transform.position;
                enemy.HP -= _bullet.Damage;
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
