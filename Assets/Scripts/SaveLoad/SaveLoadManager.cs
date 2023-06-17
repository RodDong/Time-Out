using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.Data;
using FMOD.Studio;

public class SaveLoadManager : MonoBehaviour
{
    public GameObject[] spawns { get; private set; }
    // TO BE UPDATED AS LEVELS ARE ADDED
    public int SPAWNCNT = 12;

    [SerializeField]
    private GameState state;
    private GameObject player;
    private UICtrlMain uiCtrlMain;
    private GameObject complete;

    public EventInstance BGM { get; private set; }

    private int curLevelNum;
    private float distanceToTrigger = 5.0f;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        LoadGameStateFromFile();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name.Contains("Level") && spawns != null)
        {
            SpawnUpdate(spawns);
        }
    }

    private void SpawnUpdate(GameObject[] spawns)
    {
        for (int i = 0; i < 3; i++)
        {
            if (Vector3.Distance(player.transform.position, spawns[i].transform.position) < distanceToTrigger)
            {
                UpdateGameState(curLevelNum, i + 1);
            }
        }

        //Portal
        if (Vector3.Distance(player.transform.position, spawns[3].transform.position) < distanceToTrigger)
        {

            UpdateGameState(curLevelNum + 1, 1);
            BGM.stop(STOP_MODE.ALLOWFADEOUT);
            uiCtrlMain.LoadPage(null, complete);
        }
    }

    public IEnumerator FetchSpawn()
    {
        string lastScene = SceneManager.GetActiveScene().name;
        if (!lastScene.Contains("Level")) yield break;
        GameObject spawnParent = GameObject.FindGameObjectWithTag("Spawn");
        //Debug.Log(spawnParent.transform.childCount);
        spawns = new GameObject[SPAWNCNT];
        for (int i = 0; i < spawnParent.transform.childCount; i++)
        {
            spawns[i] = spawnParent.transform.GetChild(i).gameObject;
        }
        player = GameObject.FindGameObjectWithTag("Player");
        uiCtrlMain = GameObject.FindFirstObjectByType<UICtrlMain>();
        complete = uiCtrlMain.transform.Find("complete").gameObject;
        BGM = AudioManager.instance.CreateEventInstance(FModEvents.instance.BackGroundMusic);
        yield return new WaitForEndOfFrame();
    }

    void OnLevelWasLoaded(int level)
    {
        string lastScene = SceneManager.GetActiveScene().name;
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

    public int GetStateProgress(int level)
    {
        return state.GetProgress(level);
    }

    private void LoadGameStateFromFile()
    {
        state = FileHandler.ReadFromJSON<GameState>("gamesave.json");
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

    // Get the progress in current level
    public int GetProgress(int level)
    {
        if (level == 1)
        {
            // first area encountered that still has the next area locked
            // -1 -> all areas unlocked, 0 -> no area is unlocked (level locked)
            return lvl1.FindIndex(b => !b);
        }
        else if (level == 2)
        {
            return lvl2.FindIndex(b => !b);
        }
        else if (level == 3)
        {
            return lvl3.FindIndex(b => !b);
        }
        else if (level == 4)
        {
            return lvl4.FindIndex(b => !b);
        }
        else
        {
            Debug.LogError("Update gamestate level code out of bounds");
            return -2;
        }
    }
}