using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wall : MonoBehaviour, IManipulable
{
    public void DestroySelf()
    {
        Walls.walls.Remove(transform);
        Destroy(transform.gameObject);
        print("Destroy");
    }

    public void CreateSelf()
    {
        Walls.walls.Add(transform);
        print("Create");
    }
}
