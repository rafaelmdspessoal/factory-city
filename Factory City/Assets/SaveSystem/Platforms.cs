using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Platforms : MonoBehaviour, ISaveable
{
    public static List<Transform> platforms = new List<Transform>();

    public void PopulateSaveData(SaveData saveData)
    {
        foreach (Transform platform in platforms)
        {
            SaveData.PlatformData platformData = new SaveData.PlatformData();
            platformData.position = platform.position;
            platformData.rotation = platform.localRotation;
            saveData.platformData.Add(platformData);
        }
    }

    public void LoadFromSaveData(SaveData saveData)
    {
        foreach (SaveData.PlatformData platformData in saveData.platformData)
        {
            Instantiate(
                BuildingsBuildingSystemAssets.Instance.platform.prefab,
                platformData.position,
                platformData.rotation
            );
        }
    }
}
