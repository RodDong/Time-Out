using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4Bonus : MonoBehaviour
{
    [SerializeField] public Gate gate1;
    [SerializeField] public GameObject barrle;
    // Start is called before the first frame update

    private void SaveBonusStatus()
    {
        print("saving");
        List<bool> bonus = new List<bool>();
        bonus.Add(true);
        bonus.Add(!gate1.GetOpened());        // if true, has badge
        bonus.Add(barrle.transform.position.y >= 2);
        FileHandler.SaveToJSON<bool>(bonus, "level4bonus.json");
    }

    public List<bool> GetBonusStatus()
    {
        return new List<bool> { true, !gate1.GetOpened(), barrle.transform.position.y >= 2 };
    }

    public void OnTriggerEnter(Collider other)
    {
        SaveBonusStatus();
    }
}
