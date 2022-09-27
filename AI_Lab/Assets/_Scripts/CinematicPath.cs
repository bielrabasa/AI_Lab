using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicPath : MonoBehaviour
{
    [SerializeField] GameObject targetObject;

    Vector3 dir;
    Quaternion rotation;

    //Current velocities
    float speed = 0;
    float turnSpeed = 0;

    //Max velocities
    [SerializeField] float maxSpeed;
    [SerializeField] float maxTurnSpeed;

    //Accelerations
    [SerializeField] float acceleration;
    [SerializeField] float turnAcceleration;

    //Seek update timer
    [SerializeField] float updateTimer;
    float freq;

    //Zombie tp update timer
    [SerializeField] float tpUpdateTimer;
    float tpFreq;

    //Zombie reaching distance
    [SerializeField] float stopDistance;
    float distance;

    void Start()
    {
        
    }

    void Update()
    {
        //Stop on reach distance
        distance = Vector3.Distance(targetObject.transform.position, transform.position);
        if (distance < stopDistance)
        {
            TpTarget();
            return;
        }

        //Modify acceleration to slow down
        //Slowing radius -> start slowing
        //stop distance - position = -acceleration proportion
        //modify acceleration

        //Modify velocities
        turnSpeed += turnAcceleration * Time.deltaTime;
        turnSpeed = Mathf.Min(turnSpeed, maxTurnSpeed);
        speed += acceleration * Time.deltaTime;
        speed = Mathf.Min(speed, maxSpeed);

        //Update timings
        freq += Time.deltaTime;
        tpFreq += Time.deltaTime;

        if(freq > updateTimer)
        {
            freq = 0.0f;
            SeekTarget();
        }

        if (tpFreq > tpUpdateTimer)
        {
            TpTarget();
        }

        //MOVE
        MoveToTarget();
    }

    void SeekTarget()
    {
        dir = targetObject.transform.position - transform.position;
        dir.y = 0.0f;
        dir = dir.normalized * acceleration;

        float angle = Mathf.Rad2Deg * Mathf.Atan2(dir.x, dir.z);
        rotation = Quaternion.AngleAxis(angle, Vector3.up);
    }

    void MoveToTarget()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turnSpeed);
        transform.position += transform.forward.normalized * speed * Time.deltaTime;
    }

    void TpTarget()
    {
        tpFreq = 0.0f;
        targetObject.transform.position = new Vector3(Random.Range(-20.0f, 20.0f),
                                              targetObject.transform.position.y, Random.Range(-20.0f, 20.0f));
        SeekTarget();
    }
}
