using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable_Recovery : Collectable {

    [SerializeField] private float m_recoverAmount;
    protected PlayerHealth _health;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _health = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerHealth>();
            if (_health)
            {
                _health.AddHp(m_recoverAmount);
            }

            if (_system && _transform)
            {
                // TODO:: Review how to instantiate this later
                ParticleSystem x = Instantiate(_system);
                x.transform.position = _transform.position;
                x.transform.parent = GlobalInfo.ParticleContainer.transform;
            }
            gameObject.SetActive(false);
        }
    }
}
