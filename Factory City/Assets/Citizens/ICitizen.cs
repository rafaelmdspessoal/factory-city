using System;
using System.Collections.Generic;
using UnityEngine;

public interface ICitizen
{
    bool IsIdle();
    bool HasReachedDestination();
    public Transform GetWorkPlace();
    void MoveTo(Transform destination, Action onArrivedAtPosition, float stoppingDistance);
}
