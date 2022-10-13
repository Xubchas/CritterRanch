using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CLASS DESCRIPTION
//Populates the UI listing with the stats data 
public class ListingManager : MonoBehaviour
{
    //places stats data in appropriate UI fields
    public void UpdateListing(CritterStats stats){
        //placeholder for debugging
        Debug.Log("Created Lisitng for " + stats.type);
    }
}
