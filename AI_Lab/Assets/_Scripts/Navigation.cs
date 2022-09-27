using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Navigation : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject target;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Seek();
    }

    void Seek()
    {
        agent.destination = target.transform.position;
    }
}
