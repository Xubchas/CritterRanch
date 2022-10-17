using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//INHERITANCE
//DoublerController inherits Critter Controller

//CLASS DESCRIPTION
//Doubler controls the Doubler("Vink") Critter's function;
public class DoublerController : CritterController
{
    //POLYMORPHISM
    //Make is hereby overridden to cause doubling;
    protected override void Make()
    {
        if(isHungry || isDead){
            return;
        }

        m_controller.timesCashDoubled ++;
    }
}
