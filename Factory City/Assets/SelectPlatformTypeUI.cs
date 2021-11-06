using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class SelectPlatformTypeUI : MonoBehaviour
{
    void Awake()
    {
        transform.Find("Platform1Btn").GetComponent<Button_UI>().ClickFunc = () =>
        {
            BuildingSystem.Instance.SetSelectedObject(BuildingsBuildingSystemAssets.Instance.platform1);
        };
        transform.Find("Platform2Btn").GetComponent<Button_UI>().ClickFunc = () =>
        {
            BuildingSystem.Instance.SetSelectedObject(BuildingsBuildingSystemAssets.Instance.platform2);
        };
    }
}
