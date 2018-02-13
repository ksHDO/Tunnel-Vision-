using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{

    public float Damage;


    [SerializeField] private float _shakeAmount;
    [SerializeField] private float _shakeDuration;

    private ScreenShake _screenShake;
    // Use this for initialization
    void Start ()
    {
        _screenShake = Camera.main.GetComponent<ScreenShake>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void Destroy()
    {
        if (_screenShake)
            _screenShake.Shake(_shakeAmount, _shakeDuration);
    }
}
