using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBuildingType : MonoBehaviour
{
    [SerializeField] private GameObject[] platformsBtn;
    [SerializeField] private GameObject[] doorsBtn;
    [SerializeField] private GameObject[] columnsBtn;
    [SerializeField] private GameObject[] wallsBtn;
    [SerializeField] private GameObject[] rampsBtn;
    [SerializeField] private GameObject[] machinesBtn;

    private void Start()
    {
        deactivateAllButtons();
    }

    private void Update()
    {
        if (CharacterStates.Instance.state == CharacterStates.State.Building)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                if (BuildingTypes.Instance.buildingType == BuildingTypes.BuildingType.Platforms)
                {
                    activateButtons(platformsBtn);
                }
                else if (BuildingTypes.Instance.buildingType == BuildingTypes.BuildingType.Columns)
                {
                    activateButtons(columnsBtn);
                }
                else if (BuildingTypes.Instance.buildingType == BuildingTypes.BuildingType.Ramps)
                {
                    activateButtons(rampsBtn);
                }
                else if (BuildingTypes.Instance.buildingType == BuildingTypes.BuildingType.Walls)
                {
                    activateButtons(wallsBtn);
                }
                else if (BuildingTypes.Instance.buildingType == BuildingTypes.BuildingType.Doors)
                {
                    activateButtons(doorsBtn);
                }
                else if (BuildingTypes.Instance.buildingType == BuildingTypes.BuildingType.Machines)
                {
                    activateButtons(machinesBtn);
                }
            }            
            else if (Input.GetKeyUp(KeyCode.Q))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                deactivateAllButtons();
            }
        }
    }

    private void deactivateButtons(GameObject[] btnList)
    {
        foreach (GameObject btn in btnList)
        {
            btn.SetActive(false);            
        }
    }
    private void activateButtons(GameObject[] btnList)
    {
        foreach (GameObject btn in btnList)
        {
            btn.SetActive(true);
        }
    }
    private void deactivateAllButtons()
    {
        foreach (GameObject platformBtn in platformsBtn)
        {
            platformBtn.SetActive(false);
        }
        foreach (GameObject columnBtn in columnsBtn)
        {
            columnBtn.SetActive(false);
        }
        foreach (GameObject wallBtn in wallsBtn)
        {
            wallBtn.SetActive(false);
        }
        foreach (GameObject rampBtn in rampsBtn)
        {
            rampBtn.SetActive(false);
        }
        foreach (GameObject doorBtn in doorsBtn)
        {
            doorBtn.SetActive(false);
        }
        foreach (GameObject machineBtn in machinesBtn)
        {
            machineBtn.SetActive(false);
        }
    }
}
