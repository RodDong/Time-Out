using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderController : MonoBehaviour
{
    [SerializeField] GameObject goodLadder, brokenLadder;
    private void OnLevelWasLoaded(int level)
    {
        goodLadder.SetActive(false);
        brokenLadder.SetActive(false);
        List<bool> level1WallStatus = FileHandler.ReadListFromJSON<bool>("level1bonus.json");
        if (!level1WallStatus[0])
        {
            goodLadder.SetActive(true);
        }
        else
        {
            brokenLadder.SetActive(true);
        }
    }

}
