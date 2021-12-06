using System.Collections.Generic;
using UnityEngine;


public class Platform : MonoBehaviour, IManipulable
{
    public void DestroySelf()
    {
        Platforms.platforms.Remove(transform);
        Destroy(transform.gameObject);
        print("Destroy");
    }

    public void CreateSelf()
    {
        Platforms.platforms.Add(transform);
        print("Create");
    }
}
