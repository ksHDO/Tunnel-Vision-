using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{

    [SerializeField] private float _radius;

    [SerializeField] private Rigidbody2D _player;
    [SerializeField] private PlayerScore _score;
    [SerializeField] private List<EnemyBehavior> _enemies;

    [SerializeField] private float _startTimeToGenerate;
    [SerializeField] private float _generateRand;
    [SerializeField] private float _timeReduceSpeed;
    [SerializeField] private int _numEnemiesToReduceSpawnTime;
    [SerializeField] private float _genMinCap = 0.1f;
    private int _numEnemiesCount;
    private float _currentGenTime;
    private float _currentTime;

    public bool EnableGeneration = true;

	// Use this for initialization
	void Start ()
	{
	    _currentTime = _startTimeToGenerate + Random.Range(-_generateRand, _generateRand);
	    _currentGenTime = _startTimeToGenerate;
	    _numEnemiesCount = _numEnemiesToReduceSpawnTime;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (EnableGeneration)
	    {
	        _currentTime -= Time.deltaTime;
	        if (_currentTime < 0)
	        {
	            if (--_numEnemiesCount < 0)
	            {
	                _currentGenTime -= _timeReduceSpeed;
	                _currentGenTime = Mathf.Max(_genMinCap, _currentGenTime);
	                _numEnemiesCount = _numEnemiesToReduceSpawnTime;

	            }
	            _currentTime = _currentGenTime + Random.Range(-_generateRand, _generateRand);
	            GameObject o = Instantiate(_enemies[Random.Range(0, _enemies.Count)].gameObject);
	            EnemyBehavior b = o.GetComponent<EnemyBehavior>();
	            b.Target = _player;
	            EnemyInfo e = o.GetComponent<EnemyInfo>();
	            e.PlayerScore = _score;



	            Vector2 pos = Random.insideUnitCircle.normalized * _radius;
	            o.transform.position = pos;
	        }
        }
	}

    public void AllowGeneration(bool val)
    {
        EnableGeneration = val;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(150, 150, 150);
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
