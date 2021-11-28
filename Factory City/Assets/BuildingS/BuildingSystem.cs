using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem Instance { get; private set; }

    [SerializeField] private LayerMask mouseColliderLayerMask = new LayerMask();
    [SerializeField] private float snap;

    private BuildingsScriptableObjects buildingScriptableObject;

    private Transform visual;

    private Vector3 visualsPos = Vector3.zero;
    private Vector3 mousePos = Vector3.zero;

    private bool canBuild;
    private int[] layersNotToBuildIn;

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
            visual.GetComponent<Renderer>().material.color = buildingScriptableObject.visualColor;

            RotateSelectedObject(buildingScriptableObject, visual);
            ScaleSelectedObject(buildingScriptableObject, visual);
            canBuild = CanBuild(visual);
            if (!canBuild)
            {
                visual.GetComponent<Renderer>().material.color = buildingScriptableObject.cantBuildColor;
            }

            if (Physics.Raycast(ray, out raycastHit, 999f, mouseColliderLayerMask))
            {
                mousePos = raycastHit.point;
                Transform hitTransform = raycastHit.transform;
                LayerMask layer = raycastHit.transform.gameObject.layer;

                if (layer == 9)
                {
                    GetPositionToBuildMachines();
                }

                switch (BuildingTypes.Instance.buildingType)
                {
                    case BuildingTypes.BuildingType.Platforms:
                        if (layer != 7 && layer != 8 && layer != 9 && layer != 10)
                        {
                            canBuild = false;
                            visual.GetComponent<Renderer>().material.color = buildingScriptableObject.cantBuildColor;
                        }
                        if (layer == 7)
                        {
                            visualsPos = GetPlatformPositionToBuildPlatform(hitTransform, mousePos);
                        }
                        else if (layer == 8)
                        {
                            visualsPos = GetColumnPositionToBuildPlatform(hitTransform, mousePos);
                        }
                        else if (layer == 10)
                        {
                            visualsPos = GetRampPositionToBuildPlatform(hitTransform, raycastHit.normal, mousePos);
                        }
                        break;
                    case BuildingTypes.BuildingType.Ramps:
                        if (layer != 7 && layer != 8 && layer != 9 && layer != 10)
                        {
                            canBuild = false;
                            visual.GetComponent<Renderer>().material.color = buildingScriptableObject.cantBuildColor;
                        }
                        if (layer == 7)
                        {
                            visualsPos = GetPlatformPositionToBuildRamp(hitTransform, mousePos);
                        }
                        else if (layer == 8)
                        {
                            visualsPos = GetColumnPositionToBuildPlatform(hitTransform, mousePos);
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
                            visual.GetComponent<Renderer>().material.color = buildingScriptableObject.cantBuildColor;
                        }
                        if (layer == 7)
                        {
                            visualsPos = GetPlatformPositionToBuildColumn(hitTransform, mousePos);
                        }
                        break;
                    case BuildingTypes.BuildingType.Walls:
                        if (layer != 7 && layer != 9 && layer != 11 && layer != 12)
                        {
                            canBuild = false;
                            visual.GetComponent<Renderer>().material.color = buildingScriptableObject.cantBuildColor;
                        }
                        if (layer == 7)
                        {
                            visualsPos = GetPlatformPositionToBuildWall(hitTransform, mousePos);
                        }
                        else if (layer == 11)
                        {
                            visualsPos = GetdWallPositionToBuilddWall(hitTransform, mousePos);
                        }
                        else if (layer == 12)
                        {
                            visualsPos = GetdWallPositionToBuilddWall(hitTransform, mousePos);
                        }
                        break;
                    case BuildingTypes.BuildingType.Doors:
                        if (layer != 7 && layer != 9 && layer != 11 && layer != 12)
                        {
                            canBuild = false;
                            visual.GetComponent<Renderer>().material.color = buildingScriptableObject.cantBuildColor;
                        }
                        if (layer == 7)
                        {
                            visualsPos = GetPlatformPositionToBuildWall(hitTransform, mousePos);
                        }
                        else if (layer == 11)
                        {
                            visualsPos = GetdWallPositionToBuilddWall(hitTransform, mousePos);
                        }
                        else if (layer == 12)
                        {
                            visualsPos = GetdWallPositionToBuilddWall(hitTransform, mousePos);
                        }
                        break;
                    case BuildingTypes.BuildingType.Machines:
                        layersNotToBuildIn = new int[] { 1, 2, 3, 4, 5, 6, 8, 10, 11, 12, 13, 14 };
                        NotBuildableLayer(layersNotToBuildIn, layer.value);

                        // Platform
                        if (layer == 7)
                        {
                            GetPositionToBuildMachines();
                        }
                        
                        if (canBuild)
                        {
                            DetectCollision detectCollision = visual.GetComponentInChildren<DetectCollision>();
                            foreach (Transform objTransform in detectCollision.nearbyTransforms)
                            {
                                if (objTransform.TryGetComponent<LoadStation>(out LoadStation loadStation))
                                {
                                    visual.GetComponent<Renderer>().material.color = Color.green;
                                    break;
                                }
                                else if (objTransform.TryGetComponent<UnloadStation>(out UnloadStation unloadStation))
                                {
                                    visual.GetComponent<Renderer>().material.color = Color.blue;
                                    break;
                                }
                            }
                        }
                        break;
                    case BuildingTypes.BuildingType.Storages:
                        layersNotToBuildIn = new int[] { 1, 2, 3, 4, 5, 6, 8, 10, 11, 12, 13, 14 };
                        NotBuildableLayer(layersNotToBuildIn, layer.value);

                        // Platform
                        if (layer == 7)
                        {
                            GetPositionToBuildMachines();
                        }

                        if (canBuild)
                        {
                            DetectCollision detectCollision = visual.GetComponentInChildren<DetectCollision>();
                            foreach (Transform objTransform in detectCollision.nearbyTransforms)
                            {
                                if (objTransform.TryGetComponent<LoadStation>(out LoadStation loadStation))
                                {
                                    visual.GetComponent<Renderer>().material.color = Color.green;
                                    break;
                                }
                                else if (objTransform.TryGetComponent<UnloadStation>(out UnloadStation unloadStation))
                                {
                                    visual.GetComponent<Renderer>().material.color = Color.blue;
                                    break;
                                }
                            }
                        }
                        break;
                }

                visual.position = visualsPos;
            }
            else
            {
                canBuild = false;
                visual.GetComponent<Renderer>().material.color = buildingScriptableObject.cantBuildColor;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (canBuild)
                {
                    CreateSelectedObjectPrefab(buildingScriptableObject, visualsPos, visual.rotation, visual.localScale);
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

    private void GetPositionToBuildMachines()
    {
        if (Mathf.Abs(mousePos.x) >= Mathf.Abs(visualsPos.x + snap) ||
            Mathf.Abs(mousePos.x) <= Mathf.Abs(visualsPos.x - snap)
            )
        {
            visualsPos.x = Mathf.Round(mousePos.x * 10f) / 10f;
        }
        if (Mathf.Abs(mousePos.z) >= Mathf.Abs(visualsPos.z + snap) ||
            Mathf.Abs(mousePos.z) <= Mathf.Abs(visualsPos.z - snap)
            )
        {
            visualsPos.z = Mathf.Round(mousePos.z * 10f) / 10f;
        }
        visualsPos.y = mousePos.y + buildingScriptableObject.currentDimention.y / 2;
    }

    private Vector3 GetColumnPositionToBuildPlatform(Transform referenceObject, Vector3 mousePosition)
    {
        Vector3 hitObjPos = referenceObject.position;
        Vector3 objHitPos = (mousePosition - hitObjPos);
        Vector3 hitObjRot = referenceObject.rotation.eulerAngles;
        Vector3 hitObjScale = referenceObject.localScale;
        Vector3 finalPosition = hitObjPos;

        Vector3 selectedObjDimentions = GetRotateObjectDimention(
            buildingScriptableObject.currentDimention,
            buildingScriptableObject.currentRotation
            );

        Vector3 hitObjectDimention = GetRotateObjectDimention(hitObjScale, hitObjRot);
        Vector3 offset = (selectedObjDimentions + hitObjectDimention) / 2;

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
        Vector3 hitObjectDimention = GetRotateObjectDimention(hitObjScale, hitObjRot);

        Vector3 selectedObjDimentions = GetRotateObjectDimention(
            buildingScriptableObject.currentDimention,
            buildingScriptableObject.currentRotation
            );

        Vector3 offset = (selectedObjDimentions + hitObjectDimention) / 2;

        Vector3 finalPosition = hitObjPos;

        if (objHitPos.x > 0.2f) { finalPosition.x += offset.x; }
        else if (objHitPos.x < -0.2f) { finalPosition.x -= offset.x; }

        if (objHitPos.z > 0.2f) { finalPosition.z += offset.z; }
        else if (objHitPos.z < -0.2f) { finalPosition.z -= offset.z; }

        finalPosition.y += offset.y - hitObjScale.y;

        return finalPosition;
    }

    private Vector3 GetPlatformPositionToBuildWall(Transform referenceObject, Vector3 mousePosition)
    {
        Vector3 hitObjPos = referenceObject.position;
        Vector3 objHitPos = (mousePosition - hitObjPos);
        Vector3 hitObjRot = referenceObject.rotation.eulerAngles;
        Vector3 hitObjScale = referenceObject.localScale;
        Vector3 selectedObjDimentions = GetRotateObjectDimention(
            buildingScriptableObject.currentDimention,
            buildingScriptableObject.currentRotation
            );
        Vector3 hitObjectDimention = GetRotateObjectDimention(hitObjScale, hitObjRot);

        Vector3 offset = (selectedObjDimentions + hitObjectDimention) / 2;

        Vector3 finalPosition = hitObjPos;

        if (Mathf.Abs(objHitPos.x) > Mathf.Abs(objHitPos.z))
        {
            if (Mathf.RoundToInt(visual.localRotation.eulerAngles.y) != Mathf.Abs(0) &
                    Mathf.RoundToInt(visual.localRotation.eulerAngles.y) != Mathf.Abs(180))
            {
                SelectedObjectHandler.Instance.RotateSelectedObject(buildingScriptableObject, visual);
            }

            if (objHitPos.x >= 0) { finalPosition.x += offset.x; } else { finalPosition.x -= offset.x; }
        }
        else
        {
            if (Mathf.RoundToInt(visual.localRotation.eulerAngles.y) != Mathf.Abs(90) &
                Mathf.RoundToInt(visual.localRotation.eulerAngles.y) != Mathf.Abs(270))
            {
                SelectedObjectHandler.Instance.RotateSelectedObject(buildingScriptableObject, visual);
            }

            if (objHitPos.z >= 0) { finalPosition.z += offset.z; } else { finalPosition.z -= offset.z; }
        }

        finalPosition.y += offset.y - hitObjScale.y;

        return finalPosition;
    }

    private Vector3 GetPlatformPositionToBuildPlatform(Transform referenceObject, Vector3 mousePosition)
    {     
        Vector3 hitObjPos = referenceObject.position;
        Vector3 objHitPos = (mousePosition - hitObjPos);
        Vector3 hitObjScale = referenceObject.localScale;
        Vector3 hitObjRot = referenceObject.rotation.eulerAngles;

        Vector3 selectedObjDimentions = GetRotateObjectDimention(
            buildingScriptableObject.currentDimention,
            buildingScriptableObject.currentRotation
            );

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

    private Vector3 GetRampPositionToBuildPlatform(Transform referenceObject, Vector3 normal, Vector3 mousePosition)
    {
        Vector3 hitObjPos = referenceObject.position;
        Vector3 objHitPos = (mousePosition - hitObjPos);
        Vector3 hitObjScale = referenceObject.localScale;
        Vector3 hitObjRot = referenceObject.rotation.eulerAngles;

        Vector3 vertialHitDirection = new Vector3(0, objHitPos.y, 0);
        Vector3 selectedObjDimentions = GetRotateObjectDimention(
            buildingScriptableObject.currentDimention,
            buildingScriptableObject.currentRotation
            );
        Vector3 hitObjectDimention = GetRotateObjectDimention(hitObjScale, hitObjRot);
        hitObjectDimention.y = hitObjScale.y;
        Vector3 offset = (selectedObjDimentions + hitObjectDimention) / 2;

        Vector3 finalPosition = hitObjPos;

        Vector3 dir = Vector3.zero;
        if (vertialHitDirection.y >= 0)
        {
            if (normal.x > -0.1f && normal.x < 0.1f)
            {
                dir.x = 0;
                if (normal.z < 0) { dir.z = 1; } else { dir.z = -1; }
            }
            if (normal.z > -0.1f && normal.z < 0.1f)
            {
                dir.z = 0;
                if (normal.x < 0) { dir.x = 1; } else { dir.x = -1; }
            }
            finalPosition.y += offset.y;
        }
        else
        {
            if (normal.x > -0.1f && normal.x < 0.1f)
            {
                dir.x = 0;
                if (normal.z > 0) { dir.z = 1; } else { dir.z = -1; }

            }
            if (normal.z > -0.1f && normal.z < 0.1f)
            {
                dir.z = 0;
                if (normal.x > 0) { dir.x = 1; } else { dir.x = -1; }
            }
            finalPosition.y -= (offset.y - selectedObjDimentions.y);
        }

        finalPosition.x += offset.x * dir.x;
        finalPosition.z += offset.z * dir.z;

        return finalPosition;
    }

    private Vector3 GetdWallPositionToBuilddWall(Transform referenceObject, Vector3 mousePosition)
    {
        Vector3 hitObjPos = referenceObject.position;
        Vector3 objHitPos = (mousePosition - hitObjPos);
        Vector3 hitObjScale = referenceObject.localScale;
        Vector3 hitObjRot = referenceObject.rotation.eulerAngles;

        if (visual.localRotation.eulerAngles.y != referenceObject.rotation.eulerAngles.y)
        {
            if (visual.localRotation.eulerAngles.y != referenceObject.rotation.eulerAngles.y + 180 &&
                visual.localRotation.eulerAngles.y != referenceObject.rotation.eulerAngles.y - 180)
            {   
                SelectedObjectHandler.Instance.RotateSelectedObject(buildingScriptableObject, visual);
            }
        }

        Vector3 horizontalHitDirection = new Vector3(objHitPos.x, 0, objHitPos.z);
        Vector3 vertialHitDirection = new Vector3(0, objHitPos.y, 0);

        Vector3 hitObjectDimention = GetRotateObjectDimention(hitObjScale, hitObjRot);
        hitObjectDimention.y = hitObjScale.y;

        Vector3 selectedObjDimentions = GetRotateObjectDimention(
            buildingScriptableObject.currentDimention,
            buildingScriptableObject.currentRotation
            );
        Vector3 offset = (selectedObjDimentions + hitObjectDimention) / 2;

        Vector3 finalPosition = hitObjPos;

        if (vertialHitDirection.y > 0.2f)
        {
            finalPosition.y += offset.y;
        }
        else if (vertialHitDirection.y < -0.2f)
        {
            finalPosition.y -= offset.y;
        }
        else
        {
            horizontalHitDirection.y = 0;
            if (Mathf.Abs(horizontalHitDirection.x) > Mathf.Abs(horizontalHitDirection.z)) {
                horizontalHitDirection.z = 0; 
            } else 
            {
                horizontalHitDirection.x = 0; 
            }
            horizontalHitDirection = horizontalHitDirection.normalized;

            finalPosition.x += offset.x * horizontalHitDirection.x;
            finalPosition.z += offset.z * horizontalHitDirection.z;
        }

        return finalPosition;
    }

    private Vector3 GetPlatformPositionToBuildRamp(Transform referenceObject, Vector3 mousePosition)
    {
        Vector3 hitObjPos = referenceObject.position;
        Vector3 objHitPos = (mousePosition - hitObjPos);
        Vector3 selectedObjecRotation = visual.localRotation.eulerAngles;
        Vector3 hitObjScale = referenceObject.localScale;
        Vector3 hitObjRot = referenceObject.rotation.eulerAngles;
        Vector3 selectedObjDimentions = GetRotateObjectDimention(
            buildingScriptableObject.currentDimention,
            buildingScriptableObject.currentRotation
            );
        Vector3 vertialOffset = Vector3.zero;
        objHitPos.y = 0;
        if (Mathf.Abs(objHitPos.x) > Mathf.Abs(objHitPos.z)) {
            objHitPos.z = 0; 
            if (objHitPos.x > 0)
            {
                if (Mathf.Abs(Mathf.RoundToInt(selectedObjecRotation.y)) == 0)
                {
                    vertialOffset.y = (-hitObjScale.y - selectedObjDimentions.y) / 2;
                }
                else if (Mathf.Abs(Mathf.RoundToInt(selectedObjecRotation.y)) == 180)
                {
                    vertialOffset.y = (-hitObjScale.y + selectedObjDimentions.y) / 2;
                }
                else
                {
                    SelectedObjectHandler.Instance.RotateSelectedObject(buildingScriptableObject, visual);
                }
            }
            else
            {
                if (Mathf.Abs(Mathf.RoundToInt(selectedObjecRotation.y)) == 0)
                {
                    vertialOffset.y = (-hitObjScale.y + selectedObjDimentions.y) / 2;
                }
                else if (Mathf.Abs(Mathf.RoundToInt(selectedObjecRotation.y)) == 180)
                {
                    vertialOffset.y = (-hitObjScale.y - selectedObjDimentions.y) / 2;
                }
                else
                {
                    SelectedObjectHandler.Instance.RotateSelectedObject(buildingScriptableObject, visual);
                }
            }
        }
        else 
        { 
            objHitPos.x = 0;
            if (objHitPos.z < 0)
            {
                if (Mathf.Abs(Mathf.RoundToInt(selectedObjecRotation.y)) == 90)
                {
                    vertialOffset.y = (-hitObjScale.y - selectedObjDimentions.y) / 2;
                }
                else if (Mathf.Abs(Mathf.RoundToInt(selectedObjecRotation.y)) == 270)
                {
                    vertialOffset.y = (-hitObjScale.y + selectedObjDimentions.y) / 2;
                }
                else
                {
                    SelectedObjectHandler.Instance.RotateSelectedObject(buildingScriptableObject, visual);
                }
            }
            else
            {
                if (Mathf.Abs(Mathf.RoundToInt(selectedObjecRotation.y)) == 90)
                {
                    vertialOffset.y = (-hitObjScale.y + selectedObjDimentions.y) / 2;
                }
                else if (Mathf.Abs(Mathf.RoundToInt(selectedObjecRotation.y)) == 270)
                {
                    vertialOffset.y = (-hitObjScale.y - selectedObjDimentions.y) / 2;
                }
                else
                {
                    SelectedObjectHandler.Instance.RotateSelectedObject(buildingScriptableObject, visual);
                }
            }
        }
        objHitPos = objHitPos.normalized;

        Vector3 hitObjectDimention = GetRotateObjectDimention(hitObjScale, hitObjRot);

        Vector3 horizontalOffset = (selectedObjDimentions + hitObjectDimention) / 2;

        Vector3 finalPosition = hitObjPos;
        finalPosition.x += horizontalOffset.x * objHitPos.x;
        finalPosition.y += vertialOffset.y;
        finalPosition.z += horizontalOffset.z * objHitPos.z;

        return finalPosition;
    }

    private Vector3 GetRampPositionToBuildRamp(Transform referenceObject, Vector3 normal, Vector3 mousePosition)
    {
        Vector3 hitObjPos = referenceObject.position;
        Vector3 objHitPos = (mousePosition - hitObjPos);
        Vector3 hitObjScale = referenceObject.localScale;
        Vector3 hitObjRot = referenceObject.rotation.eulerAngles;
        Vector3 selectedObjDimentions = GetRotateObjectDimention(
            buildingScriptableObject.currentDimention,
            buildingScriptableObject.currentRotation
            );
        if (visual.localRotation != referenceObject.rotation)
        {
            SelectedObjectHandler.Instance.RotateSelectedObject(buildingScriptableObject, visual);
        }

        Vector3 horizontalHitDirection = new Vector3(objHitPos.x, 0, objHitPos.z);
        Vector3 vertialHitDirection = new Vector3(0, objHitPos.y, 0);

        Vector3 hitObjectDimention = GetRotateObjectDimention(hitObjScale, hitObjRot);
        hitObjectDimention.y = hitObjScale.y;
        Vector3 offset = (selectedObjDimentions + hitObjectDimention) / 2;

        Vector3 finalPosition = hitObjPos;

        Vector3 dir = Vector3.zero;
        if (normal.y > 0)
        {
            if (vertialHitDirection.y > 0.050f)
            {
                if (normal.x > -0.1f && normal.x < 0.1f)
                {
                    dir.x = 0;
                    if (normal.z < 0) { dir.z = 1; } else { dir.z = -1; }
                }
                if (normal.z > -0.1f && normal.z < 0.1f)
                {
                    dir.z = 0;
                    if (normal.x < 0) { dir.x = 1; } else { dir.x = -1; }
                }
                finalPosition.y += offset.y;
            }
            else if (vertialHitDirection.y < -0.050f)
            {
                if (normal.x > -0.1f && normal.x < 0.1f)
                {
                    dir.x = 0;
                    if (normal.z > 0) { dir.z = 1; } else { dir.z = -1; }

                }
                if (normal.z > -0.1f && normal.z < 0.1f)
                {
                    dir.z = 0;
                    if (normal.x > 0) { dir.x = 1; } else { dir.x = -1; }
                }
                finalPosition.y -= offset.y;
            }
            else
            {
                if (normal.x > -0.1f && normal.x < 0.1f)
                {
                    horizontalHitDirection.z = 0;
                    horizontalHitDirection.x = Mathf.Sign(objHitPos.x) * 1;

                }
                if (normal.z > -0.1f && normal.z < 0.1f)
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

    public void CreateSelectedObjectPrefab(BuildingsScriptableObjects obj, Vector3 pos, Quaternion rot, Vector3 scale)
    {
        Transform prefab = Instantiate(obj.prefab, pos, rot);
        prefab.localScale = scale;
        prefab.GetComponent<IManipulable>().CreateSelf();
    }

    public void DestroyObjectPrefab(Transform prefab)
    {
        IManipulable obj = prefab.GetComponent<IManipulable>();
        if (obj != null)
        {
            obj.DestroySelf();
        }
    }

    public void SetSelectedObject(BuildingsScriptableObjects selectedSO)
    {
        if (visual != null) { Destroy(visual.gameObject); }
        buildingScriptableObject = selectedSO;
        visual = Instantiate(buildingScriptableObject.visual);
        selectedSO.currentDimention = selectedSO.dimention;
        selectedSO.currentRotation = Vector3.zero;
        selectedSO.buildingRotation = BuildingsScriptableObjects.BuildingRotation.Zero;
        selectedSO.buildingScale = BuildingsScriptableObjects.BuildingScale.One;
    }

    public void UnsetSelectedObject()
    {
        if (visual != null) { Destroy(visual.gameObject); }
        buildingScriptableObject = null;
    }

    private void RotateSelectedObject(BuildingsScriptableObjects selectedSO, Transform selectedObj)
    {
        if (Input.GetKeyDown(KeyCode.R) && selectedSO != null)
        {
            SelectedObjectHandler.Instance.RotateSelectedObject(selectedSO, selectedObj);
        }
    }

    private void ScaleSelectedObject(BuildingsScriptableObjects selectedSO, Transform selectedObj)
    {
        if (Input.GetKey(KeyCode.LeftShift) && selectedSO != null)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                selectedSO.GetNextScale();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                selectedSO.GetPreviousScale();
            }
            SelectedObjectHandler.Instance.ScaleSelectedObject(selectedSO, selectedObj);
        }
    }

    private bool CanBuild(Transform ghostObject)
    {
        DetectCollision collision = ghostObject.GetChild(0).GetComponent<DetectCollision>();
        if (collision.collided)
        {
            return false;
        }
        return true;
    }

    private void NotBuildableLayer(int[] unbuildableLayers, int currentLayer)
    {
        foreach (int layer in unbuildableLayers)
        {
            if (layer == currentLayer)
            {
                canBuild = false;
                visual.GetComponent<Renderer>().material.color = buildingScriptableObject.cantBuildColor;
            }
        }
    }
}
