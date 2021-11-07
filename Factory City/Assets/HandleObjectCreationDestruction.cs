using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleObjectCreationDestruction : MonoBehaviour, IManipulable
{
    public void DestroySelf()
    {
        Destroy(transform.parent.gameObject);
        print("Destroy");
    }

    public void CreateSelf()
    {
        print("Create");
    }
}
