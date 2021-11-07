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


    public BuildingRotation GetNextRotation(Transform obj)
    {
        switch (buildingRotation)
        {
            default:
                return BuildingRotation.Zero;
            case BuildingRotation.Zero:
                currentDimention.x = dimention.z;
                currentDimention.z = dimention.x;
                obj.rotation = Quaternion.Euler(0, 90, 0);
                buildingRotation = BuildingRotation.Ninety;
                return buildingRotation;
            case BuildingRotation.Ninety:
                currentDimention = dimention;
                obj.rotation = Quaternion.Euler(0, 180, 0);
                buildingRotation = BuildingRotation.OneEighty;
                return buildingRotation;
            case BuildingRotation.OneEighty:
                currentDimention.x = dimention.z;
                currentDimention.z = dimention.x;
                obj.rotation = Quaternion.Euler(0, 270, 0);
                buildingRotation = BuildingRotation.TwoSeventy;
                return buildingRotation;
            case BuildingRotation.TwoSeventy:
                currentDimention = dimention;
                obj.rotation = Quaternion.identity;
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
