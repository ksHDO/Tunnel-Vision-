using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCarrier : EnemyBehavior
{

    [SerializeField] private EnemyTypes m_smallShip;
    [SerializeField] private float m_spawnWaveTime = 2.0f;
    [SerializeField] private float m_spawnWavePerShipInterval = .2f;
    [SerializeField] private int m_spawnWaveCount = 4;
    [SerializeField] private float m_spawnStartTime = 7.0f;
    [Space(20.0f)]
    [SerializeField] private float m_spawnForce = 20.0f;

    private int m_numToSpawn = 0;
    private float m_spawnTimer = 0.0f;
    private float m_spawnStartTimer = 0.0f;
    private float m_spawnIntervalTimer = 0.0f;
    private bool m_spawnOnRight = false;

    private bool m_spawningWave = false;

    private EnemyInfo m_eInfo;

    private void Awake()
    {
        m_eInfo = GetComponent<EnemyInfo>();
        m_numToSpawn = m_spawnWaveCount;
        m_spawnTimer = m_spawnWaveTime;
        m_spawnIntervalTimer = m_spawnWavePerShipInterval;
    }


    private void Spawn()
    {
        Vector2 pos = Random.insideUnitCircle.normalized * .02f + (Vector2)transform.position;
        EnemyGenerator.Instance.GenerateEnemy(m_smallShip.ToInt(), pos);
        // enemy.GetComponent<Rigidbody2D>().AddForce((enemy.transform.position - transform.position).normalized * m_spawnForce, ForceMode2D.Impulse);
    }

    private void DoTimerCycle()
    {
        if(m_spawnStartTimer <= 0.0f)
        {
            if (m_spawningWave)
            {
                m_spawnIntervalTimer -= Time.deltaTime;
                if(m_spawnIntervalTimer <= 0.0f)
                {
                    m_spawnIntervalTimer = m_spawnWavePerShipInterval;
                    Spawn();
                    m_numToSpawn--;
                    if(m_numToSpawn <= 0)
                    {
                        m_numToSpawn = m_spawnWaveCount;
                        m_spawningWave = false;
                    }
                }
            }
            else
            {
                m_spawnStartTimer -= Time.deltaTime;
                if(m_spawnStartTimer <= 0)
                {
                    m_spawningWave = true;
                    m_spawnStartTimer = m_spawnWaveTime;
                }
            }
        }
        else
        {
            m_spawnStartTimer -= Time.deltaTime;
        }


    }

    protected override void FixedUpdate()
    {
        if (Target != null && Rigidbody != null) {
            Vector2 vel = Seek(Target.position, MaxSpeed);
            Rigidbody.AddForce(vel);

            DoTimerCycle();
        }

        base.FixedUpdate();
    }

    Vector2 Seek(Vector2 target, float speed)
    {
        Vector2 desired = (target - Rigidbody.position).normalized * speed;
        return desired - Rigidbody.velocity;
    }

}
