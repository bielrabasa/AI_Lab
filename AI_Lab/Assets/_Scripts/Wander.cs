using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class FollowTarget : MonoBehaviour
{
    NavMeshAgent agent;
    Vector2 targetPoint;

    public GameObject target;

    [SerializeField] float changingTargetTime;
    float targetFreq = 0.0f;
    float radius = 50.0f;
    float distance = 100.0f;

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

        targetPoint += new Vector2(transform.forward.x, transform.forward.z)  * distance;
        Vector3 dirPos = new Vector3(targetPoint.x, 0, targetPoint.y);
        
        agent.destination = dirPos;

        //Change target
        target.transform.position = dirPos;
        target.transform.Translate(new Vector3(0, 10.0f, 0));
    }
}
