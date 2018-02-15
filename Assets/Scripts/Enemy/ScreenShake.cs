using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    private Transform _transform;

    [SerializeField] public float ShakeAmount = 0.1f;
    [SerializeField] public float ShakeDuration = 0.2f;
	// Use this for initialization
	void Start ()
	{
	    _transform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Shake()
    {
        Shake(ShakeAmount, ShakeDuration);
    }

    public void Shake(float amount, float duration)
    {
        StartCoroutine(ShakeObject(amount, duration));
    }

    public void ShakeScreen(float amount, float duration)
    {
        Shake(amount, duration);
    }

    IEnumerator ShakeObject(float amount, float duration)
    {
        Vector3 originalPos = _transform.position;
        float currentTime = 0;
        while (currentTime < duration)
        {
            Vector3 newPos = originalPos + (Vector3) (Random.insideUnitCircle * amount);
            _transform.position = newPos;
            currentTime += Time.deltaTime;
            yield return null;
        }
        _transform.position = originalPos;
    }

}
