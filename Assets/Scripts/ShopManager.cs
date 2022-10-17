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
    private const float BUY_50 = 20;
    private const float BUY_500 = 190;
    private const float SELL_50 = 10;
    private const float SELL_500 = 125;

    //number of listings per page
    private const int LISTINGS_MAX = 3;
    private const int LISTING_YSIZE = 128;

    //maximum allowed slots
    public const int MAX_SLOTS = 15;

    //slot price listing
    public TextMeshProUGUI slotPriceText;
    //slot price progression
    public readonly int[] slotPrices = new int[] {25, 50, 100, 250, 500, 1000, 2500, 5000, 10000, 50000};

    

    void Awake(){
        //Finds Controller
        slotPriceText.text = "" + slotPrices[0];
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
        float nibsCost = nibsToPurchase == 50 ? BUY_50 : BUY_500;


        if (m_controller.cash < nibsCost){
            //do nothing if not enough cash
            return;
        }

        m_controller.cash -= nibsCost;
        m_controller.nibs += nibsToPurchase;
        m_controller.UpdateUI();

    }

    //Adds cash, deducts nibs according to constant exchange rate
    public void SellNibs(int nibsToSell){
        float nibsCost = nibsToSell == 50 ? SELL_50 : SELL_500;

        if (m_controller.nibs < nibsToSell){
            //do nothing if not enough nibs
            return;
        }

        m_controller.cash += nibsCost;
        m_controller.nibs -= nibsToSell;
        m_controller.UpdateUI();
    }

    //Called when critter purchased from listing
    public void PurchaseCritter(CritterStats stats){
        if(m_controller.cash < stats.cost || m_controller.slots == m_controller.critterObjects.Count){
            return;
        }

        m_controller.NameCritter(stats);
    }

    //Purchases and adds a new slot
    public void PurchaseSlot(){
        if(m_controller.slots >= MAX_SLOTS || m_controller.cash < slotPrices[m_controller.slots - DataManager.START_SLOTS] ){
            return;
        }
        m_controller.cash -= slotPrices[m_controller.slots - DataManager.START_SLOTS];
        m_controller.slots ++;
        
        slotPriceText.text = m_controller.slots < MAX_SLOTS ? "" + slotPrices[m_controller.slots - DataManager.START_SLOTS] : "N/A";
        
        m_controller.UpdateUI();
        m_controller.UpdateCritterCounter();
    }

    //Called only from special buy button
    public void Victory(){
        m_controller.Victory();
    }


}
