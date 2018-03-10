using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayerCollide : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SendMessageUpwards("Destroy");
            Destroy(transform.parent.gameObject);
        }
    }
}
