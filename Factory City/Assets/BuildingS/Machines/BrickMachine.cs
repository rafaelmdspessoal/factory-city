using System;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class BrickMachine : MonoBehaviour, IMachine, IManipulable, IHaveWorkers
{
    [SerializeField] private Transform operatorSpot;
    [SerializeField] private Transform carrierSpot;
    [SerializeField] private ResourcesScriptableObject resourceToCraft;
    [SerializeField] private int numWorkers;
    [SerializeField] private int numAcceptableResources;
    [SerializeField] private int numProducedResources;
    [SerializeField] private Citizen[] workers;
    [SerializeField] private int maxResourceAmount;
    [SerializeField] private ResourceManager.ResourceType[] acceptableResourceTypes;
    [SerializeField] private ResourceManager.ResourceType[] producedResourceTypes;
    [SerializeField] private Transform[] acceptableResourceObjects;
    [SerializeField] private int caftingTime;
    [SerializeField] private float elapsedCraftTime = 0f;
    [SerializeField] private List<UnloadStation> unloadStations;
    [SerializeField] private List<LoadStation> loadStations;

    [SerializeField] private Transform actionArea;

    [SerializeField] private List<Transform> stationsInReach;
    private List<Transform> resourcesInReach;

    private Dictionary<ResourceManager.ResourceType, int> resourceAmountDictionary;
    [SerializeField] private bool canCraftItem = false;

    void Start()
    {
        resourceAmountDictionary = new Dictionary<ResourceManager.ResourceType, int>();
        foreach (ResourceManager.ResourceType resourceType in System.Enum.GetValues(typeof(ResourceManager.ResourceType)))
        {
            resourceAmountDictionary[resourceType] = 0;
        }

        acceptableResourceTypes = new ResourceManager.ResourceType[numAcceptableResources];
        acceptableResourceTypes[0] = ResourceManager.ResourceType.Stone;

        producedResourceTypes = new ResourceManager.ResourceType[numProducedResources];
        producedResourceTypes[0] = ResourceManager.ResourceType.Brick;

        workers = new Citizen[numWorkers];
        JobManager.AddJobSpot(numWorkers, this.transform);

        resourcesInReach = new List<Transform>();
        stationsInReach = new List<Transform>();
    }

    void Update()
    {
        if (canCraftItem)
        {
            elapsedCraftTime += Time.deltaTime;
            if (elapsedCraftTime >= caftingTime)
            {                
                elapsedCraftTime = 0f;
                CraftItem(resourceToCraft.GetRecipe());
            }
        }
    }

    private void CraftItem(Dictionary<ResourceManager.ResourceType, int> requiredResources)
    {
        if(requiredResources.Keys.Count == 0)
        {
            Debug.LogError("Recipe Not Set!");
            canCraftItem = false;
            return;
        }
        foreach (ResourceManager.ResourceType resource in requiredResources.Keys)
        {
            resourceAmountDictionary[resource] -= requiredResources[resource];
            resourceAmountDictionary[resourceToCraft.GetResouceType()] += 4;
            CanCraftItem(requiredResources);
        }
    }

    void CanCraftItem(Dictionary<ResourceManager.ResourceType, int> requiredResources)
    {
        canCraftItem = true;
        foreach (ResourceManager.ResourceType resource in requiredResources.Keys)
        {
            if (resourceAmountDictionary[resource] < requiredResources[resource])
                canCraftItem = false;
        }
    }

    public void Hire(Citizen citizen)
    {
        for (int i = 0; i < numWorkers; i++)
        {
            if (workers[i] == null)
            {
                workers[i] = citizen;
                if (i == 0) workers[i].gameObject.AddComponent<MachineOperator>();
                else if (i == 1)
                {
                    workers[i].gameObject.AddComponent<Carrier>();
                    Carrier carrier = workers[i].GetComponent<Carrier>();
                    RequestResources(carrier);
                    CanCraftItem(resourceToCraft.GetRecipe());
                    carrier.OnResourceDelivered += delegate (object sender, EventArgs e)
                    {
                        RequestResources(carrier);
                        CanCraftItem(resourceToCraft.GetRecipe());
                    }; 
                }
                else
                {
                    workers[i].gameObject.AddComponent<Miner>();
                    workers[i].GetComponent<Miner>().SetResourceTypeObject(acceptableResourceObjects[0]);
                }

                break;
            }
        }
        JobManager.RemoveJobSpot(1, transform);
        print("Worker Hired");
    }

    private void RequestResources(Carrier carrier)
    {
        int amount = 0;
        ResourceManager.ResourceType typeToRequest = ResourceManager.ResourceType.Stone;

        foreach (ResourceManager.ResourceType type in producedResourceTypes)
        {            
            if (GetResourceAmout(type) >= carrier.GetMaxInventoryResourceAmount())
            {
                typeToRequest = type;
                carrier.PickupCraftedResources(type);
                return;
            }
        }

        
        foreach (ResourceManager.ResourceType type in acceptableResourceTypes)
        {
            if (amount < GetResourceAmout(type))
            {
                amount = GetResourceAmout(type);
                typeToRequest = type;
            }
        }
        carrier.GetNeededResource(typeToRequest);
    }

    public void Fire(Citizen citizen)
    {
        for (int i = 0; i < numWorkers; i++)
        {
            if (workers[i] == citizen) workers[i] = null;
        }
        JobManager.AddJobSpot(1, transform);
        print("Worker Fired");
    }

    public bool HasJobSpot()
    {
        for (int i = 0; i < numWorkers; i++)
        {
            if (workers[i] == null) return true;
        }
        return false;
    }

    public void CreateSelf()
    {
        FunctionTimer.Create(AddStationsInReach, 5);
    }

    public void DestroySelf()
    {
        for (int i = 0; i < numWorkers; i++)
        {
            workers[i] = null;
        }
        JobManager.RemoveJobSpot(1, transform);
        print("Facility destroyed");
    }

    public void GetStationsInReach(Transform station)
    {
        stationsInReach.Add(station);
    }

    public void RemoveStationsOutOfReach(Transform station)
    {
        stationsInReach.Remove(station);
    }

    public void AddStationsInReach()
    {
        foreach (Transform station in stationsInReach)
        {
            if (station.TryGetComponent<LoadStation>(out LoadStation loadStation))
            {
                loadStations.Add(loadStation);
            }
            else if (station.TryGetComponent<UnloadStation>(out UnloadStation unloadStation))
            {
                unloadStations.Add(unloadStation);
            }
        }
        stationsInReach.Clear();
    }

    public LoadStation GetLoadStation()
    {
        return loadStations[0];
    }

    public UnloadStation GetUnloadStation()
    {
        return unloadStations[0];
    }

    public void AddResourcesInReach(ResourceManager.ResourceType resourceType, Transform resorceTransform)
    {
        resourcesInReach.Add(resorceTransform);
    }

    public void RemoveResourcesOutOfReach(ResourceManager.ResourceType resourceType, Transform resorceTransform)
    {
        resourcesInReach.Remove(resorceTransform);
    }

    public List<Transform> GetResourcesInReach(ResourceManager.ResourceType resourceType)
    {
        foreach(ResourceManager.ResourceType type in acceptableResourceTypes)
        {
            if (type == resourceType)
            {
                return resourcesInReach;
            }
        }
        Debug.LogError("Not the same Resource Type!");
        return null;
    }
       
    public Transform GetOperatorSpot()
    {
        return operatorSpot;
    }

    public Transform GetCarrierSpot()
    {
        return carrierSpot;
    }

    private bool HasOperator()
    {
        return workers[0] != null;
    }

    private bool HasCarrier()
    {
        return workers[1] != null;
    }

    public ResourceManager.ResourceType[] GetAcceptedResourceTypes()
    {
        return acceptableResourceTypes;
    }

    public ResourceManager.ResourceType[] GetCraftedResourceTypes()
    {
        return producedResourceTypes;
    }

    public int AddResourceAmount(ResourceManager.ResourceType resourceType, int amount)
    {
        int resourceAmount = GetResourceAmout(resourceType);
        if (resourceAmount + amount <= maxResourceAmount)
        {
            resourceAmountDictionary[resourceType] += amount;
            ResourceManager.AddResourceAmount(ResourceManager.ResourceType.Stone, amount);
            return 0;
        }
        else
        {
            int remainingResourcesSlot = resourceAmount - amount;
            resourceAmountDictionary[resourceType] += remainingResourcesSlot;
            ResourceManager.RemoveResourceAmount(ResourceManager.ResourceType.Stone, remainingResourcesSlot);
            return amount - remainingResourcesSlot;
        }
    }

    public int RemoveResourceAmount(ResourceManager.ResourceType resourceType, int amount)
    {
        int resourceAmount = GetResourceAmout(resourceType);
        if (resourceAmount - amount > 0)
        {
            resourceAmountDictionary[resourceType] -= amount;
            ResourceManager.RemoveResourceAmount(ResourceManager.ResourceType.Brick, amount);
            return amount;
        }
        else
        {
            int remainingResources = resourceAmount;
            resourceAmountDictionary[resourceType] -= remainingResources;
            ResourceManager.RemoveResourceAmount(ResourceManager.ResourceType.Brick, remainingResources);
            return remainingResources;
        }
    }

    public int GetResourceAmout(ResourceManager.ResourceType resourceType)
    {
        return resourceAmountDictionary[resourceType];
    }
}
