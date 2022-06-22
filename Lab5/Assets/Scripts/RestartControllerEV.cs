using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartControllerEV : MonoBehaviour
{
    public Text restartText;
    public void Update(){
        if (Input.GetKeyDown(KeyCode.R)){
             SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    public void PlayerDiesRestart(){
        restartText.text = "Enter \"R\" to restart";
    }
}
