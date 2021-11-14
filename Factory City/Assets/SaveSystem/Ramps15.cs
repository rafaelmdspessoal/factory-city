using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ramps15 : MonoBehaviour, ISaveable
{
    public static List<Transform> ramps15 = new List<Transform>();

    public void PopulateSaveData(SaveData saveData)
    {
        foreach (Transform ramp15 in ramps15)
        {
            SaveData.Ramp15Data ramp15Data = new SaveData.Ramp15Data();
            ramp15Data.position = ramp15.position;
            ramp15Data.rotation = ramp15.localRotation;
            saveData.ramp15Data.Add(ramp15Data);
        }
    }

    public void LoadFromSaveData(SaveData saveData)
    {
        foreach (SaveData.Ramp15Data ramps15Data in saveData.ramp15Data)
        {
            Instantiate(
                BuildingsBuildingSystemAssets.Instance.ramp15.prefab,
                ramps15Data.position,
                ramps15Data.rotation
            );
        }
    }
}
