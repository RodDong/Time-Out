using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEditor.PackageManager;

public class FModEvents : MonoBehaviour
{
    [field: Header("Walk_Grass SFX")]
    [field:SerializeField] public EventReference playerWalkGrass { get; private set; }

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
