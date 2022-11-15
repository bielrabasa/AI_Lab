using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderState : StateMachineBehaviour
{
    Movement movement;

    float t = 0.0f;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        movement = animator.GetComponent<Movement>();
        
        t = 0.0f;
        movement.Wander();
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        t += Time.deltaTime;

        if(t > 7.0f)
        {
            t = 0.0f;
            if (Random.Range(0, 1) == 0)
            {
                animator.SetTrigger("goBench");
            }
            else
            {
                movement.Wander();
            }
        }
    }
}
