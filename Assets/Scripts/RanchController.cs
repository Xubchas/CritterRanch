using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RanchController : MonoBehaviour
{

    public const float TICK_LENGTH = 0.1f;
    //all critters will be stored in this list
    public List<GameObject> critterObjects = new List<GameObject>();

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
    public TextMeshProUGUI cashText;

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

    public GameObject shop;
    public GameObject shopButton;
    public GameObject farmButton;

    // Start is called before the first frame update
    void Start()
    {
        InitializeRanch();
        StartCoroutine(Ticker());
    }

    void InitializeRanch(){
        nibs = DataManager.instance.nibs;
        cash = DataManager.instance.cash;
        slots = DataManager.instance.maxSlots;
        time = DataManager.instance.time;
        if(DataManager.instance.critters.Count > 0){
            foreach(Critter crit in DataManager.instance.critters){
                AddCritter(crit);
            }
        }
        else{
            AddCritter(DataManager.instance.getStat("Harv"), "");
        }
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
            UpdateUI();
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

    public void OpenShop(){
        Time.timeScale = 0;
        shopButton.SetActive(false);
        farmButton.SetActive(true);
        shop.SetActive(true);
    }

    public void CloseShop(){
        Time.timeScale = 1;
        shopButton.SetActive(true);
        farmButton.SetActive(false);
        shop.SetActive(false);
    }

    //POLYMORHISM: overloaded method AddCritter()
    public void AddCritter(CritterStats stats, string name){
        GameObject newCritter = Instantiate(stats.prefab);
        int [] validPos = GetValidPos();
        newCritter.GetComponent<CritterController>().SetCritter(new Critter(stats.type, name));
        newCritter.GetComponent<CritterController>().SetPos(validPos[0], validPos[1]);
        critterObjects.Add(newCritter);
        checkLoc[validPos[1],validPos[0]] = 1;
    }

    public void AddCritter(Critter critter){
        GameObject newCritter = Instantiate(DataManager.instance.getStat(critter.type).prefab);
        int [] validPos = GetValidPos();
        newCritter.GetComponent<CritterController>().SetCritter(critter);
        newCritter.GetComponent<CritterController>().SetPos(validPos[0], validPos[1]);
        critterObjects.Add(newCritter);
        checkLoc[validPos[1],validPos[0]] = 1;
    }

    int[] GetValidPos(){
        bool foundspot = false;
        int tryX = 0;
        int tryY = 0;
        while(!foundspot){
            tryX = Random.Range(1,9);
            tryY = Random.Range(1,6);
            foundspot = checkLoc[tryY,tryX] == 0;
        } 
        return new int[] {tryX, tryY};
    }

    public void UpdateUI(){
        nibsText.text =  "" + Mathf.Floor(nibs);
        cashText.text =  "" + Mathf.Floor(cash);
    }
}
