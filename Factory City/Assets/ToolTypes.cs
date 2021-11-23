using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTypes : MonoBehaviour
{
    public static ToolTypes Instance { get; private set; }

    public Tools tool;

    private void Awake()
    {
        Instance = this;
        tool = Tools.Axe;
    }

    public enum Tools
    {
        Builder,
        Bulldozer,
        Axe,
    };


    private void Update()
    {
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftShift)) { return; }
        
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            CharacterStates.Instance.state = CharacterStates.Instance.GetNextState();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            CharacterStates.Instance.state = CharacterStates.Instance.GetNextState();
        }
    }
}
