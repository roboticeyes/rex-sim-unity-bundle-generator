using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    Person[] personsInScene;
    private void Start()
    {
        personsInScene = FindObjectsOfType<Person>();

        foreach (var p in personsInScene)
        {
            p.Initialize();
        }
    }

    public void OnSpeedUpdated (float value)
    {
        foreach (var p in personsInScene)
        {
            p.UpdateAnimation (value);
        }
    }
}
