using GameSparks.RT;
using sys = System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Constants;

public class EnemyGenerator : MonoBehaviour
{

    [SerializeField] private float radius;

    [SerializeField] public Rigidbody2D[] players;
    [SerializeField] private PlayerScore score;
    [SerializeField] private List<EnemyTypes> _enemies;
    public List<EnemyTypes> Enemies { get { return _enemies; } set { _enemies = value; } }
    [SerializeField] private float _startTimeToGenerate;
    [SerializeField] private float generateRand;
    [SerializeField] private float timeReduceSpeed;
    [SerializeField] private int numEnemiesDefeatedToReduceSpawnTime;
    [SerializeField] private float genMinCap = 0.1f;
    private int numEnemiesToKillCount;
    private float currentGenTime;
    private float currentTime;

    public bool EnableGeneration = true;
    private bool m_isHost = false;

    public static EnemyGenerator Instance;

	// Use this for initialization
	void Start ()
	{
	    currentTime = _startTimeToGenerate + Random.Range(-generateRand, generateRand);
	    currentGenTime = _startTimeToGenerate;
	    numEnemiesToKillCount = numEnemiesDefeatedToReduceSpawnTime;
	}

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void GenerateEnemy()
    {
        int enemyId = Random.Range(0, _enemies.Count);

        GenerateEnemy(enemyId);
    }

    public void GenerateEnemy(int id)
    {
        Vector2 pos = Random.insideUnitCircle.normalized * radius;
        GenerateEnemy(id, pos);
    }

    public void GenerateEnemy(int id, Vector2 pos)
    {
        int target = Random.Range(0, players.Length);

        EnemyBehavior behavior = GenerateEnemy(id, target, pos, 0, Vector2.zero);

        if (m_isHost) MultiplayerSpawnEnemy(id, behavior, target);
    }


    public EnemyBehavior GenerateEnemy(int id, int target, Vector2 position, float rotation, Vector2 velocity, bool forceGeneration = false)
    {
        if (!EnableGeneration && !forceGeneration)
        {
            return null;
        }
        GameObject enemy = Instantiate(EnemyList.Instance.Enemies[id].gameObject);

        EnemyBehavior behavior = enemy.GetComponent<EnemyBehavior>();
        behavior.Target = players[target];

        EnemyInfo enemyInfo = enemy.GetComponent<EnemyInfo>();
        enemyInfo.PlayerScore = score;

        enemy.transform.position = position;
        Quaternion rot = enemy.transform.rotation;
        Vector3 euler = rot.eulerAngles;
        euler.z = rotation;

        enemy.GetComponent<Rigidbody2D>().velocity = velocity;
        return behavior;
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

    #region Multiplayer
    private GameSparksManager gameSparksManager;
    public GameSparksManager GameSparksManager
    {
        get { return gameSparksManager; }
        set { gameSparksManager = value; }
    }
    public void SetupMultiplayer(bool isHost)
    {
        EnableGeneration = isHost;
        m_isHost = isHost;
        //if (isHost)
        //    StartCoroutine(UpdateEnemies());
    }

    private void MultiplayerSpawnEnemy(int id, EnemyBehavior behavior, int target)
    {
        if (behavior == null) return;
        Transform enemyTransform = behavior.transform;
        Rigidbody2D rigidbody = behavior.GetComponent<Rigidbody2D>();
        using (RTData data = RTData.Get())
        {
            Vector2 pos = enemyTransform.position;
            Signs posSign = SignsExt.GetSign(pos);
            Vector2 vel = rigidbody.velocity;
            Signs velSign = SignsExt.GetSign(vel);
            data.SetInt(1, id);
            data.SetInt(2, target);
            data.SetVector2(3, pos);
            data.SetInt(4, (int)posSign);
            data.SetFloat(5, enemyTransform.rotation.eulerAngles.z);
            data.SetVector2(6, vel);
            data.SetInt(7, (int)velSign);
            gameSparksManager.RTSession.SendData(
                MultiplayerCodes.ENEMY_SPAWN.Int(),
                GameSparksRT.DeliveryIntent.RELIABLE,
                data
                );
        }
    }

    private IEnumerator UpdateEnemies()
    {
        int enemyTypeCount = _enemies.Count;
        while (true)
        {


            EnemyBehavior[] allEnemies = FindObjectsOfType<EnemyBehavior>();
            using (RTData data = RTData.Get())
            {
                int length = allEnemies.Length;
                data.SetInt(1, length);
                for (int i = 0; i < length; ++i)
                {
                    EnemyBehavior enemy = allEnemies[i];
                    Transform enemyTransform = enemy.transform;
                    Rigidbody2D rigidbody = enemy.GetComponent<Rigidbody2D>();

                    Vector2 position = enemyTransform.position;
                    float rotation = enemyTransform.rotation.eulerAngles.z;
                    Vector2 velocity = rigidbody.velocity;

                    int enemyType = -1;
                    for (int j = 0; j < _enemies.Count; ++j)
                    {
                        sys.Type type = _enemies[j].GetType();
                        if (enemy.GetType().Equals(type))
                        {
                            enemyType = j;
                            break;
                        }
                    }
                    uint index = (uint)(i * 4);
                    data.SetInt(index + 1, enemyType);
                    data.SetVector2(index + 2, position);
                    data.SetFloat(index + 3, rotation);
                    data.SetVector2(index + 4, velocity);

                }
            }
            yield return null;
        }
    }

    #endregion
}
