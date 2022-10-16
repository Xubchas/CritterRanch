using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

//CLASS DESCRIPTION
//Menu Manager runs in the main menu, controls the loading of the Ranch (Main) Scene
public class MenuManager : MonoBehaviour
{

    public TextMeshProUGUI nameInput;
    public GameObject nameScreen;
    private string emptyInput;

    public VolumeSlider audioSlider;

    public Button continueButton;

    void Start(){
        audioSlider.UpdateVolume(DataManager.instance.volume);
        if(DataManager.instance.hasSaveGame()){
            continueButton.interactable = true;
        }
    }

    public void StartNewGame(){
        string name = nameInput.text != emptyInput ? nameInput.text : "My Ranch";
        DataManager.instance.newGame(name);
        SceneManager.LoadScene(1);
    }

    public void ContinueGame(){
        SceneManager.LoadScene(1);
    }

    public void NameRanch(){
        nameScreen.SetActive(true);
        nameInput.text = "";
        emptyInput = nameInput.text;
    }

    public void Exit(){
# if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
# elif UNITY_STANDALONE
        Application.Quit();
# elif UNITY_WEBGL
        Application.OpenURL("https://play.unity.com/u/danbuxta");
# endif
    }
}
