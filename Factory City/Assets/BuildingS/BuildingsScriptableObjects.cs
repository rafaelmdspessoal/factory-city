using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingsScriptableObjects", menuName = "ScriptableObjects/BuildingsScriptableObjects")]
public class BuildingsScriptableObjects : ScriptableObject
{
    public Transform prefab;
    public Transform visual;
    public Vector3 dimention = Vector3.zero;
    public string columnName;

    public Vector3 GetOffset()
    {
        Vector3 offset = new Vector3(0, dimention.y / 2, 0);
        return offset;
    }
}
