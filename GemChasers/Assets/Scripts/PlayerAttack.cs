using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack
{
    private Buff[] buffs;
    public Gem gemType;

    private int energyCost;
    private PlayerManager playerScript;


    public PlayerAttack(PlayerManager player, Buff buff1 = null, Buff buff2 = null, Gem gem = null) 
    {
        playerScript = player;
        buffs = new Buff[2];
        buffs[0] = buff1;
        buffs[1] = buff2;
        gemType = gem;

        if (buff1 != null && buff2 != null) 
        {
            int sum = buff1.GetEnergyCost() + buff2.GetEnergyCost();
            if (sum > 100)
            {
                energyCost = 100;
            }
            else
            {
                energyCost = sum;
            }
        }
    }
    public int GetEnergyCost() 
    {
        return energyCost;
    }
    public void Use() 
    {
        
    }
}
