using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CLASS DESCRIPTION
//Doubler controls the Doubler("Vink") Critter's function;
public class DoublerController : CritterController
{
    protected override void Make()
    {
        if(isHungry || isDead){
            return;
        }

        m_controller.timesCashDoubled ++;
    }
}
