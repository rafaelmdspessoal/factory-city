using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeScriptableObject", menuName = "ScriptableObjects/RecipeScriptableObjects")]
public class RecipeScriptableObject : ScriptableObject
{
    public ResourceItem[] outputResources;
    public ResourceItem[] inputResources;
    public int craftingTime;

    public bool HasMaterialInInput(ResourceItem item)
    {
        foreach(ResourceItem materialItem in inputResources)
        {
            if(item.resourceScriptableObject == materialItem.resourceScriptableObject)
            {
                return true;
            }
        }
        return false;
    }

    public int GetInputResourceAmount(ResourceItem item)
    {
        foreach (ResourceItem materialItem in inputResources)
        {
            if (item.resourceScriptableObject == materialItem.resourceScriptableObject)
            {
                return materialItem.amount;
            }
        }
        return 0;
    }

    public int GetOutputResourceAmount(ResourceItem item)
    {
        foreach (ResourceItem materialItem in outputResources)
        {
            if (item.resourceScriptableObject == materialItem.resourceScriptableObject)
            {
                return materialItem.amount;
            }
        }
        return 0;
    }

    public bool HasMaterialInOutput(ResourceItem item)
    {
        foreach (ResourceItem materialItem in outputResources)
        {
            if (item.resourceScriptableObject == materialItem.resourceScriptableObject)
            {
                return true;
            }
        }
        return false;
    }

    public bool CanCraftResourceItem(List<ResourceItem> itemsInStorage)
    {
        if (itemsInStorage.Count <= 0) return false;
        bool hasItem = false;
        foreach (ResourceItem materialItem in inputResources)
        {
            foreach (ResourceItem item in itemsInStorage)
            {
                if (item.resourceScriptableObject == materialItem.resourceScriptableObject)
                {
                    if (item.amount >= materialItem.amount)
                    {
                        hasItem = true;
                    }
                }
            }
            if (!hasItem) return hasItem;
        }
        return hasItem;
    }
}
