using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyTypes
{
    BASIC,
    FAST,
    FAST_FAST,
    LARGE,
    LARGE_SPAWNER,
    SPAWNER_MINION,
    CASUAL,
    CASUAL_FAST,
    BOOST,
    CARRIER,
    WEAK
}

public static class EnemyTypesExt
{
    public static int ToInt(this EnemyTypes type)
    {
        return (int)type;
    }
}

public class EnemyList : MonoBehaviour {

    [SerializeField] private List<EnemyBehavior> enemyList;
    public List<EnemyBehavior> Enemies
    {
        get { return enemyList; }
    }

    public static EnemyList Instance { get; private set; }
	// Use this for initialization
	void Start () {
		if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(this);
        }
	}
}
