using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ramps30 : MonoBehaviour, ISaveable
{
    public static List<Transform> ramps30 = new List<Transform>();

    public void PopulateSaveData(SaveData saveData)
    {
        foreach (Transform ramp30 in ramps30)
        {
            SaveData.Ramp30Data ramp30Data = new SaveData.Ramp30Data();
            ramp30Data.position = ramp30.position;
            ramp30Data.rotation = ramp30.localRotation;
            saveData.ramp30Data.Add(ramp30Data);
        }
    }

    public void LoadFromSaveData(SaveData saveData)
    {
        foreach (SaveData.Ramp30Data ramps30Data in saveData.ramp30Data)
        {
            Instantiate(
                BuildingsBuildingSystemAssets.Instance.ramp30.prefab,
                ramps30Data.position,
                ramps30Data.rotation
            );
        }
    }
}
