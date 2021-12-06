using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceContainer : MonoBehaviour, IDamageable
{
    public HealthSystem healthSystem;
    private Transform materialsParent;

    [SerializeField] private ResourceItem resourceItem;
    [SerializeField] private int healthAmount;
    [SerializeField] private int resourceAmount = 1;
    [SerializeField] private float resourceSpawnOffset;

    public event EventHandler OnResourceContainerDestroyed;

    private void Awake()
    {
        materialsParent = GameObject.Find("Materials").transform;
        healthSystem = new HealthSystem(healthAmount);
        healthSystem.OnDead += HealthSystem_OnDead;
    }

    public ResourceItem GetResourceItem() => resourceItem;

    private void HealthSystem_OnDead(object sender, System.EventArgs e)
    {
        Vector3 resourceSpawnPosition = transform.position;
        resourceSpawnPosition.y = resourceSpawnPosition.y / 2;
        for (int i = 0; i < resourceAmount; i++)
        {
            ResourceItemObject.CreateMaterialItemObject(resourceSpawnPosition, materialsParent, resourceItem);
            resourceSpawnPosition.y += resourceSpawnOffset;
        }
        OnResourceContainerDestroyed?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
    }

    public void Damage(float amount) { healthSystem.Damage(Mathf.RoundToInt(amount)); }
}
