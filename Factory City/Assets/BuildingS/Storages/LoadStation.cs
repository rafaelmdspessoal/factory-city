using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadStation : Storage, IHaveWorkers
{
    private void Start()
    {
        JobManager.AddJobSpot(1, this.transform);
    }
    public void Hire(Citizen citizen)
    {
        if (worker == null)
        {
            worker = citizen;
            worker.gameObject.AddComponent<LoadStationOperator>();
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
}
