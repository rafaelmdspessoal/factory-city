using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoragesInReach : MonoBehaviour
{
    IMachine machine;
    private Color originalColor;
    string load_station = "Load Station";
    string unload_station = "Unload Station";

    private void Start()
    {
        machine = transform.parent.GetComponent<IMachine>();

        originalColor = transform.parent.GetComponent<MeshRenderer>().material.color;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag(load_station) || collider.gameObject.CompareTag(unload_station))
        {
            transform.parent.GetComponent<Renderer>().material.color = Color.green;
            machine.GetStationsInReach(collider.transform);
        }
        machine.AddStationsInReach();
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag(load_station) || collider.gameObject.CompareTag(unload_station))
        {
            transform.parent.GetComponent<Renderer>().material.color = originalColor;
            machine.RemoveStationsOutOfReach(collider.transform);
        }
    }
}
