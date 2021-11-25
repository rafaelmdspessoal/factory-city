using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Door : MonoBehaviour, IManipulable
{
    public void DestroySelf()
    {
        Doors.doors.Remove(transform);
        Destroy(transform.gameObject);
        print("Destroy");
    }

    public void CreateSelf()
    {
        Doors.doors.Add(transform);
        print("Create");
    }
}
