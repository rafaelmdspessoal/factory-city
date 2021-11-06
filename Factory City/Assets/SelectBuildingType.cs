using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBuildingType : MonoBehaviour
{
    [SerializeField] private GameObject notBuildingBtn;
    [SerializeField] private GameObject[] platformsBtn;
    [SerializeField] private GameObject[] columnsBtn;

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        notBuildingBtn.SetActive(false);
        foreach (GameObject platformBtn in platformsBtn)
        {
            platformBtn.SetActive(false);
        }
        foreach (GameObject columnBtn in columnsBtn)
        {
            columnBtn.SetActive(false);
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
                    notBuildingBtn.SetActive(true);
                    foreach (GameObject platformBtn in platformsBtn)
                    {
                        platformBtn.SetActive(true);
                    }
                    foreach (GameObject columnBtn in columnsBtn)
                    {
                        columnBtn.SetActive(false);
                    }
                }
                else if (BuildingTypes.Instance.buildingType == BuildingTypes.BuildingType.Columns)
                {
                    notBuildingBtn.SetActive(true);
                    foreach (GameObject platformBtn in platformsBtn)
                    {
                        platformBtn.SetActive(false);
                    }
                    foreach (GameObject columnBtn in columnsBtn)
                    {
                        columnBtn.SetActive(true);
                    }
                }

            }
            else if (Input.GetKeyUp(KeyCode.Q))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                notBuildingBtn.SetActive(false);
                foreach (GameObject platformBtn in platformsBtn)
                {
                    platformBtn.SetActive(false);
                }
                foreach (GameObject columnBtn in columnsBtn)
                {
                    columnBtn.SetActive(false);
                }
            }
        }
    }
}
