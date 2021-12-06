using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceItemObject : MonoBehaviour
{
    public static ResourceItemObject CreateMaterialItemObject (Vector3 position, Transform parent, ResourceItem resourceItem)
    {
        Transform transform = Instantiate(
            resourceItem.GetMaterialItemPrefab(), 
            position, 
            Quaternion.Euler(UnityEngine.Random.Range(-2, 2), 0 , UnityEngine.Random.Range(-2, 2)),
            parent
        );
        ResourceItemObject materialItemObject = transform.GetComponent<ResourceItemObject>();
        return materialItemObject;
    }

    [SerializeField] private ResourceItem materialItem;
    public event EventHandler OnResourceGathered;

    public void SetMaterialItem(ResourceItem materialItem) { this.materialItem = materialItem; }

    public void SetMaterialItemComponents(ResourceItem materialItem)
    {
        transform.name = materialItem.GetMaterialname();
        transform.localScale = materialItem.GetScale();
        transform.localRotation = materialItem.GetRotation();
        MeshFilter meshFilter = transform.gameObject.AddComponent<MeshFilter>();
        meshFilter.sharedMesh = materialItem.GetMeshFilter().sharedMesh;
        MeshRenderer meshRenderer = transform.gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = materialItem.GetMeshRenderer().sharedMaterial;
    }

    public ResourceItem GetMaterialItem() => materialItem;
    private void OnDestroy() { if (OnResourceGathered != null) OnResourceGathered(this, EventArgs.Empty); }
}
