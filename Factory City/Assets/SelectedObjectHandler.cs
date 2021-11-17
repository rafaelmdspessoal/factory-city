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
                selectedObj.rotation = Quaternion.Euler(0, 0, 0);
                buildingSO.currentRotation = Vector3.zero;
                print("0 Graus");
                break;
            case BuildingsScriptableObjects.BuildingRotation.Ninety:
                selectedObj.rotation = Quaternion.Euler(0, 90, 0);
                buildingSO.currentRotation = new Vector3(0, 90, 0);
                print("90 Graus");
                break;
            case BuildingsScriptableObjects.BuildingRotation.OneEighty:
                selectedObj.rotation = Quaternion.Euler(0, 180, 0);
                buildingSO.currentRotation = new Vector3(0, 180, 0);
                print("180 Graus");
                break;
            case BuildingsScriptableObjects.BuildingRotation.TwoSeventy:
                selectedObj.rotation = Quaternion.Euler(0, 270, 0);
                buildingSO.currentRotation = new Vector3(0, 270, 0);
                print("270 Graus");
                break;
        }
    }

    public void ScaleSelectedObject(BuildingsScriptableObjects buildingSO, Transform selectedObj)
    {
        switch (buildingSO.buildingScale)
        {
            case BuildingsScriptableObjects.BuildingScale.Half:
                Vector3 buildingScaleDimensionHalf = new Vector3(
                   (-buildingSO.dimention.x * buildingSO.scalableDimentions.x * .5f) + buildingSO.dimention.x,
                   (-buildingSO.dimention.y * buildingSO.scalableDimentions.y * .5f) + buildingSO.dimention.y,
                   (-buildingSO.dimention.z * buildingSO.scalableDimentions.z * .5f) + buildingSO.dimention.z
                    );
                selectedObj.localScale = buildingScaleDimensionHalf;
                buildingSO.currentDimention = buildingScaleDimensionHalf;
                print("Size 0.5");
                break;
            case BuildingsScriptableObjects.BuildingScale.One:                
                selectedObj.localScale = buildingSO.dimention;
                buildingSO.currentDimention = buildingSO.dimention;
                print("Size 1");
                break;
            case BuildingsScriptableObjects.BuildingScale.Two:
                Vector3 buildingScaleDimensionTwo = new Vector3(
                    (buildingSO.dimention.x * buildingSO.scalableDimentions.x) + buildingSO.dimention.x,
                    (buildingSO.dimention.y * buildingSO.scalableDimentions.y) + buildingSO.dimention.y,
                    (buildingSO.dimention.z * buildingSO.scalableDimentions.z) + buildingSO.dimention.z
                    );
                selectedObj.localScale = buildingScaleDimensionTwo;
                buildingSO.currentDimention = buildingScaleDimensionTwo;
                print("Size 2");
                break;
            case BuildingsScriptableObjects.BuildingScale.Three:
                Vector3 buildingScaleDimensionThree = new Vector3(
                    (buildingSO.dimention.x * buildingSO.scalableDimentions.x * 2) + buildingSO.dimention.x,
                    (buildingSO.dimention.y * buildingSO.scalableDimentions.y * 2) + buildingSO.dimention.y,
                    (buildingSO.dimention.z * buildingSO.scalableDimentions.z * 2) + buildingSO.dimention.z
                    );
                selectedObj.localScale = buildingScaleDimensionThree;
                buildingSO.currentDimention = buildingScaleDimensionThree;
                print("Size 3");
                break;
            case BuildingsScriptableObjects.BuildingScale.Four:
                Vector3 buildingScaleDimensionFour = new Vector3(
                    (buildingSO.dimention.x * buildingSO.scalableDimentions.x * 3) + buildingSO.dimention.x,
                    (buildingSO.dimention.y * buildingSO.scalableDimentions.y * 3) + buildingSO.dimention.y,
                    (buildingSO.dimention.z * buildingSO.scalableDimentions.z * 3) + buildingSO.dimention.z
                    );
                selectedObj.localScale = buildingScaleDimensionFour;
                buildingSO.currentDimention = buildingScaleDimensionFour;
                print("Size 4");
                break;
        }
    }
}
