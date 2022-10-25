using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    NavMeshAgent agent;
    public float boundDistance = 70.0f;

    //Wander
    public float WanderMoveDist = 0f;
    public float WanderRadius = 0f;

    public void Seek(Vector3 pos)
    {
        agent.destination = pos;
    }

    public void Wander()
    {
        //Find radius
        Vector2 targetPoint = new Vector2(transform.position.x + Random.Range(-WanderRadius, WanderRadius),
                                transform.position.z + Random.Range(-WanderRadius, WanderRadius));

        //Add distance
        targetPoint += new Vector2(transform.forward.x, transform.forward.z) * WanderMoveDist;

        //Check if target is out of distance
        if (targetPoint.x > boundDistance || targetPoint.y > boundDistance ||
            targetPoint.x < -boundDistance || targetPoint.y < -boundDistance)
        {
            targetPoint.x = Random.Range(-boundDistance, boundDistance);
            targetPoint.y = Random.Range(-boundDistance, boundDistance);
        }

        //Move
        Seek(new Vector3(targetPoint.x, 0, targetPoint.y));
    }
}
