using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class SelectMachineTypeUI : MonoBehaviour
{    void Awake()
    {
        transform.Find("brickMachineBtn").GetComponent<Button_UI>().ClickFunc = () =>
        {
            BuildingSystem.Instance.SetSelectedObject(BuildingsBuildingSystemAssets.Instance.brikMacine);
        };
        transform.Find("inboundStorageBtn").GetComponent<Button_UI>().ClickFunc = () =>
        {
            BuildingSystem.Instance.SetSelectedObject(BuildingsBuildingSystemAssets.Instance.inboundStorage);
        };
        transform.Find("outboundStorageBtn").GetComponent<Button_UI>().ClickFunc = () =>
        {
            BuildingSystem.Instance.SetSelectedObject(BuildingsBuildingSystemAssets.Instance.outboundStorage);
        };
    }
}
