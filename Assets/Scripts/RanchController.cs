using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    public float cashThisTick;
    public int timesCashDoubled;

    public int slots;
    public float time;

    //UI Shop Elements to enable/disable
    public GameObject shop;
    public GameObject shopButton;
    public GameObject farmButton;
    public GameObject pauseButton;
    public GameObject playButton;

 

    //UI Elements to keep updated
    public TextMeshProUGUI nibsText;
    public TextMeshProUGUI cashText;

    public TextMeshProUGUI nibsSecondText;
    public TextMeshProUGUI cashSecondText;
    public TextMeshProUGUI nameText;

    //UI Elements to move around (and also update sometimes)

    public GameObject critterSign;
    public TextMeshProUGUI critterText;
    public readonly Vector2 critterSignFarmPos = new Vector2 (80,144);
    public readonly Vector2 critterSignShopPos = new Vector2 (16,528);

    public GameObject timeSign;
    public TextMeshProUGUI timeText;
    public readonly Vector2 timeSignFarmPos = new Vector2 (400,144);
    public readonly Vector2 timeSignShopPos = new Vector2 (464,528);

    //namePopup stuff
    public GameObject namePopup;
    public RawImage namePopupIcon;
    private CritterStats critterToPurchase;
    public TMP_InputField nameInput;
    public GameObject confirmButton;
    public GameObject cancelButton;
    public GameObject firstPurchase;
    private string emptyInput;

    //infoPopup stuff
    public GameObject infoPopup;

    //bools for UI stuff

    public bool paused = false;

    //Pause Screen Stuff;
    public GameObject pauseScreen;
    public GameObject optionsScreen;

    //Audio sliders
    public VolumeSlider audioSlider;

    //victory screen
    public GameObject victoryScreen;


    // Start is called before the first frame update
    void Start()
    {
        InitializeRanch();
        if(time> 0){
            Debug.Log("unpausing game");
            firstPurchase.SetActive(false);

            shopButton.SetActive(true);
            pauseButton.SetActive(true);
            confirmButton.SetActive(true);
            cancelButton.SetActive(true);
            namePopup.SetActive(false);
            StartCoroutine(Ticker());
            UnPause();
        }

    }

    //Takes whatever DataManager loaded or initialized and applies it to the game
    void InitializeRanch(){
        nameText.text = DataManager.instance.playerName;
        nibs = DataManager.instance.nibs;
        cash = DataManager.instance.cash;
        slots = DataManager.instance.maxSlots;
        time = DataManager.instance.time;
        audioSlider.UpdateVolume(DataManager.instance.volume);

        if(DataManager.instance.critters.Count > 0){
            List<Critter> crittersToLoad = new List<Critter>(DataManager.instance.critters);
            foreach(Critter crit in crittersToLoad){
                AddCritter(crit);
            }
        }
        else{
            NameCritter(DataManager.instance.getStat("Harv"));
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
        cashThisTick = 0;
        timesCashDoubled = 0;
        foreach(GameObject critter in critterObjects){
            critter.GetComponent<CritterController>().TickCritter();
        }
       
        cashThisTick = cashThisTick * (Mathf.Pow(2, timesCashDoubled));

        cashPerSecond += (cashPerSecond * (Mathf.Pow(2,timesCashDoubled)-1));
        cash += cashThisTick;
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
    public void UpdateDataManager(){
        DataManager.instance.nibs = nibs;
        DataManager.instance.cash = cash;
        DataManager.instance.time = time;
        DataManager.instance.maxSlots = slots;
        DataManager.instance.critters = new List<Critter>();
        foreach(GameObject critterObj in critterObjects){
            DataManager.instance.critters.Add(critterObj.GetComponent<CritterController>().me);
        }
        DataManager.instance.SaveSaveGame();
    }

    //Pauses Game, Opens Shop
    public void OpenShop(){
        ClosePauseScreen();
        Pause();
        shopButton.SetActive(false);
        farmButton.SetActive(true);
        shop.SetActive(true);
        critterSign.transform.position = critterSignShopPos;
        timeSign.transform.position = timeSignShopPos;

    }

    //Unpauses Game, Closes Shop
    public void CloseShop(){
        ClosePauseScreen();
        UnPause();
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
        UpdateDataManager();
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
        UpdateDataManager();
    }

    //ABSTRACTION
    //Like many methods in this script GetValidPos abstracts the function of the method to this call

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

    //called when naming critter
    public void NameCritter(CritterStats stats){
        //do all close shop processes
        Pause();
        farmButton.SetActive(false);
        pauseButton.SetActive(false);
        shop.SetActive(false);
        critterSign.transform.position = critterSignFarmPos;
        timeSign.transform.position = timeSignFarmPos;

        //enable naming
        critterToPurchase = stats;
        
        namePopup.SetActive(true);
        nameInput.text = string.Empty;
        Debug.Log(nameInput.text);
        emptyInput = nameInput.text;
        namePopupIcon.texture = critterToPurchase.idleIcon;
        
    }

    //cancel the naming process
    public void CancelName(){
        shopButton.SetActive(true);
        namePopup.SetActive(false);
        UnPause();
    }

    //deducts cash for critter purchase and adds them, with new name
    public void CompletePurchase(){

        cash -= critterToPurchase.cost;
        UpdateUI();
        string crittername = nameInput.text != emptyInput ? nameInput.text : "Nameless";
        AddCritter(critterToPurchase, crittername);
        namePopup.SetActive(false);
        shopButton.SetActive(true);
        pauseButton.SetActive(true);
        UnPause();

    }

    //called by the special first purchase button
    public void FirstPurchase(){

        string crittername = nameInput.text != emptyInput ? nameInput.text : "Nameless";

        AddCritter(critterToPurchase, crittername);
        firstPurchase.SetActive(false);

        shopButton.SetActive(true);
        pauseButton.SetActive(true);
        confirmButton.SetActive(true);
        cancelButton.SetActive(true);
        namePopup.SetActive(false);

        UnPause();
        StartCoroutine(Ticker());
    }

    //Opens info panel for critter in attribute
    public void OpenInfoPanel(CritterController critterToList){
        Pause();
        infoPopup.SetActive(true);
        infoPopup.GetComponent<InfoScreenManager>().UpdateInfoPage(critterToList);
    }

    //Sells the critter a crittercontroller is attached to
    public void SellCritter(CritterController critterToSell){
        infoPopup.SetActive(false);
        UnPause();
        critterObjects.Remove(critterToSell.gameObject);
        Critter critter = critterToSell.me;
        CritterStats stats = DataManager.instance.getStat(critter.type);
        if(critterToSell.isDead){
            //sells for dead price if dead
            cash += DataManager.DEAD_SELL;
        }
        else{
            //sells for normal price
            cash += stats.minSell + ((stats.maxSell-stats.minSell) * critter.age/stats.maxAge);
        }
        Destroy(critterToSell.gameObject);
        UpdateDataManager();
        UpdateCritterCounter();
    }

    //Pauses the game
    public void Pause(){
        UpdateDataManager();
        paused = true;
        Time.timeScale = 0f;
    }

    //Unpauses the game
    public void UnPause(){
        paused = false;
        Time.timeScale = 1f;
    }

    //Opens the Pause screen
    public void OpenPauseScreen(){
        Pause();
        pauseScreen.SetActive(true);
        pauseButton.SetActive(false);
        playButton.SetActive(true);
        critterSign.transform.position = critterSignShopPos;
        timeSign.transform.position = timeSignShopPos;
    }

    //Closes the pause screen
    public void ClosePauseScreen(){
        pauseScreen.SetActive(false);
        pauseButton.SetActive(true);
        playButton.SetActive(false);
        optionsScreen.SetActive(false);
        if(!shop.activeInHierarchy){
            //only moves time signs ifnot in shop
            critterSign.transform.position = critterSignFarmPos;
            timeSign.transform.position = timeSignFarmPos;
        }
        UnPause();
    }

    //Reloads the main menu scene
    public void BackToMenu(){
        UpdateDataManager();
        SceneManager.LoadScene(0);
    }

    //Ages a random 
    public void ageRandom(int ticksToAge){
        int randIndex = Random.Range(0,critterObjects.Count);
        critterObjects[randIndex].GetComponent<CritterController>().Age(ticksToAge);
    }

    //Takes you to victory screen if can buy final critter
    public void Victory(){
        if(cash > 99998f){

            StartCoroutine(VictoryScreen());
        }
    }

    IEnumerator VictoryScreen(){
        while(victoryScreen.transform.position.y < 0){
            victoryScreen.transform.position += new Vector3 (0, 4, 0);
            yield return null;
        }
        SceneManager.LoadScene(2);
    }

}
