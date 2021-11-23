using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    public bool collided = false;
    public List<Collider> nearbyColliders = new List<Collider>();


    private void OnTriggerEnter(Collider collider)
    {
        nearbyColliders.Add(collider);
    }

    private void OnTriggerExit(Collider collider)
    {
        nearbyColliders.Remove(collider);
    }

    private void OnCollistionEnter(Collider collider)
    {
        collided = true;
    }

    private void OnCollistionExit(Collider collider)
    {
        collided = false;
    }
}
