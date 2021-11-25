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
        state = State.Building;
    }

    public enum State
    {
        Building,
        Demolishing,
        Harvesting,
    };

    public State GetNextState()
    {
        switch (state)
        {
            default:
            case State.Building:
                BuildingSystem.Instance.UnsetSelectedObject();
                print("Demolishing");
                return State.Demolishing;
            case State.Demolishing:
                BuildingSystem.Instance.UnsetSelectedObject();
                print("Harvesting");
                return State.Harvesting;
            case State.Harvesting:
                BuildingSystem.Instance.UnsetSelectedObject();
                print("Building");
                return State.Building;
        }
    }
}
