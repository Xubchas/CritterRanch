using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class IntroController : MonoBehaviour
{

    public TextMeshProUGUI ranchText;
    public GameObject fairy;
    public GameObject backGround;

    private int textIndex = 0;
    public List<TextMeshProUGUI> texts;
    public List<GameObject> pages;

    private const int TEXTS_PER_PAGE = 3;


    // Start is called before the first frame update
    void Start()
    {
        try{
            ranchText.text = "Will " + DataManager.instance.playerName + " join the ranks";
        }
        catch{
            ranchText.text = "Will My Ranch join the ranks";
        }
    }

    public void NotifyEvent(GameObject objectAffected, string eventName){
        switch(eventName){
            case "bgReady":
                StartIntro();
                break;
            case "bgDone":
                SceneManager.LoadScene(1);
                break;
            default:
                break;
        }
    }

    void StartIntro(){
        fairy.SetActive(true);
        Debug.Log("started intro");
        StartCoroutine(ScrollText());
    }

    IEnumerator ScrollText(){
        float timeToWait = 2;
        Debug.Log("started scrolling");
        while(true){
            yield return new WaitForSeconds(timeToWait);
            if((textIndex % 3 == 0) && textIndex != 0)
            {
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

        Invoke("EndIntro", 0.5f);

    }

    void EndIntro(){
        fairy.SetActive(false);
        backGround.GetComponent<Animator>().SetTrigger("isOutro");
    }





}
