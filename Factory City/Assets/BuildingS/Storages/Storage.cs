using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour, IStorage, IManipulable
{
    public Transform deliverySpot;

    [SerializeField] private int maxResourceAmount;
    [SerializeField] private int resourceAmount;
    [SerializeField] private ResourceManager.ResourceType[] acceptedResourceType;
    [SerializeField] private int allowedDeliverers;


    public void LoadResource(int amount, ResourceManager.ResourceType resourceType)
    {
        if (resourceAmount + amount <= maxResourceAmount)
        {
            resourceAmount += amount;
            ResourceManager.AddResourceAmount(ResourceManager.ResourceType.Log, amount);
        }
    }

    public void UnloadResource(int amount, ResourceManager.ResourceType resourceType)
    {
        if (resourceAmount + amount >= 0)
        {
            resourceAmount -= amount;
            ResourceManager.RemoveResourceAmount(ResourceManager.ResourceType.Log, amount);
        }
    }

    public void SetMaxResourceAmount(int amount)
    {

    }

    public void SetResourcesToHold(ResourceManager.ResourceType[] resourceTypes)
    {

    }

    public void CreateSelf()
    {

    }

    public void DestroySelf()
    {
        
    }
}
