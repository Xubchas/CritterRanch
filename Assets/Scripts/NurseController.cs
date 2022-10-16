using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//INHERITANCE
//This class inherits from CritterController

//CLASS DESCRIOPTION
//Nurse controller manages the additional capacity of Nurse("Florie") critters of aging other critters
public class NurseController : CritterController
{
    //POLYMORPHISM 
    //the TickCritter method is overridden
    public override void TickCritter()
    {
        base.TickCritter();
        int ticksToAge = (int) Mathf.Floor(m_stats.makesMin + ((m_stats.makesMax - m_stats.makesMin)* me.age / m_stats.maxAge));
        m_controller.ageRandom(ticksToAge);
    }
}
