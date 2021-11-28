using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResource 
{
    ResourceManager.ResourceType GetResourceType();

    int GetResourceAmount(ResourceManager.ResourceType type);

    void TakeResource(ResourceManager.ResourceType type, int amount);

    bool HasResource(ResourceManager.ResourceType type);
}
