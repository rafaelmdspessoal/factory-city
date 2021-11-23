using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMachine 
{
    void Hire(Citizen citizen);

    void Fire(Citizen citizen);

    bool HasJobSpot();

    LoadStation GetLoadStation();

    LoadStation GetUnloadStation();
}

