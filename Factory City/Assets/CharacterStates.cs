using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStates : MonoBehaviour
{
    public static CharacterStates Instance { get; private set; }

    public State state;

    private void Awake()
    {
        Instance = this;
        state = State.NotBuilding;
    }

    public enum State
    {
        Building,
        NotBuilding,
    };

    public State GetNextState()
    {
        switch (state)
        {
            default:
                BuildingSystem.Instance.UnsetSelectedObject();
                print("not building");
                return State.NotBuilding;
            case State.Building:
                BuildingSystem.Instance.UnsetSelectedObject();
                print("not building");
                return State.NotBuilding;
            case State.NotBuilding:
                BuildingSystem.Instance.UnsetSelectedObject();
                print("building");
                return State.Building;
        }
    }
}
