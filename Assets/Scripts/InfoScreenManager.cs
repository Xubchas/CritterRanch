using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


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

    public CritterController critterObject;

    private RanchController m_controller;

    void Awake(){
        m_controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<RanchController>();
    }
    public void UpdateInfoPage(CritterController critterCon){
        critterObject = critterCon;
        Critter critter = critterCon.me;
        CritterStats stats = DataManager.instance.getStat(critter.type);
        icon.texture = stats.icon;
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

        //make bits
        bool makesAnything = stats.makesNibs || stats.makesCash;

        makeText.gameObject.SetActive(makesAnything);
        makeIcon.gameObject.SetActive(makesAnything);
        plusSigns.SetActive(makesAnything);

        makeIcon.sprite = stats.makesNibs ? nib : cash;
        float makeValue = stats.makesMin + ((stats.makesMax-stats.makesMin) * critter.age/stats.maxAge);
        string makeString = makeValue.ToString("F2");
        makeString = makeString.Replace(',','.');
        makeText.text = makeString + " /s";

        //eat bits
        bool eatsAnything = stats.eatsNibs || stats.eatsCash;

        eatText.gameObject.SetActive(eatsAnything);
        eatIcon.gameObject.SetActive(eatsAnything);
        minusSigns.SetActive(eatsAnything);

        eatIcon.sprite = stats.eatsNibs ? nib : cash;
        eatText.text = stats.eats + " /s";

        //price
        priceText.text = "" + Mathf.Floor((stats.minSell + ((stats.maxSell-stats.minSell) * critter.age/stats.maxAge)));

        //button stuff
        confirmButton.SetActive(false);
        sureSign.SetActive(false);
        cancelButton.SetActive(false);
        sellButton.SetActive(false);

    }

    public void SellCurrent(){
        confirmButton.SetActive(false);
        sureSign.SetActive(false);
        cancelButton.SetActive(false);
        sellButton.SetActive(false);
        m_controller.SellCritter(critterObject);
    }

    void Update(){
        if(Input.GetMouseButtonUp(0)){
            cancelButton.SetActive(true);
            sellButton.SetActive(true);
        }
    }
}