using System;
using System.Collections.Generic;
using UnityEngine;

public class LoadStationOperator : MonoBehaviour
{
    [SerializeField] private Transform operatorSpot;
    [SerializeField] private List<Transform> resourcesToStore;

    [SerializeField] private Transform currentResource;
    [SerializeField] private int resourceAmount;

    private ResourceManager.ResourceType resourceType;

    [SerializeField] Transform workPlace;
    private LoadStation loadStagion;
    private Citizen citizen;

    [SerializeField] private State state;

    private enum State
    {
        Idle,
        MovingToResource,
        PickingUpResources,
        MovingToStorage,
    }

    void Start()
    {
        transform.GetComponent<MeshRenderer>().material.color = Color.white;
        resourcesToStore = new List<Transform>();
        state = State.Idle;
        citizen = transform.GetComponent<Citizen>();
        workPlace = citizen.GetWorkPlace();
        loadStagion = workPlace.GetComponent<LoadStation>();
        operatorSpot = loadStagion.GetOperatorSpot();
        citizen.SetDestinationObj(operatorSpot);
    }

    void Update()
    {
        if (!HasResourceToCarry()) return;
        switch (state)
        {
            case State.Idle:
                currentResource = GetResourceToStore();
                resourceType = currentResource.GetComponent<IResource>().GetResourceType();
                state = State.MovingToResource;
                break;
            case State.MovingToResource:
                if (citizen.IsIdle())
                {
                    citizen.MoveTo(currentResource, () =>
                    {
                        state = State.PickingUpResources;
                    });
                }
                break;
            case State.PickingUpResources:
                if (citizen.IsIdle())
                {
                    if (resourceAmount > 0)
                    {
                        state = State.MovingToStorage;
                    }
                    else
                    {
                        PickUpResource(currentResource.position, () =>
                        {
                            IResource resource = currentResource.GetComponent<IResource>();
                            foreach (ResourceManager.ResourceType resourceType in loadStagion.GetAcceptedResourceTypes())
                            {
                                if (resource.GetResourceType() == resourceType)
                                {                                    
                                    resourceAmount = resource.GetResourceAmount(resourceType);
                                    resourcesToStore.Remove(currentResource.transform);
                                    Destroy(currentResource.gameObject);
                                }

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
                        resourceAmount = loadStagion.AddResourceAmount(resourceType, resourceAmount);
                        state = State.Idle;
                    });
                }
                break;
        }
    }

    private Transform GetResourceToStore()
    {
        Transform resourceObj = resourcesToStore[0];
        return resourceObj;
    }

    public void PickUpResource(Vector3 lookAtPosition, Action onAnimationCompleted)
    {
        transform.LookAt(lookAtPosition);
        // animator.PlayMiningAnimation()
        onAnimationCompleted.Invoke();
    }

    private bool HasResourceToCarry()
    {
        if (resourceAmount == 0 && resourcesToStore.Count == 0)
        {
            return false;
        }
        return true;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<IResource>(out IResource resource))
        {
            foreach(ResourceManager.ResourceType resourceType in loadStagion.GetAcceptedResourceTypes())
            {
                if (resource.GetResourceType() == resourceType)
                {
                    resourcesToStore.Add(collider.transform);
                }

            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.TryGetComponent<IResource>(out IResource resource))
        {
            if (resourcesToStore.Contains(collider.transform)) resourcesToStore.Remove(collider.transform);
        }
    }
}
