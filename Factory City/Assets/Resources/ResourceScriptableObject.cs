using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceScriptableObject", menuName = "ScriptableObjects/ResourceScriptableObject")]
public class ResourceScriptableObject : ScriptableObject
{
    public string materialName;
    public Transform pfMaterialItem;
    public int weight;
    public Sprite sprite;
}
