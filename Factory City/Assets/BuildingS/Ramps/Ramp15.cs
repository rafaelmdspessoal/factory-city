using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ramp15 : MonoBehaviour, IManipulable
{
    public void DestroySelf()
    {
        Ramps15.ramps15.Remove(transform);
        Destroy(transform.gameObject);
        print("Destroy");
    }

    public void CreateSelf()
    {
        Ramps15.ramps15.Add(transform);
        print("Create");
    }
}
