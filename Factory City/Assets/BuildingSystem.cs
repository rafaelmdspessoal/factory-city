using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem Instance { get; private set; }

    [SerializeField] private LayerMask mouseColliderLayerMask = new LayerMask();
    [SerializeField] private PositionSnap posistionSnap;

    private BuildingsScriptableObjects buildingScriptableObject;

    private float snapValue;
    private Transform visual;

    private Vector3 visualsPos = Vector3.zero;
    private Vector3 mousePos = Vector3.zero;
    private Vector3 objectOffset = Vector3.zero;

    private bool canBuild;

    private enum PositionSnap
    {
        Half,
        One,
        Two,
        Three,
        Four,
    };


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
                    IManipulable manipulableObj = hitTransform.GetComponent<IManipulable>();

                    if (manipulableObj != null) { manipulableObj.DestroySelf(); }                    
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

            ChooseSnap();
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
                                Mathf.RoundToInt(mousePos.y),
                                Mathf.RoundToInt(mousePos.z)
                            );
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
                    CreateSelectedObjectPrefab(buildingScriptableObject, visualsPos, visual.GetChild(0).rotation);
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
            selectedObjDimentions.y / 2 + hitObjScale.y,
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
        Vector3 finalPosition = hitObjPos;

        Vector3 hitObjectDimention = GetRotateObjectDimention(hitObjScale, hitObjRot);

        Vector3 offset = hitObjectDimention / 2;


        if (objHitPos.x > 0.2f) { finalPosition.x += offset.x; }
        else if (objHitPos.x < -0.2f) { finalPosition.x -= offset.x; }

        if (objHitPos.z > 0.2f) { finalPosition.z += offset.z; }
        else if (objHitPos.z < -0.2f) { finalPosition.z -= offset.z; }

        finalPosition.y += offset.y;

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
        hitObjPos.y = hitObjPos.y - (hitObjScale.y / 2);

        Vector3 hitObjectDimention = GetRotateObjectDimention(hitObjScale, hitObjRot);

        Vector3 offset = (selectedObjDimentions + hitObjectDimention) / 2;

        Vector3 finalPosition = hitObjPos;
        finalPosition.x += offset.x * objHitPos.x;
        finalPosition.y += offset.y * objHitPos.y;
        finalPosition.z += offset.z * objHitPos.z;

        return finalPosition;
    }

    private Vector3 GetRotateObjectDimention(Vector3 objDimention, Vector3 objRotation)
    {
        float hitTransformRotation = Mathf.Deg2Rad * objRotation.y;
        float cosRot = Mathf.RoundToInt(Mathf.Cos(hitTransformRotation));
        float sinRot = Mathf.RoundToInt(Mathf.Sin(hitTransformRotation));

        Vector3 rotatedObjectDimention = new Vector3(
            Mathf.Abs((objDimention.x * cosRot) - (objDimention.z * sinRot)),
            0,
            Mathf.Abs((objDimention.x * sinRot) + (objDimention.z * cosRot))
        );

        return rotatedObjectDimention;
    }

    public void CreateSelectedObjectPrefab(BuildingsScriptableObjects obj, Vector3 pos, Quaternion rot)
    {
        Transform prefab = Instantiate(obj.prefab, pos, Quaternion.identity);
        prefab.GetChild(0).rotation = rot;
        prefab.GetChild(0).GetComponent<IManipulable>().CreateSelf();
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

    void ChooseSnap()
    {
        switch (posistionSnap)
        {
            case PositionSnap.Half:
                snapValue = 0.5f;
                break;
            case PositionSnap.One:
                snapValue = 1;
                break;
            case PositionSnap.Two:
                snapValue = 2;
                break;
            case PositionSnap.Three:
                snapValue = 3;
                break;
            case PositionSnap.Four:
                snapValue = 4;
                break;
        }
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
        DetectCollision collision = ghostObject.GetChild(0).gameObject.GetComponent<DetectCollision>();
        if (collision.collided)
        {
            return false;
        }
        return true;
    }
}
