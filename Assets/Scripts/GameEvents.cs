using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake() {
        current = this;
    }
    
    public event Action<int> onTriggerEnter;
    public void TriggerEnter(int id) {
        if (onTriggerEnter != null) {
            onTriggerEnter(id);
        }
    }

    public event Action<int> onTriggerExit;
    public void TriggerExit(int id) {
        if (onTriggerExit != null) {
            onTriggerExit(id);
        }
    }
}
