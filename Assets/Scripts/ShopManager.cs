using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject upButton;
    public GameObject downButton;

    public GameObject pagePrefab;
    public GameObject listingPrefab;
    public List<GameObject> pages;
    public int currentPage = 0;
    
    //number of nibs sold for 1 cash
    private const float NIBS_TO_CASH = 10;

    //number of nibs bought for 1 cash
    private const float CASH_TO_NIBS = 5;

    private RanchController m_controller;

    void Awake(){
        m_controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<RanchController>();
    }

    void Start()
    {
        GenerateListings();
        
    }

    void GenerateListings(){
        int listingsInPage = 3;
        int currentPage = 0;
        foreach(CritterStats critter in DataManager.instance.critterMasterList){
            if(listingsInPage == 3){
                GameObject newPage = Instantiate(pagePrefab);
                newPage.transform.parent = gameObject.transform;
                pages.Add(newPage);
                currentPage++;
                newPage.gameObject.name = "PAGE " + currentPage;
                listingsInPage = 0;
            }
            GameObject newListing = Instantiate(listingPrefab);
            newListing.transform.position -= new Vector3(0,128*listingsInPage,0);
            newListing.transform.parent = pages[currentPage].transform;
            listingsInPage++;
            newListing.GetComponent<ListingManager>().UpdateListing(critter);
            
        }

    }

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

    public void PurchaseNibs(int nibsToPurchase){
        if (m_controller.cash < (float) nibsToPurchase / CASH_TO_NIBS){
            return;
        }

        m_controller.cash -= (float) nibsToPurchase / CASH_TO_NIBS;
        m_controller.nibs += nibsToPurchase;
        m_controller.UpdateUI();

    }

    public void SellNibs(int nibsToSell){
        if (m_controller.nibs < nibsToSell){
            return;
        }

        m_controller.cash += (float) nibsToSell / NIBS_TO_CASH;
        m_controller.nibs -= nibsToSell;
        m_controller.UpdateUI();

    }


}
