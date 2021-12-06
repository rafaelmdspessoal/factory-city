using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowGameResources : MonoBehaviour
{
    [SerializeField] private List<ResourceScriptableObject> resourceList = new List<ResourceScriptableObject>();

    private void Start()
    {
        ResourceManager.Init();
        UpdateResourceTextObject();
        ResourceManager.OnResourceAmountChanged += delegate (object sender, EventArgs e)
        {
            UpdateResourceTextObject();
        };
    }
    void UpdateResourceTextObject()
    {
        for (int i = 0; i < resourceList.Count; i++)
        {
            transform.Find(resourceList[i].name).GetComponent<Text>().text = resourceList[i].name + ": " + ResourceManager.GetResourceAmout(resourceList[i]);
        }
    }

    public List<ResourceScriptableObject> GetResourceList() => resourceList;
}
