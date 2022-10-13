using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListingManager : MonoBehaviour
{

    public void UpdateListing(CritterStats stats){
        Debug.Log("Created Lisitng for " + stats.type);
    }
}
