using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySeeker : EnemyBehavior {

    private enum SeekType
    {
        Seek, Pursuit
    }

    [SerializeField] private SeekType _seekingType;

    protected override void FixedUpdate()
    {

        if (Target)
        {
            Vector2 vel = Vector2.zero;

            switch (_seekingType)
            {
                case SeekType.Seek:
                    vel = Seek(Target.position);
                    break;
                case SeekType.Pursuit:
                    vel = Pursuit(Target);
                    break;
            }

            Rigidbody.AddForce(vel);
            
        }
        base.FixedUpdate();
    }

    Vector2 Pursuit(Rigidbody2D target)
    {
        Vector2 to = target.position - Rigidbody.position;
        Vector2 thisHead = Rigidbody.velocity.normalized;
        Vector2 targetHead = target.velocity.normalized;
        float heading = Vector2.Dot(thisHead, targetHead);

        if (Vector2.Dot(thisHead, targetHead) > 0 && heading < -.95)
        {
            return Seek(target.position);
        }

        float lookAhead = to.magnitude / (Rigidbody.velocity.magnitude + MaxSpeed);
        return Seek(target.position + target.velocity * lookAhead);
    }

    Vector2 Seek(Vector2 target)
    {
        Vector2 desired = (target - Rigidbody.position).normalized * MaxSpeed;
        return desired - Rigidbody.velocity;
    }
}
