using System;
using System.Collections.Generic;
using UnityEngine;

public class UnloadStationOperator : MonoBehaviour
{
    [SerializeField] private Transform operatorSpot;
    [SerializeField] private ResourceManager.ResourceType[] storedResourceTypes;
    [SerializeField] private ResourcesScriptableObject[] storedResourceObjects;

    [SerializeField] private Transform currentResource;
    [SerializeField] private int maxResourceAmount = 1;
    [SerializeField] private int resourceAmount;
    [SerializeField] private float timeToUnloadResource = 1f;

    [SerializeField] private ResourceManager.ResourceType resourceType;

    [SerializeField] Transform workPlace;
    private UnloadStation unloadStagion;
    private Citizen citizen;

    [SerializeField] private State state;

    private enum State
    {
        Idle,
        MovingToStorage,
        PickingUpResources,
        MovingToDropoffSpot,
        DroppingoffResource,
    }

    void Start()
    {
        transform.GetComponent<MeshRenderer>().material.color = Color.white;
        state = State.Idle;
        citizen = transform.GetComponent<Citizen>();
        workPlace = citizen.GetWorkPlace();
        unloadStagion = workPlace.GetComponent<UnloadStation>();
        operatorSpot = unloadStagion.GetOperatorSpot();
        citizen.SetDestinationObj(operatorSpot);
        storedResourceTypes = unloadStagion.GetAcceptedResourceTypes();
        storedResourceObjects = unloadStagion.GetStoredResourceObjects();
    }

    void Update()
    {
        if (!HasResourceToCarry()) return;
        switch (state)
        {
            case State.Idle:
                state = State.MovingToStorage;
                break;
            case State.MovingToStorage:
                if (citizen.IsIdle())
                {
                    citizen.MoveTo(unloadStagion.GetOperatorSpot(), () =>
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
                        state = State.MovingToDropoffSpot;
                    }
                    else
                    {
                        PickUpResource(unloadStagion.GetOperatorSpot().position, () =>
                        {
                            timeToUnloadResource -= Time.deltaTime;
                            if (timeToUnloadResource <= 0)
                            {
                                foreach (ResourceManager.ResourceType resourceType in storedResourceTypes)
                                {
                                    if (unloadStagion.GetResourceAmout(resourceType) >= maxResourceAmount)
                                    {
                                        this.resourceType = resourceType;
                                        resourceAmount = unloadStagion.RemoveResourceAmount(resourceType, maxResourceAmount);
                                        timeToUnloadResource = 1;
                                        break;
                                    }
                                }
                            }
                        });
                    }
                }
                break;
            case State.MovingToDropoffSpot:
                if (citizen.IsIdle())
                {
                    citizen.MoveTo(unloadStagion.GetDeliverySpot(), () =>
                    {
                        state = State.DroppingoffResource;
                    });
                }
                break;
            case State.DroppingoffResource:
                if (citizen.IsIdle())
                {
                    PlayDropOffResourceAnimation(unloadStagion.GetDeliverySpot().position, () =>
                    {
                        DropoffResource();
                        resourceAmount = 0;
                        state = State.Idle;
                    });
                }
                break;
        }
    }

    private void PlayDropOffResourceAnimation(Vector3 lookAtPosition, Action resourcesDroppedOff)
    {
        transform.LookAt(lookAtPosition);
        //playanim
        resourcesDroppedOff.Invoke();
    }

    private void DropoffResource()
    {
        TerrainData terrainData = Terrain.activeTerrain.terrainData;
        Vector3 deliveryPosition = unloadStagion.GetDeliverySpot().position;
        float offset = UnityEngine.Random.Range(0f, 1f);
        Vector3 offsetDeliveryPos = new Vector3(
            deliveryPosition.x + offset,
            terrainData.GetHeight(Mathf.RoundToInt(deliveryPosition.x), Mathf.RoundToInt(deliveryPosition.z)),
            deliveryPosition.z + offset
            );

        foreach (ResourcesScriptableObject storedResourceObject in storedResourceObjects)
        {
            if (storedResourceObject.GetResouceType() == resourceType)
            {
                currentResource = storedResourceObject.prefab;
                Instantiate(currentResource, offsetDeliveryPos, Quaternion.identity);
                print(currentResource);
                break;
            }
        }
    }

    public void PickUpResource(Vector3 lookAtPosition, Action onAnimationCompleted)
    {
        transform.LookAt(lookAtPosition);
        // animator.PlayMiningAnimation()
        onAnimationCompleted.Invoke();
    }

    private bool HasResourceToCarry()
    {
        foreach (ResourceManager.ResourceType resourceType in storedResourceTypes)
        {
            if (unloadStagion.GetResourceAmout(resourceType) >= maxResourceAmount || resourceAmount > 0)
            {
                return true;
            }
        }        

        return false;
    }

}
