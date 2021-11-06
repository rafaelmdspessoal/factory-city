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

    public Transform test;

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
        if (CharacterStates.Instance.state == CharacterStates.State.Building)
        {
            if (visual == null || UtilsClass.IsPointerOverUI()) {
                canBuild = false;
                print("Can't build");
                return;             
            }

            ChooseSnap();
            canBuild = CanBuild(visual);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit, 999f, mouseColliderLayerMask))
            {
                mousePos = raycastHit.point;
                Transform hitTransform = raycastHit.transform;
                LayerMask layer = raycastHit.transform.gameObject.layer;
                test.position = mousePos;
                Vector3 hitObjPos = hitTransform.position;
                Vector3 hitObjScale = hitTransform.localScale;
                Vector3 dimentions = buildingScriptableObject.dimention;
                Vector3 hitObjectNormal = raycastHit.normal;

                Vector3 dir = (mousePos - hitObjPos);
                switch (BuildingTypes.Instance.buildingType)
                {
                    case BuildingTypes.BuildingType.Platforms:
                        if (layer != 7 && layer != 8 && layer != 9) {
                            canBuild = false;
                            break;
                        }
                        if (layer == 7)
                        {
                            dir.y = 0;
                            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.z)) { dir.z = 0; } else { dir.x = 0; }
                            dir = dir.normalized;
                            hitObjPos.y = hitObjPos.y - (hitTransform.localScale.y / 2 );
                            Vector3 offset = new Vector3(
                                dimentions.x + hitTransform.localScale.x, 
                                0,
                                dimentions.z + hitTransform.localScale.z
                                ) / 2;

                            mousePos.x = offset.x * dir.x;
                            mousePos.y = offset.y * dir.y;
                            mousePos.z = offset.z * dir.z;

                            visualsPos = mousePos + hitObjPos;
                        }
                        else if (layer == 8)
                        {
                            if (Mathf.Abs(hitObjectNormal.y) > 0.1)
                            {
                                if (dir.x > 0.3f) { visualsPos.x = hitObjPos.x + dimentions.x / 2; }
                                else if (dir.x < -0.3f) { visualsPos.x = hitObjPos.x - dimentions.x / 2; }
                                else { visualsPos.x = hitObjPos.x; }

                                if (dir.z > 0.3f) { visualsPos.z = hitObjPos.z + dimentions.z / 2; }
                                else if (dir.z < -0.3f) { visualsPos.z = hitObjPos.z - dimentions.z / 2; }
                                else { visualsPos.z = hitObjPos.z; }
                            }

                            visualsPos.y = hitObjPos.y * 2 - dimentions.y;
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
                        visualsPos.y = hitObjScale.y;
                        if (dir.x >= 0.25f) { visualsPos.x = hitObjPos.x + hitObjScale.x / 2; }
                        else if (dir.x <= -0.25f) { visualsPos.x = hitObjPos.x - hitObjScale.x / 2; }
                        else { visualsPos.x = hitObjPos.x; }

                        if (dir.z >= 0.25f) { visualsPos.z = hitObjPos.z + hitObjScale.z / 2; }
                        else if (dir.z <= -0.25f) { visualsPos.z = hitObjPos.z - hitObjScale.z / 2; }
                        else { visualsPos.z = hitObjPos.z; }
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
                    GetSelectedObjectPrefab(buildingScriptableObject, visualsPos, visual.rotation);
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

    public Transform GetSelectedObjectPrefab(BuildingsScriptableObjects obj, Vector3 pos, Quaternion rot)
    {
        Transform prefab = Instantiate(obj.prefab, pos, rot);
        return prefab;
    }

    public void SetSelectedObject(BuildingsScriptableObjects obj)
    {
        if (visual != null) { Destroy(visual.gameObject); }
        buildingScriptableObject = obj;
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
