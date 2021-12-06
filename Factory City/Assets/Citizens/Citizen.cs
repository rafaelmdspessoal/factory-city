using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CodeMonkey.Utils;

public class Citizen : MonoBehaviour, ICitizen
{
    [SerializeField] private Transform workPlace;
    [SerializeField] private Transform destinationObj;

    [SerializeField] private ToolScriptableObject tool;

    [SerializeField] private int moveSpeed;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackSpeed;

    [SerializeField] private Transform actionArea;
    [SerializeField] private Transform fieldOfViewTransform;

    private float stoppingDistance = .5f;
    private Action onArrivedAtPosition;
    private NavMeshAgent navMeshAgent;


    private void Start()
    {
        attackDamage += tool.attackDamage;
        attackSpeed += tool.attackSpeed;

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = moveSpeed;

        PopulationManager.AddCitizen(1, this);
        FunctionTimer.Create(LookForJob, 3);
        JobManager.OnJobChanged += LookForJob;
    }


    private void Update()
    {
        HandleMovement();
    }
    public bool IsCitizenMoving() => navMeshAgent.velocity.magnitude > 0;

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
            navMeshAgent.destination = destinationObj.position;
        }
        else
        {
            if (onArrivedAtPosition != null)
            {
                Action tmpAction = onArrivedAtPosition;
                onArrivedAtPosition = null;
                tmpAction();
            }
        }
    }

    public Transform GetWorkPlace() => workPlace;
    public bool IsIdle() => destinationObj == null;
    public bool HasReachedDestination()

    {   if (IsIdle()) return true;
            
        Debug.DrawLine(destinationObj.position, transform.position);
        if (Vector3.Distance(destinationObj.position, transform.position) <= navMeshAgent.stoppingDistance + stoppingDistance) {
            destinationObj = null;
            return true;
        }
        return false;
    }

    void LookForJob()
    {
        print("Looking For Job");        
        if (JobManager.jobList.Count > 0 && workPlace == null)
        {
            workPlace = JobManager.jobList[0];
            IHaveWorkers job = workPlace.GetComponent<IHaveWorkers>();
            job.Hire(this);
            JobManager.OnJobChanged -= LookForJob;
        }
    }

    public void MoveTo(Transform destination, Action onArrivedAtPosition, float stoppingDistance)
    {
        this.stoppingDistance = stoppingDistance;
        SetDestinationObj(destination);
        this.onArrivedAtPosition = onArrivedAtPosition;
    }

    public Transform GetActionArea() => actionArea;

    public Transform GetFieldOfViewTransform() => fieldOfViewTransform;

    public float GetAttackDamage() => attackDamage + UnityEngine.Random.Range(-attackDamage / 2, attackDamage / 2);

    public float GetAttackSpeed() => attackSpeed;
}

