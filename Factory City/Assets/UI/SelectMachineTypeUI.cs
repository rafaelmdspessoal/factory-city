using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class SelectMachineTypeUI : MonoBehaviour
{    void Awake()
    {
        transform.Find("brickMachineBtn").GetComponent<Button_UI>().ClickFunc = () =>
        {
            BuildingSystem.Instance.SetSelectedObject(BuildingsBuildingSystemAssets.Instance.brikMachine);
        };
        transform.Find("sawMillBtn").GetComponent<Button_UI>().ClickFunc = () =>
        {
            BuildingSystem.Instance.SetSelectedObject(BuildingsBuildingSystemAssets.Instance.sawMill);
        };
    }
}
