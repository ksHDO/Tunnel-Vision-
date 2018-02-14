using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnDeath : MonoBehaviour
{

    [SerializeField] private List<GameObject> _objects;
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
    void OnDestroy()
    {
        if (!_isExiting)
        {
            foreach (GameObject o in _objects)
            {
                GameObject obj = Instantiate(o);
                obj.transform.position = _transform.position + (Vector3) (Random.insideUnitCircle * _radius);
                obj.GetComponent<EnemyInfo>().PlayerScore = _enemyInfo.PlayerScore;
                obj.GetComponent<EnemyBehavior>().Target = _enemyBehavior.Target;

            }
        }
    }
}
