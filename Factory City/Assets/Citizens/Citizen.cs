using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Citizen : MonoBehaviour
{
    [SerializeField] private int moveSpeed;
    [SerializeField] private Transform workPlace;

    private NavMeshAgent navMeshAgent;
    [SerializeField] private Transform destinationObj;
    private bool hasJob = false;

    public int attackDamage;
    public float attackSpeed;


    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = moveSpeed;
    }

    private void Start()
    {
        destinationObj = this.transform;
        PopulationManager.AddCitizen(1, this);

        LookForJob();
        JobManager.OnJobChanged += delegate (object sender, EventArgs e)
        {
            LookForJob();
        };
    }

    private void Update()
    {
        if (!HasDestinationObj()) return;
        Debug.DrawLine(transform.position, destinationObj.position, Color.red);
        navMeshAgent.destination = destinationObj.position;
        
        if (HasReachedDestination())
        {
            
        }
        else
        {
            if(IsDestinationMoving(destinationObj))
            {
                SetMovementSpeed(0);
            }
            else
            {
                SetMovementSpeed(-1);
            }
        }
    }

    private bool IsDestinationMoving(Transform obj)
    {
        Rigidbody destinationRB = obj.GetComponent<Rigidbody>();
        if (destinationRB == null || destinationRB.velocity.sqrMagnitude < 0.05f) return false;
        return true;
    }

    public void SetDestinationObj(Transform destObj)
    {
        destinationObj = destObj;
    }

    public Transform GetWorkPlace()
    {
        return workPlace;
    }

    public void SetMovementSpeed(float speed)
    {
        if (speed < 0)
        {
            navMeshAgent.speed = moveSpeed;
        }
        else
        {
            navMeshAgent.speed = speed;
        }
    }

    public bool HasDestinationObj()
    {
        return destinationObj != null;
    }

    public bool HasReachedDestination()
    {
        return navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance;
    }

    void LookForJob()
    {
        if (JobManager.jobList.Count > 0 && !hasJob)
        {
            hasJob = true;
            Transform jobSpot = JobManager.jobList[0];
            IHaveWorkers job = jobSpot.GetComponent<IHaveWorkers>();
            job.Hire(this);
            workPlace = jobSpot;
        }
    }
}
