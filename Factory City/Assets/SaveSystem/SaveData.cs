using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    [System.Serializable]
    public struct PlatformData
    {
        public Vector3 position;
        public Quaternion rotation;
    }
    [System.Serializable]
    public struct ColumnData
    {
        public Vector3 position;
        public Quaternion rotation;
    }
    [System.Serializable]
    public struct RampData
    {
        public Vector3 position;
        public Quaternion rotation;
    }
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

    public PlayerData playerData = new PlayerData();
    public List<PlatformData> platformData = new List<PlatformData>();
    public List<ColumnData> columnData = new List<ColumnData>();
    public List<RampData> rampData = new List<RampData>();

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
