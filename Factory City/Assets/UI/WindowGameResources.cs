using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowGameResources : MonoBehaviour
{
    private void Start()
    {
        UpdateResourceTextObject();
        ResourceManager.OnResourceAmountChanged += delegate (object sender, EventArgs e)
        {
            UpdateResourceTextObject();
        };
    }
    void UpdateResourceTextObject()
    {
        transform.Find("LogAmount").GetComponent<Text>().text = "Logs: " + ResourceManager.GetResourceAmout(ResourceManager.ResourceType.Log);
    }
}
