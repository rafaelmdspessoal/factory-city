using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowGamePopulation : MonoBehaviour
{
    private void Awake()
    {
        PopulationManager.Init();
    }
    private void Start()
    {
        UpdateJobTextObject();
        PopulationManager.OnPopulationChanged += delegate (object sender, EventArgs e)
        {
            UpdateJobTextObject();
        };
    }
    void UpdateJobTextObject()
    {
        transform.Find("Population").GetComponent<Text>().text = "Pop: " + PopulationManager.GetPopulation();
    }
}
