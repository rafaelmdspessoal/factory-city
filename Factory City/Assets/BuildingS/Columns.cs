using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Columns : MonoBehaviour, ISaveable
{
    public static List<Transform> columns = new List<Transform>();

    public void PopulateSaveData(SaveData saveData)
    {
        foreach (Transform column in columns)
        {
            SaveData.ColumnData columnData = new SaveData.ColumnData();
            columnData.position = column.position;
            columnData.rotation = column.localRotation;
            saveData.columnData.Add(columnData);
        }
    }

    public void LoadFromSaveData(SaveData saveData)
    {
        foreach (SaveData.ColumnData columnData in saveData.columnData)
        {
            Instantiate(
                BuildingsBuildingSystemAssets.Instance.column.prefab,
                columnData.position,
                columnData.rotation
            );
        }
    }
}
