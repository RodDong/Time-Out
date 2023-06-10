using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuN : MonoBehaviour
{
    private SaveLoadManager sl;
    private GameObject player;

    public void Start()
    {
        sl = GameObject.FindObjectOfType<SaveLoadManager>();
        player = GameObject.FindWithTag("Player");
    }

    public void NewGame()
    {
        sl.NewGame();
        SceneManager.LoadScene("Level 1");
    }

    public void ContinueGame(int level, int area)
    {
        if (level == 1)
        {
            SceneManager.LoadScene("Level 1");
            player.transform.position = LoadPos(area);
        }
        else if (level == 2)
        {
            SceneManager.LoadScene("Level 2");
            player.transform.position = LoadPos(area);
        }
        else if (level == 3)
        {
            SceneManager.LoadScene("Level 3");
            player.transform.position = LoadPos(area);
        }
        else if (level == 4)
        {
            SceneManager.LoadScene("Level 4");
            player.transform.position = LoadPos(area);
        }
    }

    public Vector3 LoadPos(int area)
    {
        if (area > 3 || area < 1)
        {
            Debug.LogError("Update gamestate area code out of bounds");
            return Vector3.negativeInfinity;
        }
        else
        {
            return sl.spawns[area - 1].transform.position;
        }
    }
}

