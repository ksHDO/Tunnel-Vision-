using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorTransition : MonoBehaviour
{
    [Range(0, 1)] public float Percent;
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;
    [SerializeField] private float _smoothTime;
    [SerializeField] private float _maxSpeed;
    private Graphic _graphic;

    private float hVel;
    private float sVel;
    private float vVel;
    private float aVel;

    private float _startH, _startS, _startV;
    private float _endH, _endS, _endV;

	// Use this for initialization
	void Start ()
	{
	    _graphic = GetComponent<Graphic>();
        Color.RGBToHSV(startColor, out _startH, out _startS, out _startV);
	    Color.RGBToHSV(endColor, out _endH, out _endS, out _endV);

	    float tH = Mathf.Lerp(_startH, _endH, Percent);
	    float tS = Mathf.Lerp(_startS, _endS, Percent);
	    float tV = Mathf.Lerp(_startV, _endV, Percent);
	    float tA = Mathf.Lerp(startColor.a, endColor.a, Percent);
	    Color startingColor = Color.HSVToRGB(tH, tS, tV);
	    startingColor.a = tA;
        _graphic.color = startingColor;

	    hVel = 0;
	    sVel = 0;
	    vVel = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    Color c = _graphic.color;
	    float h, s, v;
        Color.RGBToHSV(c, out h, out s, out v);

	    float tH = Mathf.Lerp(_startH, _endH, Percent);
	    float tS = Mathf.Lerp(_startS, _endS, Percent);
	    float tV = Mathf.Lerp(_startV, _endV, Percent);
	    float tA = Mathf.Lerp(startColor.a, endColor.a, Percent);

	    float outH = Mathf.SmoothDamp(h, tH, ref hVel, _smoothTime, _maxSpeed);
	    float outS = Mathf.SmoothDamp(s, tS, ref sVel, _smoothTime, _maxSpeed);
	    float outV = Mathf.SmoothDamp(v, tV, ref vVel, _smoothTime, _maxSpeed);
	    float outA = Mathf.SmoothDamp(c.a, tA, ref aVel, _smoothTime, _maxSpeed);
	    Color outColor = Color.HSVToRGB(outH, outS, outV);
	    outColor.a = outA;
	    _graphic.color = outColor;
	}

}
