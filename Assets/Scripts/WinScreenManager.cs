using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


//CLASS DESCRIPTION
//Populates the necessary data in the win screen
public class WinScreenManager : MonoBehaviour
{
    //UI Text fields
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI nameText;

    //game stats to display
    private float time;

    //display management
    private int textIndex = 0;
    public List<GameObject> texts;
    public List<GameObject> pages;
    private const int TEXT_PER_PAGE = 3;
    private const float START_WAIT = 1f;
    private const float NORMAL_WAIT = 2f;
    private const float LONG_WAIT = 4f;

    //audio stuff
    public AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        //Place game time on screen;
        time = DataManager.instance.time;
        float hours = Mathf.Floor(time/3600f);
        float minutes = Mathf.Floor(time/60f) % 60;
        float seconds = Mathf.Floor(time) % 60;

        timeText.text = hours.ToString("0") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
        nameText.text =  DataManager.instance.playerName;
        StartCoroutine(animateText());
        
    }

    //Runs through the text in the list
    IEnumerator animateText(){
        float timeToWait = START_WAIT;

        while(true){
            yield return new WaitForSeconds(timeToWait);
            timeToWait = NORMAL_WAIT;
            if(textIndex == TEXT_PER_PAGE){
                pages[(textIndex/TEXT_PER_PAGE)-1].SetActive(false);
            }
            if(textIndex == texts.Count){
                break;
            }

            texts[textIndex].gameObject.SetActive(true);
            textIndex ++;
            if((textIndex % TEXT_PER_PAGE == 0))
            {
                timeToWait = LONG_WAIT;
            }

        }

    }



}
