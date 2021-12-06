using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ResourceItem
{
    public ResourceScriptableObject resourceScriptableObject;
    public int amount;

    public int GetResourceWeight() => resourceScriptableObject.weight;
    public Sprite GetMaterialSprite() => resourceScriptableObject.sprite;
    public Transform GetMaterialItemPrefab() => resourceScriptableObject.pfMaterialItem;
    public string GetMaterialname() => resourceScriptableObject.materialName;
    public Quaternion GetRotation() => GetMaterialItemPrefab().rotation;
    public Vector3 GetScale() => GetMaterialItemPrefab().localScale;
    public MeshFilter GetMeshFilter() => GetMaterialItemPrefab().GetComponent<MeshFilter>();
    public MeshRenderer GetMeshRenderer() => GetMaterialItemPrefab().GetComponent<MeshRenderer>();
}
