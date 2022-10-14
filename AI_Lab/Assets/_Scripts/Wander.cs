using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Wander : MonoBehaviour
{
    NavMeshAgent agent;
    Vector2 targetPoint;

    [SerializeField] float changingTargetTime;
    [SerializeField] float changingTargetDist = 100.0f;
    [SerializeField] float radius = 20.0f;

    [SerializeField] float maxDistance = 70.0f;
    float targetFreq = 0.0f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Seek();
    }

    void Update()
    {
        targetFreq += Time.deltaTime;
        
        if(targetFreq > changingTargetTime)
        {
            Seek();
            targetFreq = 0;
        }
    }

    void Seek()
    {
        targetPoint = new Vector2(transform.position.x + Random.Range(-radius, radius),
                                transform.position.z + Random.Range(-radius, radius));

        targetPoint += new Vector2(transform.forward.x, transform.forward.z)  * changingTargetDist;

        if (targetPoint.x > maxDistance || targetPoint.y > maxDistance ||
            targetPoint.x < -maxDistance || targetPoint.y < -maxDistance)
        {
            targetPoint.x = Random.Range(-maxDistance, maxDistance);
            targetPoint.y = Random.Range(-maxDistance, maxDistance);
        }

        
        Vector3 dirPos = new Vector3(targetPoint.x, 0, targetPoint.y);
        
        agent.destination = dirPos;
    }
}
