using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private List<ISaveable> saveables = new List<ISaveable>();
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            saveables.Add(transform.GetComponent<Platforms>());
            saveables.Add(transform.GetComponent<Columns>());
            saveables.Add(transform.GetComponent<Player>());
            SaveDataManager.SaveJsonData(saveables);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            saveables.Add(transform.GetComponent<Platforms>());
            saveables.Add(transform.GetComponent<Columns>());
            saveables.Add(transform.GetComponent<Player>()); 
            SaveDataManager.LoadJsonData(saveables);
        }
    }
}
