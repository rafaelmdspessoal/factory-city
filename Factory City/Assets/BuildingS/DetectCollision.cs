using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    [SerializeField] private LayerMask triggerCollisionLayers = new LayerMask();

    public bool collided = false;
    public List<Transform> nearbyTransforms = new List<Transform>();

    private void OnTriggerEnter(Collider collider)
    {
        nearbyTransforms.Add(collider.transform);
        collided = HasCollided(nearbyTransforms);
    }

    private void OnTriggerExit(Collider collider)
    {
        nearbyTransforms.Remove(collider.transform);
        collided = HasCollided(nearbyTransforms);
    }

    private bool HasCollided(List<Transform> colliderTransforms)
    {
        foreach(Transform colTransform in colliderTransforms)
        {
            LayerMask layer = colTransform.gameObject.layer;
            if (triggerCollisionLayers == (triggerCollisionLayers | (1 << layer)))
            {
                return true;
            }
        }
        return false;
    }
}
