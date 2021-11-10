using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class SelectRampTypeUI : MonoBehaviour
{
    void Awake()
    {
        transform.Find("RampBtn").GetComponent<Button_UI>().ClickFunc = () =>
        {
            BuildingSystem.Instance.SetSelectedObject(BuildingsBuildingSystemAssets.Instance.ramp);
        };
    }
}
