using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorStateBehaviour : StateMachineBehaviour {

    public WarriorStates behaviorState;

    public float horizontalForce;
    public float verticalForce;

    protected Warrior warrior;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(warrior == null)
        {
            warrior = animator.gameObject.GetComponent<Warrior>();
        }

        warrior.currentState = behaviorState;

        warrior.body.AddRelativeForce(new Vector3(0, verticalForce, 0));
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        warrior.body.AddRelativeForce(new Vector3(0, 0, horizontalForce));
    }
}
