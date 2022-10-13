using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Runner : MonoBehaviour
{
    NavMeshAgent agent;

    [SerializeField] float seekChangeDistance = 0.0f;

    //4 positions to follow
    [SerializeField] 
    Vector3[] positions = { 
        new Vector3(10f, 0f, 0f),
        new Vector3(10f, 0f, 10f),
        new Vector3(0f, 0f, 10f),
        new Vector3(0f, 0f, 0f),
    };

    Vector3 currentTarget = Vector3.zero;
    int targetId = 0;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Seek();
    }

    void Update()
    {
        //Change target when in reach distance
        if ((currentTarget - transform.position).magnitude < seekChangeDistance) 
           Seek();
    }

    void Seek()
    {
        //Find next position
        targetId++;
        if (targetId >= 4) targetId = 0;

        //Randomise a little bit the target position
        currentTarget = positions[targetId] * Random.Range(0.9f, 1.1f);

        //Send position info to navMesh
        agent.destination = currentTarget;
    }
}
