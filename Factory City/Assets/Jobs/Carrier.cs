using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrier : MonoBehaviour
{
    [SerializeField] private Transform carrierSpot;
    private Transform workPlace;
    private Citizen citizen;

    void Start()
    {
        citizen = transform.GetComponent<Citizen>();
        workPlace = citizen.GetWorkPlace();
        carrierSpot = workPlace.GetComponent<BrickMachine>().GetCarrierSpot();
        citizen.SetDestinationObj(carrierSpot);
    }

    void Update()
    {

    }
}
