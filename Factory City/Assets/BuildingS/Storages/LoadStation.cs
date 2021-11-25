using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadStation : Storage
{
    private void Start()
    {
        JobManager.AddJobSpot(1, this.transform);
    }
}
