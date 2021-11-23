using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class SelectColumnTypeUI : MonoBehaviour
{
    void Awake()
    {
        transform.Find("ColumnBtn").GetComponent<Button_UI>().ClickFunc = () =>
        {
            BuildingSystem.Instance.SetSelectedObject(BuildingsBuildingSystemAssets.Instance.column);
        };
    }
}
