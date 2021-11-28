using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour, IStorage, IManipulable
{
    [SerializeField] private Transform deliverySpot;
    [SerializeField] private Transform operatorSpot;
    [SerializeField] private Transform loadSpot;

    [SerializeField] private int maxResourceAmount;
    [SerializeField] private ResourceManager.ResourceType[] acceptedResourceType;
    [SerializeField] private ResourcesScriptableObject[] storedResourceObjects;
    [SerializeField] private int allowedDeliverers;

    [SerializeField] public Citizen worker;

    [SerializeField] private Dictionary<ResourceManager.ResourceType, int> resourceAmountDictionary;

    private void Awake()
    {
        resourceAmountDictionary = new Dictionary<ResourceManager.ResourceType, int>();
        foreach (ResourceManager.ResourceType resourceType in System.Enum.GetValues(typeof(ResourceManager.ResourceType)))
        {
            resourceAmountDictionary[resourceType] = 0;
        }
    }


    public ResourceManager.ResourceType[] GetAcceptedResourceTypes()
    {
        return acceptedResourceType;
    }

    public ResourcesScriptableObject[] GetStoredResourceObjects()
    {
        return storedResourceObjects;
    }

    public int AddResourceAmount(ResourceManager.ResourceType resourceType, int amount)
    {
        int resourceAmount = GetResourceAmout(resourceType);
        if (resourceAmount + amount <= maxResourceAmount)
        {
            resourceAmountDictionary[resourceType] += amount;
            ResourceManager.AddResourceAmount(ResourceManager.ResourceType.Log, amount);
            return 0;
        }
        else
        {
            int remainingResourcesSlot = resourceAmount - amount;
            resourceAmountDictionary[resourceType] += remainingResourcesSlot;
            ResourceManager.RemoveResourceAmount(ResourceManager.ResourceType.Log, remainingResourcesSlot);
            return amount -remainingResourcesSlot;
        }
    }

    public int RemoveResourceAmount(ResourceManager.ResourceType resourceType, int amount)
    {
        int resourceAmount = GetResourceAmout(resourceType);
        if (resourceAmount - amount > 0)
        {
            resourceAmountDictionary[resourceType] -= amount;
            ResourceManager.RemoveResourceAmount(ResourceManager.ResourceType.Log, amount);
            return amount;
        }
        else
        {
            int remainingResources = resourceAmount;
            resourceAmountDictionary[resourceType] -= remainingResources;
            ResourceManager.RemoveResourceAmount(ResourceManager.ResourceType.Log, remainingResources);
            return remainingResources;
        }
    }

    public int GetResourceAmout(ResourceManager.ResourceType resourceType)
    {
        return resourceAmountDictionary[resourceType];
    }


    public void SetMaxResourceAmount(int amount)
    {

    }

    public void SetAlowedResources(ResourceManager.ResourceType[] resourceTypes)
    {

    }

    public void CreateSelf()
    {

    }

    public void DestroySelf()
    {
        
    }

    

    public bool HasJobSpot()
    {
        return worker == null;
    }

    public Transform GetDeliverySpot()
    {
        return deliverySpot;
    }

    public Transform GetLoadSpot()
    {
        return loadSpot;
    }

    public Transform GetOperatorSpot()
    {
        return operatorSpot;
    }

    private bool HasOperator()
    {
        return worker != null;
    }
}
