using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject upButton;
    public GameObject downButton;

    public List<GameObject> pages;
    public int currentPage = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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


}
