using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTracker : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void OnLevelWasLoaded(int level)
    {
        SaveProgress();
    }

    private void OnApplicationQuit()
    {
        SaveProgress();
    }

    private void SaveProgress()
    {
        string lastScene = SceneManager.GetActiveScene().name;
        if (lastScene != "Main Menu test" && lastScene != "Main Menu")
        {
            PlayerPrefs.SetString("lastScene", lastScene);
            PlayerPrefs.Save(); // Save immediately after setting
        }
    }
}
