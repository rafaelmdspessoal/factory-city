using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageStationOperator : MonoBehaviour
{
    [SerializeField] private Transform operatorSpot;
    private Transform workPlace;
    private Citizen citizen;
        
    void Start()
    {
        citizen = transform.GetComponent<Citizen>();
        workPlace = citizen.GetWorkPlace();
        operatorSpot = workPlace.GetComponent<Storage>().GetOperatorSpot();
        citizen.SetDestinationObj(operatorSpot);
    }

    void Update()
    {
        
    }
}
