using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnEnemyEnter : MonoBehaviour
{
    // [SerializeField] 
    private PlayerHealth _hp;

    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private UnityEvent _onEnemyCollision;
    [SerializeField] private ScreenShake _screenShake;
    [SerializeField] private float amount, duration;
    private Transform _transform;

    [System.Serializable]
    private class EnemyCollisionEvent : UnityEvent<float, float>
    {
    }

    // Use this for initialization
	void Start ()
	{
	    _hp = GetComponentInParent<PlayerHealth>();
	    _transform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            _onEnemyCollision.Invoke();
            if (_screenShake)
                _screenShake.Shake(amount, duration);
            _hp.AddHp(-1);
            Instantiate(_particleSystem).transform.position = _transform.position;
        }
    }
}
