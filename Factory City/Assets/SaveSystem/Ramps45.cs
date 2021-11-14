using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ramps45 : MonoBehaviour, ISaveable
{
    public static List<Transform> ramps45 = new List<Transform>();

    public void PopulateSaveData(SaveData saveData)
    {
        foreach (Transform ramp45 in ramps45)
        {
            SaveData.Ramp45Data ramp45Data = new SaveData.Ramp45Data();
            ramp45Data.position = ramp45.position;
            ramp45Data.rotation = ramp45.localRotation;
            saveData.ramp45Data.Add(ramp45Data);
        }
    }

    public void LoadFromSaveData(SaveData saveData)
    {
        foreach (SaveData.Ramp45Data ramps45Data in saveData.ramp45Data)
        {
            Instantiate(
                BuildingsBuildingSystemAssets.Instance.ramp45.prefab,
                ramps45Data.position,
                ramps45Data.rotation
            );
        }
    }
}
