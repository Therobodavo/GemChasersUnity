using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff
{
    protected int BuffTypeID = 0;
    protected int energyCost = 0;
    public Sprite buffImage;
    protected LevelManager lm;
    public IType.BuffType[] instantAttackCombinations;
    public IType.BuffType[] instantPassiveCombinations;
    public IType.BuffType[] delayedPassiveCombinations;
    public IType.BuffType[] delayedAttackCombinations;

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
    public int GetBuffID() 
    {
        return BuffTypeID;
    }
    public int GetEnergyCost() 
    {
        return energyCost;
    }
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
