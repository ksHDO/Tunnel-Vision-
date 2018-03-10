using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBarUi : MonoBehaviour
{

    [SerializeField] private PlayerHealth _health;
    public PlayerHealth Health
    {
        get { return _health; }
        set { _health = value; }
    }
    private ColorTransition _colorTransition;
    private ImageCutOff _cutOff;

	// Use this for initialization
	void Start ()
	{
	    _colorTransition = GetComponent<ColorTransition>();
	    _cutOff = GetComponent<ImageCutOff>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateHealth(float val)
    {
        float percent = (_health.HP / _health.MaxHealth);
        _colorTransition.Percent = 1 - percent;
        _cutOff.Percent = percent;

    }
}
