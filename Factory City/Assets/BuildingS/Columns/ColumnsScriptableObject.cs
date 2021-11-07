using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColumnsScriptableObject", menuName = "ScriptableObjects/ColumnsScriptableObject")]
public class ColumnsScriptableObject : ScriptableObject
{
    public Transform prefab;
    public Transform visual;
    public string columnName;
}
