using System;
using System.Collections.Generic;
using UnityEngine;

public class UnloadStationOperator : MonoBehaviour
{
    [SerializeField] private Citizen citizen;

    [SerializeField] private Transform operatorSpot;

    [SerializeField] private int maxResourceAmount = 1;

    [SerializeField] private Transform workPlace;
    [SerializeField] private UnloadStation unloadStagion;

    [SerializeField] private int timeToPickDropMaterialItem = 1;
    [SerializeField] private float elapseTimeToAction = 0;

    [SerializeField] private State state;

    [SerializeField] private ResourceItem resourcesToPickup;
    [SerializeField] private ResourceItem resourcesCarried;


    private TerrainData terrainData;

    private enum State
    {
        Idle,
        MovingToUnloadStation,
        PickingUpResources,
        MovingToDropoffSpot,
        DroppingoffResource,
    }

    void Start()
    {
        state = State.Idle;
        transform.GetComponent<MeshRenderer>().material.color = Color.white;
        citizen = transform.GetComponent<Citizen>();
        workPlace = citizen.GetWorkPlace();


        unloadStagion = workPlace.GetComponent<UnloadStation>();
        operatorSpot = unloadStagion.GetOperatorSpot();

        citizen.SetDestinationObj(operatorSpot);
        terrainData = Terrain.activeTerrain.terrainData;
    }

    void Update()
    {
        if (!HasResourceToCarry(resourcesToPickup)) return;
        switch (state)
        {
            case State.Idle:
                if (resourcesToPickup != null && resourcesToPickup.amount > 0)
                {
                    state = State.MovingToUnloadStation;
                }
                break;
            case State.MovingToUnloadStation:
                if (citizen.IsIdle())
                {
                    citizen.MoveTo(unloadStagion.GetOperatorSpot(), () =>
                    {
                        state = State.PickingUpResources;
                    }, 1f);
                }
                break;
            case State.PickingUpResources:
                if (citizen.IsIdle())
                {
                    if (resourcesToPickup == null)
                    {
                        state = State.Idle;
                        break;
                    }
                    PickUpResource(unloadStagion.GetLoadSpot().position, () =>
                    {
                        elapseTimeToAction += Time.deltaTime;
                        if (elapseTimeToAction >= timeToPickDropMaterialItem)
                        {
                            if (!IsInventoryFull())
                            {
                                resourcesCarried = CreateNewItemInstance(resourcesToPickup);
                                resourcesCarried.amount = 1;
                                unloadStagion.RemoveResourceItem(resourcesCarried);
                                resourcesToPickup.amount -= 1;
                            }
                            state = State.MovingToDropoffSpot;
                            elapseTimeToAction = 0;
                        }
                    });
                }
                break;
            case State.MovingToDropoffSpot:
                if (citizen.IsIdle())
                {
                    citizen.MoveTo(unloadStagion.GetDeliverySpot(), () =>
                    {
                        state = State.DroppingoffResource;
                    }, 1f);
                }
                break;
            case State.DroppingoffResource:
                if (citizen.IsIdle())
                {
                    PlayDropOffResourceAnimation(unloadStagion.GetDeliverySpot().position, () =>
                    {
                        elapseTimeToAction += Time.deltaTime;
                        if (elapseTimeToAction >= timeToPickDropMaterialItem)
                        {
                            Vector3 deliveryPosition = unloadStagion.GetDeliverySpot().position;

                            float offset = UnityEngine.Random.Range(0f, 1f);
                            Vector3 offsetDeliveryPos = new Vector3(
                                deliveryPosition.x + offset,
                                terrainData.GetHeight(Mathf.RoundToInt(deliveryPosition.x), Mathf.RoundToInt(deliveryPosition.z)) + 2f,
                                deliveryPosition.z + offset
                                );

                            ResourceItemObject.CreateMaterialItemObject(
                                offsetDeliveryPos,
                                GameObject.Find("Materials").transform,
                                resourcesCarried
                            );
                            resourcesCarried.amount -= 1;
                            state = State.Idle;
                            elapseTimeToAction = 0;
                        }
                    });
                }
                break;
        }
    }

    private void PlayDropOffResourceAnimation(Vector3 lookAtPosition, Action resourcesDroppedOff)
    {
        transform.LookAt(lookAtPosition);
        resourcesDroppedOff.Invoke();
    }

    public void PickUpResource(Vector3 lookAtPosition, Action onAnimationCompleted)
    {
        transform.LookAt(lookAtPosition);
        onAnimationCompleted.Invoke();
    }

    private bool HasResourceToCarry(ResourceItem resourceItem)
    {
        if (resourceItem == null) return false;

        int resourceAmount = unloadStagion.GetStorage().GetResourceAmout(resourceItem);
        print(resourceAmount);
        print(resourceItem.amount);
        if (resourceAmount - resourceItem.amount >= 0)
        {
            return true;
        }
        return false;
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

    public void RequestResourceItems(ResourceItem resourceItem)
    {
        resourcesToPickup = resourceItem;
    }

    public void GetItemToPickup(ResourceItem resourceItem)
    {
        ResourceItem newItem = CreateNewItemInstance(resourceItem);
        resourcesToPickup = newItem;
    }

    private ResourceItem CreateNewItemInstance(ResourceItem item)
    {
        ResourceItem newItem = new ResourceItem();
        newItem.resourceScriptableObject = item.resourceScriptableObject;
        newItem.amount = 0;
        return newItem;
    }
}
