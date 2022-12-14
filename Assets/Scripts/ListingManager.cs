using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//CLASS DESCRIPTION
//Populates the UI listing with the stats data 
public class ListingManager : MonoBehaviour
{
    //Images to use
    public Sprite cash;
    public Sprite nib;

    //UI Elements to populate
    //Images
    public RawImage critterIcon;
    public Image makeIcon;
    public Image eatIcon;

    //Text Fields
    public TextMeshProUGUI critterName;
    public TextMeshProUGUI makeText;
    public TextMeshProUGUI eatText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI specialMakeText;


    //other objects
    public GameObject pluses;
    public GameObject minuses;

    public GameObject buyButton;
    public GameObject specialBuyButton;


    //stats for this listing
    public CritterStats stats;

    private ShopManager m_manager;

    void Awake(){
        m_manager = GameObject.FindGameObjectWithTag("ShopManager").GetComponent<ShopManager>();
    }
    //places stats data in appropriate UI fields
    public void UpdateListing(){
        critterName.text = stats.type;
        costText.text = "" + stats.cost;
        critterIcon.texture = stats.idleIcon;
        if(stats.makesSpecial || (!stats.makesNibs && !stats.makesCash)){
            makeText.gameObject.SetActive(false);
            makeIcon.gameObject.SetActive(false);
            pluses.SetActive(false);
            if(stats.makesSpecial){
                specialMakeText.text = stats.makesSpecialText;
                specialMakeText.gameObject.SetActive(true);
            }
        }
        else{
            makeIcon.sprite = stats.makesNibs ? nib : cash;
            if(stats.specialHasUnits){
                makeText.text = stats.makesSpecialText;
            }
            else{
                makeText.text = stats.makesMin + "-" + stats.makesMax + " /s";
            }
        }

        if(!stats.eatsNibs && !stats.eatsCash){
            eatText.gameObject.SetActive(false);
            eatIcon.gameObject.SetActive(false);
            minuses.SetActive(false);
        }
        else{
            eatIcon.sprite = stats.eatsNibs ? nib : cash;
            eatText.text = stats.eats + " /s";
        }

        //turns on special button for win critter
        if(stats.type == "??????"){
            buyButton.SetActive(false);
            specialBuyButton.SetActive(true);
        }

    }

    //Called when listed critter is bought
    public void Purchase(){
        m_manager.PurchaseCritter(stats);
    }

    //Called ONLY when special critter is bought
    public void Victory(){
        m_manager.Victory();
    }
}
