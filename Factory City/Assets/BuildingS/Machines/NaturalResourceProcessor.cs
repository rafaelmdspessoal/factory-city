using System;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class NaturalResourceProcessor : ResourceProcessor, IHaveWorkers, IMachine
{
    [SerializeField] private Transform carrierSpot;
    [SerializeField] private Transform actionAreaTransform;

    [SerializeField] private List<ResourceItemObject> resourcesInReach;
    [SerializeField] private List<Transform> resouceContainersInReach;

    [SerializeField] private int maxResourceGatherers = 1;
    [SerializeField] private List<Citizen> resourceGatherers;
    [SerializeField] private int maxResourceCarriers = 1;
    [SerializeField] private List<Citizen> resourceCarriers;

    private ResourceProcessor resourceProcessor;

    public override void Awake()
    {
        base.Awake();
        resourceProcessor = GetComponent<ResourceProcessor>();
        resourcesInReach = new List<ResourceItemObject>();
        resouceContainersInReach = new List<Transform>();

        resourceGatherers = new List<Citizen>();
        resourceCarriers = new List<Citizen>();

    }

    public override void Start()
    {
        base.Start();
        AddResourcesInReach();
        OpenJobSpots();
        actionAreaTransform.GetComponent<ObjectsInReach>().OnObjectAdded += delegate (object sender, EventArgs e)
        {
            AddResourcesInReach();
        };
        GetFieldOfViewTransform().GetComponent<StoragesInReach>().OnObjectAdded += delegate (object sender, EventArgs e)
        {
            OpenJobSpots();
        };
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    private void AddResourcesInReach()
    {
        foreach (Transform obj in actionAreaTransform.GetComponent<ObjectsInReach>().GetObjectsInReach())
        {
            if (obj == null) continue;
            if (obj.TryGetComponent<ResourceItemObject>(out ResourceItemObject materialItemObject))
            {
                foreach (ResourceItem resourceItem in GetRecipe().inputResources)
                {
                    if (materialItemObject.GetMaterialItem().resourceScriptableObject == resourceItem.resourceScriptableObject)
                    {
                        if (!resourcesInReach.Contains(materialItemObject))
                        {
                            resourcesInReach.Add(materialItemObject);
                        }
                    }
                }
            }
            if (obj.TryGetComponent<ResourceContainer>(out ResourceContainer resourceContainer))
            {
                if (!GetRecipe().HasMaterialInInput(resourceContainer.GetResourceItem())) return;
                if (!resouceContainersInReach.Contains(resourceContainer.transform))
                {
                    obj.GetComponent<ResourceContainer>().OnResourceContainerDestroyed += delegate (object sender, EventArgs e)
                    {
                        RemoveGatheredResourceFromReach(resourceContainer.transform);
                    };
                    resouceContainersInReach.Add(resourceContainer.transform);
                }
            }
        }
    }

    public Transform GetResourceToGather()
    {
        if (resourcesInReach.Count == 0)
        {
            int index = UnityEngine.Random.Range(0, resourcesInReach.Count);
            try
            {
                Transform resouceContainer = resouceContainersInReach[index];
                if (resouceContainer != null) return resouceContainer; else resouceContainersInReach.Remove(resouceContainer);
            }
            catch (MissingReferenceException)
            {
                resouceContainersInReach.Remove(resouceContainersInReach[index]);
            }
            return null;
        }
        else
        {
            int index = UnityEngine.Random.Range(0, resourcesInReach.Count);
            try
            {
                Transform resource = resourcesInReach[index].transform;
                if (resource != null) return resource; else resourcesInReach.Remove(resourcesInReach[index]);
            }
            catch (MissingReferenceException)
            {
                resourcesInReach.Remove(resourcesInReach[index]);
            }
        }
        return null;
    }

    public void RemoveGatheredResourceFromReach(Transform materialItem)
    {
        if (materialItem.TryGetComponent<ResourceItemObject>(out ResourceItemObject materialItemObject))
        {
            resourcesInReach.Remove(materialItemObject);
        }
        else if (materialItem.TryGetComponent<ResourceContainer>(out ResourceContainer resourceContainer))
        {
            resouceContainersInReach.Remove(resourceContainer.transform);
        }
    }

    public bool HasResourceToGather()
    {
        return (resouceContainersInReach.Count > 0 || resourcesInReach.Count > 0);
    }

    public void Hire(Citizen citizen)
    {
        if (!HasOperator())
        {
            SetOperator(citizen);
            citizen.gameObject.AddComponent<MachineOperator>();
            JobManager.RemoveJobSpot(1, transform);
            print(transform.name + "Hired Worker");
        }
        else if (resourceGatherers.Count < maxResourceGatherers)
        {
            resourceGatherers.Add(citizen);
            citizen.gameObject.AddComponent<NaturalResourceGatherer>();
            JobManager.RemoveJobSpot(1, transform);
            print(transform.name + "Hired Worker");
        }
        else if (resourceCarriers.Count < maxResourceCarriers)
        {
            resourceCarriers.Add(citizen);
            citizen.gameObject.AddComponent<Carrier>();
            JobManager.RemoveJobSpot(1, transform);
            print(transform.name + "Hired Worker");
        }
    }

    public void Fire(Citizen citizen)
    {
        if (resourceCarriers.Contains(citizen)) resourceCarriers.Remove(citizen);
        else if (resourceGatherers.Contains(citizen)) resourceGatherers.Remove(citizen);
        else SetOperator(null);

        JobManager.AddJobSpot(1, transform);
        print("Worker Fired");
    }
    public void OpenJobSpots()
    {
        if (HasStorages() && HasJobSpot()) JobManager.AddJobSpot(3, transform);
    }
    public bool HasJobSpot()
    {
        return (!HasOperator() || resourceCarriers.Count < maxResourceCarriers || resourceGatherers.Count < maxResourceGatherers);
    }

    public Transform GetCarrierSpot() => carrierSpot;
    private bool HasCarrier() => resourceCarriers.Count > 0;
    private bool HasGatherers() => resourceGatherers.Count > 0;

    public ResourceItem GetItemToDeliver()
    {
        for (int i = 0; i < GetStoredResources().Count; i++)
        {
            if (GetRecipe().HasMaterialInOutput(GetStoredResources()[i]) && GetStoredResources()[i].amount > 0)
            {
                if (GetStoredResources()[i].amount > 0)
                {
                    ResourceItem newItem = CreateNewItemInstance(GetStoredResources()[i]);
                    newItem.amount = 1;
                    return newItem;
                }
                return GetStoredResources()[i];
            }
        }
        return null;
    }

    public ResourceItem GetItemToPickup()
    {
        
        for (int i = 0; i < GetLoadStation().GetStoredResources().Count; i++)
        {
            if (
                resourceProcessor.GetRecipe().HasMaterialInInput(GetLoadStation().GetStoredResources()[i]) && 
                GetLoadStation().GetStoredResources()[i].amount > 0
            )
            {
                ResourceItem newItem = CreateNewItemInstance(GetLoadStation().GetStoredResources()[i]);
                if (IsInventoryFull())
                {
                    newItem.amount = 0;
                }
                else
                {
                    newItem.amount = 1;
                }
                return newItem;
            }
        }
        return null;
    }

}
