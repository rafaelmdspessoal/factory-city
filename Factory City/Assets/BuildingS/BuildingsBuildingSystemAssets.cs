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
    public BuildingsScriptableObjects door;
    public BuildingsScriptableObjects column;
    public BuildingsScriptableObjects wall;
    public BuildingsScriptableObjects ramp15;
    public BuildingsScriptableObjects ramp30;
    public BuildingsScriptableObjects ramp45;
    public BuildingsScriptableObjects loadStation;
    public BuildingsScriptableObjects unloadStation;
    public BuildingsScriptableObjects brikMachine;
    public BuildingsScriptableObjects sawMill;
}
