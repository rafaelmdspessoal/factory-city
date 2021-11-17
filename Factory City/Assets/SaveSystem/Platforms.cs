using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platforms : MonoBehaviour, ISaveable
{
    public static List<Transform> platforms = new List<Transform>();

    public void PopulateSaveData(SaveData saveData)
    {
        foreach (Transform platform in platforms)
        {
            SaveData.PlatformData platformData = new SaveData.PlatformData();
            platformData.position = platform.position;
            platformData.scale = platform.localScale;
            platformData.rotation = platform.localRotation;
            saveData.platformData.Add(platformData);
        }
    }

    public void LoadFromSaveData(SaveData saveData)
    {
        foreach (Transform obj in platforms)
        {
            obj.GetComponent<Platform>().DestroySelf();
        }
        foreach (SaveData.PlatformData platformData in saveData.platformData)
        {
            Transform platform = Instantiate(
                BuildingsBuildingSystemAssets.Instance.platform.prefab,
                platformData.position,
                platformData.rotation
                        );
            platform.localScale = platformData.scale;
            platform.GetComponent<IManipulable>().CreateSelf();
        }
    }
}