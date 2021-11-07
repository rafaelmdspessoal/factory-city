using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class SelectBuildingTypeUI : MonoBehaviour
{
    void Awake()
    {
        Transform platforms = transform.Find("SelectPlatformTypeUI");
        platforms.Find("Platform1Btn").GetComponent<Button_UI>().ClickFunc = () =>
        {

        };
    }
}