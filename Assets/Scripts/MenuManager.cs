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
    //variables for ranch naming
    public TMP_InputField nameInput;
    public GameObject nameScreen;
    private string emptyInput;

    //Volume slider in this screen
    public VolumeSlider audioSlider;

    //Button to enable when there is a save
    public Button continueButton;




    void Start(){
        //set volume to default or previously set state
        audioSlider.UpdateVolume(DataManager.instance.volume);
        if(DataManager.instance.hasSaveGame()){
            //set continue button to interactable if there is a save
            continueButton.interactable = true;
        }

    }

    //called when new game pressed
    public void StartNewGame(){
        //default name if none entered
        string name = nameInput.text != emptyInput ? nameInput.text : "My Ranch";
        DataManager.instance.newGame(name);
        SceneManager.LoadScene(3);
    }

    //called when continue game pressed
    public void ContinueGame(){
        SceneManager.LoadScene(1);
    }

    //starts the ranch naming process
    public void NameRanch(){
        nameScreen.SetActive(true);
        nameInput.text = "";
        emptyInput = nameInput.text;
    }

    //exits the game
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
