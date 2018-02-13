using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCasual : EnemyBehavior
{

    private Vector2 _travelVel;

	// Use this for initialization
	protected override void Start ()
	{
	    base.Start();

	    _travelVel = (Target.position - (Vector2) Transform.position).normalized * MaxSpeed;
	    Rigidbody.AddForce(_travelVel);
    }

    protected override void FixedUpdate()
    {

        base.FixedUpdate();
    }
}
