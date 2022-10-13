using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//CLASS DESCRIPTION
//Menu Manager runs in the main menu, controls the loading of the Ranch (Main) Scene
public class MenuManager : MonoBehaviour
{

    public void StartNewGame(){
        DataManager.instance.newGame();
        SceneManager.LoadScene(1);
    }
   
}
