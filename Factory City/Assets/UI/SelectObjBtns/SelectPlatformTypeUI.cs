using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class SelectPlatformTypeUI : MonoBehaviour
{
    void Awake()
    {
        transform.Find("PlatformBtn").GetComponent<Button_UI>().ClickFunc = () =>
        {
            BuildingSystem.Instance.SetSelectedObject(BuildingsBuildingSystemAssets.Instance.platform);
        };
    }
}
