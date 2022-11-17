using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    NavMeshAgent agent;
    public float boundDistance = 50.0f;

    GameObject[] hidingSpots;
    public GameObject hideTarget;

    //Wander
    public float WanderMoveDist = 7f;
    public float WanderRadius = 5f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        hidingSpots = GameObject.FindGameObjectsWithTag("hide");
    }

    public void Seek(Vector3 pos)
    {
        agent.SetDestination(pos);
        //agent.destination = pos;
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

    public void Hide()
    {
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        Vector3 chosenDir = Vector3.zero;
        GameObject chosenGO = hidingSpots[0];

        for (int i = 0; i < hidingSpots.Length; i++)
        {
            Vector3 hideDir = hidingSpots[i].transform.position - hideTarget.transform.position;
            Vector3 hidePos = hidingSpots[i].transform.position + hideDir.normalized * 100;

            if (Vector3.Distance(hideTarget.transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                chosenDir = hideDir;
                chosenGO = hidingSpots[i];
                dist = Vector3.Distance(this.transform.position, hidePos);
            }
        }

        Collider hideCol = chosenGO.GetComponent<Collider>();
        Ray backRay = new Ray(chosenSpot, -chosenDir.normalized);
        RaycastHit info;
        float distance = 250.0f;
        hideCol.Raycast(backRay, out info, distance);

        Seek(info.point + chosenDir.normalized);
    }
}
