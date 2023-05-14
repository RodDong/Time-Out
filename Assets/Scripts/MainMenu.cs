using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        // Load your first game scene here.
        // You might also want to clear PlayerPrefs here, 
        // or at least the "lastScene" setting, to start fresh.
        PlayerPrefs.DeleteKey("lastScene");
        SceneManager.LoadScene("Level 1");
    }

    public void ContinueGame()
    {
        // Load the last visited scene here.
        // If no scene has been visited yet, load a default scene.
        string lastScene = PlayerPrefs.GetString("lastScene", "Level 1");
        SceneManager.LoadScene(lastScene);
    }
}

