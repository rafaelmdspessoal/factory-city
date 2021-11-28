using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsInReach : MonoBehaviour
{
    [SerializeField] private int radius;

    SphereCollider mCollider;

    private void Start()
    {
        mCollider = this.transform.GetComponent<SphereCollider>();
        mCollider.radius = radius;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<IResource>(out IResource resource))
        {
            GetComponentInParent<BrickMachine>().AddResourcesInReach(resource.GetResourceType(), collider.transform);         
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.TryGetComponent<IResource>(out IResource resource))
        {
            GetComponentInParent<BrickMachine>().RemoveResourcesOutOfReach(resource.GetResourceType(), collider.transform);
        }
    }
}
