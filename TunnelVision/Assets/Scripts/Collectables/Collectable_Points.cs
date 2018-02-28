using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable_Points : Collectable {

    [SerializeField] public float m_pointValue { get; set; }
    protected PlayerScore _score;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _score = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerScore>();
            if (_score)
            {
                _score.AddScore(m_pointValue);
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
