using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuN : MonoBehaviour
{
    [SerializeField]
    private Vector3 spawn1_1, spawn1_2, spawn1_3, spawn2_1, spawn2_2, spawn2_3;

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
            player.transform.position = LoadPos(level, area);
        }
        else if (level == 2)
        {
            SceneManager.LoadScene("Level 2");
            player.transform.position = LoadPos(level, area);
        }
    }

    public Vector3 LoadPos(int level, int area)
    {
        if (level == 1)
        {
            switch (area)
            {
                case 1:
                    return spawn1_1;
                case 2:
                    return spawn1_2;
                case 3:
                    return spawn1_3;
                default:
                    Debug.LogError("Update gamestate area code out of bounds");
                    return Vector3.negativeInfinity;
            }
        }
        else if (level == 2)
        {
            switch (area)
            {
                case 1:
                    return spawn2_1;
                case 2:
                    return spawn2_2;
                case 3:
                    return spawn2_3;
                default:
                    Debug.LogError("Update gamestate area code out of bounds");
                    return Vector3.negativeInfinity;
            }
        }
        else
        {
            Debug.LogError("Update gamestate level code out of bounds");
            return Vector3.negativeInfinity;
        }
    }
}

