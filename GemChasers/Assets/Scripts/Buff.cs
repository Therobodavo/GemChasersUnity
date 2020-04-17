using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff
{
    private int BuffTypeID = 0;
    private int energyCost = 0;

    public Buff() 
    {

    }
    public int GetBuffID() 
    {
        return BuffTypeID;
    }
    public int GetEnergyCost() 
    {
        return energyCost;
    }

}
