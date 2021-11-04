using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    public bool collided = false;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer != 0)
        {
            collided = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        collided = false;
    }
}
