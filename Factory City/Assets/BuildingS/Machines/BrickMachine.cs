using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickMachine : MonoBehaviour, IMachine, IManipulable
{
    [SerializeField] private int numWorkers;
    [SerializeField] private int numAcceptableResources;
    [SerializeField] private int numProducedResources;
    [SerializeField] private Citizen[] workers;

    [SerializeField] private ResourceManager.ResourceType[] acceptableResourceTypes;
    [SerializeField] private ResourceManager.ResourceType[] producedResourceTypes;

    [SerializeField] private LoadStation[] unloadStation;
    [SerializeField] private LoadStation[] loadStation;

    [SerializeField] private Transform actionArea;

    private List<Transform> resourcesInReach;

       
    void Start()
    {
        acceptableResourceTypes = new ResourceManager.ResourceType[numAcceptableResources];
        acceptableResourceTypes[0] = ResourceManager.ResourceType.Stone;

        producedResourceTypes = new ResourceManager.ResourceType[numProducedResources];
        producedResourceTypes[0] = ResourceManager.ResourceType.Brick;

        workers = new Citizen[numWorkers];
        JobManager.AddJobSpot(numWorkers, this.transform);

        resourcesInReach = new List<Transform>();
        GetResourcesInReach(acceptableResourceTypes);
    }

    void Update()
    {
        
    }

    public void Hire(Citizen citizen)
    {
        for (int i = 0; i < numWorkers; i++)
        {
            if (workers [i] == null)
            {
                workers[i] = citizen;
                workers[i].gameObject.AddComponent<Miner>();
                break;
            }
        }     
        JobManager.RemoveJobSpot(1, transform);
        print("Worker Hired");
    }

    public void Fire(Citizen citizen)
    {
        for (int i = 0; i < numWorkers; i++)
        {
            if (workers[i] == citizen) workers[i] = null;
        }
        JobManager.RemoveJobSpot(1, transform);
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

    }

    public void DestroySelf()
    {

    }

    public LoadStation GetLoadStation()
    {
        return loadStation[0];
    }

    public LoadStation GetUnloadStation()
    {
        return unloadStation[0];
    }

    private void GetLoadStationInReach(Transform loadStation)
    {

    }

    private void GetUnloadStationInReach(Transform loadStation)
    {

    }

    private void GetResourcesInReach(ResourceManager.ResourceType[] resourceTypes)
    {
        resourcesInReach = actionArea.GetComponent<ObjectsInReach>().GetResourcesInReach(resourceTypes);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Load Station")
        {
            print("Load Station in reach!");
            GetLoadStationInReach(collider.transform);
            transform.GetComponent<Renderer>().material.color = Color.green;
        }
        else if (collider.tag == "Unload Station")
        {
            print("Unload Station in reach!");
            GetUnloadStationInReach(collider.transform);
            transform.GetComponent<Renderer>().material.color = Color.blue;
        }
        else
        {
            transform.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Load Station" || collider.tag == "Unload Station")
        {
            transform.GetComponent<Renderer>().material.color = Color.white;
        }
    }
}
