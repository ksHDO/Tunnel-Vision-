using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyInfo : MonoBehaviour
{

    public float HP = 10;
    public float ScoreValue = 100;
    public PlayerScore PlayerScore;
    // [SerializeField] private ParticleSystem _deathParticleSystem;


    private Transform _transform;
	// Use this for initialization
	void Start ()
	{
	    _transform = transform;

    }
	
	// Update is called once per frame
	void Update () {
	    if (HP < 0)
	    {
	        PlayerScore.AddScore(ScoreValue);

	        
            // OnDeath.Invoke();
            SendMessage("Destroy");
            Destroy(gameObject);
	    }
    }

}
