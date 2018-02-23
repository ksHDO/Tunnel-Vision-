using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalInfo : MonoBehaviour {


    [SerializeField] private EnemyGenerator enemyGenerator;
    [SerializeField] private GameObject m_particleContainer;
    [SerializeField] private GameObject m_bulletContainer;
    [SerializeField] private GameObject m_collectableContainer;
    public static EnemyGenerator EnemyGenerator { get; private set; }
    public static GameObject ParticleContainer { get; private set; }
    public static GameObject BulletContainer { get; private set; }
    public static GameObject CollectableContainer { get; private set; }
    

    // Use this for initialization
    void Awake () {
        EnemyGenerator = enemyGenerator;
        ParticleContainer = m_particleContainer;
        BulletContainer = m_bulletContainer;
        CollectableContainer = m_collectableContainer;
    }


    private void OnDestroy()
    {
        EnemyGenerator = null;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
