using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public void StartNewGame(){
        DataManager.instance.newGame();
        SceneManager.LoadScene(1);
    }
   
}
