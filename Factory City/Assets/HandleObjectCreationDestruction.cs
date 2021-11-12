using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class HandleObjectCreationDestruction : MonoBehaviour, IManipulable
{
    public void DestroySelf()
    {
        Destroy(transform.gameObject);
        print("Destroy");
    }

    public void CreateSelf()
    {
        print("Create");
    }
}
