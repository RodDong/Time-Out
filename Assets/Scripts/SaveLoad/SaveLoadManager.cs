using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.Data;

public class SaveLoadManager : MonoBehaviour
{
    public GameObject[] spawns { get; private set; }

    [SerializeField]
    private GameState state;
    private GameObject player;

    private int curLevelNum;
    private float distanceToTrigger = 5.0f;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        state = new GameState();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        SpawnUpdate(spawns);
    }

    private void SpawnUpdate(GameObject[] spawns)
    {
        for (int i = 0; i < 3; i++)
        {
            if (Vector3.Distance(player.transform.position, spawns[i].transform.position) < distanceToTrigger)
            {
                state.UpdateState(curLevelNum, i + 1);
            }
        }
        if (Vector3.Distance(player.transform.position, spawns[4].transform.position) < distanceToTrigger)
        {
            state.UpdateState(curLevelNum + 1, 1);
            //TODO: Portal Interaction
        }
    }

    void OnLevelWasLoaded(int level)
    {
        string lastScene = SceneManager.GetActiveScene().name;

        GameObject spawnParent = GameObject.FindGameObjectWithTag("Spawn");
        spawns = spawnParent.GetComponentsInChildren<GameObject>();

        if (lastScene == "Level 1")
        {
            UpdateGameState(1, 1);
            SaveGameState();
            curLevelNum = 1;
        }
        else if (lastScene == "Level 2")
        {
            UpdateGameState(2, 1);
            SaveGameState();
            curLevelNum = 2;
        }
        else if (lastScene == "Level 3")
        {
            UpdateGameState(3, 1);
            SaveGameState();
            curLevelNum = 3;
        }
        else if (lastScene == "Level 4")
        {
            UpdateGameState(4, 1);
            SaveGameState();
            curLevelNum = 4;
        }
    }

    public void NewGame()
    {
        state = new GameState();
        SaveGameState();
    }
    
    public void SaveGameState()
    {
        FileHandler.SaveToJSON<GameState>(state, "gamesave.json");
    }

    public GameState LoadGameState()
    {
        return FileHandler.ReadFromJSON<GameState>("gamesave.json");
    }

    // call this when collide with new area, with corresponding level and area numbers
    public void UpdateGameState(int level, int area)
    {
        state.UpdateState(level, area);
        SaveGameState();
    }
}

[Serializable]
public class GameState
{
    // unlocked areas for each level, add as we go
    public List<bool> lvl1;
    public List<bool> lvl2;
    public List<bool> lvl3;
    public List<bool> lvl4;
    public GameState()
    {
        lvl1 = new List<bool>(3);
        lvl2 = new List<bool>(3);
        lvl3 = new List<bool>(3);
        lvl4 = new List<bool>(3);
        lvl1.Add(true);
        lvl1.Add(false);
        lvl1.Add(false);
        lvl2.Add(false);
        lvl2.Add(false);
        lvl2.Add(false);
        lvl3.Add(false);
        lvl3.Add(false);
        lvl3.Add(false);
        lvl4.Add(false);
        lvl4.Add(false);
        lvl4.Add(false);

    }

    // Set the unlocked status of the level area
    // PREREQ: level area exist
    public void UpdateState(int level, int area)
    {
        int i = area - 1;
        if (level == 1)
        {
            if (0 <= i && i <= 2)
            {
                lvl1[i] = true;
            }
            else
            {
                Debug.LogError("Update gamestate area code out of bounds");
            }
        }
        else if (level == 2)
        {
            if (0 <= i && i <= 2)
            {
                lvl2[i] = true;
            }
            else
            {
                Debug.LogError("Update gamestate area code out of bounds");
            }
        }
        else if (level == 3)
        {
            if (0 <= i && i <= 2)
            {
                lvl3[i] = true;
            }
            else
            {
                Debug.LogError("Update gamestate area code out of bounds");
            }
        }
        else if (level == 4)
        {
            if (0 <= i && i <= 2)
            {
                lvl4[i] = true;
            }
            else
            {
                Debug.LogError("Update gamestate area code out of bounds");
            }
        }
        else
        {
            Debug.LogError("Update gamestate level code out of bounds");
        }
    }
}