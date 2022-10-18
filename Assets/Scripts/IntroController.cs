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

    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        try{
            ranchText.text = "Will "  + DataManager.instance.playerName + " join the ranks";
        }
        catch{
            ranchText.text = "Will My Ranch join the ranks";
        }

        audioSource.volume = DataManager.instance.volume;
        StartCoroutine(FadeInMusic());

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
        StartCoroutine(ScrollText());
    }

    IEnumerator ScrollText(){
        float timeToWait = 1f;

        while(true){
            yield return new WaitForSeconds(timeToWait);
            timeToWait = 2f;
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
        StartCoroutine(FadeOutMusic());
        backGround.GetComponent<Animator>().SetTrigger("isOutro");
    }

    public void SkipIntro(){
        SceneManager.LoadScene(1);
    }

    IEnumerator FadeOutMusic(){
        float startVolume = audioSource.volume;
 
        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / 5f;
 
            yield return null;
        }
 
        audioSource.Stop ();
    }

    IEnumerator FadeInMusic(){
        float targetVolume = audioSource.volume;
        audioSource.volume = 0;
 
        while (audioSource.volume < targetVolume) {
            audioSource.volume += targetVolume * Time.deltaTime / 2.5f;
 
            yield return null;
        }
 
        audioSource.volume = targetVolume;
    }





}
