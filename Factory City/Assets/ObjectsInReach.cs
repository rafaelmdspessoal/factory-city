using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsInReach : MonoBehaviour
{
    [SerializeField] private List<Transform> objectsInReach;
    public event EventHandler OnObjectAdded;

    public List<Transform> GetObjectsInReach()
    {
        return objectsInReach;
    }

    private void Start()
    {
        objectsInReach = new List<Transform>();
    }

    private void OnTriggerEnter(Collider collider)
    {        
        objectsInReach.Add(collider.transform);
        OnObjectAdded?.Invoke(this, EventArgs.Empty);
    }

    private void OnTriggerExit(Collider collider)
    {
        objectsInReach.Remove(collider.transform);
    }
}
