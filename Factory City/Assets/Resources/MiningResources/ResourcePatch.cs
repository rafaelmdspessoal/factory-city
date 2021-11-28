using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePatch : MonoBehaviour
{
    [SerializeField] private Transform miningResourceTransform;
    [SerializeField] private int amountPerResource;
    [SerializeField] private int numberOfResources;
    [SerializeField] private float patchSize;
    [SerializeField] ResourceManager.ResourceType resourceType;

    private Terrain terrain;

    void Start()
    {
        TerrainData terrainData = Terrain.activeTerrain.terrainData;
        for (int i = 0; i < numberOfResources; i++)
        {
            float patchPosX = Random.Range(0, patchSize);
            float patchPosZ = Mathf.Sqrt(Mathf.Pow(patchSize, 2) - Mathf.Pow(patchPosX, 2));
            float randomSignX = (Random.Range(0, 2) - 0.5f) * 2;
            float randomSignZ = (Random.Range(0, 2) - 0.5f) * 2;

            patchPosX = Random.Range(0, patchPosX) * randomSignX;
            patchPosZ = Random.Range(0, patchPosZ) * randomSignZ;

            Vector3 stoneObjPos = new Vector3(
                patchPosX,
                terrainData.GetHeight(Mathf.RoundToInt(patchPosX), Mathf.RoundToInt(patchPosZ)),
                patchPosZ
               );

            Transform miningResource = Instantiate(miningResourceTransform, stoneObjPos + transform.position, Quaternion.identity, transform);
            MiningResource resource = miningResource.GetComponent<MiningResource>();
            resource.SetResourceType(resourceType);
            resource.SetResourceAmount(resourceType, amountPerResource);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
