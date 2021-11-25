using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationManager : MonoBehaviour
{
    public static event EventHandler OnPopulationChanged;
    public static int population;
    public static List<Citizen> citizenList;

    public static void Init()
    {
        citizenList = new List<Citizen>();
        population = 0;
    }

    public static void AddCitizen(int amount, Citizen citizen)
    {
        population += amount;
        if (OnPopulationChanged != null) OnPopulationChanged(null, EventArgs.Empty);
    }

    public static void RemoveJobSpot(int amount, Citizen citizen)
    {
        population -= amount;
        if (OnPopulationChanged != null) OnPopulationChanged(null, EventArgs.Empty);
    }

    public static int GetPopulation()
    {
        return population;
    }
}
