using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour, IStorage, IManipulable, IHaveWorkers
{
    [SerializeField] private Transform deliverySpot;
    [SerializeField] private Transform operatorSpot;

    [SerializeField] private int maxResourceAmount;
    [SerializeField] private int resourceAmount;
    [SerializeField] private ResourceManager.ResourceType[] acceptedResourceType;
    [SerializeField] private int allowedDeliverers;

    [SerializeField] private Citizen worker;

    private void Start()
    {
        JobManager.AddJobSpot(1, this.transform);
    }


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

    public void SetAlowedResources(ResourceManager.ResourceType[] resourceTypes)
    {

    }

    public void CreateSelf()
    {

    }

    public void DestroySelf()
    {
        
    }

    public void Hire(Citizen citizen)
    {
        if (worker == null)
        {
            worker = citizen;
            worker.gameObject.AddComponent<StorageStationOperator>();
        }        
        JobManager.RemoveJobSpot(1, transform);
        print("Worker Hired");
    }

    public void Fire(Citizen citizen)
    {
        if (worker == citizen) worker = null;
        JobManager.AddJobSpot(1, transform);
        print("Worker Fired");
    }

    public bool HasJobSpot()
    {
        return worker == null;
    }

    public Transform GetDeliverySpot()
    {
        return deliverySpot;
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
