using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedObjectHandler : MonoBehaviour
{
    public static SelectedObjectHandler Instance;

    void Awake()
    {
        Instance = this;
    }

    public void RotateSelectedObject(BuildingsScriptableObjects buildingSO, Transform selectedObj)
    {
        buildingSO.GetNextRotation();
        switch (buildingSO.buildingRotation)
        {            
            case BuildingsScriptableObjects.BuildingRotation.Zero:
                buildingSO.currentDimention = buildingSO.dimention;
                selectedObj.rotation = Quaternion.Euler(0, 0, 0);
                print("0 Graus");
                break;
            case BuildingsScriptableObjects.BuildingRotation.Ninety:
                buildingSO.currentDimention.x = buildingSO.dimention.z;
                buildingSO.currentDimention.z = buildingSO.dimention.x;
                selectedObj.rotation = Quaternion.Euler(0, 90, 0);
                print("90 Graus");
                break;
            case BuildingsScriptableObjects.BuildingRotation.OneEighty:
                buildingSO.currentDimention = buildingSO.dimention;
                selectedObj.rotation = Quaternion.Euler(0, 180, 0);
                print("180 Graus");
                break;
            case BuildingsScriptableObjects.BuildingRotation.TwoSeventy:
                buildingSO.currentDimention.x = buildingSO.dimention.z;
                buildingSO.currentDimention.z = buildingSO.dimention.x;
                selectedObj.rotation = Quaternion.Euler(0, 270, 0);
                print("270 Graus");
                break;
        }
    }
}
