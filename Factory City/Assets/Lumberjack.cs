using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Lumberjack : MonoBehaviour, IHarvestResource
{
    public static Lumberjack Instance;

    [SerializeField] private Transform actionAarea;
    [SerializeField] private Transform tool;
    [SerializeField] public bool hasTools;

    public State currentState;

    private Collider actionCollider;
    private SphereCollider FOVCollider;

    private Citizen citizen;


    private int minDamage;
    private int maxDamage;

    private int maxResourceAmount;
    [SerializeField] private int resourceAmount;

    private float attakFrequence;
    private bool chopping = false;

    public ITreeDamageable tree;
    private Transform treeObj;

    public Transform deliveryStorage;

    public enum State
    {
        ChoppingTree,
        ChoppingLog,
        GatheringLog,
        ReturningToFacilty,
        LookingForTree,
        LookingForLog,
        LookingForLogHalf,
        LookingForTools,
        DeliveringLogs,
    }

    public void SetResourceToHarvest(Transform resource)
    {
        treeObj = resource;
        tree = resource.GetComponent<ITreeDamageable>();
        Tree tempTree = resource.GetComponent<Tree>();
        switch (tempTree.treeType)
        {
            case Tree.Type.Tree: tempTree.healthSystem.OnDead += OnTreeChoped; break;
            case Tree.Type.Log: tempTree.healthSystem.OnDead += OnLogChoped; break;
        }        
        chopping = false;
    }

    public bool hasTask()
    {
        return (treeObj != null);
    }

    private void Awake()
    {
        Instance = this;
        maxResourceAmount = 2;
        resourceAmount = 0;
    }

    private void Start()
    {
        citizen = GetComponent<Citizen>();
        minDamage = citizen.attackDamage - 2;
        maxDamage = citizen.attackDamage + 2;
        attakFrequence = 1 / citizen.attackSpeed;

        actionAarea = transform.Find("ActionArea");
        actionCollider = transform.Find("ActionArea").GetComponent<BoxCollider>();
        FOVCollider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        if (tree == null && currentState != State.DeliveringLogs)
        {
            print("Looking for Tree");
            currentState = State.LookingForTree;
            return;
        }

        if (citizen.HasReachedDestination() && !chopping)
        {
            switch (currentState)
            {
                case State.LookingForTree:
                    currentState = State.ChoppingTree;
                    break;
                case State.ChoppingTree:
                    FunctionTimer.Create(ChopTree, attakFrequence);
                    chopping = true;
                    break;
                case State.LookingForLog:
                    currentState = State.ChoppingLog;
                    break;
                case State.ChoppingLog:
                    FunctionTimer.Create(ChopLog, attakFrequence);
                    chopping = true;
                    break;
                case State.GatheringLog:
                    GetLog();
                    break;
                case State.ReturningToFacilty:
                    ReturnToFacility();
                    break;
                case State.DeliveringLogs:
                    DeliverLogHalf();
                    break;                    
            }
        }
    }

    private void ChopTree()
    {
        chopping = false;
        print("Chopping Tree");
        if (treeObj.GetComponent<Tree>().treeType == Tree.Type.Tree)
        {
            int damageAmount = Random.Range(minDamage, maxDamage);
            tree.Damage(damageAmount);
        }
        else
        {
            Debug.LogError("NOT A TREE!");
        }
    }

    private void ChopLog()
    {
        chopping = false;
        print("Chopping Log");
        if (treeObj != null && treeObj.GetComponent<Tree>().treeType == Tree.Type.Log)
        {
            if (treeObj.GetComponent<Rigidbody>().velocity.magnitude < 0.1f)
            {
                int damageAmount = Random.Range(minDamage, maxDamage);
                tree.Damage(damageAmount);
            }
        }
        else
        {
            Debug.LogError("NOT A LOG!");
        }
    }

    private void GetLog()
    {
        print("Get the log!");
        if (treeObj != null && treeObj.GetComponent<LogResource>().resourceType == ResourceManager.ResourceType.Log)
        {
            print("Got the LOG!");
            Destroy(treeObj.gameObject);
            resourceAmount += treeObj.GetComponent<LogResource>().resourceAmount;
            if (resourceAmount >= maxResourceAmount)
            {
                tree = null;
                treeObj = null;
                currentState = State.ReturningToFacilty;
                ReturnToFacility();
            }
            else
            {
                currentState = State.LookingForLogHalf;
                LookForLogHalf();
            }
        }
        else
        {
            print(treeObj);
            currentState = State.LookingForLogHalf;
            LookForLogHalf();
            Debug.LogError("NOT A LOG");
        }
    }

    private void DeliverLogs()
    {
        if (citizen.HasReachedDestination())
        {
            print("Logs delivered");
            Storage storage = deliveryStorage.GetComponent<Storage>();
            storage.LoadResource(resourceAmount, ResourceManager.ResourceType.Log);
            resourceAmount = 0;
            currentState = State.LookingForTree;
        }
    }

    private void ReturnToFacility()
    {
        print("Returning to facility");
        Transform deliverySpot = deliveryStorage.GetComponent<Storage>().GetDeliverySpot();
        citizen.SetDestinationObj(deliverySpot);
        currentState = State.DeliveringLogs;
    }

    void OnTreeChoped(object sender, System.EventArgs e)
    {
        tree = null;
        treeObj = null;
        currentState = State.LookingForLog;
        LookForLog();
    }

    void OnLogChoped(object sender, System.EventArgs e)
    {
        tree = null;
        treeObj = null;
        currentState = State.LookingForLogHalf;
        LookForLogHalf();
    }

    void LookForLogHalf()
    {
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, FOVCollider.radius);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent<Tree>(out Tree tree))
            {
                print("Looking for LogHalf");
                if (tree.treeType == Tree.Type.LogHalf && currentState == State.LookingForLogHalf)
                {
                    print("Found LogHalf");
                    treeObj = tree.transform;
                    SetResourceToHarvest(treeObj);
                    citizen.SetDestinationObj(treeObj); 
                    currentState = State.GatheringLog;
                    return;
                }
            }
        }
        if (resourceAmount > 0)
        { 
            currentState = State.ReturningToFacilty; 
        }
        else
        {
            print("No LogHalf found");
            currentState = State.LookingForTree;
        }
    }

    void LookForLog()
    {
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, FOVCollider.radius);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent<Tree>(out Tree tree))
            {
                print("Looking for Log");
                print(tree.treeType);
                print(currentState);
                if (tree.treeType == Tree.Type.Log && currentState == State.LookingForLog)
                {
                    print("Found Log");
                    treeObj = tree.transform;
                    SetResourceToHarvest(treeObj);
                    citizen.SetDestinationObj(treeObj);
                    currentState = State.ChoppingLog;
                    break;
                }
                else
                {
                    print("No Log found");
                    if (resourceAmount > 0)
                    {
                        currentState = State.ReturningToFacilty;
                    }
                }
            }
        }
    }

    void DeliverLogHalf()
    {
        Collider[] colliderArray = Physics.OverlapBox(transform.position, Vector3.one * .5f);
        foreach (Collider collider in colliderArray)
        {
            if (collider.tag == "Storage")
            {
                print("Logs Delivered");
                DeliverLogs();
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (
            currentState == State.GatheringLog || 
            currentState == State.ReturningToFacilty ||
            currentState == State.DeliveringLogs
            ) return;

        if (collider.TryGetComponent<LogResource>(out LogResource logResource))
        {
            if (logResource.resourceType == ResourceManager.ResourceType.Log)
            {
                treeObj = logResource.transform;
                SetResourceToHarvest(treeObj);
                citizen.SetDestinationObj(treeObj);
            }
        }

        if (collider.TryGetComponent<Tree>(out Tree tree))
        {
            switch (tree.treeType)
            {
                case Tree.Type.Tree: currentState = State.ChoppingTree; break;
                case Tree.Type.Log: currentState = State.ChoppingLog; break;
                case Tree.Type.LogHalf: currentState = State.GatheringLog; break;
            }
            treeObj = tree.transform;
            SetResourceToHarvest(treeObj);
            citizen.SetDestinationObj(treeObj);
        }
    }
}


