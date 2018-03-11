using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalInfo : MonoBehaviour {

    [SerializeField] private GameObject m_particleContainer;
    [SerializeField] private GameObject m_bulletContainer;
    [SerializeField] private GameObject m_collectableContainer;
    [SerializeField] private GameObject m_upperLeft;
    [SerializeField] private GameObject m_lowerRight;
    public static Rigidbody2D[] Players
    {
        get
        {
            return EnemyGenerator.Instance.players;
        }
    }
    public static GameObject ParticleContainer { get; private set; }
    public static GameObject BulletContainer { get; private set; }
    public static GameObject CollectableContainer { get; private set; }
    public static GameObject UpperLeft { get; private set; }
    public static GameObject LowerRight { get; private set; }


    // Use this for initialization
    void Awake () {
        ParticleContainer = m_particleContainer;
        BulletContainer = m_bulletContainer;
        CollectableContainer = m_collectableContainer;
        UpperLeft = m_upperLeft;
        LowerRight = m_lowerRight;
    }


    private void OnDestroy()
    {
        //EnemyGenerator = null;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
