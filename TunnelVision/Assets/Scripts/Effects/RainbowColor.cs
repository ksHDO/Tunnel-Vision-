using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RainbowColor : MonoBehaviour {

    [SerializeField] private float _rainbowSpeed;
    [SerializeField] private float _rainbowFrequency;
    [SerializeField] private float _smoothTime = 0.1f;
    [SerializeField] private float _maxSpeed = 300;

    private Graphic _graphic;

    private float _time;

    private float _rVel;
    private float _gVel;
    private float _bVel;

	// Use this for initialization
	void Start ()
	{
	    _graphic = GetComponent<Graphic>();
	    _time = 0;

	    _rVel = 0;
	    _gVel = 0;
	    _bVel = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    _time += Time.deltaTime * _rainbowSpeed;

	    Color cur = _graphic.color;

	    float h, s, v;
        Color.RGBToHSV(cur, out h, out s, out v);
	    h = _time;
	    if (_time >= 1)
	        _time = 0;

	    Color dest = Color.HSVToRGB(h, s, v);
	    dest.a = cur.a;
        _graphic.color = dest;
	}
}
