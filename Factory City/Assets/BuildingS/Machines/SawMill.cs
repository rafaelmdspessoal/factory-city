using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawMill : MonoBehaviour, IMachine
{
    [SerializeField] private Transform storageFacility;
    [SerializeField] private Citizen worker;
    [SerializeField] private Citizen lumberjackWorker;
    private Lumberjack lumberjack;

    private Transform resourceObj;

    [SerializeField] private ResourceManager.ResourceType resourceType;

    void Start()
    {
        JobManager.jobList.Add(transform);
    }

    void Update()
    {
        if (lumberjackWorker != null)
        {
            if (resourceObj == null)
            {
                resourceObj = GetClosestResource(ResourceManager.ResourceType.Log);
            }
            if (resourceObj == null)
            {
                Fire(lumberjackWorker);
            }
            if (!lumberjack.hasTask() && resourceObj != null && lumberjack.currentState == Lumberjack.State.LookingForTree)
            {
                print("SawMill order");
                lumberjackWorker.SetDestinationObj(resourceObj);
                lumberjack.SetResourceToHarvest(resourceObj);
            }
        }
    }

    public void Hire(Citizen citizen)
    {
        if (lumberjackWorker == null)
        {
            lumberjackWorker = citizen;
            citizen.transform.gameObject.AddComponent<Lumberjack>();
            lumberjack = lumberjackWorker.GetComponent<Lumberjack>();
            lumberjack.deliveryStorage = storageFacility;
            lumberjack.hasTools = true;
            lumberjack.currentState = Lumberjack.State.LookingForTree;
        }
        else if (lumberjackWorker != null && worker == null)
        {
            worker = citizen;
        }

        if (lumberjackWorker != null && worker != null) JobManager.jobList.Remove(transform);
    }

    public void Fire(Citizen citizen)
    {
        print("Lumberjack Fired");
    }

    private Transform GetClosestResource(ResourceManager.ResourceType type)
    {
        Vector3 currentPosition = transform.position;
        if (type == ResourceManager.ResourceType.Log)
        {
            Transform bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            foreach (Transform tree in Trees.trees)
            {
                Vector3 directionToTarget = tree.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = tree;
                }
            }
            return bestTarget;
        }
        else
        {
            return null;
        }
    }
    public bool HasJobSpot()
    {
        return true;
    }

    public bool IsNearStorage()
    {
        return true;
    }

    public LoadStation GetLoadStation()
    {
        return null;
    }

    public UnloadStation GetUnloadStation()
    {
            return null;
    }
    public void AddStationsInReach()
    {

    }

    public void RemoveStationsOutOfReach(Transform station)
    {

    }

    public void GetStationsInReach(Transform station)
    {

    }
}
