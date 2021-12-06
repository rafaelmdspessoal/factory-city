using System;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class LoadStation : Storage, IHaveWorkers
{
    private Storage storage;
    [SerializeField] private Transform actionTansform;
    [SerializeField] private List<Transform> resourcesToStore;
    [SerializeField] private List<ResourceItemObject> resourcesInReach;
    [SerializeField] private List<Transform> resouceContainersInReach;


    private void Awake()
    {
        storage = GetComponent<Storage>();
        resourcesInReach = new List<ResourceItemObject>();
        resouceContainersInReach = new List<Transform>();
        resourcesToStore = new List<Transform>();
    }

    private void Start()
    {
        JobManager.AddJobSpot(1 + storage.GetGathererNumber(), transform);

        AddResourcesInReach();
        actionTansform.GetComponent<ObjectsInReach>().OnObjectAdded += delegate (object sender, EventArgs e)
        {
            AddResourcesInReach();
        };
        GetDeliverySpot().GetComponent<ObjectsInReach>().OnObjectAdded += delegate (object sender, EventArgs e)
        {
            AddResourcesToStoreInReach();
        };
    }

    private void AddResourcesInReach()
    {
        foreach (Transform obj in actionTansform.GetComponent<ObjectsInReach>().GetObjectsInReach())
        {
            if (obj == null) continue;
            if (obj.TryGetComponent<ResourceItemObject>(out ResourceItemObject materialItemObject))
            {
                foreach (ResourceItem resourceItem in storage.GetStoredResources())
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
                if (storage.HasResourceInStorage(resourceContainer.GetResourceItem()) == null) return;
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

    private void AddResourcesToStoreInReach()
    {
        foreach (Transform obj in GetDeliverySpot().GetComponent<ObjectsInReach>().GetObjectsInReach())
        {
            if (obj == null) continue;
            if (obj.TryGetComponent<ResourceItemObject>(out ResourceItemObject resourceItemObject))
            {
                foreach (ResourceItem resourceItem in storage.GetStoredResources())
                {
                    if (resourceItemObject.GetMaterialItem().resourceScriptableObject == resourceItem.resourceScriptableObject)
                    {
                        if (!resourcesToStore.Contains(resourceItemObject.transform))
                        {
                            resourcesToStore.Add(resourceItemObject.transform);
                        }
                    }
                }
            }
        }
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

    public void RemoveResouceToStore(Transform resource)
    {
        resourcesToStore.Remove(resource);
    }

    public Transform GetResourceToStore()
    {
        if (resourcesToStore.Count <= 0) return null;
        return resourcesToStore[0];
    }

    public void Hire(Citizen citizen)
    {
        if (storage.storageOperator == null)
        {
            storage.storageOperator = citizen;
            citizen.gameObject.AddComponent<LoadStationOperator>();
            JobManager.RemoveJobSpot(1, transform);
            print(transform.name + "Hired Worker");
        }
        else if (storage.storageGatherers.Count < storage.maxGatherersNumber)
        {
            storage.storageGatherers.Add(citizen);
            citizen.gameObject.AddComponent<NaturalResourceGatherer>();
            JobManager.RemoveJobSpot(1, transform);
            print(transform.name + "Hired Worker");
        }
    }

    public void Fire(Citizen citizen)
    {
        if (citizen == storage.storageOperator)
        {
            Destroy(citizen.gameObject.GetComponent<LoadStationOperator>());
            storage.storageOperator = null;
        }
        else if (storage.storageGatherers.Contains(citizen)) storage.storageGatherers.Remove(citizen);
        //JobManager.AddJobSpot(1, transform);
        print("Worker Fired");
    }

    private void OnDestroy()
    {
        Fire(storage.storageOperator);
    }

    public bool HasResourceToGather()
    {
        return (resouceContainersInReach.Count > 0 || resourcesInReach.Count > 0);
    }
}
