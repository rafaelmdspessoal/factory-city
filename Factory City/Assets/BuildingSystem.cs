using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem Instance { get; private set; }

    [SerializeField] private LayerMask mouseColliderLayerMask = new LayerMask();

    private BuildingsScriptableObjects buildingScriptableObject;

    private Transform visual;

    private Vector3 visualsPos = Vector3.zero;
    private Vector3 mousePos = Vector3.zero;

    private bool canBuild;


    void Awake()
    {
        Instance = this;
    }

    void Update()
    {       
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        if (CharacterStates.Instance.state == CharacterStates.State.Demolishing)
        {
            if (UtilsClass.IsPointerOverUI())
            {
                return;
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out raycastHit, 999f, mouseColliderLayerMask))
                {
                    Transform hitTransform = raycastHit.transform;                   
                    DestroyObjectPrefab(hitTransform);                                        
                }
            }
        }
        else if (CharacterStates.Instance.state == CharacterStates.State.Building)
        {
            if (visual == null || UtilsClass.IsPointerOverUI())
            {
                canBuild = false;
                return;
            }

            RotateSelectedObject(buildingScriptableObject, visual);
            canBuild = CanBuild(visual);

            if (Physics.Raycast(ray, out raycastHit, 999f, mouseColliderLayerMask))
            {
                mousePos = raycastHit.point;
                Transform hitTransform = raycastHit.transform;
                LayerMask layer = raycastHit.transform.gameObject.layer;

                switch (BuildingTypes.Instance.buildingType)
                {
                    case BuildingTypes.BuildingType.Platforms:
                        if (layer != 7 && layer != 8 && layer != 9)
                        {
                            canBuild = false;
                            break;
                        }
                        if (layer == 7)
                        {
                            visualsPos = GetPlatformPositionToBuildPlatform(hitTransform, mousePos);
                        }
                        else if (layer == 8)
                        {
                            visualsPos = GetColumnPositionToBuildPlatform(hitTransform, mousePos);
                        }
                        else if (layer == 9)
                        {  
                            visualsPos = new Vector3(
                                Mathf.RoundToInt(mousePos.x),
                                buildingScriptableObject.dimention.y / 2,
                                Mathf.RoundToInt(mousePos.z)
                            );
                        }
                        break;
                    case BuildingTypes.BuildingType.Ramps:
                        if (layer != 7 && layer != 8 && layer != 9 && layer != 10)
                        {
                            canBuild = false;
                            break;
                        }
                        if (layer == 7)
                        {
                            visualsPos = GetPlatformPositionToBuildRamp(hitTransform, mousePos);
                        }
                        else if (layer == 8)
                        {
                            visualsPos = GetColumnPositionToBuildPlatform(hitTransform, mousePos);
                        }
                        else if (layer == 9)
                        {
                            visualsPos = new Vector3(
                                Mathf.RoundToInt(mousePos.x),
                                buildingScriptableObject.dimention.y / 2,
                                Mathf.RoundToInt(mousePos.z)
                            ); 
                        }
                        else if (layer == 10)
                        {
                            visualsPos = GetRampPositionToBuildRamp(hitTransform, raycastHit.normal, mousePos);
                        }
                        break;
                    case BuildingTypes.BuildingType.Columns:
                        if (layer != 7)
                        {
                            canBuild = false;
                            break;
                        }
                        visualsPos = GetPlatformPositionToBuildColumn(hitTransform, mousePos);
                        break;
                }

                visual.position = visualsPos;
            }
            else
            {
                canBuild = false;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (canBuild)
                {
                    CreateSelectedObjectPrefab(buildingScriptableObject, visualsPos, visual.rotation);
                }
                else
                {
                    UtilsClass.DrawTextUIPopup(
                        buildingScriptableObject.name,
                        Vector2.zero,
                        20,
                        null);
                }
            }
        }
       
    }

    private Vector3 GetColumnPositionToBuildPlatform(Transform referenceObject, Vector3 mousePosition)
    {
        Vector3 hitObjPos = referenceObject.position;
        Vector3 objHitPos = (mousePosition - hitObjPos);
        Vector3 selectedObjDimentions = buildingScriptableObject.currentDimention;
        Vector3 hitObjScale = referenceObject.localScale;
        Vector3 finalPosition = hitObjPos;

        Vector3 offset = new Vector3(
            selectedObjDimentions.x,
            selectedObjDimentions.y + hitObjScale.y,
            selectedObjDimentions.z
            ) / 2;            

        if (objHitPos.x > 0.2f) { finalPosition.x += offset.x; }
        else if (objHitPos.x < -0.2f) { finalPosition.x -= offset.x; }

        if (objHitPos.z > 0.2f) { finalPosition.z += offset.z; }
        else if (objHitPos.z < -0.2f) { finalPosition.z -= offset.z; }

        finalPosition.y += offset.y;

        return finalPosition;
    }

    private Vector3 GetPlatformPositionToBuildColumn(Transform referenceObject, Vector3 mousePosition)
    {
        Vector3 hitObjPos = referenceObject.position;
        Vector3 objHitPos = (mousePosition - hitObjPos);
        Vector3 hitObjRot = referenceObject.rotation.eulerAngles;
        Vector3 hitObjScale = referenceObject.localScale;
        Vector3 selectedObjDimentions = buildingScriptableObject.currentDimention;
        Vector3 hitObjectDimention = GetRotateObjectDimention(hitObjScale, hitObjRot);

        Vector3 offset = hitObjectDimention / 2;

        Vector3 finalPosition = hitObjPos;

        if (objHitPos.x > 0.2f) { finalPosition.x += offset.x; }
        else if (objHitPos.x < -0.2f) { finalPosition.x -= offset.x; }

        if (objHitPos.z > 0.2f) { finalPosition.z += offset.z; }
        else if (objHitPos.z < -0.2f) { finalPosition.z -= offset.z; }

        finalPosition.y += offset.y + selectedObjDimentions.y / 2;

        return finalPosition;
    }

    private Vector3 GetPlatformPositionToBuildPlatform(Transform referenceObject, Vector3 mousePosition)
    {     
        Vector3 hitObjPos = referenceObject.position;
        Vector3 objHitPos = (mousePosition - hitObjPos);
        Vector3 selectedObjDimentions = buildingScriptableObject.currentDimention;
        Vector3 hitObjScale = referenceObject.localScale;
        Vector3 hitObjRot = referenceObject.rotation.eulerAngles;

        objHitPos.y = 0;
        if (Mathf.Abs(objHitPos.x) > Mathf.Abs(objHitPos.z)) { objHitPos.z = 0; } else { objHitPos.x = 0; }
        objHitPos = objHitPos.normalized;

        Vector3 hitObjectDimention = GetRotateObjectDimention(hitObjScale, hitObjRot);

        Vector3 offset = (selectedObjDimentions + hitObjectDimention) / 2;

        Vector3 finalPosition = hitObjPos;
        finalPosition.x += offset.x * objHitPos.x;
        finalPosition.y += 0;
        finalPosition.z += offset.z * objHitPos.z;

        return finalPosition;
    }

    private Vector3 GetPlatformPositionToBuildRamp(Transform referenceObject, Vector3 mousePosition)
    {
        Vector3 hitObjPos = referenceObject.position;
        Vector3 objHitPos = (mousePosition - hitObjPos);
        Vector3 selectedObjDimentions = buildingScriptableObject.currentDimention;
        Vector3 hitObjScale = referenceObject.localScale;
        Vector3 hitObjRot = referenceObject.rotation.eulerAngles;

        objHitPos.y = 0;
        if (Mathf.Abs(objHitPos.x) > Mathf.Abs(objHitPos.z)) { objHitPos.z = 0; } else { objHitPos.x = 0; }
        objHitPos = objHitPos.normalized;

        Vector3 hitObjectDimention = GetRotateObjectDimention(hitObjScale, hitObjRot);

        Vector3 offset = (selectedObjDimentions + hitObjectDimention) / 2;

        Vector3 finalPosition = hitObjPos;
        finalPosition.x += offset.x * objHitPos.x;
        finalPosition.y += offset.y - hitObjectDimention.y;
        finalPosition.z += offset.z * objHitPos.z;

        return finalPosition;
    }

    private Vector3 GetRampPositionToBuildRamp(Transform referenceObject, Vector3 normal, Vector3 mousePosition)
    {
        Vector3 hitObjPos = referenceObject.position;
        Vector3 objHitPos = (mousePosition - hitObjPos);
        Vector3 selectedObjDimentions = buildingScriptableObject.currentDimention;
        Vector3 hitObjScale = referenceObject.localScale;
        Vector3 hitObjRot = referenceObject.rotation.eulerAngles;

        Vector3 horizontalHitDirection = new Vector3(objHitPos.x, 0, objHitPos.z);
        Vector3 vertialHitDirection = new Vector3(0, objHitPos.y, 0);

        Vector3 hitObjectDimention = GetRotateObjectDimention(hitObjScale, hitObjRot);
        hitObjectDimention.y = hitObjScale.y;
        Vector3 offset = (selectedObjDimentions + hitObjectDimention) / 2;

        Vector3 finalPosition = hitObjPos;

        Vector3 dir = Vector3.zero;
        if (normal.y > 0)
        {
            if (vertialHitDirection.y > 0.2f)
            {
                if (Mathf.RoundToInt(normal.x) == 0)
                {
                    dir.x = 0;
                    if (normal.z < 0) { dir.z = 1; } else { dir.z = -1; }
                }
                if (Mathf.RoundToInt(normal.z) == 0)
                {
                    dir.z = 0;
                    if (normal.x < 0) { dir.x = 1; } else { dir.x = -1; }
                }
                finalPosition.y += offset.y;
            }
            else if (vertialHitDirection.y < -0.2f)
            {
                if (Mathf.RoundToInt(normal.x) == 0)
                {
                    dir.x = 0;
                    if (normal.z > 0) { dir.z = 1; } else { dir.z = -1; }

                }
                if (Mathf.RoundToInt(normal.z) == 0)
                {
                    dir.z = 0;
                    if (normal.x > 0) { dir.x = 1; } else { dir.x = -1; }
                }
                finalPosition.y -= offset.y;
            }
            else
            {
                if (Mathf.RoundToInt(normal.x) == 0)
                {
                    horizontalHitDirection.z = 0;
                    horizontalHitDirection.x = Mathf.Sign(objHitPos.x) * 1;

                }
                if (Mathf.RoundToInt(normal.z) == 0)
                {
                    horizontalHitDirection.x = 0;
                    horizontalHitDirection.z = Mathf.Sign(objHitPos.z) * 1;
                }

                finalPosition.x += offset.x * horizontalHitDirection.x;
                finalPosition.z += offset.z * horizontalHitDirection.z;
            }
        }
        
        finalPosition.x += offset.x * dir.x;        
        finalPosition.z += offset.z * dir.z;

        return finalPosition;
    }

    private Vector3 GetRotateObjectDimention(Vector3 objDimention, Vector3 objRotation)
    {
        float hitTransformRotation = Mathf.Deg2Rad * objRotation.y;
        float cosRot = Mathf.RoundToInt(Mathf.Cos(hitTransformRotation));
        float sinRot = Mathf.RoundToInt(Mathf.Sin(hitTransformRotation));

        Vector3 rotatedObjectDimention = new Vector3(
            Mathf.Abs((objDimention.x * cosRot) - (objDimention.z * sinRot)),
            objDimention.y,
            Mathf.Abs((objDimention.x * sinRot) + (objDimention.z * cosRot))
        );

        return rotatedObjectDimention;
    }

    public void CreateSelectedObjectPrefab(BuildingsScriptableObjects obj, Vector3 pos, Quaternion rot)
    {
        Transform prefab = Instantiate(obj.prefab, pos, Quaternion.identity);
        prefab.rotation = rot;
        prefab.GetComponent<IManipulable>().CreateSelf();
        switch (BuildingTypes.Instance.buildingType)
        {
            case BuildingTypes.BuildingType.Platforms:
                Platforms.platforms.Add(prefab);
                break;
            case BuildingTypes.BuildingType.Columns:
                Columns.columns.Add(prefab);
                break;
        }
    }

    public void DestroyObjectPrefab(Transform prefab)
    {
        IManipulable obj = prefab.GetComponent<IManipulable>();
        if (obj != null)
        {
            switch (BuildingTypes.Instance.buildingType)
            {
                case BuildingTypes.BuildingType.Platforms:
                    Platforms.platforms.Remove(this.transform);
                    break;
                case BuildingTypes.BuildingType.Columns:
                    Columns.columns.Remove(this.transform);
                    break;
            }
            obj.DestroySelf();
        }
    }

    public void SetSelectedObject(BuildingsScriptableObjects selectedSO)
    {
        if (visual != null) { Destroy(visual.gameObject); }
        buildingScriptableObject = selectedSO;
        visual = Instantiate(buildingScriptableObject.visual);
    }

    public void UnsetSelectedObject()
    {
        if (visual != null) { Destroy(visual.gameObject); }
        buildingScriptableObject = null;
    }

    void RotateSelectedObject(BuildingsScriptableObjects selectedSO, Transform selectedObj)
    {
        if (Input.GetKeyDown(KeyCode.R) && selectedSO != null)
        {
            SelectedObjectHandler.Instance.RotateSelectedObject(selectedSO, selectedObj);
        }
    }

    private bool CanBuild(Transform ghostObject)
    {
        DetectCollision collision = ghostObject.gameObject.GetComponent<DetectCollision>();
        if (collision.collided)
        {
            return false;
        }
        return true;
    }
}
