using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsBuildingSystemAssets : MonoBehaviour
{
    public static BuildingsBuildingSystemAssets Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public BuildingsScriptableObjects platform;
    public BuildingsScriptableObjects column;
    public BuildingsScriptableObjects ramp;
}
