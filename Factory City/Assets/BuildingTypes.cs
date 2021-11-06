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
    };

    private void Update()
    {
        if (CharacterStates.Instance.state == CharacterStates.State.Building)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            { 
                buildingType = BuildingType.Platforms;
                BuildingSystem.Instance.UnsetSelectedObject();
                print("Platform");
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                buildingType = BuildingType.Columns;
                BuildingSystem.Instance.UnsetSelectedObject();
                print("Column");
            }
        }
    }
}
