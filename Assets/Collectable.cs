using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public float PointValue;

    [SerializeField] private ParticleSystem _system;
    [SerializeField] private float _lifeTime = 5;
    [SerializeField] private float _fadeTime = 1;

    private SpriteRenderer _renderer;
    private Transform _transform;
    private PlayerScore _score;

    void Start()
    {
        _transform = transform;
        _renderer = GetComponent<SpriteRenderer>();
        StartCoroutine(Stay(_lifeTime));
    }

    IEnumerator Stay(float time)
    {
        float currentTime = 0;
        while (currentTime < time)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(Fade(_fadeTime));
    }

    IEnumerator Fade(float time)
    {
        float currentTime = 0;
        while (currentTime < time)
        {
            Color c = _renderer.color;
            c.a = 1 - (currentTime / time);
            _renderer.color = c;
            currentTime += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _score = GameObject.FindGameObjectWithTag("PlayerManager").GetComponent<PlayerScore>();
            if (_score)
            {
                _score.AddScore(PointValue);
            }
            if (_system && _transform)
                Instantiate(_system).transform.position = _transform.position;
            Destroy(gameObject);
        }
    }
}
