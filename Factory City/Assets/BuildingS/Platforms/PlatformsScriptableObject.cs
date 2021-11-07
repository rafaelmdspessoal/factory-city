using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlatformsScriptableObject", menuName = "ScriptableObjects/PlatformsScriptableObject")]
public class PlatformsScriptableObject : ScriptableObject
{
    public Transform prefab;
    public Transform visual;
    public string platformName;
}
