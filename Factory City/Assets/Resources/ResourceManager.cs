using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourceManager 
{
    public static event EventHandler OnResourceAmountChanged;

    private static Dictionary<ResourceScriptableObject, int> resourceAmountDictionary;

    public static void Init()
    {
        resourceAmountDictionary = new Dictionary<ResourceScriptableObject, int>();
        foreach (ResourceScriptableObject resourceType in GameObject.Find("WindowGameResources").GetComponent<WindowGameResources>().GetResourceList())
        {
            resourceAmountDictionary[resourceType] = 0;
        }
    }

    public static void AddResourceAmount(ResourceScriptableObject resourceType, int amount)
    {
        resourceAmountDictionary[resourceType] += amount;
        if (OnResourceAmountChanged != null) OnResourceAmountChanged(null, EventArgs.Empty);
    }

    public static void RemoveResourceAmount(ResourceScriptableObject resourceType, int amount)
    {
        resourceAmountDictionary[resourceType] -= amount;
        if (OnResourceAmountChanged != null) OnResourceAmountChanged(null, EventArgs.Empty);
    }

    public static int GetResourceAmout(ResourceScriptableObject resourceType)
    {
        return resourceAmountDictionary[resourceType];
    }
}
