using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMachine 
{
    LoadStation GetLoadStation();

    UnloadStation GetUnloadStation();

    Transform GetCarrierSpot();

    Transform GetOperatorSpot();

    Transform GetResourceToGather();

    bool HasResourceToGather();

    void RemoveResourceItem(ResourceItem resourceItem);

    void AddResourceItem(ResourceItem resourceItem);

    int GetResourceAmout();

    bool IsInventoryFull();

    ResourceItem GetItemToDeliver();

    ResourceItem GetItemToPickup();
    
    void SetRecipe(RecipeScriptableObject recipe);

    void RemoveGatheredResourceFromReach(Transform materialItem);
}

