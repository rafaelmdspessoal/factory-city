using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour, ISaveable
{
    public static List<Transform> doors = new List<Transform>();

    public void PopulateSaveData(SaveData saveData)
    {
        foreach (Transform door in doors)
        {
            SaveData.DoorData doorData = new SaveData.DoorData();
            doorData.position = door.position;
            doorData.scale = door.localScale;
            doorData.rotation = door.localRotation;
            saveData.doorData.Add(doorData);
        }
    }

    public void LoadFromSaveData(SaveData saveData)
    {
        foreach (Transform obj in doors)
        {
            obj.GetComponent<Door>().DestroySelf();
        }
        foreach (SaveData.DoorData doorData in saveData.doorData)
        {
            Transform door = Instantiate(
                BuildingsBuildingSystemAssets.Instance.door.prefab,
                doorData.position,
                doorData.rotation
                        );
            door.localScale = doorData.scale;
            door.GetComponent<IManipulable>().CreateSelf();
        }
    }
}
