using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Relax Buff Class
 * Programmed by David Knolls
 * 
 * Sets up functionality for relax buff
 * 
 * Effect: Restores energy to player
 */

public class RelaxBuff : Buff
{
    //Properties
    public float instantEnergyValue = 75;
    public float delayedEnergyValue = 35;

    public RelaxBuff() : base(4)
    {
        energyCost = 25;
        instantPassiveCombinations = new IType.BuffType[3];

        instantPassiveCombinations[0] = IType.BuffType.Heal;
        instantPassiveCombinations[1] = IType.BuffType.Shield;
        instantPassiveCombinations[2] = IType.BuffType.Speed;

        delayedPassiveCombinations = new IType.BuffType[1];
        delayedPassiveCombinations[0] = IType.BuffType.Linger;
    }

    //Function call for effects triggered on use
    public override void OnInstantPassive(PlayerAttack attackObj)
    {
        base.OnInstantPassive(attackObj);
        attackObj.turnStatBuffs[4] = instantEnergyValue;
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
                playerManager.turnEffects.Add(new BattleEffect(playerManager, IType.Stat.Energy, delayedEnergyValue, (IType.ElementType)attackObj.gemType.gemTypeID, 3));
            }
        }
    }
}
