using System;
using System.Collections.Generic;
using UnityEngine;

public class StoragesInReach : MonoBehaviour
{
    [SerializeField] private List<Storage> storagesInReach;
    public event EventHandler OnObjectAdded;

    public List<Storage> GetObjectsInReach()
    {
        return storagesInReach;
    }

    private void Start()
    {
        storagesInReach = new List<Storage>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.TryGetComponent<Storage>(out Storage storage))
        {
            storagesInReach.Add(storage);
            OnObjectAdded?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.transform.TryGetComponent<Storage>(out Storage storage))
            storagesInReach.Remove(storage);
    }
}
