using System;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class ResourceProcessor : MonoBehaviour
{
    [SerializeField] private Citizen machineOperator;
    [SerializeField] private Transform operatorSpot;

    [SerializeField] private Transform fieldOfViewTransform;

    [SerializeField] private int maxStoredResources = 20;
    [SerializeField] private List<ResourceItem> storedResourceItems;
    [SerializeField] private RecipeScriptableObject recipeScriptableObject;

    [SerializeField] private List<LoadStation> loadStations;
    [SerializeField] private List<UnloadStation> unloadStations;

    private float elapsedCraftTime = 0;

    public virtual void Awake()
    {
        storedResourceItems = new List<ResourceItem>();
        loadStations = new List<LoadStation>();
        unloadStations = new List<UnloadStation>();
    }

    public virtual void Start()
    {
        FunctionTimer.Create(AddStorageStationInReach, 3);       
        fieldOfViewTransform.GetComponent<StoragesInReach>().OnObjectAdded += delegate (object sender, EventArgs e)
        {
            AddStorageStationInReach();
        };
        PopulateStorageAcceptableResources();
    }

    public virtual void Update()
    {
        if (recipeScriptableObject.CanCraftResourceItem(storedResourceItems))
        {
            elapsedCraftTime += Time.deltaTime;
            if (elapsedCraftTime >= recipeScriptableObject.craftingTime)
            {
                elapsedCraftTime = 0f;
                CraftItem(recipeScriptableObject);
            }
        }
    }
    private void CraftItem(RecipeScriptableObject recipe)
    {
        foreach (ResourceItem storedItem in storedResourceItems)
        {
            if (recipe.HasMaterialInInput(storedItem))
            {
                storedItem.amount -= recipe.GetInputResourceAmount(storedItem);
                ResourceManager.RemoveResourceAmount(storedItem.resourceScriptableObject, recipe.GetInputResourceAmount(storedItem));
            }
        }
        foreach (ResourceItem storedItem in storedResourceItems)
        {
            if (recipe.HasMaterialInOutput(storedItem))

            {
                storedItem.amount += recipe.GetOutputResourceAmount(storedItem);
                ResourceManager.AddResourceAmount(storedItem.resourceScriptableObject, recipe.GetOutputResourceAmount(storedItem));
            }
        }
    }

    private void AddStorageStationInReach()
    {
        foreach (Storage storage in fieldOfViewTransform.GetComponent<StoragesInReach>().GetObjectsInReach())
        {
            if (storage == null) continue;
            if (storage.transform.TryGetComponent<LoadStation>(out LoadStation loadStation))
            {
                if (!loadStations.Contains(loadStation))
                {
                    loadStations.Add(loadStation);
                    foreach (ResourceItem materialItem in recipeScriptableObject.inputResources)
                    {
                        loadStation.AddNewResource(materialItem);
                    }
                }
            }
            if (storage.transform.TryGetComponent<UnloadStation>(out UnloadStation unloadStation))
            {
                if (!unloadStations.Contains(unloadStation))
                {
                    unloadStations.Add(unloadStation);
                    foreach (ResourceItem materialItem in recipeScriptableObject.outputResources)
                    {
                        unloadStation.AddNewResource(materialItem);
                    }
                }
            }            
        }
    }

    private ResourceItem HasResourceInStorage(ResourceItem item)
    {
        for (int i = 0; i < storedResourceItems.Count; i++)
        {
            if (item.resourceScriptableObject == storedResourceItems[i].resourceScriptableObject)
            {
                return storedResourceItems[i];
            }
        }
        Debug.LogError("Storage does NOT contain resource: " + item.GetMaterialname());
        return null;
    }

    public LoadStation GetLoadStation() => loadStations[0];
    public UnloadStation GetUnloadStation() => unloadStations[0];
    public Transform GetOperatorSpot() => operatorSpot;
    public bool HasOperator() => machineOperator != null;
    public bool IsInventoryFull() => GetResourceAmout() >= maxStoredResources;
    public void SetOperator(Citizen citizen) => machineOperator = citizen;
    public void AddResourceItem(ResourceItem item)
    {
        int resourceAmount = GetResourceAmout();
        if (resourceAmount + item.amount <= maxStoredResources)
        {
            ResourceItem storedItem = HasResourceInStorage(item);
            if (storedItem != null)
            {
                storedItem.amount += item.amount;
                ResourceManager.AddResourceAmount(item.resourceScriptableObject, item.amount);
            }
            else
            {
                Debug.LogError("Item " + item.GetMaterialname() + " not found in Machine");
            }
        }
        else
        {
            Debug.LogError("Machine is full");
            return;
        }
    }

    public void RemoveResourceItem(ResourceItem item)
    {
        int resourceAmount = GetResourceAmout();
        if (resourceAmount - item.amount >= 0)
        {
            ResourceItem storedItem = HasResourceInStorage(item);
            if (storedItem != null)
            {
                storedItem.amount -= item.amount;
                ResourceManager.RemoveResourceAmount(item.resourceScriptableObject, item.amount);
            }
            else
            {
                Debug.LogError("Item " + item.GetMaterialname() + " not found in Machine");
            }
        }
        else
        {
            Debug.LogError("Not enough resources");
        }
    }
    public int GetResourceAmout(ResourceItem resourceItem)
    {
        int amount = 0;
        foreach (ResourceItem item in storedResourceItems)
        {
            if (item == resourceItem)
            {
                amount += item.amount;
            }
        }
        return amount;
    }

    public int GetResourceAmout()
    {
        int amount = 0;
        foreach (ResourceItem item in storedResourceItems)
        {
            amount += item.amount;
        }
        return amount;
    }
    public void SetRecipe(RecipeScriptableObject recipe) { recipeScriptableObject = recipe; }

    public RecipeScriptableObject GetRecipe() => recipeScriptableObject; 

    private void PopulateStorageAcceptableResources()
    {
        foreach (ResourceItem item in recipeScriptableObject.inputResources)
        {
            storedResourceItems.Add(CreateNewItemInstance(item));
        }
        foreach (ResourceItem item in recipeScriptableObject.outputResources)
        {
            storedResourceItems.Add(CreateNewItemInstance(item));
        }
    }

    public  ResourceItem CreateNewItemInstance(ResourceItem item)
    {
        ResourceItem newItem = new ResourceItem();
        newItem.resourceScriptableObject = item.resourceScriptableObject;
        newItem.amount = 0;
        return newItem;
    }

    public bool HasStorages()
    {
        if (loadStations.Count > 0 && unloadStations.Count > 0) return true;
        return false;
    }

    public Transform GetFieldOfViewTransform() => fieldOfViewTransform;
    public List<ResourceItem> GetStoredResources() => storedResourceItems;
}
