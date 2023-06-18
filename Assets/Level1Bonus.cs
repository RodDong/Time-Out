using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Bonus : MonoBehaviour
{

    [SerializeField] public WallBreak wall;
    [SerializeField] public WallBreak wall2;
    // Start is called before the first frame update
    
    public void SaveBonusStatus() {
        print("saving");
        List<bool> bonus = new List<bool>();
        bonus.Add(wall != null);
        bonus.Add(wall2 != null);
        FileHandler.SaveToJSON<List<bool>>(bonus, "level1bonus.json");
    }

    public void OnTriggerEnter(Collider other) {
        SaveBonusStatus();
    }
}
