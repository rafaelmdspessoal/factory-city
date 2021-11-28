using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private int resourceAmount;
    [SerializeField] private ResourceManager.ResourceType resourceType;

    public ResourceManager.ResourceType GetResrouceType()
    {
        return resourceType;
    }
}
