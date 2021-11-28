using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class SelectStorageTypeUI : MonoBehaviour
{
    void Awake()
    {
        transform.Find("loadStationBtn").GetComponent<Button_UI>().ClickFunc = () =>
        {
            BuildingSystem.Instance.SetSelectedObject(BuildingsBuildingSystemAssets.Instance.loadStation);
        };
        transform.Find("unloadStationBtn").GetComponent<Button_UI>().ClickFunc = () =>
        {
            BuildingSystem.Instance.SetSelectedObject(BuildingsBuildingSystemAssets.Instance.unloadStation);
        };
    }
}
