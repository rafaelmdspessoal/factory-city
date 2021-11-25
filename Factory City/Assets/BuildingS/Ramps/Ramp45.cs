using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ramp45 : MonoBehaviour, IManipulable
{
    public void DestroySelf()
    {
        Ramps45.ramps45.Remove(transform);
        Destroy(transform.gameObject);
        print("Destroy");
    }

    public void CreateSelf()
    {
        Ramps45.ramps45.Add(transform);
        print("Create");
    }
}