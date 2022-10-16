using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

//CLASS DESCRIPTION
//Menu Manager runs in the main menu, controls the loading of the Ranch (Main) Scene
public class MenuManager : MonoBehaviour
{

    public TextMeshProUGUI nameInput;
    public GameObject nameScreen;
    private string emptyInput;

    public VolumeSlider audioSlider;

    void Start(){
        audioSlider.UpdateVolume(DataManager.instance.volume);
    }

    public void StartNewGame(){
        string name = nameInput.text != emptyInput ? nameInput.text : "My Ranch";
        DataManager.instance.newGame(name);
        SceneManager.LoadScene(1);
    }

    public void NameRanch(){
        nameScreen.SetActive(true);
        nameInput.text = "";
        emptyInput = nameInput.text;
    }
}
