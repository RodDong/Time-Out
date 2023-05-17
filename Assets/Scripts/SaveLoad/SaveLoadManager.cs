using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

public class SaveLoadManager : MonoBehaviour
{
    // On collision, invoke UpdateGameState
    [SerializeField]
    private Collider coll1_2, coll1_3, coll2_2, coll2_3;

    [SerializeField]
    private GameState state;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        state = new GameState();
    }

    void OnLevelWasLoaded(int level)
    {
        string lastScene = SceneManager.GetActiveScene().name;
        if (lastScene == "Level 1")
        {
            UpdateGameState(1, 1);
            SaveGameState();
        }
        else if (lastScene == "Level 2")
        {
            UpdateGameState(2, 1);
            SaveGameState();
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
    public GameState()
    {
        lvl1 = new List<bool>(3);
        lvl2 = new List<bool>(3);
        lvl1.Add(true);
        lvl1.Add(false);
        lvl1.Add(false);
        lvl2.Add(false);
        lvl2.Add(false);
        lvl2.Add(false);
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
        else
        {
            Debug.LogError("Update gamestate level code out of bounds");
        }
    }
}