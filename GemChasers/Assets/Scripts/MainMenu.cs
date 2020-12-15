using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Main Menu Class
 * Programmed by David Knolls
 * 
 * Basic button events for main menu screen
 */

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Function to load into specificed scene (other menu screen, level start)
    public void LoadScene(string sceneName) 
    {
        SceneManager.LoadScene(sceneName);
    }

    //End game function
    public void ExitGame() 
    {
        Application.Quit();
    }
}
