using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialAssets : MonoBehaviour
{
    public static MaterialAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Transform pfMaterialObject;
}
