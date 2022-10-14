using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//CLASS DESCRIPTION
//Shop Manager primarly manages the Shop UI and basic functions
public class ShopManager : MonoBehaviour
{
    //Ranch controller
    private RanchController m_controller;


    //Scroll Buttons
    public GameObject upButton;
    public GameObject downButton;

    //Prefabs for dynamic listings
    public GameObject pagePrefab;
    public GameObject listingPrefab;

    //Page indescing
    public List<GameObject> pages;
    public int currentPage = 0;
    
    //number of nibs sold for 1 cash
    private const float NIBS_TO_CASH = 10;

    //number of nibs bought for 1 cash
    private const float CASH_TO_NIBS = 5;

    //number of listings per page
    private const int LISTINGS_MAX = 3;
    private const int LISTING_YSIZE = 128;

    //slot price (WILL LIKELY CHANGE)
    private const int SLOT_PRICE = 150;

    //slot price listing
    public TextMeshProUGUI slotPriceText;

    

    void Awake(){
        //Finds Controller
        m_controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<RanchController>();
        GenerateListings();
    }

    //Takes the master list of Critter types and generates shop lisitngs for each critter
    void GenerateListings(){
        //page 0 has max listings already
        int listingsInPage = LISTINGS_MAX;
        int currentPage = 0;
        //iterates through critter types
        foreach(CritterStats critter in DataManager.instance.critterMasterList){
            if(listingsInPage == LISTINGS_MAX){
                //Make new page
                GameObject newPage = Instantiate(pagePrefab);
                newPage.transform.SetParent(gameObject.transform);
                pages.Add(newPage);
                currentPage++;
                newPage.gameObject.name = "PAGE " + currentPage;
                listingsInPage = 0;
            }
            //Place Listing
            GameObject newListing = Instantiate(listingPrefab);
            newListing.transform.position -= new Vector3(0,LISTING_YSIZE*listingsInPage,0);
            newListing.transform.SetParent(pages[currentPage].transform);
            listingsInPage++;
            ListingManager newLS = newListing.GetComponent<ListingManager>();
            newLS.stats = critter;
            newLS.UpdateListing();
            
        }

    }

    //Scrolls Up the shop pages, disabling up arrow if it reaches the first page
    public void ScrollUp(){
        if(currentPage > 0){
            pages[currentPage].SetActive(false);
            currentPage--;
            pages[currentPage].SetActive(true);
        }

        if(currentPage == 0){
            upButton.SetActive(false);
        }
        if(currentPage < pages.Count-1){
            downButton.SetActive(true);
        }
    }

    //Scrolls Down the shop pages, disabling the down arrow if it reaches the last page
    public void ScrollDown(){
        if(currentPage < pages.Count - 1){
            pages[currentPage].SetActive(false);
            currentPage++;
            pages[currentPage].SetActive(true);
        }

        if(currentPage == pages.Count - 1){
            downButton.SetActive(false);
        }

        if(currentPage > 0){
            upButton.SetActive(true);
        }
    }

    //Adds nibs, deducts cash according to constant exchange rate
    public void PurchaseNibs(int nibsToPurchase){
        if (m_controller.cash < (float) nibsToPurchase / CASH_TO_NIBS){
            //do nothing if not enough cash
            return;
        }

        m_controller.cash -= (float) nibsToPurchase / CASH_TO_NIBS;
        m_controller.nibs += nibsToPurchase;
        m_controller.UpdateUI();

    }

    //Adds cash, deducts nibs according to constant exchange rate
    public void SellNibs(int nibsToSell){
        if (m_controller.nibs < nibsToSell){
            //do nothing if not enough nibs
            return;
        }

        m_controller.cash += (float) nibsToSell / NIBS_TO_CASH;
        m_controller.nibs -= nibsToSell;
        m_controller.UpdateUI();
    }


    public void PurchaseCritter(CritterStats stats){
        if(m_controller.cash < stats.cost || m_controller.slots == m_controller.critterObjects.Count){
            return;
        }

        m_controller.NameCritter(stats);
    }

    public void PurchaseSlot(){
        if(m_controller.cash < SLOT_PRICE || m_controller.slots == 10){
            return;
        }

        m_controller.slots ++;
        m_controller.cash -= SLOT_PRICE;
        m_controller.UpdateUI();
        m_controller.UpdateCritterCounter();
    }


}
