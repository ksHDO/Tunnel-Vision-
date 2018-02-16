using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalInfo : MonoBehaviour {


    [SerializeField] private EnemyGenerator enemyGenerator;
    public static EnemyGenerator EnemyGenerator { get; private set; }



    // Use this for initialization
    void Awake () {
        EnemyGenerator = enemyGenerator;

    }


    private void OnDestroy()
    {
        EnemyGenerator = null;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
