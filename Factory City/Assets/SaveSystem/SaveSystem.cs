using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveSystem : MonoBehaviour
{
    private List<ISaveable> saveables = new List<ISaveable>();

    private void Awake()
    {
        ResourceManager.Init();
        JobManager.Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            saveables.Add(transform.GetComponent<Platforms>());
            saveables.Add(transform.GetComponent<Columns>());
            saveables.Add(transform.GetComponent<Player>());
            saveables.Add(transform.GetComponent<Ramps15>());
            saveables.Add(transform.GetComponent<Ramps30>());
            saveables.Add(transform.GetComponent<Ramps45>());
            saveables.Add(transform.GetComponent<Walls>());
            saveables.Add(transform.GetComponent<Doors>());
            SaveDataManager.SaveJsonData(saveables);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            saveables.Clear();
            saveables.Add(transform.GetComponent<Platforms>());
            saveables.Add(transform.GetComponent<Columns>());
            saveables.Add(transform.GetComponent<Player>());
            saveables.Add(transform.GetComponent<Ramps15>());
            saveables.Add(transform.GetComponent<Ramps30>());
            saveables.Add(transform.GetComponent<Ramps45>());
            saveables.Add(transform.GetComponent<Walls>());
            saveables.Add(transform.GetComponent<Doors>());
            SaveDataManager.LoadJsonData(saveables);
        }
    }
}
