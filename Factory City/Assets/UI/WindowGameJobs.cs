using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowGameJobs : MonoBehaviour
{
    private void Awake()
    {
        JobManager.Init();
    }
    private void Start()
    {        
        UpdateJobTextObject();
        JobManager.OnJobChanged += UpdateJobTextObject;
    }
    void UpdateJobTextObject()
    {
        transform.Find("JobsAmount").GetComponent<Text>().text = "Jobs: " + JobManager.GetJobAmout();
    }
}
