using System;
using System.Collections.Generic;
using UnityEngine;

public class NaturalResourceGatherer : MonoBehaviour
{
    [SerializeField] private Citizen citizen;

    [SerializeField] private Transform workPlace;
    [SerializeField] private Transform actionAreaTransform;

    [SerializeField] private int maxResourceAmount = 1;
    [SerializeField] private ResourceItem resourcesCarried;

    [SerializeField] private int timeToChopTree = 5;
    [SerializeField] private int timeToPickDropMaterialItem = 1;
    [SerializeField] private float elapseTimeToAction = 0;

    [SerializeField] private State state;

    [SerializeField] private Transform resourceToGather;

    private TerrainData terrainData;

    public enum State
    {
        Idle,
        MovingToResourceContainer,
        HarvestingResourceContainer,
        MovingToResource,
        PickingUpResource,
        MovingToStorage,
        DroppingOffResource,
    }

    private void Start()
    {
        state = State.Idle;

        transform.GetComponent<MeshRenderer>().material.color = Color.black;
        citizen = GetComponent<Citizen>();
        workPlace = citizen.GetWorkPlace();

        actionAreaTransform = citizen.GetActionArea();
        terrainData = Terrain.activeTerrain.terrainData;

        timeToChopTree -= Mathf.RoundToInt(citizen.GetAttackSpeed());
    }

    private void Update()
    {
        if (!HasResourceToCarry()) return;

        switch (state)
        {
            case State.Idle:
                resourceToGather = workPlace.GetComponent<LoadStation>().GetResourceToGather();
                if (resourceToGather == null)
                {
                    state = State.Idle;
                    Debug.LogError("Missing material to gather!");
                    break;
                }
                if (resourceToGather.TryGetComponent<ResourceContainer>(out ResourceContainer resourceContainer))
                {
                    resourceContainer.OnResourceContainerDestroyed += delegate (object sender, EventArgs e)
                    {
                        state = State.Idle;
                        workPlace.GetComponent<LoadStation>().RemoveGatheredResourceFromReach(resourceContainer.transform);
                        resourceToGather = null;
                    };
                    state = State.MovingToResourceContainer;
                }
                else if (resourceToGather.TryGetComponent<ResourceItemObject>(out ResourceItemObject material))
                {
                    state = State.MovingToResource;
                    material.OnResourceGathered += delegate (object sender, EventArgs e)
                    {
                        state = State.MovingToStorage;
                        workPlace.GetComponent<LoadStation>().RemoveGatheredResourceFromReach(material.transform);
                        resourceToGather = null;
                    };
                }
                break;
            case State.MovingToResourceContainer:
                if (citizen.IsIdle())
                {
                    citizen.MoveTo(resourceToGather, () =>
                    {
                        state = State.HarvestingResourceContainer;
                    }, 2f);
                }
                break;
            case State.HarvestingResourceContainer:
                if (citizen.IsIdle())
                {
                    if (resourceToGather == null)
                    {
                        state = State.Idle;
                        break;
                    }
                    ChoppTree(resourceToGather.position, () =>
                    {
                        elapseTimeToAction += Time.deltaTime;
                        if (elapseTimeToAction >= timeToChopTree)
                        {
                            Vector3 colliderSize = Vector3.one * 1f;

                            Collider[] colliderArray = Physics.OverlapBox(actionAreaTransform.position, colliderSize);
                            foreach (Collider collider in colliderArray)
                            {
                                if (collider.TryGetComponent<IDamageable>(out IDamageable treeDamageable))
                                {
                                    treeDamageable.Damage(citizen.GetAttackDamage());
                                }
                            }
                            elapseTimeToAction = 0;
                        }
                    });
                }
                break;
            case State.MovingToResource:
                if (citizen.IsIdle())
                {
                    citizen.MoveTo(resourceToGather, () =>
                    {
                        state = State.PickingUpResource;
                    }, 2f);
                }
                break;
            case State.PickingUpResource:
                if (citizen.IsIdle())
                {
                    if (resourceToGather == null)
                    {
                        state = State.Idle;
                        break;
                    }
                    PickUpMaterial(resourceToGather.position, () =>
                    {
                        elapseTimeToAction += Time.deltaTime;
                        if (elapseTimeToAction >= timeToPickDropMaterialItem)
                        {
                            if (resourcesCarried == null || resourcesCarried.amount < maxResourceAmount)
                            {
                                // Bug em caso de poder carregar mais de um item
                                resourcesCarried = resourceToGather.GetComponent<ResourceItemObject>().GetMaterialItem();
                                workPlace.GetComponent<LoadStation>().RemoveGatheredResourceFromReach(resourceToGather);
                                Destroy(resourceToGather.gameObject);
                                resourceToGather = null;
                                state = State.MovingToStorage;
                            }
                            else
                            {
                                state = State.Idle;
                            }
                            elapseTimeToAction = 0;
                        }
                    });
                }
                break;
            case State.MovingToStorage:
                if (citizen.IsIdle())
                {
                    citizen.MoveTo(workPlace.GetComponent<LoadStation>().GetDeliverySpot(), () =>
                    {
                        state = State.DroppingOffResource;
                    }, 2f);
                }
                break;
            case State.DroppingOffResource:
                if (citizen.IsIdle())
                {
                    DropOffMaterial(workPlace.GetComponent<LoadStation>().GetDeliverySpot().position, () =>
                    {
                        elapseTimeToAction += Time.deltaTime;
                        if (elapseTimeToAction >= timeToPickDropMaterialItem)
                        {
                            Vector3 deliveryPosition = workPlace.GetComponent<LoadStation>().GetDeliverySpot().position;

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
                            resourcesCarried = null;
                            state = State.Idle;
                            elapseTimeToAction = 0;
                        }
                    });
                }
                break;
        }
    }

    public void DropOffMaterial(Vector3 lookAtPosition, Action OnMaterialDropped)
    {
        transform.LookAt(lookAtPosition);
        OnMaterialDropped.Invoke();
    }

    public void ChoppTree(Vector3 lookAtPosition, Action OnTreeChopped)
    {
        transform.LookAt(lookAtPosition);
        OnTreeChopped.Invoke();
    }

    public void PickUpMaterial(Vector3 lookAtPosition, Action OnMaterialPicked)
    {
        transform.LookAt(lookAtPosition);
        OnMaterialPicked.Invoke();
    }

    private bool HasResourceToCarry()
    {
        //if (workPlace.GetComponent<IMachine>().HasResourceToGather())
        if (workPlace.GetComponent<LoadStation>().HasResourceToGather())
        {
            return true;
        }
        return false;
    }
}


