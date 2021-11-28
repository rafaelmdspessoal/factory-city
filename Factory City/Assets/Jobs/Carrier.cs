using System;
using System.Collections.Generic;
using UnityEngine;

public class Carrier : MonoBehaviour
{
    [SerializeField] private ResourceManager.ResourceType resourceType;
    [SerializeField] private Transform carrierSpot;
    [SerializeField] private float timeToLoadResource = 1f;
    [SerializeField] private LoadStation loadStation;
    [SerializeField] private UnloadStation unloadstation;
    [SerializeField] private int maxResourceAmount = 2;
    [SerializeField] private int resourceAmount;
    [SerializeField] private State state;

    private BrickMachine workPlace;
    private Citizen citizen;

    public event EventHandler OnResourceDelivered;

    private enum State
    {
        Idle,
        MovingToMachine,
        PickingUpResources,
        MovingToLoadStation,
        DeliveringResources,
        PickingUpCraftedResources,
        MovingToUnloadStation,
        UnloadingResources,
    }

    void Start()
    {
        transform.GetComponent<MeshRenderer>().material.color = Color.yellow;
        state = State.Idle;
        citizen = transform.GetComponent<Citizen>();
        workPlace = citizen.GetWorkPlace().GetComponent<BrickMachine>();
        loadStation = workPlace.GetLoadStation();
        unloadstation = workPlace.GetUnloadStation();
        carrierSpot = workPlace.GetComponent<BrickMachine>().GetCarrierSpot();
        citizen.SetDestinationObj(carrierSpot);
    }

    void Update()
    {
        if (!HasResourceToCarry()) return;
        switch (state)
        {
            case State.Idle:
                state = State.MovingToLoadStation;
                break;
            case State.MovingToLoadStation:
                if (citizen.IsIdle())
                {
                    citizen.MoveTo(loadStation.GetLoadSpot(), () =>
                    {
                        state = State.PickingUpResources;
                    });
                }
                break;
            case State.PickingUpResources:
                if (citizen.IsIdle())
                {
                    if (IsInventoryFull())
                    {
                        state = State.MovingToMachine;
                    }
                    else
                    {
                        PickUpResource(loadStation.GetLoadSpot().position, () =>
                        {
                            timeToLoadResource -= Time.deltaTime;
                            if (timeToLoadResource <= 0)
                            {
                                resourceAmount += loadStation.RemoveResourceAmount(resourceType, maxResourceAmount - resourceAmount);
                                timeToLoadResource = 1;
                            }
                        });
                    }
                }
                break;
            case State.MovingToMachine:
                if (citizen.IsIdle())
                {
                    citizen.MoveTo(workPlace.GetCarrierSpot(), () =>
                    {
                        foreach(ResourceManager.ResourceType type in workPlace.GetAcceptedResourceTypes())
                        {
                            if (resourceType == type)
                            {
                                state = State.DeliveringResources;
                                return;
                            }                                
                        }
                        foreach (ResourceManager.ResourceType type in workPlace.GetCraftedResourceTypes())
                        {
                            if (resourceType == type)
                            {
                                state = State.PickingUpCraftedResources;
                                return;
                            }
                        }
                    });
                }
                break;
            case State.DeliveringResources:
                if (citizen.IsIdle())
                {
                    if (resourceAmount <= 0)
                    {
                        state = State.Idle;
                    }
                    else
                    {
                        DeliverResource(workPlace.GetCarrierSpot().position, () =>
                        {
                            timeToLoadResource -= Time.deltaTime;
                            if (timeToLoadResource <= 0)
                            {
                                resourceAmount = workPlace.AddResourceAmount(resourceType, resourceAmount);
                                timeToLoadResource = 1;
                                if (OnResourceDelivered != null)
                                {
                                    OnResourceDelivered(this, EventArgs.Empty);
                                }
                            }
                        });
                    }
                }
                break;
            case State.PickingUpCraftedResources:
                if (citizen.IsIdle())
                {
                    if (IsInventoryFull())
                    {
                        state = State.MovingToUnloadStation;
                    }
                    else
                    {
                        PickUpResource(workPlace.GetCarrierSpot().position, () =>
                        {
                            timeToLoadResource -= Time.deltaTime;
                            if (timeToLoadResource <= 0)
                            {                                
                                resourceAmount += workPlace.RemoveResourceAmount(resourceType, maxResourceAmount - resourceAmount);
                                timeToLoadResource = 1;
                            }
                        });
                    }
                }
                break;
            case State.MovingToUnloadStation:
                if (citizen.IsIdle())
                {
                    citizen.MoveTo(unloadstation.GetLoadSpot(), () =>
                    {
                        state = State.UnloadingResources;
                    });
                }
                break;
            case State.UnloadingResources:
                if (citizen.IsIdle())
                {
                    if (resourceAmount <= 0)
                    {
                        state = State.Idle;
                    }
                    else
                    {
                        DeliverResource(unloadstation.GetLoadSpot().position, () =>
                        {
                            timeToLoadResource -= Time.deltaTime;
                            if (timeToLoadResource <= 0)
                            {
                                resourceAmount = unloadstation.AddResourceAmount(resourceType, resourceAmount);
                                timeToLoadResource = 1;
                                if (OnResourceDelivered != null)
                                {
                                    OnResourceDelivered(this, EventArgs.Empty);
                                }
                            }
                        });
                    }
                }
                break;
        }
    }


    public void PickUpResource(Vector3 lookAtPosition, Action OnResourcedPicked)
    {
        transform.LookAt(lookAtPosition);
        OnResourcedPicked.Invoke();
    }

    public void DeliverResource(Vector3 lookAtPosition, Action OnResourcedPicked)
    {
        transform.LookAt(lookAtPosition);
        OnResourcedPicked.Invoke();
    }

    private bool HasResourceToCarry()
    {
        if (workPlace.GetResourceAmout(resourceType) == 0 && loadStation.GetResourceAmout(resourceType) == 0 && resourceAmount == 0)
        {
            return false;
        }
        return true;
    }

    public void GetNeededResource(ResourceManager.ResourceType type)
    {
        resourceType = type;
        state = State.MovingToLoadStation;
    }

    public void PickupCraftedResources(ResourceManager.ResourceType type)
    {
        resourceType = type;
        state = State.MovingToMachine;
    }

    private bool IsInventoryFull()
    {
        return resourceAmount >= maxResourceAmount;
    }

    public int GetMaxInventoryResourceAmount()
    {
        return maxResourceAmount;
    }
}
