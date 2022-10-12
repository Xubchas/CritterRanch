using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RanchController : MonoBehaviour
{

    //all critters will be stored in this list
    public List<GameObject> critters;

    //initialize empty ranch location check
    private int[,] checkLoc = new int[7,10]
        {{1,1,1,1,1,1,1,1,1,1},
         {1,0,0,0,0,0,0,0,0,1},
         {1,0,0,0,0,0,0,0,0,1},
         {1,0,0,0,0,0,0,0,0,1},
         {1,0,0,0,0,0,0,0,0,1},
         {1,0,0,0,0,0,0,0,0,1},
         {1,1,1,1,1,1,1,1,1,1}}    
    ;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ticks every 0.1 seconds
    IEnumerator Ticker(){
        while(true){
            TickRanch();
            yield return new WaitForSeconds(0.1f);
        }
    }

    void TickRanch(){
        foreach(GameObject critter in critters){
            critter.GetComponent<CritterController>().TickCritter();
        }
    }
}
