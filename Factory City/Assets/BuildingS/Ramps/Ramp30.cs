using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ramp30 : MonoBehaviour, IManipulable
{
    public void DestroySelf()
    {
        Ramps30.ramps30.Remove(transform);
        Destroy(transform.gameObject);
        print("Destroy");
    }

    public void CreateSelf()
    {
        Ramps30.ramps30.Add(transform);
        print("Create");
    }
}