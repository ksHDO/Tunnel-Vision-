using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollide : MonoBehaviour
{

    private GameObject _parent;
    [SerializeField] private GameObject _particleSystem;

    void Start()
    {
        _parent = transform.parent.gameObject;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Transform t = Instantiate(_particleSystem).transform;
        t.position = transform.position;
        if (transform.parent != null)
        {
            SendMessageUpwards("Destroy");
        }

        if (_parent != null)
        {
            _parent.gameObject.SetActive(false);
        }

    }
}
