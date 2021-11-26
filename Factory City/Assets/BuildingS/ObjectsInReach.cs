using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsInReach : MonoBehaviour
{
    [SerializeField] private int radius;

    public List<Transform> GetResourcesInReach(ResourceManager.ResourceType[] resourceTypes)
    {
        List<Transform> resources = new List<Transform>();
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent<IResource>(out IResource resource))
            {
                foreach(ResourceManager.ResourceType resourceType in resourceTypes)
                {
                    if (resource.GetResourceType() == resourceType)
                    {
                        resources.Add(collider.transform);
                    }
                }
            }
        }
        return resources;
    }
}
