using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTypes : MonoBehaviour
{
    public static BuildingTypes Instance { get; private set; }

    public BuildingType buildingType;

    private void Awake()
    {
        Instance = this;
        buildingType = BuildingType.Platforms;
    }

    public enum BuildingType
    {
        Platforms,
        Walls,
        Columns,
        Ramps,
        Doors,
        Machines,
        Storages,
    };

    private void Update()
    {
        if (CharacterStates.Instance.state == CharacterStates.State.Building)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            { 
                buildingType = BuildingType.Platforms;
                BuildingSystem.Instance.UnsetSelectedObject();
                BuildingSystem.Instance.SetSelectedObject(BuildingsBuildingSystemAssets.Instance.platform);
                print("Platform");
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                buildingType = BuildingType.Columns;
                BuildingSystem.Instance.UnsetSelectedObject();
                BuildingSystem.Instance.SetSelectedObject(BuildingsBuildingSystemAssets.Instance.column);
                print("Column");
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                buildingType = BuildingType.Ramps;
                BuildingSystem.Instance.UnsetSelectedObject();
                BuildingSystem.Instance.SetSelectedObject(BuildingsBuildingSystemAssets.Instance.ramp15);
                print("Ramp");
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                buildingType = BuildingType.Walls;
                BuildingSystem.Instance.UnsetSelectedObject();
                BuildingSystem.Instance.SetSelectedObject(BuildingsBuildingSystemAssets.Instance.wall);
                print("Wall");
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                buildingType = BuildingType.Doors;
                BuildingSystem.Instance.UnsetSelectedObject();
                BuildingSystem.Instance.SetSelectedObject(BuildingsBuildingSystemAssets.Instance.door);
                print("Door");
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                buildingType = BuildingType.Machines;
                BuildingSystem.Instance.UnsetSelectedObject();
                BuildingSystem.Instance.SetSelectedObject(BuildingsBuildingSystemAssets.Instance.brikMachine);
                print("Machines");
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                buildingType = BuildingType.Storages;
                BuildingSystem.Instance.UnsetSelectedObject();
                BuildingSystem.Instance.SetSelectedObject(BuildingsBuildingSystemAssets.Instance.loadStation);
                print("Storage");
            }
        }
    }
}
