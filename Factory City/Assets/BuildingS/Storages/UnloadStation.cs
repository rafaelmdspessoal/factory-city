using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnloadStation : Storage
{
    private void Start()
    {
        JobManager.AddJobSpot(1, this.transform);
    }
}
