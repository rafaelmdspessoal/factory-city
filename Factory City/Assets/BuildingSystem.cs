using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class BuildingSystem : MonoBehaviour
{
    [SerializeField] private LayerMask mouseColliderLayerMask = new LayerMask();
    [SerializeField] private PlatformsScriptableObject testScriptableObject;
    [SerializeField] private PositionSnap posistionSnap;
    [SerializeField] private BuildingType buildingType;
    
    private State state;

    private float snapValue;
    private Transform visual;

    private Vector3 visualsPos = Vector3.zero;
    private Vector3 mousePos = Vector3.zero;

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
        Ground,
        Floor,
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit, 999f, mouseColliderLayerMask))
            {
                mousePos = raycastHit.point;
                LayerMask layer = raycastHit.transform.gameObject.layer;

                switch (layer)
                {
                    case 7:
                        buildingType = BuildingType.Ground;
                        break;
                }
            }
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

            switch (buildingType)
            {
                case BuildingType.Ground:
                    break;
            }

            if ((mousePos - visualsPos).magnitude > snapValue)
            {
                visualsPos = new Vector3(
                    Mathf.RoundToInt(mousePos.x), 
                    Mathf.RoundToInt(mousePos.y), 
                    Mathf.RoundToInt(mousePos.z)
                );
            }


            visual.position = visualsPos;
            bool canBuild = CanBuild(visual);

            if (Input.GetMouseButtonDown(0))
            {
                if (canBuild)
                {
                    Instantiate(testScriptableObject.prefab, visualsPos, visual.rotation);
                }
                else
                {
                    UtilsClass.CreateWorldTextPopup(testScriptableObject.name, visualsPos);
                }
            }
        }
       
    }

    void ChooseState()
    {
        if (Input.GetKeyDown("1"))
        {
            state = State.Building;
            visual = Instantiate(testScriptableObject.visual);
            print("one");
        }
        else if (Input.GetKeyDown("2"))
        {
            print("two");
            Destroy(visual.gameObject);
            state = State.NotBuilding;
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
