using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBenchState : StateMachineBehaviour
{
    Movement movement;

    Vector3 currentBench;
    Vector3[] benchLocations = { 
        new Vector3(23, 0, -47), 
        new Vector3(-16, 0, -47), 
        new Vector3(-49, 0, -21), 
        new Vector3(-49, 0, 23) 
    };

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.ResetTrigger("goBench");
        movement = animator.GetComponent<Movement>();

        currentBench = benchLocations[Random.Range(0, 4)];
        movement.Seek(currentBench);
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 dist = (animator.gameObject.transform.position - currentBench);
        if ( dist.sqrMagnitude < 6)
        {
            animator.SetInteger("state", 2);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.transform.position = currentBench;
        animator.gameObject.transform.Rotate(new Vector3(0, 180, 0));
    }
}
