using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBuildingType : MonoBehaviour
{
    [SerializeField] private GameObject[] platformsBtn;
    [SerializeField] private GameObject[] columnsBtn;
    [SerializeField] private GameObject[] wallsBtn;
    [SerializeField] private GameObject[] rampsBtn;

    private void Start()
    {
        foreach (GameObject platformBtn in platformsBtn)
        {
            platformBtn.SetActive(false);
        }
        foreach (GameObject columnBtn in columnsBtn)
        {
            columnBtn.SetActive(false);
        }
        foreach (GameObject rampBtn in rampsBtn)
        {
            rampBtn.SetActive(false);
        }
        foreach (GameObject wallBtn in wallsBtn)
        {
            wallBtn.SetActive(false);
        }
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
                    foreach (GameObject platformBtn in platformsBtn)
                    {
                        platformBtn.SetActive(true);
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
                }
                else if (BuildingTypes.Instance.buildingType == BuildingTypes.BuildingType.Columns)
                {
                    foreach (GameObject platformBtn in platformsBtn)
                    {
                        platformBtn.SetActive(false);
                    }
                    foreach (GameObject columnBtn in columnsBtn)
                    {
                        columnBtn.SetActive(true);
                    }
                    foreach (GameObject wallBtn in wallsBtn)
                    {
                        wallBtn.SetActive(false);
                    }
                    foreach (GameObject rampBtn in rampsBtn)
                    {
                        rampBtn.SetActive(false);
                    }
                }
                else if (BuildingTypes.Instance.buildingType == BuildingTypes.BuildingType.Ramps)
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
                        rampBtn.SetActive(true);
                    }
                }
                else if (BuildingTypes.Instance.buildingType == BuildingTypes.BuildingType.Walls)
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
                        wallBtn.SetActive(true);
                    }
                    foreach (GameObject rampBtn in rampsBtn)
                    {
                        rampBtn.SetActive(false);
                    }
                }

            }
            else if (Input.GetKeyUp(KeyCode.Q))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
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
            }
        }
    }
}
