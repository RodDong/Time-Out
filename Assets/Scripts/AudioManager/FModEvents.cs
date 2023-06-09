using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEditor.PackageManager;

public class FModEvents : MonoBehaviour
{
    [field: Header("Walk_Grass SFX")]
    [field:SerializeField] public EventReference playerWalkGrass { get; private set; }
    [field: SerializeField] public EventReference playerWalkWater { get; private set; }
    [field: SerializeField] public EventReference playerWalkStair { get; private set; }
    [field: SerializeField] public EventReference playerWalkStone { get; private set; }
    [field: SerializeField] public EventReference playerWalkSand { get; private set; }
    [field: SerializeField] public EventReference barrelRoll { get; private set; }
    [field: SerializeField] public EventReference wallBreak { get; private set; }
    [field: SerializeField] public EventReference gateOpen { get; private set; }
    [field: SerializeField] public EventReference pushWoodBox { get; private set; }
    [field: SerializeField] public EventReference fire { get; private set; }
    [field: SerializeField] public EventReference button { get; private set; }
    [field: SerializeField] public EventReference lever { get; private set; }
    public static FModEvents instance { get; private set; }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Found more than one FMod Event instance");
        }
        instance = this;
    }
}
