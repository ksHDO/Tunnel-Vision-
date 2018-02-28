using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableManager : MonoBehaviour {
    [Header("Configuration")]
    [Header("External References")]
    // Note: These two must be the same size for this to work
    [SerializeField] private Collectable[] m_collectablePrefabList;
    [SerializeField] private float[] m_collectableWeights;
    [SerializeField] private float padding = 4.0f;

    [Header("Internal Values")]
    [SerializeField] private int m_maxCollectables;
    [SerializeField] private int m_maxFieldCollectables = 5;
    [SerializeField] private float m_spawnDelayStart = 1.0f;
    [SerializeField] private float m_spawnDelayEnd = 0.05f;
    [SerializeField] private float m_spawnDelayDelta = 1.0f;

    // Internal Information
    private Collectable[] m_collectables;
    private bool[] m_spawnPositionsOpen;
    private float[] m_collectableThresholds;
    private float m_spawnDelayCurrent;
    private float m_timer;
    private int m_collectableCount = 0;

    // Use this for initialization
    void Awake () {
        // Verify the weights and collectable prefabs match up
		if(m_collectablePrefabList.Length != m_collectableWeights.Length)
        {
            print("Collectable manager was given an inconsistent amount of collectables and weights");
        }

        // Create an array o thresholds
        m_collectableThresholds = new float[m_collectableWeights.Length];
        float total = m_collectableWeights[0];
        for (int x = 1; x < m_collectableWeights.Length; ++x)
        {
            total += m_collectableWeights[x];
        }
        
        // Fill out thresholds
        m_collectableThresholds[0] = m_collectableWeights[0] / total;
        for (int x = 1; x < m_collectableThresholds.Length; ++x)
        {
            m_collectableThresholds[x] = m_collectableThresholds[x - 1] + (m_collectableThresholds[x] / total);
        }

        m_collectables = new Collectable[m_maxCollectables];
        m_spawnPositionsOpen = new bool[m_maxCollectables];

        for (int j = 0; j < m_maxCollectables; ++j)
        {
            m_collectables[j] = Instantiate(GetRandomPrefab(), this.transform).GetComponent<Collectable>();
            m_collectables[j].transform.parent = GlobalInfo.CollectableContainer.transform;
            m_collectables[j].transform.localPosition = Vector3.zero;
            m_collectables[j].gameObject.SetActive(false);
        }

        m_spawnDelayCurrent = m_spawnDelayStart;
        m_timer = m_spawnDelayStart;
    }
    
    private GameObject GetRandomPrefab()
    {
        float roll = UnityEngine.Random.Range(0.0f, 1.0f);
        for (int x = 0; x < m_collectablePrefabList.Length; ++x)
        {
            if (roll <= m_collectableThresholds[x])
            {
                return m_collectablePrefabList[x].gameObject;
            }
        }

        return m_collectablePrefabList[m_collectablePrefabList.Length - 1].gameObject;
    }

    public void CollectAt(int idx)
    {
        --m_collectableCount;
        m_collectables[idx].transform.position = Vector3.zero;
        m_collectables[idx].gameObject.SetActive(false);
        m_spawnPositionsOpen[idx] = true;
    }


    // Update is called once per frame
    void Update ()
    {
        m_timer -= Time.deltaTime;


        if (m_timer <= 0.0f)
        {
            m_timer += m_spawnDelayCurrent;
            SpawnCollectable();
        }

        m_spawnDelayCurrent = Mathf.Lerp(m_spawnDelayCurrent, m_spawnDelayEnd, m_spawnDelayDelta * Time.deltaTime);
    }

    private void SpawnCollectable()
    {
        if (m_collectableCount < m_maxFieldCollectables)
        {
            ++m_collectableCount;

            int firstSlot = 0;
            for (int i = 0; i < m_maxCollectables; ++i)
            {
                if (m_spawnPositionsOpen[i]) { firstSlot = i; break; }
            }
            //something here
            m_spawnPositionsOpen[firstSlot] = false;
            Collectable toAdd = m_collectables[firstSlot];
            toAdd.gameObject.SetActive(true);
            toAdd.gameObject.transform.position = GetRandomSpawnPos();
        }
    }

    private Vector3 GetRandomSpawnPos()
    {
        Vector3 newPosition = Vector3.zero;
        newPosition.z = 1;

        float leftBound = GlobalInfo.UpperLeft.transform.position.x + padding;
        float rightBound = GlobalInfo.LowerRight.transform.position.x - padding;
        float upperBound = GlobalInfo.UpperLeft.transform.position.y - padding;
        float lowerBound = GlobalInfo.LowerRight.transform.position.y + padding;

        newPosition.x = UnityEngine.Random.Range(leftBound, rightBound);
        newPosition.y = UnityEngine.Random.Range(upperBound, lowerBound);

        return newPosition;
    }
}
