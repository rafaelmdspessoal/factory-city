using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHaveWorkers 
{
    void Hire(Citizen citizen);

    void Fire(Citizen citizen);

    bool HasJobSpot();
}
