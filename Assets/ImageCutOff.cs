using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCutOff : MonoBehaviour
{
    [Range(0, 1)]
    public float Percent;

    [SerializeField] private Image.FillMethod _fillMethod;
    [SerializeField] private int _fillOrigin;

    [SerializeField] private float _smoothTime;
    [SerializeField] private float _maxSpeed;
    private Image _graphic;

    private float _velPercent;
	// Use this for initialization
	void Start ()
	{
	    _graphic = GetComponent<Image>();
	    _graphic.fillAmount = Percent;

	}

    void Update()
    {
        _graphic.fillMethod = _fillMethod;
        _graphic.fillAmount = Mathf.SmoothDamp(_graphic.fillAmount, Percent, ref _velPercent, _smoothTime, _maxSpeed);
        _graphic.fillOrigin = _fillOrigin;
    }
	
}
