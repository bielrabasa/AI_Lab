using UnityEngine;
using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using Pada1.BBCore.Framework; // BasePrimitiveAction
using UnityEngine.AI;

[Action("MyActions/Hide")]
[Help("Get the Vector3 for hiding.")]
public class HideBB : BasePrimitiveAction
{
    [InParam("game object")]
    [Help("Game object to add the component, if no assigned the component is added to the game object of this behavior")]
    public GameObject targetGameobject;

    [OutParam("hide")]
    [Help("Vector3 for higing.")]
    public Vector3 hide;
    Movement moves;

    public override void OnStart()
    {
        moves = targetGameobject.GetComponent<Movement>();
        targetGameobject.GetComponent<NavMeshAgent>().isStopped = false;
    }

    public override TaskStatus OnUpdate()
    {
        targetGameobject.GetComponent<NavMeshAgent>().isStopped = false;
        moves.Hide();
        return TaskStatus.RUNNING;
    }
}