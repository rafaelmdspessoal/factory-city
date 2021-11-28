using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Citizen : MonoBehaviour, ICitizen
{
    [SerializeField] private int moveSpeed;
    [SerializeField] private Transform workPlace;

    private NavMeshAgent navMeshAgent;
    [SerializeField] private Transform destinationObj;
    private bool hasJob = false;

    public int attackDamage;
    public float attackSpeed;

    public bool reachedDestination;
    private Action onArrivedAtPosition;

    private void Awake()
    {
    }

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = moveSpeed;
        PopulationManager.AddCitizen(1, this);

        LookForJob();
        JobManager.OnJobChanged += delegate (object sender, EventArgs e)
        {
            LookForJob();
        };
        
    }

    private void Update()
    {
        HandleMovement();
    }

    private bool IsDestinationMoving(Transform obj)
    {
        Rigidbody destinationRB = obj.GetComponent<Rigidbody>();
        if (destinationRB == null || destinationRB.velocity.sqrMagnitude < 0.05f) return false;
        return true;
    }

    public void SetDestinationObj(Transform destObj)
    {
        if (navMeshAgent.pathPending) return;
        destinationObj = destObj;
    }

    private void HandleMovement()
    {
        if (!HasReachedDestination())
        {
            reachedDestination = false;
            navMeshAgent.destination = destinationObj.position;
        }
        else
        {
            reachedDestination = true;
            if (onArrivedAtPosition != null)
            {
                Action tmpAction = onArrivedAtPosition;
                onArrivedAtPosition = null;
                tmpAction();
            }
        }
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

    public bool IsIdle()
    {
        return destinationObj == null;
    }

    public bool HasReachedDestination()
    {   if (IsIdle()) return true;

        if (Vector3.Distance(destinationObj.position, transform.position) <= navMeshAgent.stoppingDistance + .5f) {
            destinationObj = null;
            return true;
        }
        return false;
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

    public void MoveTo(Transform destination, Action onArrivedAtPosition)
    {
        SetDestinationObj(destination);
        this.onArrivedAtPosition = onArrivedAtPosition;
    }
}

