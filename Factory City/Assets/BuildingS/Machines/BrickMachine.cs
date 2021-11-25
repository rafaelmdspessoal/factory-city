using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class BrickMachine : MonoBehaviour, IMachine, IManipulable, IHaveWorkers
{
    [SerializeField] private Transform operatorSpot;
    [SerializeField] private Transform carrierSpot;
    [SerializeField] private int numWorkers;
    [SerializeField] private int numAcceptableResources;
    [SerializeField] private int numProducedResources;
    [SerializeField] private Citizen[] workers;

    [SerializeField] private ResourceManager.ResourceType[] acceptableResourceTypes;
    [SerializeField] private ResourceManager.ResourceType[] producedResourceTypes;

    [SerializeField] private List<UnloadStation> unloadStations;
    [SerializeField] private List<LoadStation> loadStations;

    [SerializeField] private Transform actionArea;

    [SerializeField] private List<Transform> stationsInReach;
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
        stationsInReach = new List<Transform>();
        GetResourcesInReach(acceptableResourceTypes);
    }

    void Update()
    {

    }

    public void Hire(Citizen citizen)
    {
        for (int i = 0; i < numWorkers; i++)
        {
            if (workers[i] == null)
            {
                workers[i] = citizen;
                if (i == 0) workers[i].gameObject.AddComponent<MachineOperator>();
                else if (i == 1) workers[i].gameObject.AddComponent<Carrier>();
                else workers[i].gameObject.AddComponent<Miner>();

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

    private void GetResourcesInReach(ResourceManager.ResourceType[] resourceTypes)
    {
        resourcesInReach = actionArea.GetComponent<ObjectsInReach>().GetResourcesInReach(resourceTypes);
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
}
