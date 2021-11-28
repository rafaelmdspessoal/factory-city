using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStorage 
{
    int AddResourceAmount(ResourceManager.ResourceType resourceType, int amount);

    int RemoveResourceAmount(ResourceManager.ResourceType resourceType, int amount);

    int GetResourceAmout(ResourceManager.ResourceType resourceType);

    void SetMaxResourceAmount(int amount);

    void SetAlowedResources(ResourceManager.ResourceType[] resourceTypes);
}
