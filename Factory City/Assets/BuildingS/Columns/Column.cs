using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Column : MonoBehaviour, IManipulable
{
    public void DestroySelf()
    {
        Columns.columns.Remove(transform);
        Destroy(transform.gameObject);
        print("Destroy");
    }

    public void CreateSelf()
    {
        Columns.columns.Add(transform);
        print("Create");
    }
}
