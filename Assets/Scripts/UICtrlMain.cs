using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UICtrlMain : MonoBehaviour
{
    public MainMenuNew _mm;

    private GameObject main, lvlslt, complete, pause, help, help2, loading;

    private GameObject returnTarget;

    private VisualElement mainRoot, lvlsltRoot, complRoot, pauseRoot, helpRoot, help2Root;

    [HideInInspector]
    public UnityEngine.UIElements.Button
        mainStart, mainHelp, mainCred,
        lvlsltBack,
        complBack, complCont,
        pauseCont, pauseRetry, pauseEnd,
        helpBack, helpNext,
        help2Back, help2Prev;

    [HideInInspector]
    public List<UnityEngine.UIElements.Button> lvlsltIcons;

    private bool paused;

    private void OnEnable()
    {
        main.SetActive(true);
        lvlslt.SetActive(true);
        complete.SetActive(true);
        pause.SetActive(true);
        help.SetActive(true);
        help2.SetActive(true);
        loading.SetActive(true);

        SetInteractive(lvlslt, false);
        SetInteractive(complete, false);
        SetInteractive(pause, false);
        SetInteractive(help, false);
        SetInteractive(help2, false);
        SetInteractive(loading, false);
    }

    void Awake()
    {
        _mm = GameObject.FindObjectOfType<MainMenuNew>();

        // Find UI children gameobjects
        main = transform.Find("main").gameObject;
        lvlslt = transform.Find("level select").gameObject;
        complete = transform.Find("complete").gameObject;
        pause = transform.Find("pause").gameObject;
        help = transform.Find("help").gameObject;
        help2 = transform.Find("help2").gameObject;
        loading = transform.Find("loading").gameObject;

        main.SetActive(true);
        lvlslt.SetActive(true);
        complete.SetActive(true);
        pause.SetActive(true);
        help.SetActive(true);
        help2.SetActive(true);
        loading.SetActive(true);

        paused = false;
    }

    void Start() 
    {
        DontDestroyOnLoad(gameObject);

        // Get button UI elements
        mainRoot = main.GetComponent<UIDocument>().rootVisualElement;
        mainStart = mainRoot.Q<UnityEngine.UIElements.Button>("startbtn");
        mainHelp = mainRoot.Q<UnityEngine.UIElements.Button>("helpbtn");
        mainCred = mainRoot.Q<UnityEngine.UIElements.Button>("credbtn");

        lvlsltRoot = lvlslt.GetComponent<UIDocument>().rootVisualElement;
        lvlsltBack = lvlsltRoot.Q<UnityEngine.UIElements.Button>("backbutton");
        //lvlsltIcons = lvlsltRoot.Q<VisualElement>("select").Children() as List<UnityEngine.UIElements.Button>;
        //lvlsltIcons = new List<UnityEngine.UIElements.Button>();
        //lvlsltRoot.Query<UnityEngine.UIElements.Button>(className: "levelicon").ToList(lvlsltIcons);

        complRoot = complete.GetComponent<UIDocument>().rootVisualElement;
        complBack = complRoot.Q<UnityEngine.UIElements.Button>("backbutton");
        complCont = complRoot.Q<UnityEngine.UIElements.Button>("contbutton");

        pauseRoot = pause.GetComponent<UIDocument>().rootVisualElement;
        pauseCont = pauseRoot.Q<UnityEngine.UIElements.Button>("contbtn");
        pauseRetry = pauseRoot.Q<UnityEngine.UIElements.Button>("retrybtn");
        pauseEnd = pauseRoot.Q<UnityEngine.UIElements.Button>("endbtn");

        helpRoot = help.GetComponent<UIDocument>().rootVisualElement;
        helpBack = helpRoot.Q<UnityEngine.UIElements.Button>("backbutton");
        helpNext = helpRoot.Q<UnityEngine.UIElements.Button>("nexthelpbtn");

        help2Root = help2.GetComponent<UIDocument>().rootVisualElement;
        help2Back = help2Root.Q<UnityEngine.UIElements.Button>("backbutton");
        help2Prev = help2Root.Q<UnityEngine.UIElements.Button>("prevhelpbtn");

        // Add on-click responses
        mainStart.clicked += () => LoadPage(main, lvlslt);
        mainStart.clicked += () => _mm.NewGame();
        mainHelp.clicked += () => LoadPage(main, help);
        mainCred.clicked += () => LoadCred(main);

        lvlsltBack.clicked += () => GoBack(lvlslt);
        lvlsltRoot.Query<UnityEngine.UIElements.Button>(className: "levelicon").ForEach(btn =>
            btn.clicked += () => LoadLevel(lvlslt, int.Parse(btn.name.Substring(3)))
        ); 
        //for (int i = 0; i < lvlsltIcons.Count; i++)
        //{
        //    // verify this is correct
        //    UnityEngine.UIElements.Button btn = lvlsltIcons[i];
        //    //Debug.Log(btn.ToString());
        //    //btn.name.Substring(3);
        //    btn.clicked += () => LoadLevel(lvlslt, i + 1);
        //}
        //lvlsltIcons[0].clicked += () => LoadLevel(lvlslt, 1);

        complBack.clicked += () => LoadPage(complete, main);
        complCont.clicked += () => LoadNextLvl();

        pauseCont.clicked += () => Unpause();
        pauseRetry.clicked += () => Retry();
        pauseEnd.clicked += () => EndGame();

        helpBack.clicked += () => GoBack(help);
        helpNext.clicked += () => Help1to2();

        help2Back.clicked += () => GoBack(help);
        help2Prev.clicked += () => Help2to1();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Unpause();
        }
    }

    // unity built-in
    // called when a new scene finished loading

    private void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name.Contains("Level"))
        {
            SetInteractive(main, false);
            SetInteractive(lvlslt, false);
            SetInteractive(complete, false);
            SetInteractive(pause, false);
            SetInteractive(help, false);
            SetInteractive(loading, false);
        }
    }

    // For the gameobjects, make sure to set visible state as false whenever leaving its screen (including scene transitions)
    // This ensures only one UI child GO is interactable at a time

    public void SetInteractive(GameObject uigo, bool state)
    {
        if (state)
        {
            uigo.GetComponent<UIDocument>().rootVisualElement.style.display = DisplayStyle.Flex;
        } 
        else
        {
            uigo.GetComponent<UIDocument>().rootVisualElement.style.display = DisplayStyle.None;
        }
    }

    // this function is universal but no detail for specific scenarios, use mostly this for now
    public void LoadPage(GameObject fromPage, GameObject toPage)
    {
        returnTarget = fromPage;
        if (fromPage)
        {
            SetInteractive(fromPage, false);
        }

        if (toPage)
        {
            SetInteractive(toPage, true);
        }
        
    }

    public void LoadLevel(GameObject fromPage, int level)
    {
        if (_mm.LvlInBounds(level))
        {
            if (_mm.ContinueFromSaved(level))
            {
                // load level was successful
                Debug.Log("Loading level: " + level);
                SetInteractive(fromPage, false);
                SetInteractive(loading, true);
            }
            else
            {
                Debug.Log("Level " + level + " locked");
            }
        }
        else
        {
            Debug.LogWarning("Level " + level + " is not in range");
        }
    }

    public void LoadSelect(GameObject fromPage)
    {
        returnTarget = fromPage;
        SetInteractive(fromPage, false);
        SetInteractive(lvlslt, true);
    }

    // used for loading help from UI pages other than help2
    public void LoadHelp(GameObject fromPage)
    {
        // specify from which scene and UI screen was help loaded
        // pass this info to script dedicated to help screen

        returnTarget = fromPage;
        SetInteractive(fromPage, false);
        SetInteractive(help, true);
    }

    // below two used for switching between different pages of help
    public void Help2to1()
    {
        SetInteractive(help, true);
        SetInteractive(help2, false);
    }

    public void Help1to2()
    {
        SetInteractive(help, false);
        SetInteractive(help2, true);
    }

    public void LoadCred(GameObject fromPage)
    {
        //fromPage.SetActive(false);
        Debug.Log("unimplemented");
    }

    // return target should be non-null
    public void GoBack(GameObject fromPage)
    {
        if (!returnTarget)
        {
            Debug.LogError("Attempting to go back to a null UI gameObject");
            return;
        }
        SetInteractive(fromPage, false);
        SetInteractive(returnTarget, true);
        returnTarget = null;
    }

    public void LoadNextLvl()
    {
        if (SceneManager.GetActiveScene().name.Contains("Level"))
        {
            string curLevel = SceneManager.GetActiveScene().name;
            int curLevelNum = curLevel[curLevel.Length - 1] - '0';
            LoadLevel(complete, curLevelNum + 1);
        }
        else
        {
            Debug.LogError("Tried to Load Next Level in Main Menu");
        }

    }

    public void Pause()
    {
        if (!paused)
        {
            SetInteractive(pause, true);
            _mm.masterBus.setVolume(0.0f);
            Time.timeScale = 0;
            paused = true;
        }
    }

    public void Unpause()
    {
        if (paused)
        {
            SetInteractive(pause, false);
            _mm.masterBus.setVolume(1.0f);
            Time.timeScale = 1;
            paused = false;
        }
    }

    public void Retry()
    {
        string lastScene = SceneManager.GetActiveScene().name;
        if (!lastScene.Contains("Level")) {
            Debug.Log("Tried to Retry in Main Menu Scene");
        }
        else
        {
            /*_mm.ContinueFromSaved(lastScene[lastScene.Length - 1] - '0');*/
            LoadLevel(pause, lastScene[lastScene.Length - 1] - '0');
        }
        Time.timeScale = 1;
        paused = false;
    }

    // Returns from pause to main menu
    public void EndGame()
    {
        LoadPage(pause, main);
        Time.timeScale = 1;
        // TODO
    }

    // Updates progress indicators in level select
    // To be called whenever level select is loaded (made interactive)
    private void UpdateProgressVisuals()
    {
        void UpdateLevelSpecificVisuals(UnityEngine.UIElements.Button btn, int lvl)
        {
            if (lvl < 1 || lvl > 4)
            {
                Debug.LogError("Attempting to update progress visual for an invalid level: " + lvl);
                return;
            }
            // load progress from distinct files
            List<bool> lvlNbonus = FileHandler.ReadFromJSON<List<bool>>("level" + lvl + "bonus.json");
            // TODO: Get progress child of btn and for each child of progress, update its visual by index
            // where 0th is indicated in gameState (all true + next lvl 1st true if level complete)
            // and 1st 2nd are simply stored in the read booleans
        }

        lvlsltRoot.Query<UnityEngine.UIElements.Button>(className: "levelicon").ForEach(btn =>
            UpdateLevelSpecificVisuals(btn, int.Parse(btn.name.Substring(3)))
        );

}
