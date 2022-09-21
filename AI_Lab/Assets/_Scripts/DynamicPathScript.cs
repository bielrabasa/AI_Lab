using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicPathScript : MonoBehaviour
{
    [SerializeField] GameObject targetObject;

    Vector3 dir;
    Quaternion rotation;

    float dist;

    [SerializeField] float vel;
    [SerializeField] float rotVel;
    [SerializeField] float updateTimer;
    [SerializeField] float tpUpdateTimer;

    float freq;
    float tpFreq;

    void Start()
    {
        
    }

    void Update()
    {
        freq += Time.deltaTime;
        tpFreq += Time.deltaTime;

        if(freq > updateTimer)
        {
            freq = 0.0f;
            SeekTarget();
        }

        if(dist < 1.2f)
        {
            TpTarget();
            tpFreq = 0.0f;
        }

        if (tpFreq > tpUpdateTimer)
        {
            tpFreq = 0.0f;
            TpTarget();
        }

        MoveToTarget();
    }

    void SeekTarget()
    {
        dir = targetObject.transform.position - transform.position;
        dir.y = 0.0f;
        dist = dir.magnitude;
        dir.Normalize();

        float angle = Mathf.Rad2Deg * Mathf.Atan2(dir.x, dir.z);
        rotation = Quaternion.AngleAxis(angle, Vector3.up);
    }

    void MoveToTarget()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotVel);
        transform.position += transform.forward.normalized * vel * Time.deltaTime;
    }

    void TpTarget()
    {
        targetObject.transform.position = new Vector3(Random.Range(-20.0f, 20.0f),
                                              targetObject.transform.position.y, Random.Range(-20.0f, 20.0f));
        SeekTarget();
    }
}
