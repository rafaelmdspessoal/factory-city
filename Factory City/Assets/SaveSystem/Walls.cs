using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour, ISaveable
{
    public static List<Transform> walls = new List<Transform>();

    public void PopulateSaveData(SaveData saveData)
    {
        foreach (Transform wall in walls)
        {
            SaveData.WallData wallData = new SaveData.WallData();
            wallData.position = wall.position;
            wallData.scale = wall.localScale;
            wallData.rotation = wall.localRotation;
            saveData.wallData.Add(wallData);
        }
    }

    public void LoadFromSaveData(SaveData saveData)
    {
        foreach (Transform obj in walls)
        {
            obj.GetComponent<Wall>().DestroySelf();
        }
        foreach (SaveData.WallData wallData in saveData.wallData)
        {
            Transform wall = Instantiate(
                BuildingsBuildingSystemAssets.Instance.wall.prefab,
                wallData.position,
                wallData.rotation
                        );
            wall.localScale = wallData.scale;
            wall.GetComponent<IManipulable>().CreateSelf();
        }
    }
}