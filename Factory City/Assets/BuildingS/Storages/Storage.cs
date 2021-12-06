using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{ 
    [SerializeField] private Transform operatorSpot;
    [SerializeField] private Transform deliverySpot;
    [SerializeField] private Transform loadSpot;
    [SerializeField] private Transform fieldOfViewTransform;

    [SerializeField] private int maxStoredResources = 100;
    [SerializeField] private List<ResourceItem> storedResourceItems;
    [SerializeField] private int allowedDeliverers;

    [SerializeField] public Citizen storageOperator;
    [SerializeField] public int maxGatherersNumber;
    [SerializeField] public List<Citizen> storageGatherers;


    private void Awake()
    {
        storageGatherers = new List<Citizen>();
        storedResourceItems = new List<ResourceItem>();        
    }

    public void AddResourceItem(ResourceItem item)
    {
        int resourceAmount = GetResourceAmout();
        if (resourceAmount + item.amount <= maxStoredResources)
        {
            ResourceItem storedItem = HasResourceInStorage(item);
            if (storedItem != null)
            {
                storedItem.amount += item.amount;
                ResourceManager.AddResourceAmount(item.resourceScriptableObject, item.amount);
            }
            else
            {
                Debug.LogError("Item " + item.GetMaterialname() + " not found in Storage");
            }
        }
        else
        {
            Debug.LogError("Station is full");
            return;
        }
    }

    public void RemoveResourceItem(ResourceItem item)
    {
        int resourceAmount = GetResourceAmout();
        if (resourceAmount - item.amount >= 0)
        {
            ResourceItem storedItem = HasResourceInStorage(item);
            if (storedItem != null)
            {
                storedItem.amount -= item.amount;
                ResourceManager.RemoveResourceAmount(item.resourceScriptableObject, item.amount);
            }
            else
            {
                Debug.LogError("Item " + item.GetMaterialname() +  " not found in Storage");
            }
        }
        else
        {
            Debug.LogError("Not enough resources");
        }
    }

    public int GetResourceAmout(ResourceItem item)
    {
        int amount = 0;
        if (HasResourceInStorage(item) != null) amount += item.amount;
        return amount;
    }

    public int GetResourceAmout()
    {
        int amount = 0;
        foreach (ResourceItem resourceItem in storedResourceItems)
        {
            amount += resourceItem.amount;
        }
        return amount;
    }

    public void SetMaxResourceAmount(int amount)
    {
        maxStoredResources = amount;
    }

    public void AddNewResource(ResourceItem item)
    {
        if (HasResourceInStorage(item) != null) return;
        storedResourceItems.Add(CreateNewItemInstance(item));
    }

    private ResourceItem CreateNewItemInstance(ResourceItem item)
    {
        ResourceItem newItem = new ResourceItem();
        newItem.resourceScriptableObject = item.resourceScriptableObject;
        newItem.amount = 0;
        return newItem;
    }

    public ResourceItem HasResourceInStorage(ResourceItem item)
    {
        if (item == null) return null;
        for (int i = 0; i < storedResourceItems.Count; i++)
        {        
            if (item.resourceScriptableObject == storedResourceItems[i].resourceScriptableObject)
            {
                return storedResourceItems[i];
            }
        }
        Debug.LogError("Storage does NOT contain resource: " + item.GetMaterialname());
        return null;
    }

    public bool HasJobSpot() => (!HasOperator() || storageGatherers.Count < maxGatherersNumber);
    public Transform GetDeliverySpot() => deliverySpot;
    public Transform GetLoadSpot() => loadSpot;
    public Transform GetOperatorSpot() => operatorSpot;
    private bool HasOperator() => storageOperator != null;
    public bool HasGatherer() => storageGatherers.Count > 0;
    public int GetGathererNumber() => maxGatherersNumber;
    public bool HasResources() => GetResourceAmout() > 0;
    public bool IsInventoryFull() => GetResourceAmout() >= maxStoredResources;
    public List<ResourceItem> GetStoredResources() => storedResourceItems;
}
