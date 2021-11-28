using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourcesScriptableObjects", menuName = "ScriptableObjects/ResourcesScriptableObjects")]
public class ResourcesScriptableObject : ScriptableObject
{
    public string Name;
    public Transform prefab;
    public ResourceManager.ResourceType resourceType;

    public List<ResourceManager.ResourceType> resource;
    public List<int> amount;

    //Unity doesn't know how to serialize a Dictionary
    private Dictionary<ResourceManager.ResourceType, int> recipe;


    public void OnAfterDeserialize()
    {
        recipe = new Dictionary<ResourceManager.ResourceType, int>();

        for (int i = 0; i != Math.Min(resource.Count, amount.Count); i++)
            recipe[resource[i]] = amount[i];
    }


    public Dictionary<ResourceManager.ResourceType, int> GetRecipe()
    {
        OnAfterDeserialize();
        return recipe;
    }

    public ResourceManager.ResourceType GetResouceType()
    {
        return resourceType;
    }
}
