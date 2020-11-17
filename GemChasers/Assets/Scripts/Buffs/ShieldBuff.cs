using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Shield Buff Class
 * Programmed by David Knolls
 * 
 * Sets up functionality for shield buff
 * 
 * Effect: Reduce incoming damage for player
 */

public class ShieldBuff : Buff
{
    public float defenseMultiplier = 1.5f;
    public ShieldBuff() : base(2)
    {
        energyCost = 10;

        //Sets up combinations
        instantPassiveCombinations = new IType.BuffType[3];
        instantPassiveCombinations[0] = IType.BuffType.Heal;
        instantPassiveCombinations[0] = IType.BuffType.Relax;
        instantPassiveCombinations[0] = IType.BuffType.Speed;

        delayedPassiveCombinations = new IType.BuffType[1];
        delayedPassiveCombinations[0] = IType.BuffType.Linger;
    }

    //Function call for effects triggered on use
    public override void OnInstantPassive(PlayerAttack attackObj)
    {
        base.OnInstantPassive(attackObj);
        attackObj.turnStatBuffs[1] = defenseMultiplier;
    }

    //Function call for effects triggered over X number of turns
    public override void OnDelayedPassive(PlayerAttack attackObj)
    {
        base.OnDelayedPassive(attackObj);
        if (attackObj != null)
        {
            PlayerManager playerManager = attackObj.playerScript;
            if (playerManager != null) 
            {
                playerManager.turnEffects.Add(new BattleEffect(playerManager, IType.Stat.Defense, defenseMultiplier, (IType.ElementType)attackObj.gemType.gemTypeID, 3));
            }
        }
    }
}
