using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    [System.Serializable]
    public struct PlayerData
    {
        public Vector3 playerCapsulePosition;
        public Quaternion playerCapsuleRotation;

        public Vector3 playerCameraPosition;
        public Quaternion playerCameraRotation;

        public Vector3 followCameraPosition;
        public Quaternion followCameraRotation;
    }
    [System.Serializable]
    public struct PlatformData
    {
        public Vector3 position;
        public Vector3 scale;
        public Quaternion rotation;
    }
    [System.Serializable]
    public struct ColumnData
    {
        public Vector3 position;
        public Vector3 scale;
        public Quaternion rotation;
    }
    [System.Serializable]
    public struct Ramp15Data
    {
        public Vector3 position;
        public Vector3 scale;
        public Quaternion rotation;
    }
    [System.Serializable]
    public struct Ramp30Data
    {
        public Vector3 position;
        public Vector3 scale;
        public Quaternion rotation;
    }
    [System.Serializable]
    public struct Ramp45Data
    {
        public Vector3 position;
        public Vector3 scale;
        public Quaternion rotation;
    }
    [System.Serializable]
    public struct WallData
    {
        public Vector3 position;
        public Vector3 scale;
        public Quaternion rotation;
    }
    [System.Serializable]
    public struct DoorData
    {
        public Vector3 position;
        public Vector3 scale;
        public Quaternion rotation;
    }


    public PlayerData playerData = new PlayerData();
    public List<PlatformData> platformData = new List<PlatformData>();
    public List<ColumnData> columnData = new List<ColumnData>();
    public List<Ramp15Data> ramp15Data = new List<Ramp15Data>();
    public List<Ramp30Data> ramp30Data = new List<Ramp30Data>();
    public List<Ramp45Data> ramp45Data = new List<Ramp45Data>();
    public List<DoorData> doorData = new List<DoorData>();
    public List<WallData> wallData = new List<WallData>();

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadFromJson(string a_Json)
    {
        JsonUtility.FromJsonOverwrite(a_Json, this);
    }
}

public interface ISaveable
{
    void PopulateSaveData(SaveData a_SaveData);
    void LoadFromSaveData(SaveData a_SaveData);
}
