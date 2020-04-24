using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff
{
    protected int BuffTypeID = 0;
    protected int energyCost = 0;
    public Sprite buffImage;
    protected LevelManager lm;
    public Buff(int id) 
    {
        BuffTypeID = id;
        lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();

        
        if (lm) 
        {
            Sprite temp = lm.buffImages[BuffTypeID];
            if (temp) 
            {
                buffImage = temp;
            }
        }
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
