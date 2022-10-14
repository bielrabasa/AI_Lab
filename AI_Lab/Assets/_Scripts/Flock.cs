using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockManager myManager;
    public Vector3 direction;
    float speed = 0.0f;

    // Update is called once per frame
    void Update()
    {
        //Movement
        transform.rotation = Quaternion.Slerp(transform.rotation,
                                      Quaternion.LookRotation(direction),
                                      myManager.rotationSpeed * Time.deltaTime);
        transform.Translate(0.0f, 0.0f, Time.deltaTime * speed);
    }

    public void CalculateDirection()
    {
        direction = Vector3.zero;

        if (myManager.followLiderForce > 0f) direction += FollowLeader() * myManager.followLiderForce;
        if (myManager.cohesionForce > 0f) direction += Cohesion() * myManager.cohesionForce;
        if (myManager.alignForce > 0f) direction += Align() * myManager.alignForce;
        if (myManager.separationForce > 0f) direction += Separation() * myManager.separationForce;

        direction.Normalize();
        direction *= speed;
    }

    Vector3 Cohesion()
    {
        //COHESION
        Vector3 cohesion = Vector3.zero;
        int num = 0;
        foreach (GameObject go in myManager.allBees)
        {
            if (go != this.gameObject)
            {
                float distance = Vector3.Distance(go.transform.position,
                                                  transform.position);
                if (distance <= myManager.neighbourDistance)
                {
                    cohesion += go.transform.position;
                    num++;
                }
            }
        }
        
        //Normal ocasion
        if (num > 0)
        {
            return (cohesion / num - transform.position).normalized;
        }

        //Alone fella
        foreach (GameObject go in myManager.allBees)
        {
            if (go != this.gameObject)
            {
                cohesion += go.transform.position;
                num++;
            }
        }
        return (cohesion / num - transform.position).normalized;
    }

    Vector3 Align()
    {
        Vector3 align = Vector3.zero;
        int num = 0;
        foreach (GameObject go in myManager.allBees)
        {
            if (go != this.gameObject)
            {
                float distance = Vector3.Distance(go.transform.position,
                                                  transform.position);
                if (distance <= myManager.neighbourDistance)
                {
                    align += go.GetComponent<Flock>().direction;
                    num++;
                }
            }
        }
        if (num > 0)
        {
            align /= num;
            speed = Mathf.Clamp(align.magnitude, myManager.minSpeed, myManager.maxSpeed);
        }

        return align;
    }

    Vector3 Separation()
    {
        Vector3 separation = Vector3.zero;
        foreach (GameObject go in myManager.allBees)
        {
            if (go != this.gameObject)
            {
                float distance = Vector3.Distance(go.transform.position,
                                                  transform.position);
                if (distance <= myManager.neighbourDistance)
                    separation -= (transform.position - go.transform.position) /
                                  (distance * distance);
            }
        }

        return separation;
    }

    Vector3 FollowLeader()
    {
        return (myManager.liderPos - transform.position).normalized;
    }
}
