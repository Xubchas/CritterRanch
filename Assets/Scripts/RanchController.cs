using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RanchController : MonoBehaviour
{

    public const float TICK_LENGTH = 0.1f;
    //all critters will be stored in this list
    public List<GameObject> critterObjects;
    public List<Critter> critters;

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


    public TextMeshProUGUI nibsText;

    private float _nibs;
    public float nibs {
        get{
            return _nibs;
        }
        set{
            _nibs = value;
            if(_nibs > 99999f){
                _nibs = 99999;
            }
            if(_nibs < 0f){
                _nibs = 0;
            }
        }
    }

    private float _cash;
    public float cash {
        get{
            return _cash;
        }
        set{
            _cash = value;
            if(_cash > 99999f){
                _cash = 99999;
            }
            if(_cash < 0f){
                _cash = 0;
            }
        }
    }

    public int slots;
    public float time;

    // Start is called before the first frame update
    void Start()
    {
        nibs = DataManager.instance.nibs;
        cash = DataManager.instance.cash;
        slots = DataManager.instance.maxSlots;
        time = DataManager.instance.time;
        StartCoroutine(Ticker());
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
    }

    //ticks every 0.1 seconds
    IEnumerator Ticker(){
        while(true){
            TickRanch();
            nibsText.text =  "" + Mathf.Floor(nibs);
            yield return new WaitForSeconds(TICK_LENGTH);
        }
    }

    void TickRanch(){
        foreach(GameObject critter in critterObjects){
            critter.GetComponent<CritterController>().TickCritter();
        }
    }

    public bool Move(int x, int y, int[] direction){
        int newX = x + direction[1];
        int newY = y + direction[0];

        if(checkLoc[newY,newX] == 0){
            checkLoc[newY,newX] = 1;
            checkLoc[y,x] = 0;
            return true;
        }

        return false;
    }

    void UpdateDataManager(){

    }
}
