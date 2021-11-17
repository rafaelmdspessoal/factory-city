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
    public Vector3 scalableDimentions;
    public Vector3 currentRotation;
    public string columnName;
    public BuildingRotation buildingRotation;
    public BuildingScale buildingScale;
    public Color cantBuildColor;
    public Color visualColor;

    void Start()
    {
        buildingRotation = BuildingRotation.Zero;
        buildingScale = BuildingScale.One;
        currentDimention = dimention;
        currentRotation = Vector3.zero;
        visualColor = visual.GetComponent<Renderer>().material.color;
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

    public BuildingRotation GetNextScale()
    {
        switch (buildingScale)
        {
            default:
                buildingScale = BuildingScale.One;
                return buildingRotation;
            case BuildingScale.Four:
                buildingScale = BuildingScale.Half;
                return buildingRotation;
            case BuildingScale.Half:
                buildingScale = BuildingScale.One;
                return buildingRotation;
            case BuildingScale.One:
                buildingScale = BuildingScale.Two;
                return buildingRotation;
            case BuildingScale.Two:
                buildingScale = BuildingScale.Three;
                return buildingRotation;
            case BuildingScale.Three:
                buildingScale = BuildingScale.Four;
                return buildingRotation;
        }
    }

    public BuildingRotation GetPreviousScale()
    {
        switch (buildingScale)
        {
            default:
                buildingScale = BuildingScale.One;
                return buildingRotation;
            case BuildingScale.Four:
                buildingScale = BuildingScale.Three;
                return buildingRotation;
            case BuildingScale.Three:
                buildingScale = BuildingScale.Two;
                return buildingRotation;
            case BuildingScale.Two:
                buildingScale = BuildingScale.One;
                return buildingRotation;
            case BuildingScale.One:
                buildingScale = BuildingScale.Half;
                return buildingRotation;
            case BuildingScale.Half:
                buildingScale = BuildingScale.Four;
                return buildingRotation;
        }
    }

    public enum BuildingRotation
    {
        Zero,
        Ninety,
        OneEighty,
        TwoSeventy,
    };

    public enum BuildingScale
    {
        Half,
        One,
        Two,
        Three,
        Four,
    };
}
