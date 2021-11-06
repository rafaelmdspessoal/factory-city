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
    };
}
