using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Bonus : MonoBehaviour
{
    [SerializeField] public Lever lever1;
    // Start is called before the first frame update

    private void SaveBonusStatus()
    {
        print("saving");
        List<bool> bonus = new List<bool>();
        bonus.Add(true);
        bonus.Add(!lever1.interacted);
        bonus.Add(true);
        FileHandler.SaveToJSON<bool>(bonus, "level2bonus.json");
    }

    public List<bool> GetBonusStatus()
    {
        return new List<bool> { true, !lever1.interacted, true };
    }

    public void OnTriggerEnter(Collider other)
    {
        SaveBonusStatus();
    }
}
