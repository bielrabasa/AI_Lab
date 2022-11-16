using UnityEngine;

using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using Pada1.BBCore.Framework;
using UnityEngine.AI;

[Action("MyActions/Move Random (Wander)")]
[Help("Wander")]
public class MoveRandom : BasePrimitiveAction
{
    float t = 5.0f;

    GameObject Robber;
    Movement movement;

    public override void OnStart()
    {
        Robber = GameObject.Find("ROBBER");
        movement = Robber.GetComponent<Movement>();
        Robber.GetComponent<NavMeshAgent>().isStopped = false;
    }

    public override TaskStatus OnUpdate()
    {
        t += Time.deltaTime;

        if(t > 5.0f)
        {
            t = 0.0f;

            movement.Wander();
        }

        return TaskStatus.RUNNING;
    }
}