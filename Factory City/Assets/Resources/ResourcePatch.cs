using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePatch : MonoBehaviour
{
    [SerializeField] private Transform resourceContainer;
    [SerializeField] private int numberOfResources;
    [SerializeField] private float patchRadius;

    private void Start()
    {        
        TerrainData terrainData = Terrain.activeTerrain.terrainData;
        for (int i = 0; i < numberOfResources; i++)
        {
            float patchPosX = UnityEngine.Random.Range(0, patchRadius);
            float patchPosZ = Mathf.Sqrt(Mathf.Pow(patchRadius, 2) - Mathf.Pow(patchPosX, 2));
            float randomSignX = (UnityEngine.Random.Range(0, 2) - 0.5f) * 2;
            float randomSignZ = (UnityEngine.Random.Range(0, 2) - 0.5f) * 2;

            patchPosX = UnityEngine.Random.Range(0, patchPosX) * randomSignX;
            patchPosZ = UnityEngine.Random.Range(0, patchPosZ) * randomSignZ;

            Vector3 randomOffset = new Vector3(
                patchPosX,
                terrainData.GetHeight(Mathf.RoundToInt(patchPosX), Mathf.RoundToInt(patchPosZ)) + resourceContainer.localScale.y,
                patchPosZ
               );

            Transform treeObj = Instantiate(
            resourceContainer, randomOffset + transform.position, Quaternion.identity, transform);
        }
    }

}
