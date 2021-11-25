using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowGameJobs : MonoBehaviour
{
    private void Start()
    {
        UpdateJobTextObject();
        JobManager.OnJobChanged += delegate (object sender, EventArgs e)
        {
            UpdateJobTextObject();
        };
    }
    void UpdateJobTextObject()
    {
        transform.Find("JobsAmount").GetComponent<Text>().text = "Jobs: " + JobManager.GetJobAmout();
    }
}
