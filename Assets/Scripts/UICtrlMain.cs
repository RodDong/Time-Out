using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UICtrlMain : MonoBehaviour
{
    public MainMenuNew _mm;

    private GameObject main, lvlslt, complete, pause, help, loading;

    private GameObject returnTarget;

    [HideInInspector]
    public UnityEngine.UIElements.Button
        mainStart, mainHelp, mainCred,
        lvlsltBack,
        complBack, complCont,
        pauseCont, pauseRetry, pauseEnd,
        helpBack;

    [HideInInspector]
    public List<UnityEngine.UIElements.Button> lvlsltIcons;

    private void OnEnable()
    {
        main.SetActive(true);
        lvlslt.SetActive(true);
        complete.SetActive(true);
        pause.SetActive(true);
        help.SetActive(true);
        loading.SetActive(true);

        SetInteractive(lvlslt, false);
        SetInteractive(complete, false);
        SetInteractive(pause, false);
        SetInteractive(help, false);
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
        loading = transform.Find("loading").gameObject;

        main.SetActive(true);
        lvlslt.SetActive(true);
        complete.SetActive(true);
        pause.SetActive(true);
        help.SetActive(true);
        loading.SetActive(true);
    }

    void Start() 
    {
        DontDestroyOnLoad(gameObject);

        // Get button UI elements
        VisualElement mainRoot = main.GetComponent<UIDocument>().rootVisualElement;
        mainStart = mainRoot.Q<UnityEngine.UIElements.Button>("startbtn");
        mainHelp = mainRoot.Q<UnityEngine.UIElements.Button>("helpbtn");
        mainCred = mainRoot.Q<UnityEngine.UIElements.Button>("credbtn");

        VisualElement lvlsltRoot = lvlslt.GetComponent<UIDocument>().rootVisualElement;
        lvlsltBack = lvlsltRoot.Q<UnityEngine.UIElements.Button>("backbutton");
        //lvlsltIcons = lvlsltRoot.Q<VisualElement>("select").Children() as List<UnityEngine.UIElements.Button>;
        //lvlsltIcons = new List<UnityEngine.UIElements.Button>();
        //lvlsltRoot.Query<UnityEngine.UIElements.Button>(className: "levelicon").ToList(lvlsltIcons);

        VisualElement complRoot = complete.GetComponent<UIDocument>().rootVisualElement;
        complBack = complRoot.Q<UnityEngine.UIElements.Button>("backbutton");
        complCont = complRoot.Q<UnityEngine.UIElements.Button>("contbutton");

        VisualElement pauseRoot = pause.GetComponent<UIDocument>().rootVisualElement;
        pauseCont = pauseRoot.Q<UnityEngine.UIElements.Button>("contbtn");
        pauseRetry = pauseRoot.Q<UnityEngine.UIElements.Button>("retrybtn");
        pauseEnd = pauseRoot.Q<UnityEngine.UIElements.Button>("endbtn");

        VisualElement helpRoot = help.GetComponent<UIDocument>().rootVisualElement;
        helpBack = helpRoot.Q<UnityEngine.UIElements.Button>("backbutton");

        // Add on-click responses
        mainStart.clicked += () => LoadPage(main, lvlslt);
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
    }

    private void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name.Contains("Level"))
        {
            SetInteractive(main, false);
            SetInteractive(loading, false);
            SetInteractive(lvlslt, false);
            SetInteractive(complete, false);
            SetInteractive(pause, false);
            SetInteractive(help, false);
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
        SetInteractive(fromPage, false);
        SetInteractive(toPage, true);
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

    public void LoadHelp(GameObject fromPage)
    {
        // specify from which scene and UI screen was help loaded
        // pass this info to script dedicated to help screen

        returnTarget = fromPage;
        SetInteractive(fromPage, false);
        SetInteractive(help, true);
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
        Debug.Log("unimplemented");
    }

    public void Unpause()
    {
        Debug.Log("unimplemented");
    }

    public void Retry()
    {
        Debug.Log("unimplemented");
    }

    public void EndGame()
    {
        Debug.Log("unimplemented");
    }
}
