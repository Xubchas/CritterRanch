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
    //Make is overridden to remove normal Make function with cash doubling
    protected override void Make()
    {
        if(isHungry || isDead){
            return;
        }

        m_controller.timesCashDoubled ++;
    }
}

