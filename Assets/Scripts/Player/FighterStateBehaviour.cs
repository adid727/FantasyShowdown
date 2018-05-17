using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterStateBehaviour : StateMachineBehaviour {

    public FighterStates behaviorState;

    public AudioClip sfx;

    protected ThirdPersonController player;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null)
        {
            player = animator.gameObject.GetComponent<ThirdPersonController>();
        }

        player.currentState = behaviorState;

        if (sfx != null)
        {
            player.playSound(sfx);
        }
    }
}
