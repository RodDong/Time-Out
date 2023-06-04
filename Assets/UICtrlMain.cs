using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UICtrlMain : MonoBehaviour
{
    [HideInInspector] public UnityEngine.UIElements.Button startbtn, helpbtn, credbtn;

    // Start is called before the first frame update
    void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        startbtn = root.Q<UnityEngine.UIElements.Button>("startbtn");
        helpbtn = root.Q<UnityEngine.UIElements.Button>("helpbtn");
        credbtn = root.Q<UnityEngine.UIElements.Button>("credbtn");

        startbtn.clicked += () => LoadSelect();
        helpbtn.clicked += () => LoadHelp();
        credbtn.clicked += () => LoadCred();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadSelect()
    {

    }

    public void LoadHelp()
    {
        // specify from which scene and UI screen was help loaded
        // pass this info to script dedicated to help screen

    }

    public void LoadCred()
    {

    }
}
