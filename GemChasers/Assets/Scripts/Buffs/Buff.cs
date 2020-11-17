using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Base Buff Class
 * Programmed by David Knolls
 * 
 * Creates structure all buffs follow
 */

public class Buff
{

    //Properties for each buff

    protected int BuffTypeID = 0; //ID related to enum
    protected int energyCost = 0; //Energy cost used when calculating move cost
    public Sprite buffImage; //Image used in UI
    protected LevelManager lm; //Reference to level manager

    //Combinations allowed by type (using ID)
    public IType.BuffType[] instantAttackCombinations;
    public IType.BuffType[] instantPassiveCombinations;
    public IType.BuffType[] delayedPassiveCombinations;
    public IType.BuffType[] delayedAttackCombinations;

    //Constructor for Base Buff
    public Buff(int id) 
    {
        BuffTypeID = id;
        lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();

        //Attempt to get sprite image based off ID
        if (lm) 
        {
            Sprite temp = lm.buffImages[BuffTypeID];
            if (temp) 
            {
                buffImage = temp;
            }
        }
    }

    //Method called to set the player type (changes on each new move)
    public virtual void InitAttack(PlayerAttack attackObj) 
    {
        if (attackObj != null) 
        {
            if (attackObj.gemType != null) 
            {
                attackObj.playerScript.currentType = (IType.ElementType)attackObj.gemType.gemTypeID;
            }
        }
    }

    //ID Getter
    public int GetBuffID() 
    {
        return BuffTypeID;
    }

    //Energy Cost Getter
    public int GetEnergyCost() 
    {
        return energyCost;
    }

    //Base functions to trigger based on attack type combination
    public virtual void OnInstantAttack(PlayerAttack attackObj) 
    {
        InitAttack(attackObj);
    }
    public virtual void OnInstantPassive(PlayerAttack attackObj)
    {
        InitAttack(attackObj);
    }
    public virtual void OnDelayedAttack(PlayerAttack attackObj)
    {
        InitAttack(attackObj);
    }
    public virtual void OnDelayedPassive(PlayerAttack attackObj)
    {
        InitAttack(attackObj);
    }

}
