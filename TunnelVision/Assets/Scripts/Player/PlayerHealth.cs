using Assets.Scripts.Constants;
using GameSparks.RT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] private float _hp;
    [SerializeField] private float _maxHp;
    public float HP {
        get { return _hp; } set { _hp = value; }
    }

    public float MaxHealth
    {
        get { return _maxHp; } private set { _maxHp = value; }
    }

    [SerializeField] public FloatEvent _onPlayerHpRemoved;
    [SerializeField] public FloatEvent _onPlayerHpAdded;
    [SerializeField] public FloatEvent _onPlayerHpModified;
    [SerializeField] public FloatEvent _onPlayerHpModifiedNewHealth;
    [SerializeField] public UnityEvent _onPlayerDeath;

    public void AddHp(float value)
    {
        if (value > 0)
            _onPlayerHpAdded.Invoke(value);
        else if (value < 0)
            _onPlayerHpRemoved.Invoke(value);

        HP += value;
        _onPlayerHpModified.Invoke(value);
        _onPlayerHpModifiedNewHealth.Invoke(HP);
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (_hp <= 0)
	    {
	        _onPlayerDeath.Invoke();
            
	    }
	}
}
