using System;
using System.Collections.Generic;
using UnityEngine;

public class LoadStationOperator : MonoBehaviour
{
    [SerializeField] private Citizen citizen;

    [SerializeField] private Transform fieldOfViewTransform;

    [SerializeField] private Transform operatorSpot;

    [SerializeField] private Transform workPlace;
    [SerializeField] private LoadStation loadStation;

    [SerializeField] private int maxResourceAmount = 1;
    [SerializeField] private List<ResourceItem> carriedResources;


    [SerializeField] private State state;

    [SerializeField] private int timeToPickDropMaterialItem = 1;
    [SerializeField] private float elapseTimeToAction = 0;

    [SerializeField] private Transform resourceToStore;


    private enum State
    {
        Idle,
        MovingToResource,
        PickingUpResources,
        MovingToStorage,
        DroppingOffResource,
    }

    private void Awake()
    {
        carriedResources = new List<ResourceItem>();        
    }

    void Start()
    {
        state = State.Idle;

        transform.GetComponent<MeshRenderer>().material.color = Color.white;
        citizen = transform.GetComponent<Citizen>();
        workPlace = citizen.GetWorkPlace();
        fieldOfViewTransform = citizen.GetFieldOfViewTransform();

        loadStation = workPlace.GetComponent<LoadStation>();
        operatorSpot = loadStation.GetOperatorSpot();
        citizen.SetDestinationObj(operatorSpot);
    }

    void Update()
    {
        if (!HasResourceToCarry()) return;
        switch (state)
        {
            case State.Idle:
                state = State.MovingToResource;
                resourceToStore = loadStation.GetResourceToStore();
                break;
            case State.MovingToResource:
                if (citizen.IsIdle())
                {
                    citizen.MoveTo(resourceToStore, () =>
                    {
                        state = State.PickingUpResources;
                    }, 1f);
                }
                break;
            case State.PickingUpResources:
                if (citizen.IsIdle())
                {
                    if (carriedResources.Count >= maxResourceAmount)
                    {
                        state = State.MovingToStorage;
                        break;
                    }
                    else
                    {
                        PickUpResource(resourceToStore.position, () =>
                        {
                            elapseTimeToAction += Time.deltaTime;
                            if (elapseTimeToAction >= timeToPickDropMaterialItem)
                            {
                                carriedResources.Add(resourceToStore.GetComponent<ResourceItemObject>().GetMaterialItem());
                                loadStation.RemoveResouceToStore(resourceToStore);
                                Destroy(resourceToStore.gameObject);
                                elapseTimeToAction = 0;
                            }
                        });
                    }
                }
                break;
            case State.MovingToStorage:
                if (citizen.IsIdle())
                {
                    citizen.MoveTo(operatorSpot, () =>
                    {
                        state = State.DroppingOffResource;
                    }, 1f);
                }
                break;
            case State.DroppingOffResource:
                if (citizen.IsIdle())
                {
                    DropOffResource(operatorSpot.position, () =>
                    {
                        elapseTimeToAction += Time.deltaTime;
                        if (elapseTimeToAction >= timeToPickDropMaterialItem)
                        {
                            for (int i = 0; i < carriedResources.Count; i++)
                            {
                                loadStation.AddResourceItem(carriedResources[i]);
                                carriedResources.Remove(carriedResources[i]);
                            }                            
                            state = State.Idle;
                            elapseTimeToAction = 0;
                        }
                    });
                }
                break;
        }
    }



    public void PickUpResource(Vector3 lookAtPosition, Action OnResourcePicked)
    {
        transform.LookAt(lookAtPosition);
        OnResourcePicked.Invoke();
    }

    public void DropOffResource(Vector3 lookAtPosition, Action OnResourceDropped)
    {
        transform.LookAt(lookAtPosition);
        OnResourceDropped.Invoke();
    }

    private bool HasResourceToCarry()
    {
        if (carriedResources.Count == 0 && loadStation.GetResourceToStore() == null)
        {
            return false;
        }
        return true;
    }
}