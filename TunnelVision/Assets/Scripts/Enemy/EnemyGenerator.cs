using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{

    [SerializeField] private float radius;

    [SerializeField] private Rigidbody2D player;
    [SerializeField] private PlayerScore score;
    [SerializeField] private List<EnemyBehavior> _enemies;

    [SerializeField] private float _startTimeToGenerate;
    [SerializeField] private float generateRand;
    [SerializeField] private float timeReduceSpeed;
    [SerializeField] private int numEnemiesDefeatedToReduceSpawnTime;
    [SerializeField] private float genMinCap = 0.1f;
    private int numEnemiesToKillCount;
    private float currentGenTime;
    private float currentTime;

    public bool EnableGeneration = true;

	// Use this for initialization
	void Start ()
	{
	    currentTime = _startTimeToGenerate + Random.Range(-generateRand, generateRand);
	    currentGenTime = _startTimeToGenerate;
	    numEnemiesToKillCount = numEnemiesDefeatedToReduceSpawnTime;
	}
	
    void GenerateEnemy()
    {
        GameObject enemy = Instantiate(_enemies[Random.Range(0, _enemies.Count)].gameObject);

        EnemyBehavior behavior = enemy.GetComponent<EnemyBehavior>();
        behavior.Target = player;

        EnemyInfo enemyInfo = enemy.GetComponent<EnemyInfo>();
        enemyInfo.PlayerScore = score;

        Vector2 pos = Random.insideUnitCircle.normalized * radius;
        enemy.transform.position = pos;
    }

    void DoSpawnLogic()
    {
        //--numEnemiesCount;

        if (numEnemiesToKillCount <= 0)
        {
            currentGenTime -= timeReduceSpeed;
            currentGenTime = Mathf.Max(genMinCap, currentGenTime);
            numEnemiesToKillCount = numEnemiesDefeatedToReduceSpawnTime;
        }
        currentTime = currentGenTime + Random.Range(-generateRand, generateRand);
    }


    public void EnemyKilled()
    {
        --numEnemiesToKillCount;
    }

	// Update is called once per frame
	void Update ()
	{
	    if (EnableGeneration)
	    {
	        currentTime -= Time.deltaTime;
	        if (currentTime < 0)
	        {
                DoSpawnLogic();
                GenerateEnemy();
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
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
