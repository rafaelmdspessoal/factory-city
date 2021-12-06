using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineOperator : MonoBehaviour
{
    [SerializeField] private Transform operatorSpot;
    private Transform workPlace;
    private Citizen citizen;

    void Start()
    {
        transform.GetComponent<MeshRenderer>().material.color = Color.green;
        citizen = transform.GetComponent<Citizen>();
        workPlace = citizen.GetWorkPlace();
        operatorSpot = workPlace.GetComponent<IMachine>().GetOperatorSpot();
        citizen.SetDestinationObj(operatorSpot);
    }

    void Update()
    {

    }
}
