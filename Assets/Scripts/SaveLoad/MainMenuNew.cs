using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuNew : MonoBehaviour
{
    private SaveLoadManager sl;
    private GameObject player;

    public bool unlockAllLevels;

    public const float defaultVolume = 1.0f;
    string masterBusString = "bus:/";
    public FMOD.Studio.Bus masterBus { get; private set; }

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
        sl = GameObject.FindObjectOfType<SaveLoadManager>();
    }

    public void OnLevelWasLoaded(int level)
    {
        string lastScene = SceneManager.GetActiveScene().name;
        if (!lastScene.Contains("Level")) return; 
        player = GameObject.FindWithTag("Player");
        if (!player) Debug.LogWarning("player not found in this scene: " + lastScene);
    }

    public bool LvlInBounds(int level)
    {
        return 1 <= level && level <= 4;
    }

    public void NewGame()
    {
        sl.NewGame();
        SceneManager.LoadScene("Level 1");
    }

    public bool ContinueFromSaved(int level)
    {
        if (!LvlInBounds(level))
        {
            Debug.LogError("level code " + level + " out of bounds");
            return false;
        }
        int area = sl.GetStateProgress(level);
        // all areas are unlocked
        if (area == -1)
        {
            // if next level 1st area also unlocked, player passed this level before, play from start
            if (LvlInBounds(level+1) && sl.GetStateProgress(level+1) != 0)
            {
                ContinueGame(level, 1);
            }
            // else play from last area
            else
            {
                ContinueGame(level, 3);
            }
        }
        // level is locked
        else if (area == 0)
        {
            // dev option
            if (unlockAllLevels)
            {
                ContinueGame(level, 1);
            }
            else
            {
                return false;
            }
        }
        else if (area > 0)
        {
            // load latest area
            ContinueGame(level, area);
        }
        return true;
    }
    private IEnumerator LoadScene(string scene, int area)
    {
        // Start loading the scene
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
        // Wait until the level finish loading
        while (!asyncLoadLevel.isDone)
            yield return null;
        yield return sl.FetchSpawn();
        // Wait a frame so every Awake and Start method is called
        yield return new WaitForEndOfFrame();
        player = GameObject.FindWithTag("Player");
        if (!player) Debug.LogError("no player");
        player.transform.position = LoadPos(area);
        masterBus = FMODUnity.RuntimeManager.GetBus(masterBusString);
        if (SceneManager.GetActiveScene().name.Contains("Level"))
        {
            sl.BGM.start();
            sl.BGM.release();
        }
    }

    public void ContinueGame(int level, int area)
    {
        if (level == 1)
        {
            //SceneManager.LoadScene("Level 1");
            StartCoroutine(LoadScene("Level 1", area));
        }
        else if (level == 2)
        {
            StartCoroutine(LoadScene("Level 2", area));
        }
        else if (level == 3)
        {
            StartCoroutine(LoadScene("Level 3", area));
        }
        else if (level == 4)
        {
            StartCoroutine(LoadScene("Level 4", area));
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

