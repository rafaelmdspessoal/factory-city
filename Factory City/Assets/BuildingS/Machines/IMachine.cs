using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMachine 
{
    LoadStation GetLoadStation();

    UnloadStation GetUnloadStation();

    void AddStationsInReach();
    void RemoveStationsOutOfReach(Transform station);

    void GetStationsInReach(Transform station);

    List<Transform> GetResourcesInReach(ResourceManager.ResourceType resourceType);

    public void RemoveResourcesOutOfReach(ResourceManager.ResourceType resourceType, Transform resorceTransform);

    int AddResourceAmount(ResourceManager.ResourceType resourceType, int amount);

    int RemoveResourceAmount(ResourceManager.ResourceType resourceType, int amount);

    int GetResourceAmout(ResourceManager.ResourceType resourceType);
}

