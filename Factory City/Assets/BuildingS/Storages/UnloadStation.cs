using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnloadStation : Storage, IHaveWorkers
{
    private Storage storage;

    private void Awake()
    {
        storage = GetComponent<Storage>();        
    }

    private void Start()
    {
        JobManager.AddJobSpot(1, transform);
    }

    public void Hire(Citizen citizen)
    {        
        if (storage.storageOperator == null)
        {
            storage.storageOperator = citizen;
            citizen.gameObject.AddComponent<UnloadStationOperator>();
            JobManager.RemoveJobSpot(1, transform);
            print(transform.name + "Hired Worker");
        }
        else if (storage.storageGatherers.Count < storage.maxGatherersNumber)
        {
            storage.storageGatherers.Add(citizen);
            JobManager.RemoveJobSpot(1, transform);
            print(transform.name + "Hired Worker");
        }
    }

    public void Fire(Citizen citizen)
    {
        if (citizen == storage.storageOperator)
        {
            Destroy(citizen.gameObject.GetComponent<UnloadStationOperator>());
            storage.storageOperator = null;
        }
        else if (storage.storageGatherers.Contains(citizen)) storage.storageGatherers.Remove(citizen);
        //JobManager.AddJobSpot(1, transform);
        print("Worker Fired");
    }

    private void OnDestroy() { Fire(storage.storageOperator); }

    public Storage GetStorage() { return storage; }
}
