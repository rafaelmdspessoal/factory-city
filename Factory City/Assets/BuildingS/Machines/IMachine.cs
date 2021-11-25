using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMachine 
{
    LoadStation GetLoadStation();

    UnloadStation GetUnloadStation();

    void AddStationsInReach();
    void RemoveStationsOutOfReach(Transform station);

    void GetStationsInReach(Transform station);

}

