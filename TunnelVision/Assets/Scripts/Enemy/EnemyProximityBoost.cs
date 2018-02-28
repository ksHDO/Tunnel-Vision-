using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProximityBoost : EnemyBehavior
{


    [SerializeField] private float boostDistance;
    private bool isBoosting = false;
    private float boostDistanceSqr;

    [SerializeField] private float boostSpeed;

    private void Awake()
    {
        boostDistanceSqr = boostDistance * boostDistance;
    }

    protected override void FixedUpdate()
    {
        Vector2 vel;


        if ((transform.position-GlobalInfo.Player.transform.position).sqrMagnitude <= boostDistanceSqr)
        {
            vel = Seek(Target.position, boostSpeed);
        }
        else
        {
            vel = Seek(Target.position, MaxSpeed);
        }

        Rigidbody.AddForce(vel);
        base.FixedUpdate();
    }

    Vector2 Seek(Vector2 target, float speed)
    {
        Vector2 desired = (target - Rigidbody.position).normalized * speed;
        return desired - Rigidbody.velocity;
    }


}
