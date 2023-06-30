using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Bonus : MonoBehaviour
{
    [SerializeField] public WallBreak wall;
    // Start is called before the first frame update

    private void SaveBonusStatus()
    {
        print("saving");
        List<bool> bonus = new List<bool>();
        bonus.Add(true);
        bonus.Add(wall == null);        // if true, has badge
        bonus.Add(true);
        FileHandler.SaveToJSON<bool>(bonus, "level3bonus.json");
    }

    public List<bool> GetBonusStatus()
    {
        return new List<bool> { true, wall == null, true };
    }

    public void OnTriggerEnter(Collider other)
    {
        SaveBonusStatus();
    }
}
