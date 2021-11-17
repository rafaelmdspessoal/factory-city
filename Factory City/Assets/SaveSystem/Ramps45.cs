using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ramps45 : MonoBehaviour, ISaveable
{
    public static List<Transform> ramps45 = new List<Transform>();

    public void PopulateSaveData(SaveData saveData)
    {
        foreach (Transform ramp45 in ramps45)
        {
            SaveData.Ramp45Data ramp45Data = new SaveData.Ramp45Data();
            ramp45Data.position = ramp45.position;
            ramp45Data.scale = ramp45.localScale;
            ramp45Data.rotation = ramp45.localRotation;
            saveData.ramp45Data.Add(ramp45Data);
        }
    }

    public void LoadFromSaveData(SaveData saveData)
    {
        foreach (Transform obj in ramps45)
        {
            obj.GetComponent<Ramp45>().DestroySelf();
        }
        foreach (SaveData.Ramp45Data ramp45Data in saveData.ramp45Data)
        {
            Transform ramp45 = Instantiate(
                BuildingsBuildingSystemAssets.Instance.ramp45.prefab,
                ramp45Data.position,
                ramp45Data.rotation
                        );
            ramp45.localScale = ramp45Data.scale;
            ramp45.GetComponent<IManipulable>().CreateSelf();
        }
    }
}