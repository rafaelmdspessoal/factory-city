using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStorage 
{
    void LoadResource(int amount, ResourceManager.ResourceType resourceType);

    void UnloadResource(int amount, ResourceManager.ResourceType resourceType);

    void SetMaxResourceAmount(int amount);

    void SetResourcesToHold(ResourceManager.ResourceType[] resourceTypes);
}
