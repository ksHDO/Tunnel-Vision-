using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] protected ParticleSystem _system;
    [SerializeField] protected float _lifeTime = 5;
    [SerializeField] protected float _fadeTime = 1;

    protected SpriteRenderer _renderer;
    protected Transform _transform;


    void Awake()
    {
        _transform = transform;
        _renderer = GetComponent<SpriteRenderer>();
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
        gameObject.SetActive(false);
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //}
}
