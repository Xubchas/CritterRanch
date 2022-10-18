using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//CLASS DESCRIPTION
//this class manages the critter info screen, setting the appropriate values and turning on the correct fields
public class InfoScreenManager : MonoBehaviour
{
    //utility sprites
    public Sprite nib;
    public Sprite cash;

    //Texts to set
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI typeText;
    public TextMeshProUGUI ageText;
    public TextMeshProUGUI makeText;
    public TextMeshProUGUI eatText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI statusText;

    //images to set
    public RawImage icon;
    public Image makeIcon;
    public Image eatIcon;

    //objects to activate/deactivate
    public GameObject plusSigns;
    public GameObject minusSigns;
    public GameObject sureSign;
    public GameObject confirmButton;
    public GameObject cancelButton;
    public GameObject sellButton;

    //Critter displayed
    public CritterController critterObject;

    //Scene controller
    private RanchController m_controller;

    void Awake(){
        //find controller
        m_controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<RanchController>();
    }

    //Main class method, sets the screen to reflect the critter info
    public void UpdateInfoPage(CritterController critterCon){
        critterObject = critterCon;
        Critter critter = critterCon.me;
        CritterStats stats = DataManager.instance.getStat(critter.type);
        nameText.text = "Name: " + critter.name;
        typeText.text = "Type: " + critter.type;
        string agevalue = "";
        if(critter.age < stats.maxAge/4){
            agevalue = "baby";
        }
        else if(critter.age < stats.maxAge/2){
            agevalue = "young";
        }
        else if(critter.age < stats.maxAge*0.9){
            agevalue  = "adult";
        }
        else{
            agevalue = "max";
        }
        ageText.text = "Age: " + agevalue;

        //make bits stats
        bool makesAnything = (stats.makesNibs || stats.makesCash) && !critterCon.isDead;

        makeText.gameObject.SetActive(makesAnything);
        makeIcon.gameObject.SetActive(makesAnything);
        plusSigns.SetActive(makesAnything);

        makeIcon.sprite = stats.makesNibs ? nib : cash;
        float makeValue;
        if(critterCon.isHungry){
            makeValue = 0;
        }else{
            makeValue = stats.makesMin + ((stats.makesMax-stats.makesMin) * critter.age/stats.maxAge);
        }

        string makeString;
        if(!stats.specialHasUnits){
            makeString = makeValue.ToString("F2");
            makeString = makeString.Replace(',','.');
            makeString = makeString + " /s";
        }
        else{
            makeString = stats.makesSpecialText;
        }
        makeText.text = makeString;

        //eat bits stats
        bool eatsAnything = (stats.eatsNibs || stats.eatsCash) && !critterCon.isDead;

        eatText.gameObject.SetActive(eatsAnything);
        eatIcon.gameObject.SetActive(eatsAnything);
        minusSigns.SetActive(eatsAnything);

        eatIcon.sprite = stats.eatsNibs ? nib : cash;
        eatText.text = stats.eats + " /s";

        //price
        if(critterCon.isDead){
            priceText.text = "" + DataManager.DEAD_SELL;
        }
        else{
            priceText.text = "" + Mathf.Floor((stats.minSell + ((stats.maxSell-stats.minSell) * critter.age/stats.maxAge)));
        }

        //button deactivate
        confirmButton.SetActive(false);
        sureSign.SetActive(false);
        cancelButton.SetActive(false);
        sellButton.SetActive(false);

        //status stuff
        if (critterCon.isDead){
            statusText.text = "DEAD";
            icon.texture = stats.deadIcon;
        }
        else if(critterCon.isHungry){
            statusText.text = "Hungry";
            icon.texture = stats.hungryIcon;
        }
        else{
            statusText.text = "OK";
            icon.texture = stats.idleIcon;
        }

    }

    //Resets the screen to its default state
    //also calls the sell method
    public void SellCurrent(){
        confirmButton.SetActive(false);
        sureSign.SetActive(false);
        cancelButton.SetActive(false);
        sellButton.SetActive(false);
        m_controller.SellCritter(critterObject);
    }

    //makes sure you don't click a button by accident
    void Update(){
        if(Input.GetMouseButtonUp(0)){
            cancelButton.SetActive(true);
            sellButton.SetActive(true);
        }
    }
}
