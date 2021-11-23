using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogResource : MonoBehaviour
{
    public int resourceAmount;
    public ResourceManager.ResourceType resourceType;

    private void Awake()
    {
        resourceType = ResourceManager.ResourceType.Log;   
    }
}
