using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingsScriptableObjects", menuName = "ScriptableObjects/BuildingsScriptableObjects")]
public class BuildingsScriptableObjects : ScriptableObject
{
    public Transform prefab;
    public Transform visual;
    public Vector3 dimention;
    public Vector3 currentDimention;
    public string columnName;
    public BuildingRotation buildingRotation;

    void Awake()
    {
        buildingRotation = BuildingRotation.Zero;
        currentDimention = dimention;
    }

    public BuildingRotation GetNextRotation()
    {
        switch (buildingRotation)
        {
            default:
                buildingRotation = BuildingRotation.Zero;
                return buildingRotation;
            case BuildingRotation.Zero:
                buildingRotation = BuildingRotation.Ninety;
                return buildingRotation;
            case BuildingRotation.Ninety:
                buildingRotation = BuildingRotation.OneEighty;
                return buildingRotation;
            case BuildingRotation.OneEighty:
                buildingRotation = BuildingRotation.TwoSeventy;
                return buildingRotation;
            case BuildingRotation.TwoSeventy:
                buildingRotation = BuildingRotation.Zero;
                return buildingRotation;
        }
    }

    public Vector3 GetOffset()
    {
        Vector3 offset = new Vector3(0, dimention.y / 2, 0);
        return offset;
    }

    public enum BuildingRotation
    {
        Zero,
        Ninety,
        OneEighty,
        TwoSeventy,
    };
}
