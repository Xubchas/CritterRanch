using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//CLASS DESCRIPTION
//Font Change changes the color of text-only buttons in the main and pause screens
public class FontChange : StateMachineBehaviour
{
    //Color to switch to in this state
    public Color targetColor;

    //text element to change
    public TextMeshProUGUI text;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //get text object, set its color
        text = animator.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.color = targetColor;
    }

}
