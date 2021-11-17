using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ramps15 : MonoBehaviour, ISaveable
{
    public static List<Transform> ramps15 = new List<Transform>();

    public void PopulateSaveData(SaveData saveData)
    {
        foreach (Transform ramp15 in ramps15)
        {
            SaveData.Ramp15Data ramp15Data = new SaveData.Ramp15Data();
            ramp15Data.position = ramp15.position;
            ramp15Data.scale = ramp15.localScale;
            ramp15Data.rotation = ramp15.localRotation;
            saveData.ramp15Data.Add(ramp15Data);
        }
    }

    public void LoadFromSaveData(SaveData saveData)
    {
        foreach (Transform obj in ramps15)
        {
            obj.GetComponent<Ramp15>().DestroySelf();
        }
        foreach (SaveData.Ramp15Data ramp15Data in saveData.ramp15Data)
        {
            Transform ramp15 = Instantiate(
                BuildingsBuildingSystemAssets.Instance.ramp15.prefab,
                ramp15Data.position,
                ramp15Data.rotation
                        );
            ramp15.localScale = ramp15Data.scale;
            ramp15.GetComponent<IManipulable>().CreateSelf();
        }
    }
}