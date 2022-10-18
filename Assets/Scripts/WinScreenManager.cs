using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


//CLASS DESCRIPTION
//Populates the necessary data in the win screen
public class WinScreenManager : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI nameText;
    private float time;
    private int textIndex = 0;
    public List<GameObject> texts;
    public List<GameObject> pages;

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

    IEnumerator animateText(){
        float timeToWait = 1f;

        while(true){
            yield return new WaitForSeconds(timeToWait);
            timeToWait = 2f;
            if(textIndex == 3){
                pages[(textIndex/3)-1].SetActive(false);
            }
            if(textIndex == texts.Count){
                break;
            }

            texts[textIndex].gameObject.SetActive(true);
            textIndex ++;
            if((textIndex % 3 == 0))
            {
                timeToWait = 4f;
            }

        }

    }



}
