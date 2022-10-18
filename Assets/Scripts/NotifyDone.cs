using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CLASS DESCRIPTION
//Used primarily to send notifications to a gamecontroller in the scene once an animation is done;
public class NotifyDone : StateMachineBehaviour
{
    public IntroController controller;
    public string eventName;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<IntroController>();
        controller.NotifyEvent(animator.gameObject, eventName);
    }
}
