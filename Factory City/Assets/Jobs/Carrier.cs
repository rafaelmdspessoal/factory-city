using System;
using System.Collections.Generic;
using UnityEngine;

public class Carrier : MonoBehaviour
{
    [SerializeField] private Citizen citizen;

    [SerializeField] private Transform workPlaceTransform;
    [SerializeField] private Transform carrierSpot;

    [SerializeField] private float timeToLoadUnloadResource = 1f;
    [SerializeField] private float elapseTimeToAction = 0;

    [SerializeField] private int maxResourceAmount = 1;

    [SerializeField] private ResourceItem resourcesCarried;
    [SerializeField] private ResourceItem resourceToCarry;

    [SerializeField] private State state;

    private IMachine workPlace;

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
        state = State.Idle;
        transform.GetComponent<MeshRenderer>().material.color = Color.yellow;
        citizen = transform.GetComponent<Citizen>();

        workPlaceTransform = citizen.GetWorkPlace();
        workPlace = workPlaceTransform.GetComponent<IMachine>();

        carrierSpot = workPlace.GetCarrierSpot();
        citizen.SetDestinationObj(carrierSpot);
    }

    void Update()
    {
        if (!HasResourceToCarry()) return;

        switch (state)
        {
            case State.Idle:
                resourceToCarry = workPlace.GetItemToDeliver();
                state = State.MovingToMachine;
                if (resourceToCarry == null || resourceToCarry.amount == 0)
                {
                    resourceToCarry = workPlace.GetItemToPickup();
                    state = State.MovingToLoadStation;
                }
                if (resourceToCarry == null || resourceToCarry.amount == 0)
                {
                    state = State.Idle;
                    Debug.LogError("No item to carry");
                    break;
                }
                break;
            case State.MovingToLoadStation:
                if (citizen.IsIdle())
                {
                    citizen.MoveTo(workPlace.GetLoadStation().GetLoadSpot(), () =>
                    {
                        state = State.PickingUpResources;
                    }, 1f);
                }
                break;
            case State.PickingUpResources:
                if (citizen.IsIdle())
                {
                    if (resourceToCarry == null)
                    {
                        state = State.Idle;
                        break;
                    }
                    PickUpResource(workPlace.GetLoadStation().GetLoadSpot().position, () =>
                    {
                        elapseTimeToAction += Time.deltaTime;
                        if (elapseTimeToAction >= timeToLoadUnloadResource)
                        {
                            if (!IsInventoryFull())
                            {
                                resourcesCarried = resourceToCarry;
                                workPlace.GetLoadStation().RemoveResourceItem(resourceToCarry);
                                resourceToCarry = null;
                            }
                            state = State.MovingToMachine;
                            elapseTimeToAction = 0;
                        }
                    });
                }
                break;
            case State.MovingToMachine:
                if (citizen.IsIdle())
                {
                    citizen.MoveTo(carrierSpot, () =>
                    {
                        if (resourcesCarried == null)
                        {
                            state = State.PickingUpCraftedResources;
                            return;
                        }
                        state = State.DeliveringResources;
                        return;
                    }, 1f);
                }
                break;
            case State.DeliveringResources:
                if (citizen.IsIdle())
                {
                    if (resourcesCarried == null)
                    {
                        state = State.Idle;
                        break;
                    }
                    DeliverResource(carrierSpot.position, () =>
                    {
                        elapseTimeToAction += Time.deltaTime;
                        if (elapseTimeToAction >= timeToLoadUnloadResource)
                        {
                            workPlace.AddResourceItem(resourcesCarried);
                            resourcesCarried = null;
                            state = State.Idle;
                            elapseTimeToAction = 0;
                        }
                    });
                }
                break;
            case State.PickingUpCraftedResources:
                if (citizen.IsIdle())
                {
                    if (resourceToCarry == null)
                    {
                        state = State.Idle;
                        break;
                    }
                    PickUpResource(carrierSpot.position, () =>
                    {
                        elapseTimeToAction += Time.deltaTime;
                        if (elapseTimeToAction >= timeToLoadUnloadResource)
                        {
                            resourcesCarried = resourceToCarry;
                            workPlace.RemoveResourceItem(resourceToCarry);
                            resourceToCarry = null;
                            elapseTimeToAction = 0;
                            state = State.MovingToUnloadStation;
                        }
                    });
                }
                break;
            case State.MovingToUnloadStation:
                if (citizen.IsIdle())
                {
                    citizen.MoveTo(workPlace.GetUnloadStation().GetLoadSpot(), () =>
                    {
                        state = State.UnloadingResources;
                    }, 1f);
                }
                break;
            case State.UnloadingResources:
                if (citizen.IsIdle())
                {
                    if (resourcesCarried == null)
                    {
                        state = State.Idle;
                        break;
                    }
                    DeliverResource(workPlace.GetUnloadStation().GetLoadSpot().position, () =>
                    {
                        elapseTimeToAction += Time.deltaTime;
                        if (elapseTimeToAction >= timeToLoadUnloadResource)
                        {
                            workPlace.GetUnloadStation().AddResourceItem(resourcesCarried);
                            resourcesCarried = null;
                            state = State.Idle;
                            elapseTimeToAction = 0;
                        }
                    });
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
        if (
            workPlace.GetUnloadStation().IsInventoryFull() ||
            workPlace.GetLoadStation().GetResourceAmout() <= 0 
            )
        {
            return false;
        }
        return true;
    }

    private bool IsInventoryFull()
    {
        if (resourcesCarried != null)
        {
            return resourcesCarried.amount > maxResourceAmount;
        }
        else
        {
            return false;
        }
    }
}
