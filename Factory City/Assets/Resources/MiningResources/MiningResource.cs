using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningResource : MonoBehaviour, IResource
{
    [SerializeField] private int resourceAmount;
    [SerializeField] private ResourceManager.ResourceType resourceType;


    public void SetResourceAmount(ResourceManager.ResourceType type, int amount)
    {
        if (type != resourceType) Debug.LogError("Wrong resource type.");
        resourceAmount = amount;
    }

    public ResourceManager.ResourceType GetResourceType()
    {
        return resourceType;
    }

    public int GetResourceAmount(ResourceManager.ResourceType type)
    {
        if (type != resourceType) Debug.LogError("Wrong resource type.");
        return resourceAmount;
    }

    public void TakeResource(ResourceManager.ResourceType type, int amount)
    {
        if (type != resourceType) Debug.LogError("Wrong resource type.");
        resourceAmount -= amount;
    }

    public bool HasResource(ResourceManager.ResourceType type)
    {
        return resourceAmount > 0;
    }

    public void SetResourceType(ResourceManager.ResourceType type)
    {
        resourceType = type;
    }
}
