using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//CLASS DESCRIPTION
//Ranch Controller is the man Game Controller, during runtime the authoritative game statistics are held here
public class RanchController : MonoBehaviour
{

    //How long, in seconds, a tick lasts
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

    //Game stats
    private float _nibs;
    public float nibs {
        get{
            return _nibs;
        }
        set{
            _nibs = value;
            //Sets minimum and maximum nibs values
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
            //sets minimum and maximum cash values
            if(_cash > 99999f){
                _cash = 99999;
            }
            if(_cash < 0f){
                _cash = 0;
            }
        }
    }

    public float nibsPerSecond;
    public float cashPerSecond;

    public int slots;
    public float time;

    //UI Elements to enable/disable
    public GameObject shop;
    public GameObject shopButton;
    public GameObject farmButton;

    //UI Elements to keep updated
    public TextMeshProUGUI nibsText;
    public TextMeshProUGUI cashText;

    public TextMeshProUGUI nibsSecondText;
    public TextMeshProUGUI cashSecondText;

    //UI Elements to move around (and also update sometimes)

    public GameObject critterSign;
    public TextMeshProUGUI critterText;
    public readonly Vector2 critterSignFarmPos = new Vector2 (80,144);
    public readonly Vector2 critterSignShopPos = new Vector2 (16,528);

    public GameObject timeSign;
    public TextMeshProUGUI timeText;
    public readonly Vector2 timeSignFarmPos = new Vector2 (400,144);
    public readonly Vector2 timeSignShopPos = new Vector2 (464,528);

    // Start is called before the first frame update
    void Start()
    {
        InitializeRanch();
        StartCoroutine(Ticker());
    }

    //Takes whatever DataManager loaded or initialized and applies it to the game
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
        UpdateUI();
        UpdateCritterCounter();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
    }

    //ticks every TICK_LENGTH seconds
    IEnumerator Ticker(){
        while(true){
            TickRanch();
            UpdateUI();
            yield return new WaitForSeconds(TICK_LENGTH);
        }
    }

    //goes through every critter and tells it to do its actions for that tick
    void TickRanch(){
        nibsPerSecond = 0;
        cashPerSecond = 0;
        foreach(GameObject critter in critterObjects){
            critter.GetComponent<CritterController>().TickCritter();
        }
    }

    //Receives the current position of the critter and a direction of motion
    //checks the location array for a valid move
    //if valid, updates array and returns true
    //if invalid, return false
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

    //Transfers data to DataManager for safekeeping and save/loading
    void UpdateDataManager(){

    }

    //Pauses Game, Opens Shop
    public void OpenShop(){
        Time.timeScale = 0;
        shopButton.SetActive(false);
        farmButton.SetActive(true);
        shop.SetActive(true);
        critterSign.transform.position = critterSignShopPos;
        timeSign.transform.position = timeSignShopPos;

    }

    //Unpauses Game, Closes Shop
    public void CloseShop(){
        Time.timeScale = 1;
        shopButton.SetActive(true);
        farmButton.SetActive(false);
        shop.SetActive(false);
        critterSign.transform.position = critterSignFarmPos;
        timeSign.transform.position = timeSignFarmPos;
    }

    //POLYMORHISM: overloaded method AddCritter()

    //Adds new critter in random spot made with critter type and name
    public void AddCritter(CritterStats stats, string name){
        GameObject newCritter = Instantiate(stats.prefab);
        int [] validPos = GetValidPos();
        newCritter.GetComponent<CritterController>().SetCritter(new Critter(stats.type, name));
        newCritter.GetComponent<CritterController>().SetPos(validPos[0], validPos[1]);
        critterObjects.Add(newCritter);
        checkLoc[validPos[1],validPos[0]] = 1;
        UpdateCritterCounter();
    }

    //Adds critter in random spot based on existing critter data
    public void AddCritter(Critter critter){
        GameObject newCritter = Instantiate(DataManager.instance.getStat(critter.type).prefab);
        int [] validPos = GetValidPos();
        newCritter.GetComponent<CritterController>().SetCritter(critter);
        newCritter.GetComponent<CritterController>().SetPos(validPos[0], validPos[1]);
        critterObjects.Add(newCritter);
        checkLoc[validPos[1],validPos[0]] = 1;
        UpdateCritterCounter();
    }

    //Checks for empty space in array, returns a random valid position
    int[] GetValidPos(){
        bool foundspot = false;
        int tryX = 0;
        int tryY = 0;
        while(!foundspot){
            tryX = Random.Range(1,checkLoc.GetLength(1));
            tryY = Random.Range(1,checkLoc.GetLength(0));
            foundspot = checkLoc[tryY,tryX] == 0;
        } 
        return new int[] {tryX, tryY};
    }

    //Updates UI elements that need updating
    public void UpdateUI(){
        nibsText.text =  "" + Mathf.Floor(nibs);
        cashText.text =  "" + Mathf.Floor(cash);
        string npstext = nibsPerSecond.ToString("F2") + "/s";
        npstext = npstext.Replace(',','.');
        string cpstext =  cashPerSecond.ToString("F2") + "/s";
        cpstext = cpstext.Replace(',','.');
        nibsSecondText.text =  npstext;
        cashSecondText.text =  cpstext;

        float hours = Mathf.Floor(time/3600f);
        float minutes = Mathf.Floor(time/60f) % 60;
        float seconds = Mathf.Floor(time) % 60;

        timeText.text = hours.ToString("0") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    //this counter needs far fewer updates
    public void UpdateCritterCounter(){
        critterText.text = critterObjects.Count + "/" + slots;
    }
}
