
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobManager 
{
    public static event EventHandler OnJobChanged;
    public static int jobAmount;
    public static List<Transform> jobList;

    public static void Init()
    {
        jobList = new List<Transform>();
        jobAmount = 0;
    }

    public static void AddJobSpot(int amount, Transform jobSpot)
    {
        jobAmount += amount;
        if (!jobList.Contains(jobSpot)) jobList.Add(jobSpot);
        if (OnJobChanged != null) OnJobChanged(null, EventArgs.Empty);
    }

    public static void RemoveJobSpot(int amount, Transform jobSpot)
    {
        jobAmount -= amount;
        if (!jobSpot.GetComponent<IHaveWorkers>().HasJobSpot()) jobList.Remove(jobSpot);
        if (OnJobChanged != null) OnJobChanged(null, EventArgs.Empty);
    }

    public static int GetJobAmout()
    {
        return jobAmount;
    }
}
