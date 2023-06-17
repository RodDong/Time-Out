using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;

public class BackGroundMusic : MonoBehaviour
{

    private EventInstance BGM;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name.Contains("Level"))
        {
            BGM = AudioManager.instance.CreateEventInstance(FModEvents.instance.BackGroundMusic);
            BGM.start();
            BGM.release();
        }
    }

    private void OnDestroy()
    {
        BGM.stop(STOP_MODE.ALLOWFADEOUT);
    }
}
