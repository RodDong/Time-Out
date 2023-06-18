using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Bonus : MonoBehaviour
{

    [SerializeField] public WallBreak wall;
    [SerializeField] public WallBreak wall2;
    // Start is called before the first frame update
    
    private void SaveBonusStatus() {
        print("saving");
        List<bool> bonus = new List<bool>();
        bonus.Add(wall != null);
        bonus.Add(wall2 != null);
        FileHandler.SaveToJSON<bool>(bonus, "level1bonus.json");
    }

    public List<bool> GetBonusStatus()
    {
        return new List<bool> { wall != null, wall2 != null };
    }

    public void OnTriggerEnter(Collider other) {
        SaveBonusStatus();
    }
}
