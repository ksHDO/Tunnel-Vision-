using Assets.Scripts.Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnDeath : MonoBehaviour
{

    [SerializeField] private List<EnemyTypes> _enemyIds;
    [SerializeField] private float _radius;

    private Transform _transform;

    private EnemyInfo _enemyInfo;

    private EnemyBehavior _enemyBehavior;
	// Use this for initialization
	void Start ()
	{
	    _transform = transform;
	    _enemyInfo = GetComponent<EnemyInfo>();
	    _enemyBehavior = GetComponent<EnemyBehavior>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private bool _isExiting = false;
    void OnApplicationQuit()
    {
        _isExiting = true;
    }
    
    void Destroy()
    {
        if (!_isExiting)
        {
            foreach (int enemyId in _enemyIds)
            {
                Vector2 position = _transform.position + (Vector3)(Random.insideUnitCircle * _radius);
                EnemyGenerator.Instance.GenerateEnemy(enemyId, position);

            }
        }
    }
}
