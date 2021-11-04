using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class BuildingSystem : MonoBehaviour
{
    [SerializeField] private LayerMask mouseColliderLayerMask = new LayerMask();
    [SerializeField] private List<BuildingsScriptableObjects> buildingScriptableObjectList;
    [SerializeField] private PositionSnap posistionSnap;
    [SerializeField] private BuildingType buildingType;

    private BuildingsScriptableObjects buildingScriptableObject;
    private State state;

    private float snapValue;
    private Transform visual;

    private Vector3 visualsPos = Vector3.zero;
    private Vector3 mousePos = Vector3.zero;
    private Vector3 objectOffset = Vector3.zero;

    public Transform test;

    private enum PositionSnap
    {
        Half,
        One,
        Two,
        Three,
        Four,
    };

    private enum State
    {
        Building,
        NotBuilding,
    };

    private enum BuildingType
    {
        Platforms,
        Walls,
        Columns,
    };

    void Start()
    {
        state = State.NotBuilding;
    }

    void Update()
    {
        ChooseState();
        if (state == State.Building)
        {
            ChooseSnap();
            bool canBuild = CanBuild(visual);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit, 999f, mouseColliderLayerMask))
            {
                mousePos = raycastHit.point;
                Transform hitTransform = raycastHit.transform;
                LayerMask layer = raycastHit.transform.gameObject.layer;
                Vector3 normalHit = raycastHit.normal;
                test.position = mousePos;

                switch (buildingType)
                {
                    case BuildingType.Platforms:
                        if (layer != 7 && layer != 8 && layer != 9) {
                            canBuild = false;
                            break;
                        }
                        if (layer == 7 && normalHit.y >= -0.01f && normalHit.y <= 0.01f)
                        {
                            Vector3 normalDist = (mousePos - hitTransform.position);
                            mousePos.x += normalHit.x / 2;
                            mousePos.y += normalHit.y / 2;
                            mousePos.z += normalHit.z / 2;
                        }
                        else if (layer == 8)
                        {
                            print("xuxa");

                        }
                        else if (layer == 9)
                        {
                            print("xuxa");
                        }
                        break;
                    case BuildingType.Columns:
                        if (layer != 7) 
                        { 
                            canBuild = false;
                            break;
                        }
                        break;
                }                
                visualsPos = new Vector3(
                   Mathf.RoundToInt(mousePos.x), 
                    Mathf.RoundToInt(mousePos.y), 
                    Mathf.RoundToInt(mousePos.z)
                );
                

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
                    Instantiate(buildingScriptableObject.prefab, visualsPos, visual.rotation);
                }
                else
                {
                    UtilsClass.CreateWorldTextPopup(buildingScriptableObject.name, visualsPos);
                }
            }
        }
       
    }

    void ChooseState()
    {
        if (Input.GetKeyDown("1"))
        {
            if (visual != null) { Destroy(visual.gameObject); }

            state = State.Building;
            buildingType = BuildingType.Platforms;
            ChooseBuildingType();
            print("one");
        }
        if (Input.GetKeyDown("2"))
        {
            if (visual != null) { Destroy(visual.gameObject); }
            state = State.Building;
            buildingType = BuildingType.Columns;
            ChooseBuildingType();
            print("one");
        }
        else if (Input.GetKeyDown("3"))
        {
            print("two");
            if (visual != null) { Destroy(visual.gameObject); }
            state = State.NotBuilding;
        }
    }

    void ChooseBuildingType()
    {
        switch (buildingType)
        {
            case BuildingType.Platforms:
                buildingScriptableObject = buildingScriptableObjectList[0];
                break;
            case BuildingType.Columns:
                buildingScriptableObject = buildingScriptableObjectList[1];
                break;
        }
        visual = Instantiate(buildingScriptableObject.visual);
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
