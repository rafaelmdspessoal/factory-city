using System;
using System.Collections.Generic;
using UnityEngine;

public class Miner : MonoBehaviour
{
    [SerializeField] private ResourceManager.ResourceType resourceType;
    [SerializeField] private Transform resourceTransform;
    [SerializeField] private Transform currentResource;
    [SerializeField] private int resourceAmount;
    [SerializeField] private int maxResourceAmount = 5;
    [SerializeField] private float timeToMine = 1;

    private IMachine workPlace;
    private ICitizen citizen;
    private IResource resource;
    private LoadStation loadStation;

    [SerializeField] private State state;

    private enum State
    {
        Idle,
        MovingToResource,
        GatheringResources,
        MovingToStorage,
        DroppingOffResources,
    }
    private void Start()
    {
        state = State.Idle;
        resourceType = ResourceManager.ResourceType.Stone;
        transform.GetComponent<MeshRenderer>().material.color = Color.blue;
        resourceAmount = 0;
        citizen = transform.GetComponent<Citizen>().GetComponent<ICitizen>();
        workPlace = citizen.GetWorkPlace().GetComponent<IMachine>();
        loadStation = workPlace.GetLoadStation();
    }

    private void Update()
    {
        if (!HasResourceToGather()) return;
        switch (state)
        {
            case State.Idle:
                currentResource = GetRandomResource(workPlace.GetResourcesInReach(resourceType));
                state = State.MovingToResource;
                break;
            case State.MovingToResource:
                if (citizen.IsIdle())
                {
                    citizen.MoveTo(currentResource, () =>
                    {
                        state = State.GatheringResources;
                    });
                }
                break;
            case State.GatheringResources:
                if (citizen.IsIdle())
                {
                    if (IsInventoryFull())
                    {
                        state = State.MovingToStorage;
                    }
                    else
                    {
                        GatherResource(currentResource.position, () =>
                        {
                            if (resource.HasResource(resourceType))
                            {
                                if (timeToMine <= 0)
                                {
                                    resourceAmount++;
                                    resource.TakeResource(resourceType, 1);
                                    timeToMine = 1;
                                }
                                timeToMine -= Time.deltaTime;
                            }
                            else
                            {
                                workPlace.RemoveResourcesOutOfReach(resourceType, currentResource);
                                Destroy(currentResource.gameObject);

                                state = State.Idle;
                            }
                        });
                    }
                }
                break;
            case State.MovingToStorage:
                if (citizen.IsIdle())
                {
                    citizen.MoveTo(loadStation.GetDeliverySpot(), () =>
                    {
                        state = State.DroppingOffResources;
                    });
                }
                break;
            case State.DroppingOffResources:
                if (citizen.IsIdle())
                {
                    PlayDropOffResourceAnimation(loadStation.GetDeliverySpot().position, () =>
                        {
                            DropResourcesForLoadStation(resourceType, resourceAmount);
                            resourceAmount = 0;
                            state = State.Idle;
                        });
                }
                break;



        }
    }

    private bool IsInventoryFull()
    {
        return resourceAmount >= maxResourceAmount;
    }

    private bool HasResourceToGather()
    {
        if (workPlace.GetResourcesInReach(resourceType).Count == 0)
        {
            return false;
        }
        return true;
    }

    private Transform GetRandomResource(List<Transform> resources)
    {
        if (resources.Count == 0) return null;
        int index = UnityEngine.Random.Range(0, resources.Count);
        Transform resourceObj = workPlace.GetResourcesInReach(resourceType)[index];
        resource = resourceObj.GetComponent<IResource>();
        return resourceObj;
    }

    public void GatherResource(Vector3 lookAtPosition, Action onInventoryFull)
    {
        transform.LookAt(lookAtPosition);
        onInventoryFull.Invoke();
    }
    private void PlayDropOffResourceAnimation(Vector3 lookAtPosition, Action resourcesDroppedOff)
    {
        transform.LookAt(lookAtPosition);
        //playanim
        resourcesDroppedOff.Invoke();
    }

    private void DropResourcesForLoadStation(ResourceManager.ResourceType type, int amount)
    {
        TerrainData terrainData = Terrain.activeTerrain.terrainData;
        Vector3 deliveryPosition = loadStation.GetDeliverySpot().position;

        float offset = UnityEngine.Random.Range(0f, 1f);
        Vector3 offsetDeliveryPos = new Vector3(
            deliveryPosition.x + offset,
            terrainData.GetHeight(Mathf.RoundToInt(deliveryPosition.x), Mathf.RoundToInt(deliveryPosition.z)),
            deliveryPosition.z + offset
            );
        Transform resourceToDropObj = Instantiate(resourceTransform, offsetDeliveryPos, Quaternion.identity);
        MiningResource resourceToDrop = resourceToDropObj.GetComponent<MiningResource>();
        resourceToDrop.SetResourceType(type);
        resourceToDrop.SetResourceAmount(type, amount);
    }

    public void SetResourceTypeObject(Transform resourceObj)
    {
        resourceTransform = resourceObj;
    }
}
